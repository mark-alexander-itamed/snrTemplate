using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BugReporter.Api.Models
{
    public class BugComment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CommentedAt { get; set; }
        public Bug Bug { get; set; }
    }

    public class BugCommentConfiguration : IEntityTypeConfiguration<BugComment>
    {
        public void Configure(EntityTypeBuilder<BugComment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Text)
                .HasMaxLength(2048);
            builder.HasOne(x => x.Bug)
                .WithMany(x => x.Comments)
                .HasForeignKey("BugId");
        }
    }
}