namespace Notebook.Models.Domain
{
    public class FeatureResult<T> : Dictionary<string, T>
    {
        public ErrorType? Error;
        public string? ErrorMessage;
        public T? Response;
    }
}
