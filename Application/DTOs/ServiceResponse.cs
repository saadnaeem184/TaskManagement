using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// A generic service response to indicate success/failure and carry messages.
    /// </summary>
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }

        public ServiceResponse(T data, bool success = true, string message = "", List<string>? errors = null)
        {
            Data = data;
            Success = success;
            Message = message;
            Errors = errors;
        }

        public ServiceResponse(bool success, string message, List<string>? errors = null)
        {
            Success = success;
            Message = message;
            Errors = errors;
        }
    }
}
