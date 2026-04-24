
using System.Net;

namespace Domain.Models
{
    public class ResponseModel<T>
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public string Error { get; set; }

        public T Result { get; set; }

        public ResponseModel(T result,HttpStatusCode statusCode = HttpStatusCode.OK) 
        {
            StatusCode = statusCode;
            Result = result;        
        }

        public ResponseModel(string error, HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            Error = error;
        }

        public ResponseModel(string error,T result, HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            Result = result;
            Error = error;
        }
    }
}
