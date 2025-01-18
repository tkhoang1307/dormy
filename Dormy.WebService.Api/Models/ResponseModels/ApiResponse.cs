using System.Net;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; private set; }
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }
        public object Result { get; private set; }

        public ApiResponse SetOk(object result = null)
        {
            IsSuccess = true;
            StatusCode = HttpStatusCode.OK;
            Result = result;
            return this;
        }

        public ApiResponse SetNotFound(object result = null, string message = null)
        {
            IsSuccess = false;
            StatusCode = HttpStatusCode.NotFound;
            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = message;
            }
            Result = result;
            return this;
        }

        public ApiResponse SetBadRequest(object result = null, string message = null)
        {
            IsSuccess = false;
            StatusCode = HttpStatusCode.BadRequest;
            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = message;
            }
            Result = result;
            return this;
        }

        public ApiResponse SetCreated(object result = null)
        {
            IsSuccess = true;
            StatusCode = HttpStatusCode.Created;
            Result = result;
            return this;
        }

        public ApiResponse SetAccepted(object result = null)
        {
            IsSuccess = true;
            StatusCode = HttpStatusCode.Accepted;
            Result = result;
            return this;
        }

        public ApiResponse SetConflict(object result = null, string message = null)
        {
            IsSuccess = false;
            StatusCode = HttpStatusCode.Conflict;
            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = message;
            }
            Result = result;
            return this;
        }

        public ApiResponse SetForbidden(object result = null, string message = null)
        {
            IsSuccess = false;
            StatusCode = HttpStatusCode.Forbidden;
            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = message;
            }
            Result = result;
            return this;
        }

        public ApiResponse SetPreconditionFailed(object result = null, string message = null)
        {
            IsSuccess = false;
            StatusCode = HttpStatusCode.PreconditionFailed;
            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = message;
            }
            Result = result;
            return this;
        }

        public ApiResponse SetUnprocessableEntity(object result = null, string message = null)
        {
            IsSuccess = false;
            StatusCode = HttpStatusCode.UnprocessableEntity;
            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = message;
            }
            Result = result;
            return this;
        }

        public ApiResponse SetApiResponse(HttpStatusCode statusCode, bool isSuccess, string message = null, object result = null)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = message;
            }
            Result = result;
            return this;
        }
    }
}
