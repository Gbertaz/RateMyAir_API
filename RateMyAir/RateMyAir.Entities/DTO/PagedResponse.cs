using System;
using System.Collections.Generic;

namespace RateMyAir.Entities.DTO
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, string message = null) : base(message)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Success = true;
            Errors = new List<object>();
        }
    }
}
