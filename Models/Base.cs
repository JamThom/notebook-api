namespace Domain.Base
{
    public abstract class BaseResponse
    {
        public bool NotFound { get; set; }
        public List<IssueField> Issues { get; set; } = new List<IssueField>();
    }
    public class IssueField
    {
        public string Key { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}