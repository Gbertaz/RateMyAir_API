using System;
using System.Collections.Generic;

namespace RateMyAir.Entities.DTO
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int RecordsTotal { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int recordsTotal, string message = null) : base(message)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.RecordsTotal = recordsTotal;
            this.Data = data;
            this.Success = true;
            Errors = new List<object>();
        }
    }

}
