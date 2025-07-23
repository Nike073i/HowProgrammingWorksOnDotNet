using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace HowProgrammingWorksOnDotNet.WebApi.Etag;

public static class ResultExtensions
{
    public static IResult WithETag(this IResult result, string etag) =>
        new WithHeaders(result, [new(HeaderNames.ETag, etag)]);
}
