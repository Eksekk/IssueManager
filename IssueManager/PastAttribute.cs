using System.ComponentModel.DataAnnotations;

namespace IssueManager
{
    public class PastAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            return ((System.DateTime)value) < System.DateTime.Now;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }
}
