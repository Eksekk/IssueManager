namespace IssueManager.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; } // TODO: in future change to real user reference
        public string Content { get; set; }
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
