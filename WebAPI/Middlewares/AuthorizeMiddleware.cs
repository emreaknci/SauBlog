using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using Core.Utilities.Results;
using IResult = Core.Utilities.Results.IResult;

namespace WebAPI.Middlewares
{
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authorizeAttribute = context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>();
            if (authorizeAttribute != null)
            {
                var roles = authorizeAttribute.Roles?.Split(',').ToList();
                if (!roles.IsNullOrEmpty() && roles.Contains("Admin") && context.User.IsInRole("Admin"))
                {
                    await _next(context);
                    return;
                }


                var endpoint = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
                var userId = Convert.ToInt32(context.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var writerService = context.RequestServices.GetService<IWriterService>();
                var writer = writerService!.GetByUserId(userId).Result.Data;


                Func<int, int, Task<IResult>>? action = endpoint.ControllerName switch
                {
                    "Blogs" => writerService.DoesBlogBelongToThisWriter,
                    "Comments" => writerService.DoesCommentBelongToThisWriter,
                    "Users" => (currentUserid, paramId) =>
                    {
                        IResult result = currentUserid == paramId ? new SuccessResult() : new ErrorResult();
                        return Task.FromResult(result);
                    }
                    ,
                    "Auth" => (currentUserid, paramId) =>
                    {
                        IResult result = currentUserid == paramId ? new SuccessResult() : new ErrorResult();
                        return Task.FromResult(result);
                    }
                    ,
                    _ => null
                };

                if (action is not null)
                {
                    if (context.Request.ContentType == "application/json")
                    {
                        using var jsonDocument = await JsonDocument.ParseAsync(context.Request.Body);
                        if (jsonDocument.RootElement.TryGetProperty("id", out var idValue) && idValue.ValueKind == JsonValueKind.Number && idValue.TryGetInt32(out var id))
                        {
                            var result = await action(id, writer!.Id);
                            if (!result.Success)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                return;
                            }
                        }
                    }
                    else if (context.Request.Query.TryGetValue("id", out var idValue) && int.TryParse(idValue, out var id))
                    {
                        var result = await action(id, writer!.Id);
                        if (!result.Success)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            return;
                        }
                    }
                }
            }


            await _next(context);
        }
    }
}
