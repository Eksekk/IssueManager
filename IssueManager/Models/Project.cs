using Microsoft.Identity.Client;
using System.ComponentModel;

namespace IssueManager.Models
{
    public class Project
    {
        public int Id { get; set; }
        [DisplayName("Project Name")]
        public string Name { get; set; }
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
