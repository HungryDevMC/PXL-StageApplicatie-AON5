using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StageAPI.Models;
using StageAPI.Services;

namespace StageAPI.Controllers
{
    public class AuthController : ControllerTemplate
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await AccountService.Login(model.Email, model.Password);

                if (response == null)
                {
                    ModelState.AddModelError("LoginFailed", "Verkeerde email/wachtwoord.");
                }
                else
                {
                    return Ok(response);
                }
            }

            return BadRequest(ModelState);
        }
    }
}