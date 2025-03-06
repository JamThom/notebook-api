using System.Collections.Generic;

namespace Domain.Base {
    public abstract class BaseListResponse<T>: BaseResponse
    {
        public List<T> Items { get; set; } = new List<T>();
    }
}