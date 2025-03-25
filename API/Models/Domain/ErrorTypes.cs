namespace Notebook.Models.Domain
{
    public enum ErrorType
    {
        None,
        DuplicateName,
        NameEmpty,
        InvalidRequest,
        NotFound,
        Unauthorized,
        Unknown,
        NoName,
        NoEmail
    }
}