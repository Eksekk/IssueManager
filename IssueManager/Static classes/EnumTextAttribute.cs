using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.StaticClasses
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumTextAttribute : Attribute
    {
        public string Text { get; }

        public EnumTextAttribute(string text)
        {
            Text = text;
        }
    }
}
