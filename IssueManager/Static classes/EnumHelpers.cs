using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.StaticClasses
{
    public static class EnumHelpers
    {
        public static Dictionary<T, string> GetValuesWithNames<T>() where T : Enum
        {
            Dictionary<T, string> values = new();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                var memberInfo = typeof(T).GetMember(value.ToString());
                EnumTextAttribute? attr = memberInfo[0].GetCustomAttribute<EnumTextAttribute>();
                if (attr is not null)
                {
                    values.Add(value, attr.Text);
                }
                else
                {
                    values.Add(value, value.ToString());
                }
            }
            return values;
        }

        public static string GetValueName<T>(T value) where T: Enum
        {
            var memberInfo = value.GetType().GetMember(value.ToString());
            EnumTextAttribute? attr = memberInfo[0].GetCustomAttribute<EnumTextAttribute>();
            if (attr is not null)
            {
                return attr.Text;
            }
            return value.ToString();
        }
    }
}
