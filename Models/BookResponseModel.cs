using Domain.Base;

namespace Notebook.Models
{
    public class BookResponseModel : BaseResponse
    {
        required public string Id { get; set; }
        required public string Name { get; set; }
        required public ISet<PageResponseModel> Pages { get; set; }
    }
}