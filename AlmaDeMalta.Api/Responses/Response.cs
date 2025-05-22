using System.Net;

namespace AlmaDeMalta.api.Responses;

public record Response(object Body, HttpStatusCode Status, string Message)
{
    public static Response Success(object body, string message = "", HttpStatusCode status = HttpStatusCode.OK)
        => new(body, status, message);

    public static Response Error(string message, HttpStatusCode status = HttpStatusCode.BadRequest)
        => new(null!, status, message);

    public static Response NotFound(string message = "Resource not found")
        => Error(message, HttpStatusCode.NotFound);

    public static Response ServerError(string message = "Internal server error")
        => Error(message, HttpStatusCode.InternalServerError);

    public bool IsSuccess => (int)Status >= 200 && (int)Status < 300;
}
//public record Response(object Body, HttpStatusCode Status, string SuccessMessage, string ErrorMessage)
//{
//    public class Builder
//    {
//        private object _body = default!;
//        private HttpStatusCode _status = HttpStatusCode.OK;
//        private string _successMessage = string.Empty;
//        private string _errorMessage = string.Empty;

//        public Builder WithBody(object body)
//        {
//            _body = body;
//            return this;
//        }

//        public Builder WithStatus(HttpStatusCode status)
//        {
//            _status = status;
//            return this;
//        }

//        public Builder WithSuccessMessage(string message)
//        {
//            _successMessage = message;
//            return this;
//        }

//        public Builder WithErrorMessage(string message)
//        {
//            _errorMessage = message;
//            return this;
//        }

//        public Builder AsSuccess(string message = "")
//        {
//            _status = HttpStatusCode.OK;
//            _successMessage = message;
//            _errorMessage = string.Empty;
//            return this;
//        }

//        public Builder AsError(string message = "", HttpStatusCode status = HttpStatusCode.BadRequest)
//        {
//            _status = status;
//            _errorMessage = message;
//            _successMessage = string.Empty;
//            return this;
//        }

//        public Builder AsNotFound(string message = "Resource not found")
//        {
//            return AsError(message, HttpStatusCode.NotFound);
//        }

//        public Builder AsServerError(string message = "Internal server error")
//        {
//            return AsError(message, HttpStatusCode.InternalServerError);
//        }

//        public Response Build()
//        {
//            return new Response(_body, _status, _successMessage, _errorMessage);
//        }
//    }
//    public static Builder CreateBuilder()
//    {
//        return new Builder();
//    }

//    public bool IsSuccess => (int)Status >= 200 && (int)Status < 300;

//    public bool IsError => !IsSuccess;
//}
