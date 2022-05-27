using System;
using BugReporter.Api.Models;

namespace BugReporter.Api.ViewModels
{
    public class BugCommentViewModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
        public DateTime CommentedAt { get; set; }

        public static BugCommentViewModel FromData(BugComment comment)
        {
            return new BugCommentViewModel
            {
                Id = comment.Id,
                Text = comment.Text,
                CommentedAt = comment.CommentedAt.ToLocalTime()
            };
        }
    }
}