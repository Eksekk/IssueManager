using IssueManager.StaticClasses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text.Json.Serialization;

namespace IssueManager.Models
{
    public enum IssueStatus
    {
        [EnumText("Submitted")]
        SUBMITTED,
        [EnumText("Acknowledged")]
        ACKNOWLEDGED,
        [EnumText("Work in progress")]
        WORK_IN_PROGRESS,
        [EnumText("Cannot reproduce")]
        CANNOT_REPRODUCE,
        [EnumText("Reproduced")]
        REPRODUCED,
        [EnumText("Won't fix")]
        WONT_FIX,
        [EnumText("Closed")]
        CLOSED,
        [EnumText("Fixed")]
        FIXED
    }
    public class Issue
    {
        public int Id { get; set; }
        [DisplayName("Issue Title")] // so that comments view has proper label, don't want to somehow hardcode it in view
        [StringLength(maximumLength: 60, MinimumLength = 3)]
        public string Title { get; set; }
        [StringLength(maximumLength: 2000, MinimumLength = 3)]
        public string Description { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string? Author { get; set; }
        [DisplayName("Submit date")]
        [Past]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? SubmitDate { get; set; }
        [DisplayName("Close date")]
        [Past]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? CloseDate { get; set; }
        [DisplayName("Last update date")]
        [Past]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? LastUpdateDate { get; set; }
        [Required]
        public IssueStatus Status { get; set; } // TODO: make non-nullable
        public Project project { get; set; }

        // FIXME: Assignee field as real user!!!

        private List<Comment> _Comments;
        public List<Comment> Comments
        {
            get
            {
                return _Comments;
            }
            set
            {
                _Comments = value;
                foreach (Comment c in value)
                {
                    c.Issue = this;
                }
            }
        }

        public static string getIssueStatusEnumText(IssueStatus status)
        {
            return EnumHelpers.GetValueName(status);
        }

        public static List<SelectListItem> getStatusEnumValuesSelectList()
        {
            return EnumHelpers.GetValuesWithNames<IssueStatus>().Select(pair => new SelectListItem(pair.Value, pair.Key.ToString())).ToList();
        }

        public static List<SelectListItem> getStatusEnumValuesSelectListWithPlaceholder(string text)
        {
            var list = getStatusEnumValuesSelectList();
            list.Prepend(new SelectListItem(text, "", true, true));
            return list;
        }

        // this simple method is there just for abstraction, so views don't have to directly use EnumHelpers method
        public static Dictionary<IssueStatus, string> getStatusEnumValuesDictionary()
        {
            return EnumHelpers.GetValuesWithNames<IssueStatus>();
        }
    }
}
