using Microsoft.AspNetCore.Mvc;
using Stage_API.Dto;
using System.Threading.Tasks;
using System;
using System.Linq;
using Stage_API.Business.Abstractions;
using Stage_API.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Stage_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InternshipController : ControllerBase
    {
        private readonly IInternshipService _service;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public InternshipController(IInternshipService service, IUserService userService, UserManager<User> userManager)
        {
            _service = service;
            _userService = userService;
            _userManager = userManager;
        }

        [Authorize(Roles = Role.Company)]
        [HttpPost]
        public async Task<ActionResult<Internship>> Create([FromBody] InternshipCreationDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _service.Insert(model, await _userService.GetCompany(user));
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ActionResult<ICollection<Internship>>> GetAll()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return Unauthorized();
            }

            var roleName = await _userService.GetRoleName(user.Id);

            switch(roleName.ToLower())
            {
                case Role.Company:
                    return Ok(await _service.GetAllInternshipsOfCompany((await _userService.GetCompany(user)).Id));
                case Role.Student:
                    return Ok(await _service.GetAllInternshipsForStudent());
                case Role.Teacher:
                    return Ok(await _service.GetAllInternshipsOfTeacher(user.Id));
                case Role.Coordinator:
                    return Ok(await _service.GetAll());
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet("all/{status}")]
        public async Task<ActionResult<ICollection<Internship>>> GetAllByStatus(int status)
        {
            var result = (await _service.GetAll()).Where(x => x.InternshipState == (InternshipState)status);
            return Ok(result);
        }

        [Authorize(Roles = Role.Teacher)]
        [HttpGet("assigned/teacher/{teacherId}")]
        public async Task<ActionResult<ICollection<Internship>>> GetInternshipsOfTeacher(Guid teacherId)
        {
            var result = await _service.GetAllInternshipsOfTeacher(teacherId);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{internshipId}")]
        public async Task<ActionResult<Internship>> GetByID(Guid internshipId)
        {
            var result = await _service.Get(internshipId);
            return Ok(result);
        }

        [Authorize(Roles = Role.Coordinator)]
        [HttpGet("teachers/{internshipId}")]
        public async Task<ActionResult<ICollection<User>>> GetTeachersByID(Guid internshipId)
        {
            var result = await _service.GetAllTeachersForInternship(internshipId);
            var linkedTeacherList = result?.Select(r => r.Id).ToList();
            return Ok(linkedTeacherList);
        }

        [Authorize(Roles = Role.Coordinator)]
        [HttpGet("teachers")]
        public async Task<ActionResult<ICollection<UserReadDto>>> GetTeachers()
        {
            var result = await _userService.GetUsersByRole("TEACHER");
            return Ok(result);
        }

        [Authorize(Roles = Role.Coordinator)]
        [HttpPost("teachers/assign")]
        public async Task<IActionResult> AssignTeachers([FromBody] InternshipAssign model)
        {
            if (!(await _service.AssignReviewer(model.InternshipID, model.Teachers)))
            {
                return BadRequest();
            }

            return Ok();
        }

        [Authorize(Roles = Role.Teacher)]
        [HttpPost("feedback/teacher")]
        public async Task<ActionResult<bool>> FeedbackTeacher(InternshipFeedbackTeacherDto model)
        {
            var result = await _service.FeedbackTeacher(model);
            return Ok(result);
        }

        [Authorize(Roles = Role.Coordinator)]
        [HttpPost("feedback/coordinator")]
        public async Task<ActionResult<bool>> FeedbackCoordinator(InternshipFeedbackCoordinatorDto model)
        {
            var result = await _service.FeedbackCoordinator(model);
            return Ok(result);
        }

        [Authorize(Roles = Role.Company)]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateInternship([FromBody] InternshipUpdate model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _service.Update(model, await _userService.GetCompany(user));

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [Authorize(Roles = Role.Coordinator)]
        [HttpDelete("remove/{internshipId}")]
        public async Task<IActionResult> RemoveInternship(Guid internshipId)
        {
            var internship = await _service.Get(internshipId);
            if(internship == null)
            {
                return BadRequest();
            }
            await _service.Delete(internshipId);
            return Ok();
        }

    }
}