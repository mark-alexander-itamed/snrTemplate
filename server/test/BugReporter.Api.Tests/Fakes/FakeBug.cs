using System;
using Bogus;
using BugReporter.Api.Models;

namespace BugReporter.Api.Tests.Fakes
{
    public sealed class FakeBug : Faker<Bug>
    {
        public static readonly FakeBug I = new FakeBug();
        public FakeBug()
        {
            RuleFor(b => b.Id, () => Guid.NewGuid());
            RuleFor(b => b.Title, f => f.Lorem.Sentence());
            RuleFor(b => b.Description, f => f.Lorem.Paragraphs());
            RuleFor(b => b.State, f => f.Random.Enum<BugState>());
            RuleFor(b => b.ReportedAt, f => f.Date.Recent());
            RuleFor(b => b.ReportedBy, f => f.Name.FullName());
        }
    }
}