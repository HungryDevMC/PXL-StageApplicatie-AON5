using Microsoft.AspNetCore.Mvc;
using StageAPI.Data;
using StageAPI.Services;
using System;
using System.Threading.Tasks;
using System.Linq;
using StageAPI.Models;

namespace StageAPI.Controllers
{
    public class ProposalController : ControllerTemplate
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            // TODO: Authentication

            var list = await ProposalService.GetAll();

            return Ok(list);
        }

        [HttpGet("inreview")]
        public async Task<IActionResult> GetInReview()
        {
            // TODO: Authentication

            var inReviewList = (await ProposalService.GetAll()).Where(x => x.State != ProposalState.Approved);

            return Ok(inReviewList);
        }

        [HttpGet("approved")]
        public async Task<IActionResult> GetApproved()
        {
            // TODO: Authentication

            var approvedList = (await ProposalService.GetAll()).Where(x => x.State == ProposalState.Approved);

            return Ok(approvedList);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateProposalModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await ProposalService.Exists(model.Description))
                {
                    // This should be something 
                    var isOk = await ProposalService.Create(model);
                    return Ok(isOk);
                }

                ModelState.AddModelError("Status", "Er bestaat al een proposal met deze naam.");
            }

            return BadRequest(ModelState);
        }
    }
}