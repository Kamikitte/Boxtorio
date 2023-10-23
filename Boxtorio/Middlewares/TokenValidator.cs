using Boxtorio.Services;

namespace Boxtorio.Middlewares;

public sealed class TokenValidatorMiddleware
{
	private readonly RequestDelegate next;
	public TokenValidatorMiddleware(RequestDelegate next)
	{
		this.next = next;
	}

	public async Task InvokeAsync(HttpContext context, AuthService authService)
	{
		var isOk = true;
		var sessionIdString = context.User.Claims.FirstOrDefault(x => x.Type == "sessionId")?.Value;
		if (Guid.TryParse(sessionIdString, out var sesssionId))
		{
			var session = await authService.GetSessionById(sesssionId);
			if (!session.IsActive)
			{
				isOk = false;
				context.Response.Clear();
				context.Response.StatusCode = 401;
			}
		}
		if (isOk)
		{
			await next(context);
		}
	}
}
public static class TokenValidatorMiddlewareExtensions
{
	public static void UseTokenValidator(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<TokenValidatorMiddleware>();
    }
}
