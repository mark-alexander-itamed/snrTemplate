using System;
using Bogus;
using BugReporter.Api.Models;

namespace BugReporter.Api.Tests.Fakes
{
    public sealed class FakeComment : Faker<BugComment>
    {
        public static readonly FakeComment I = new FakeComment();
        public FakeComment()
        {
            RuleFor(x => x.Id, () => Guid.NewGuid());
            RuleFor(x => x.Text, f => f.Lorem.Paragraph());
            RuleFor(x => x.CommentedAt, f => f.Date.Past());
        }
    }
}