﻿using Microsoft.Identity.Client;

namespace IssueManager.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Issue> Issues { get; set; }
    }
}
