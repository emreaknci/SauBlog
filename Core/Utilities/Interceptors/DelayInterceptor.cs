using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Utilities.Interceptors;

public class DelayInterceptor
{
    private readonly RequestDelegate _next;

    public DelayInterceptor(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await Task.Delay(200);
        await _next.Invoke(context);
    }
}