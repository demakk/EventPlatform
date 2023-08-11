using System.Web.Http.Filters;
using EventPlatform.Domain.Common;
using EventPlatform.WebApi.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IExceptionFilter = Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter;

namespace EventPlatform.WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var error = new ApiError
        {
            Code = "500",
            ErrorMessage = context.Exception.Message
        };

        context.Result = new JsonResult(error) {StatusCode = 500};
    }
}