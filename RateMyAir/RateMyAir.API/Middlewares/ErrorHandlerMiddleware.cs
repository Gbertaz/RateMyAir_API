using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RateMyAir.Entities.DTO;
using RateMyAir.Entities.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RateMyAir.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Success = false, Message = error?.Message };

                switch (error)
                {
                    case UnauthorizedException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        response.Headers.Add("WWW-Authenticate", "ApyKey=\"invalid_token\"");
                        responseModel.Success = true;
                        break;
                    case ForbiddenException e:
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        responseModel.Success = true;
                        break;
                    case ApiException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Success = false;
                        break;
                    case NotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.Success = true;
                        break;
                    case BadRequestException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Success = true;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Success = false;
                        break;
                }

                var result = JsonConvert.SerializeObject(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
