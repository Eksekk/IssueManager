using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IssueManager.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        [Required]
        public string Author { get; set; } // TODO: in future change to real user reference
        [Required]
        [StringLength(maximumLength: 1000, MinimumLength = 3)]
        public string Content { get; set; }
        [DisplayName("Submit date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Required]
        [Past]
        public DateTime SubmitDate { get; set; }
        public Issue Issue { get; set; }

        public Comment() { }

        public Comment(string author, string content, DateTime submitDate)
        {
            Author = author;
            Content = content;
            SubmitDate = submitDate;
        }
    }
}
