using FastEndpoints;
using FluentValidation.Results;

namespace ModernToDoList.Api.Domain.Logging;

public class MyRequestLogger : IGlobalPreProcessor
{
    public Task PreProcessAsync(object req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
    {
        var logger = ctx.Resolve<ILogger>();

        logger.LogInformation($"Request: {req.GetType().FullName} Path: {ctx.Request.Path}");

        return Task.CompletedTask;
    }
}