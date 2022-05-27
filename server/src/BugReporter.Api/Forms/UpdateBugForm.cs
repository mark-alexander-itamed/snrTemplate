using System.ComponentModel.DataAnnotations;
using BugReporter.Api.Models;

namespace BugReporter.Api.Forms
{
    public class UpdateBugForm
    {
        [Required] 
        [MaxLength(256)]
        public string Title { get; set; }
        
        [Required] 
        [MaxLength(2048)]
        public string Description { get; set; }
        
        [Required] 
        public BugState? State { get; set; }
    }
}