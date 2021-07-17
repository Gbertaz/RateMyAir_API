using System;
using System.Collections.Generic;

namespace RateMyAir.Entities.DTO
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<object> Errors { get; set; }
        public T Data { get; set; }

        public Response()
        {
            Message = string.Empty;
            Errors = new List<object>();
        }

        public Response(T data, string message = null)
        {
            Success = true;
            Message = string.IsNullOrWhiteSpace(message) ? string.Empty : message;
            Data = data;
            Errors = new List<object>();
        }

        public Response(string message)
        {
            Success = false;
            Message = string.IsNullOrWhiteSpace(message) ? string.Empty : message;
            Errors = new List<object>();
        }

    }
}
