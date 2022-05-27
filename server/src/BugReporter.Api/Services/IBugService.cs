using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugReporter.Api.Forms;
using BugReporter.Api.Models;
using BugReporter.Api.Utils;
using BugReporter.Api.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BugReporter.Api.Services
{
    public interface IBugService
    {
        Task<IEnumerable<BugViewModel>> Query();
        Task<Option<BugViewModel>> Find(Guid id);
        Task<BugViewModel> Create(CreateBugForm form);
        Task<Option<BugViewModel>> Update(Guid id, UpdateBugForm form);

        Task<Option<BugViewModel>> AddComment(Guid bugId, AddCommentForm form);
    }
    
    public class BugService : IBugService
    {
        private readonly BugReporterDbContext _dbContext;

        public BugService(BugReporterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IQueryable<Bug> DbBugs => _dbContext.Bugs
            .Include(bug => bug
                .Comments
                .OrderBy(x => x.CommentedAt))
            .AsQueryable();

        public async Task<IEnumerable<BugViewModel>> Query()
        {
            var bugs = await _dbContext
                .Bugs
                .OrderByDescending(x => x.ReportedAt)
                .ToListAsync();

            return bugs.Select(BugViewModel.FromData);
        }

        public async Task<Option<BugViewModel>> Find(Guid id)
        {
            if (await DbBugs.SingleOrDefaultAsync(x => x.Id == id) is { } bug)
            {
                return new Option<BugViewModel>.Some(BugViewModel.FromData(bug));
            }

            return new Option<BugViewModel>.None();
        }

        public async Task<BugViewModel> Create(CreateBugForm form)
        {
            var bug = new Bug
            {
                Title = form.Title,
                Description = form.Description,
                ReportedBy = form.ReportedBy,
                ReportedAt = DateTime.UtcNow
            };

            await _dbContext.Bugs.AddAsync(bug);
            await _dbContext.SaveChangesAsync();
            
            return BugViewModel.FromData(bug);
        }

        public async Task<Option<BugViewModel>> Update(Guid id, UpdateBugForm form)
        {
            if (await DbBugs.SingleOrDefaultAsync(x => x.Id == id) is { } bug)
            {
                var hasChanged = false;

                if (bug.Title != form.Title)
                {
                    bug.Title = form.Title;
                    hasChanged = true;
                }

                if (bug.Description != form.Description)
                {
                    bug.Description = form.Description;
                    hasChanged = true;
                }

                if (bug.State != form.State && form.State.HasValue)
                {
                    bug.State = form.State.Value;
                    hasChanged = true;
                }

                if (hasChanged)
                {
                    await _dbContext.SaveChangesAsync();
                }

                return new Option<BugViewModel>.Some(BugViewModel.FromData(bug));
            }

            return new Option<BugViewModel>.None();
        }

        public Task<Option<BugViewModel>> AddComment(Guid bugId, AddCommentForm form)
        {
            // TODO - implement this method.
            throw new NotImplementedException();
        }
    }
}