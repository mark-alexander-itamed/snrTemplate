using System;
using System.Collections.Generic;
using System.Linq;
using BugReporter.Api.Models;

namespace BugReporter.Api.ViewModels
{
    public class BugViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReportedBy { get; set; }
        public DateTime ReportedAt { get; set; }
        public string State { get; set; }
        public List<BugCommentViewModel> Comments { get; set; }

        public static BugViewModel FromData(Bug bug)
        {
            return new BugViewModel
            {
                Id = bug.Id,
                Title = bug.Title,
                Description = bug.Description,
                ReportedBy = bug.ReportedBy,
                ReportedAt = bug.ReportedAt.ToLocalTime(),
                State = bug.State.ToString("G"),
                Comments = bug.Comments.Select(BugCommentViewModel.FromData).ToList()
            };
        }
    }
}