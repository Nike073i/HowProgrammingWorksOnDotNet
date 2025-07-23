using Microsoft.AspNetCore.Http;

namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public class WithHeaders(IResult result, params KeyValuePair<string, string>[] headers) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext)
    {
        foreach (var kvp in headers)
            httpContext.Response.Headers.Append(kvp.Key, kvp.Value);
        return result.ExecuteAsync(httpContext);
    }
}
