using Microsoft.AspNetCore.Http;

namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public class NotModifiedByEtagMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;
        var ifNoneTag = request.Headers.IfNoneMatch;

        if (
            string.IsNullOrEmpty(ifNoneTag)
            || (request.Method != HttpMethods.Get && request.Method != HttpMethods.Head)
        )
        {
            await next(context);
            return;
        }

        var response = context.Response;
        var originalStream = response.Body;

        using var ms = new MemoryStream();
        response.Body = ms;

        await next(context);
        var responseEtag = response.Headers.ETag;

        if (responseEtag == ifNoneTag)
        {
            response.Body = originalStream;
            response.StatusCode = StatusCodes.Status304NotModified;
            return;
        }

        ms.Position = 0;
        await ms.CopyToAsync(originalStream);
        return;
    }
}
