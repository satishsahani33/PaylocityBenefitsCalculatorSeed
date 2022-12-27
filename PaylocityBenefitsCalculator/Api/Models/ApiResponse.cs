using System.Text.Json;

namespace Api.Models
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public ApiResponse() 
        { 
        }
        public ApiResponse(string errorMessage)
        {
            this.Error = errorMessage;
        }
    }
}
