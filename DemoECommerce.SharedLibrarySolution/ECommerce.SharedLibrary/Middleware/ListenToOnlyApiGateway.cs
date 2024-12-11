using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.SharedLibrary.Middleware
{
    public class ListenToOnlyApiGateway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //Extract specifc header from the request
            var signedHeader = context.Request.Headers["Api-Gateway"];

            //NULL means, the request is not coming from the Api Gateway // 503 service unavaliable
            if (signedHeader.FirstOrDefault() != null) 
            {
                context.Response.StatusCode = (int)StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry service is unavaliable");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
