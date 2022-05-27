using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BugReporter.Api.Forms;
using BugReporter.Api.Services;
using BugReporter.Api.Utils;
using BugReporter.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BugReporter.Api.Controllers
{
    [ApiController]
    [Route("api/bug")]
    public class BugController : ControllerBase
    {
        private readonly IBugService _bugService;

        public BugController(IBugService bugService)
        {
            _bugService = bugService;
        }

        [HttpGet]
        public async Task<IActionResult> Query()
        {
            return Ok(await _bugService.Query());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Find(Guid id)
        {
            return await _bugService.Find(id) switch
            {
                Option<BugViewModel>.Some(var bug) => Ok(bug),
                Option<BugViewModel>.None => NotFound(),
                _ => throw new InvalidOperationException()
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBugForm form)
        {
            var result = await _bugService.Create(form);
            return CreatedAtAction("Find", new {id = result.Id}, result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBugForm form)
        {
            return await _bugService.Update(id, form) switch
            {
                Option<BugViewModel>.Some(var bug) => Ok(bug),
                Option<BugViewModel>.None => NotFound(),
                _ => throw new InvalidOperationException()
            };
        }

        [HttpPost("{bugId}/comment")]
        public async Task<IActionResult> AddComment(Guid bugId, [FromBody] AddCommentForm form)
        {
            return await _bugService.AddComment(bugId, form) switch
            {
                Option<BugViewModel>.Some(var bug) => Ok(bug),
                Option<BugViewModel>.None => NotFound(),
                _ => throw new InvalidOperationException()
            };
        }
    }
}