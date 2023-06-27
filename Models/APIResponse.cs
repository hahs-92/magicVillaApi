using System.Net;

namespace MagicVilla_API.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSucceeded { get; set; } = true;
        public List<string>? ErrorsMessage { get; set; }
        public object? Result { get; set; }
    }
}
