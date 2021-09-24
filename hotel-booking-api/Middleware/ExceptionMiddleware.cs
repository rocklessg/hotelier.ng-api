using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace hotel_booking_api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = Response<string>.Fail(error.Message);
                
                switch (error)
                {
                    case BadRequest e:
                        // custom application error
                        _logger.LogError(e.StackTrace, e);
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case NotFoundException e:
                        // not found error
                        _logger.LogError(e.StackTrace, e);
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.Message = e.Message;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = "Internal Server Error. Please Try Again Later.";
                        break;
                }
                var result = JsonConvert.SerializeObject(responseModel);
                await response.WriteAsync(result);
            }
        }
    }

    public class Response<T>
    {
        public T Data {  get; set; }
        public string Message { get; set; }
        public string StatusCode {  get; set; }        
        public bool Succeeded {  get; set; }
        public static object Fail(string message)
        {
            return new Response<T>
            {
                Succeeded = false,
                Message = message
            };
        }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class NotFoundException : Exception
    {
        
    }


    public class BadRequest : Exception
    {
    }
}