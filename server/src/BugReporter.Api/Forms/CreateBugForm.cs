using System.ComponentModel.DataAnnotations;

namespace BugReporter.Api.Forms
{
    public class CreateBugForm
    {
        [MaxLength(256)]
        public string Title { get; set; }
        
        [MaxLength(2048)]
        public string Description { get; set; }
        
        [MaxLength(128)]
        public string ReportedBy { get; set; }
    }
}