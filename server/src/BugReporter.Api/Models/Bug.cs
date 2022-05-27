using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BugReporter.Api.Models
{
    public class Bug
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReportedBy { get; set; }
        public BugState State { get; set; }
        public DateTime ReportedAt { get; set; }

        public List<BugComment> Comments { get; } = new List<BugComment>();
    }

    public class BugConfiguration : IEntityTypeConfiguration<Bug>
    {
        public void Configure(EntityTypeBuilder<Bug> builder)
        {
            builder.HasKey(x => x.Id);
            builder
                .Property(x => x.Title)
                .HasMaxLength(256);

            builder
                .Property(x => x.Description)
                .HasMaxLength(2048);

            builder.Property(x => x.ReportedBy)
                .HasMaxLength(128);
        }
    }
}