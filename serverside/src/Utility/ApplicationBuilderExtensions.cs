/*
 * @bot-written
 *
 * WARNING AND NOTICE
 * Any access, download, storage, and/or use of this source code is subject to the terms and conditions of the
 * Full Software Licence as accepted by you before being granted access to this source code and other materials,
 * the terms of which can be accessed on the Codebots website at https://codebots.com/full-software-licence. Any
 * commercial use in contravention of the terms of the Full Software Licence may be pursued by Codebots through
 * licence termination and further legal action, and be required to indemnify Codebots for any loss or damage,
 * including interest and costs. You are deemed to have accepted the terms of the Full Software Licence on any
 * access, download, storage, and/or use of this source code.
 *
 * BOT WARNING
 * This file is bot-written.
 * Any changes out side of "protected regions" will be lost next time the bot makes any changes.
 */
using System;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Firstapp2257.Configuration;
using Firstapp2257.Models;
using Firstapp2257.Services.Interfaces;
using Serilog;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Utility
{
	/// <summary>
	/// Extension methods for the ApplicationBuilder
	/// </summary>
	public static class ApplicationBuilderExtensions
	{
		// % protected region % [Customise UseAuthorizationPolicy method here] off begin
		/// <summary>
		/// Middleware that will return a 401 if the user is not logged in or it does not satisfy the provided
		/// authorization policy.
		/// </summary>
		/// <param name="app">The application builder to augment.</param>
		/// <param name="policy">The name of the authorization policy to use.</param>
		/// <returns>The application builder.</returns>
		public static IApplicationBuilder UseAuthorizationPolicy(
			this IApplicationBuilder app,
			string policy)
		{
			// Custom middleware registration to ensure that only admins can access the hangfire dashboard
			app.Use(async (httpContext, next) =>
			{
				var authorizationService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
				var principal = httpContext.User;

				// To continue down the middleware pipeline a user must satisfy the requested policy
				var satisfiesPolicy = await authorizationService.AuthorizeAsync(principal, httpContext, policy);

				if (satisfiesPolicy.Succeeded)
				{
					await next();
					return;
				}

				httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
			});

			return app;
		}
		// % protected region % [Customise UseAuthorizationPolicy method here] end

		// % protected region % [Customise UseHangfireTaskServer method here] off begin
		/// <summary>
		/// Configure the application to use the Hangfire server to run background jobs if it was not disabled in the
		/// configuration.
		/// </summary>
		/// <param name="app">The application builder to augment.</param>
		/// <returns>The application builder.</returns>
		public static IApplicationBuilder UseHangfireTaskServer(this IApplicationBuilder app)
		{
			var configuration = app.ApplicationServices.GetRequiredService<IOptions<SchedulerConfiguration>>();

			if (configuration.Value.DisableTaskRunner)
			{
				return app;
			}

			app.UseHangfireServer();

			return app;
		}
		// % protected region % [Customise UseHangfireTaskServer method here] end

		// % protected region % [Customise UseAdminHangfireDashboard method here] off begin
		/// <summary>
		/// Configures the use of the Hangfire Dashboard and makes it only accessible to users that satisfy a provided
		/// authorization policy.
		/// </summary>
		/// <param name="app">The application builder to augment.</param>
		/// <param name="route">The route to serve the Hangfire dashboard on.</param>
		/// <param name="policyName">The name of the authorization policy to use.</param>
		/// <returns>The application builder.</returns>
		public static IApplicationBuilder UseAdminHangfireDashboard(
			this IApplicationBuilder app,
			string route = "/api/hangfire",
			string policyName = "HangfireDashboardPolicy")
		{
			var configuration = app.ApplicationServices.GetRequiredService<IOptions<SchedulerConfiguration>>();
			if (configuration.Value.DisableDashboard)
			{
				return app;
			}

			app.Map(route, builder =>
			{
				builder.UseAuthorizationPolicy(policyName);
				builder.UseHangfireDashboard("", new DashboardOptions
				{
					Authorization = Array.Empty<IDashboardAuthorizationFilter>(),
				});
			});

			return app;
		}
		// % protected region % [Customise UseAdminHangfireDashboard method here] end

		// % protected region % [Customise UseSecurityHeaders method here] off begin
		/// <summary>
		/// Middleware that adds extra security headers to the response.
		/// </summary>
		/// <param name="app">The application builder to augment.</param>
		/// <returns>The application builder.</returns>
		public static IApplicationBuilder UseSecurityHeaders(
			this IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				context.Response.OnStarting(async state =>
				{
					if (state is HttpContext httpContext)
					{
						httpContext.Response.Headers["X-Content-Type-Options"] = "nosniff";
						httpContext.Response.Headers["Referrer-Policy"] = "no-referrer";
						httpContext.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
					}
				}, context);
				await next();
			});
			return app;
		}
		// % protected region % [Customise UseSecurityHeaders method here] end

		// % protected region % [Customise UseRequestLogging method here] off begin
		/// <summary>
		/// Middleware to add serilog request logging to the pipeline
		/// </summary>
		/// <param name="app">The application builder to augment.</param>
		/// <returns>The application builder.</returns>
		public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
		{
			return app.UseSerilogRequestLogging(options =>
			{
				options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} by user: {User} " +
					"responded {StatusCode} in {Elapsed:0.0000} ms";
				options.EnrichDiagnosticContext = (context, httpContext) =>
				{
					context.Set("User", httpContext.User?.Identity?.Name);
					context.Set("UserId", httpContext.User?.FindFirst("UserId")?.Value);
				};
			});
		}
		// % protected region % [Customise UseRequestLogging method here] end

		// % protected region % [Customise UseRequestLogging method here] off begin
		/// <summary>
		/// Middleware to add user fields to the database context for auditing purposes.
		/// </summary>
		/// <param name="app">The application builder to augment.</param>
		/// <returns>The application builder.</returns>
		public static IApplicationBuilder UseDatabaseUserAuditing(this IApplicationBuilder app)
		{
			return app.Use(async (context, next) =>
			{
				var dbContext = context.RequestServices.GetRequiredService<Firstapp2257DBContext>();
				var httpContextAccessor = context.RequestServices.GetRequiredService<IHttpContextAccessor>();

				dbContext.SessionId = httpContextAccessor?.HttpContext?.TraceIdentifier;
				dbContext.SessionUser = httpContextAccessor?.HttpContext?.User.Identity?.Name;
				dbContext.SessionUserId = httpContextAccessor?.HttpContext?.User.FindFirst("UserId")?.Value;

				await next();
			});
		}
		// % protected region % [Customise UseRequestLogging method here] end

		// % protected region % [Customise UseXsrfToken method here] off begin
		/// <summary>
		/// Middleware to add an XSRF token as a cookie to an outgoing response.
		/// </summary>
		/// <param name="app">The application builder to augment.</param>
		/// <returns>The application builder.</returns>
		public static IApplicationBuilder UseXsrfToken(this IApplicationBuilder app)
		{
			return app.Use(async (context, next) =>
			{
				context.RequestServices.GetRequiredService<IXsrfService>().AddXsrfToken(context);
				await next();
			});
		}
		// % protected region % [Customise UseXsrfToken method here] end

		// % protected region % [Customise UseReactbot method here] off begin
		/// <summary>
		/// Configures the serving and running of a Reactbot clientside.
		/// </summary>
		/// <param name="app">The application builder to augment.</param>
		/// <returns>The application builder.</returns>
		public static IApplicationBuilder UseReactbot(this IApplicationBuilder app)
		{
			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "Client";

				// If we are in a production environment then no further action is required
				var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
				if (!env.IsDevelopment())
				{
					return;
				}

				// In a development environment then proxy the react clientside through the server
				var configuration = app.ApplicationServices.GetRequiredService<IOptions<ClientServerConfiguration>>();
				spa.Options.SourcePath = configuration.Value.ClientSourcePath;

				// We can either proxy from a running react server or we can serve our own
				if (configuration.Value.UseProxyServer)
				{
					spa.UseProxyToSpaDevelopmentServer(configuration.Value.ProxyServerAddress);
				}
				else
				{
					spa.UseReactDevelopmentServer("start");
				}
			});

			return app;
		}
		// % protected region % [Customise UseReactbot method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}