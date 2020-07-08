using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stage_API.Business.Abstractions;
using Stage_API.Domain;
using Stage_API.Dto;
using Stage_API.IdentityModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections;
using System.Collections.Generic;

namespace Stage_API.Controllers
{
    [ApiController]
    [Route("/api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IOptions<TokenSettings> _tokenSettings;
        private readonly IUserService _userService;
        private readonly PasswordHasher<User> _hasher;

        public AuthenticationController(UserManager<User> userManager, RoleManager<Role> roleManager, IPasswordHasher<User> passwordHasher, IOptions<TokenSettings> tokenSettings, IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _tokenSettings = tokenSettings;
            _userService = userService;
            _hasher = new PasswordHasher<User>();
        }

        [HttpGet("users/{role}")]
        [Authorize(Roles = Role.Coordinator)]
        public async Task<ActionResult<ICollection<UserReadDto>>> GetAllUsersByRole(string role)
        {
            var usersByRole = await _userService.GetUsersByRole(role);
            if(usersByRole == null)
            {
                return BadRequest();
            }
            return Ok(usersByRole);
        }

        [HttpPost("register/pxl")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                City = model.City,
                DateAdded = DateTime.UtcNow,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                StreetName = model.StreetName,
                HouseNumber = model.HouseNumber,
                ZipCode = model.ZipCode,
                AssignedInternshipsToReview = null,
                Bus = model.Bus,
                FieldOfStudy = model.FieldOfStudy
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var role = Role.Student;
                if (user.Email.ToLower().EndsWith("@pxl.be"))
                {
                    role = Role.Teacher;
                }

                await EnsureRoleExists(role);
                await _userManager.AddToRoleAsync(user, role);

                var verifToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                verifToken = HttpUtility.UrlEncode(verifToken);

                var client = new SendGridClient(Environment.GetEnvironmentVariable("SG_API_KEY"));
                await client.SendEmailAsync(MailHelper.CreateSingleEmail(
                    new EmailAddress("sazexd@gmail.com", "PXL Stageplatform"),
                    new EmailAddress(user.Email),
                    "Verifiëer uw emailadres",
                    $"Beste gebruiker, verfiëer uw emailadres door op de volgende link te klikken: http://localhost:4200/#/verify/?t={HttpUtility.UrlEncode(verifToken)}&u={user.Id}",
                    $"Beste gebruiker,<br><br>Verfiëer uw emailadres door op de volgende link te klikken: <a href=\"http://localhost:4200/#/verify?t={HttpUtility.UrlEncode(verifToken)}&u={user.Id}\">http://localhost:4200/#/verify?t={HttpUtility.UrlEncode(verifToken)}&u={user.Id}</a>")
                );

                return Ok();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        private async Task EnsureRoleExists(string role)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                return;
            }

            await _roleManager.CreateAsync(new Role
            {
                Name = role,
                NormalizedName = role.ToUpper()
            });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyMail([FromBody] VerifyMailModel model)
        {
            var user = await _userManager.FindByIdAsync(model.ID);

            if (user == null)
            {
                return BadRequest();
            }

            model.VerificationToken = HttpUtility.UrlDecode(model.VerificationToken);
            var res = await _userManager.ConfirmEmailAsync(user, model.VerificationToken);

            if (res.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPost("password/change")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("update/company")]
        [Authorize(Roles = Role.Company)]
        public async Task<IActionResult> UpdateAccountCompany([FromBody] CompanyUpdate model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            await _userService.UpdateCompany(await _userService.GetCompany(user), model);
            return Ok();
        }

        [HttpPost("update/student")]
        [Authorize(Roles = Role.Student)]
        public async Task<IActionResult> UpdateAccountStudent([FromBody] UserUpdate model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            await _userService.UpdateUser(user, model);
            return Ok();
        }

        [HttpPost("validate")]
        [Authorize(Roles = Role.Coordinator)]
        public async Task<IActionResult> Validate([FromBody] UserValidateDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                return BadRequest();
            }
            
            await _userService.ValidateUser(model);

            return Ok();
        }

        [HttpGet("unvalidated")]
        [Authorize(Roles = Role.Coordinator)]
        public async Task<IActionResult> GetUnvalidated()
        {
            var unvalidatedUsers = await _userService.GetUnvalidated();
            return Ok(unvalidatedUsers);
        }

        [HttpPost("register/company")]
        public async Task<IActionResult> RegisterCompany([FromBody] CompanyRegisterModel model)
        {
            var contactUser = new User
            {
                DateAdded = DateTime.UtcNow,
                UserName = model.Contact_Email,
                Email = model.Contact_Email,
                FirstName = model.Contact_Name,
                LastName = model.Contact_Surname,
                PhoneNumber = model.Contact_Number,
            };

            var companyUser = new User
            {
                DateAdded = DateTime.UtcNow,
                UserName = model.Company_Email,
                Email = model.Company_Email,
                FirstName = model.Company_Name,
                LastName = model.Company_Surname,
                PhoneNumber = model.Company_Number,
            };

            var contactResult = await _userManager.CreateAsync(contactUser, model.Password);
            var companyResult = await _userManager.CreateAsync(companyUser, model.Password);

            if (contactResult.Succeeded && companyResult.Succeeded)
            {
                var role = Role.Company;

                await EnsureRoleExists(role);
                await _userManager.AddToRoleAsync(contactUser, role);
                await _userManager.AddToRoleAsync(companyUser, role);

                var company = new Company
                {
                    ContactAccountGuid = contactUser.Id,
                    CompanyAccountGuid = companyUser.Id,
                    Name = model.CompanyName,
                    EmployeeCount = model.EmployeeCount,
                    ITEmployeeCount = model.ITEmployeeCount,
                    SupportingITEmployees = model.SupportingITEmployees,
                    Latitude1 = model.Lat1,
                    Longitude1 = model.Lng1,
                    Latitude2 = model.Lat2,
                    Longitude2 = model.Lng2,
                    ContactTitle = model.Contact_Title,
                    CompanyTitle = model.Company_Title
                };

                await _userService.CreateCompany(company);

                await _userService.AssignCompany(contactUser, company.Id);
                await _userService.AssignCompany(companyUser, company.Id);

                var verifToken = await _userManager.GenerateEmailConfirmationTokenAsync(contactUser);

                var client = new SendGridClient(Environment.GetEnvironmentVariable("SG_API_KEY"));
                await client.SendEmailAsync(MailHelper.CreateSingleEmail(
                    new EmailAddress("sazexd@gmail.com", "PXL Stageplatform"),
                    new EmailAddress(contactUser.Email),
                    "Verifiëer uw emailadres",
                    $"Beste gebruiker, verfiëer uw emailadres door op de volgende link te klikken: http://localhost:4200/#/verify/?t={HttpUtility.UrlEncode(verifToken)}&u={contactUser.Id}",
                    $"Beste gebruiker,<br><br>Verfiëer uw emailadres door op de volgende link te klikken: <a href=\"http://localhost:4200/#/verify?t={HttpUtility.UrlEncode(verifToken)}&u={contactUser.Id}\">http://localhost:4200/#/verify?t={HttpUtility.UrlEncode(verifToken)}&u={contactUser.Id}</a>")
                );

                verifToken = await _userManager.GenerateEmailConfirmationTokenAsync(companyUser);

                await client.SendEmailAsync(MailHelper.CreateSingleEmail(
                    new EmailAddress("sazexd@gmail.com", "PXL Stageplatform"),
                    new EmailAddress(companyUser.Email),
                    "Verifiëer uw emailadres",
                    $"Beste gebruiker, verfiëer uw emailadres door op de volgende link te klikken: http://localhost:4200/#/verify/?t={HttpUtility.UrlEncode(verifToken)}&u={companyUser.Id}",
                    $"Beste gebruiker,<br><br>Verfiëer uw emailadres door op de volgende link te klikken: <a href=\"http://localhost:4200/#/verify?t={HttpUtility.UrlEncode(verifToken)}&u={companyUser.Id}\">http://localhost:4200/#/verify?t={HttpUtility.UrlEncode(verifToken)}&u={companyUser.Id}</a>")
                );

                return Ok();
            }

            foreach (var error in contactResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }



        [HttpPost(("register/pxl/csv"))]

        // field of study = null => teacher
        public async Task<IActionResult> RegisterCSV(IFormFile file)
        {
            TextReader reader = new StreamReader(file.OpenReadStream());

            var csvReader = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture, false);

            var records = csvReader.GetRecords<CsvRegisterModel>();

            foreach (var record in records)
            {
                User user = new User
                {
                    City = record.City,
                    DateAdded = DateTime.UtcNow,
                    Email = record.Email,
                    FirstName = record.FirstName,
                    HouseNumber = record.HouseNumber,
                    Id = Guid.NewGuid(),
                    LastName = record.LastName,
                    StreetName = record.StreetName,
                    PhoneNumber = record.PhoneNumber,
                    UserName = record.Email,
                    ZipCode = record.ZipCode,
                    Bus = record.Bus,
                    FieldOfStudy = record.FieldOfStudy,
                    IsValidated = true,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, record.Password);
                if (result.Succeeded)
                {
                    var role = Role.Student;
                    if (record.FieldOfStudy == "")
                    {
                        role = Role.Teacher;
                    }

                    await EnsureRoleExists(role);
                    await _userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) != PasswordVerificationResult.Success)
            {
                return Unauthorized();
            }

            if (!user.IsValidated)
            {
                return Unauthorized();
            }

            string token = await CreateJwtToken(user);
            var data = new { Token = token, User = user, Role = await _userService.GetRoleName(user.Id) };
            return Ok(data);
        }

        private async Task<string> CreateJwtToken(User user)
        {
            var claims = (await _userManager.GetClaimsAsync(user)).ToList();
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            });

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var keyBytes = Encoding.UTF8.GetBytes(_tokenSettings.Value.Key);
            var symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _tokenSettings.Value.Issuer,
                _tokenSettings.Value.Audience,
                claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(_tokenSettings.Value.ExpirationTimeInMinutes)
            );

            var encryptedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encryptedToken;
        }
    }
}
