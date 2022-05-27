using System.ComponentModel.DataAnnotations;

namespace BugReporter.Api.Forms
{
    public class AddCommentForm
    {
        [Required]
        [MaxLength(2048)]
        public string Text { get; set; }
    }
}