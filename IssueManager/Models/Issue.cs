﻿using IssueManager.StaticClasses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text.Json.Serialization;

namespace IssueManager.Models
{
//     [JsonConverter(typeof(JsonStringEnumConverter))]
//     public enum IssueLanguage
//     {
//         All,
//         [EnumText("C#")]
//         Csharp,
//         [EnumText("C++")]
//         Cpp,
//         Lua,
//         Python,
//         Java
//     }
//     [JsonConverter(typeof(JsonStringEnumConverter))]
//     public enum SnippetComplexity
//     {
//         Any,
//         Low,
//         [EnumText("Medium-low")]
//         MediumLow,
//         Medium,
//         [EnumText("Medium-high")]
//         MediumHigh,
//         High
//     }

    public enum IssueStatus
    {
        [EnumText("Submitted")]
        SUBMITTED,
        [EnumText("Acknowledged")]
        ACKNOWLEDGED,
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
        public string Title { get; set; }
        public string Description { get; set; }
        [DisplayName("Code snippet")]
        public string? CodeSnippet { get; set; }
        public string? Author { get; set; }
        [DisplayName("Submit date")]
        public DateTime? SubmitDate { get; set; }
        [DisplayName("Close date")]
        public DateTime? CloseDate { get; set; }
        [DisplayName("Last update date")]
        public DateTime? LastUpdateDate { get; set; }
        public IssueStatus Status { get; set; } // TODO: make non-nullable
        public Project project { get; set; }

        public List<Comment> Comments { get; set; }

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
