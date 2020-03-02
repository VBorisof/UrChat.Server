using System.Net;
using UrChat.Extensions;

namespace UrChat.Services.Shared
{
    public class ServiceOperationResult
    {
        public bool IsSuccessful => StatusCode.IsSuccessful();
        public HttpStatusCode StatusCode { get; set; }
        public string Errors { get; set; }

        public static ServiceOperationResult Ok() => new ServiceOperationResult()
        {
            StatusCode = HttpStatusCode.OK
        };

        public static ServiceOperationResult<TData> Ok<TData>(TData data) => new ServiceOperationResult<TData>()
        {
            StatusCode = HttpStatusCode.OK,
            Data = data
        };


        public static ServiceOperationResult Accepted() => new ServiceOperationResult()
        {
            StatusCode = HttpStatusCode.Accepted
        };

        public static ServiceOperationResult<TData> Accepted<TData>(TData data) => new ServiceOperationResult<TData>()
        {
            StatusCode = HttpStatusCode.Accepted,
            Data = data
        };


        public static ServiceOperationResult BadRequest(string errors) => new ServiceOperationResult()
        {
            StatusCode = HttpStatusCode.BadRequest,
            Errors = errors
        };

        public static ServiceOperationResult<TData> BadRequest<TData>(string errors) =>
            new ServiceOperationResult<TData>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Errors = errors
            };


        public static ServiceOperationResult Unauthorized() => new ServiceOperationResult()
        {
            StatusCode = HttpStatusCode.Unauthorized,
        };

        public static ServiceOperationResult<TData> Unauthorized<TData>() => new ServiceOperationResult<TData>()
        {
            StatusCode = HttpStatusCode.Unauthorized,
        };


        public static ServiceOperationResult NotFound(string errors) => new ServiceOperationResult()
        {
            StatusCode = HttpStatusCode.NotFound,
            Errors = errors
        };

        public static ServiceOperationResult<TData> NotFound<TData>(string errors) =>
            new ServiceOperationResult<TData>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Errors = errors
            };


        public static ServiceOperationResult Conflict(string errors) => new ServiceOperationResult()
        {
            StatusCode = HttpStatusCode.Conflict,
            Errors = errors
        };

        public static ServiceOperationResult<TData> Conflict<TData>(string errors) =>
            new ServiceOperationResult<TData>()
            {
                StatusCode = HttpStatusCode.Conflict,
                Errors = errors
            };


        public static ServiceOperationResult UnprocessableEntity(string errors) => new ServiceOperationResult()
        {
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Errors = errors
        };

        public static ServiceOperationResult<TData> UnprocessableEntity<TData>(string errors) =>
            new ServiceOperationResult<TData>()
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Errors = errors
            };

        public static ServiceOperationResult NotImplemented(string errors) => new ServiceOperationResult()
        {
            StatusCode = HttpStatusCode.NotImplemented,
            Errors = errors
        };

        public static ServiceOperationResult<TData> NotImplemented<TData>(string errors) =>
            new ServiceOperationResult<TData>()
            {
                StatusCode = HttpStatusCode.NotImplemented,
                Errors = errors
            };

        public static ServiceOperationResult InvalidPassword(string errors) => new ServiceOperationResult()
        {
            StatusCode = (HttpStatusCode) HttpStatusCodeExtras.InvalidPassword,
            Errors = errors
        };

        public static ServiceOperationResult<TData> InvalidPassword<TData>(string errors) =>
            new ServiceOperationResult<TData>()
            {
                StatusCode = (HttpStatusCode) HttpStatusCodeExtras.InvalidPassword,
                Errors = errors
            }; 
        
        //
        // In case you will need to provide a custom API return code, add an entry to Extensions.HttpStatusCodeExtras
        // and cast make an object like above and fill the status code as:
        //
        //   StatusCode = (HttpStatusCode) HttpStatusCodeExtras.MyCustomStatus
        // 
    }

    public class ServiceOperationResult<TData> : ServiceOperationResult
    {
        public TData Data { get; set; }

        public static ServiceOperationResult<TData> FromDataless(ServiceOperationResult serviceOperationResult) =>
            new ServiceOperationResult<TData>
            {
                Errors = serviceOperationResult.Errors,
                StatusCode = serviceOperationResult.StatusCode
            };
    }
}