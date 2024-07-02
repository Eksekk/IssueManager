using Microsoft.Identity.Client;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IssueManager.Models
{
    public class Project
    {
        public int Id { get; set; }
        [DisplayName("Project Name")]
        [StringLength(maximumLength: 70, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        [StringLength(maximumLength: 2000, MinimumLength = 3)]
        [Required]
        public string Description { get; set; }
        private List<Issue> _Issues;
        public List<Issue> Issues
        {
            get
            {
                return _Issues;
            }
            set
            {
                _Issues = value;
                foreach (Issue i in value)
                {
                    i.project = this;
                }
            }
        }
    }
}
