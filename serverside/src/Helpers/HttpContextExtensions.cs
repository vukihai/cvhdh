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
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Helpers
{
	/// <summary>
	/// Extensions for the http context
	/// </summary>
	public static class HttpContextExtensions
	{
		private const string ClaimsPrincipalKey = "LoginUserClaimsPrincipal";

		private static readonly ActionDescriptor EmptyActionDescriptor = new();

		// % protected region % [Customise WriteModelAsync method here] off begin
		/// <summary>
		/// Writes an object to a http context response.
		/// </summary>
		/// <param name="context">The http context to write the response to.</param>
		/// <param name="model">The model to write to the response.</param>
		/// <typeparam name="TModel">The type of the model to write.</typeparam>
		/// <returns>A task that completes when response has finished being written to.</returns>
		public static Task WriteModelAsync<TModel>(this HttpContext context, TModel model)
		{
			var result = new ObjectResult(model)
			{
				DeclaredType = typeof(TModel)
			};

			return context.ExecuteResultAsync(result);
		}
		// % protected region % [Customise WriteModelAsync method here] end

		// % protected region % [Customise WriteProblemAsync method here] off begin
		/// <summary>
		/// Writes a problem result to a http context response.
		/// </summary>
		/// <param name="context">The http context to write the response to.</param>
		/// <param name="statusCode">The value for <see cref="ProblemDetails.Status"/>.</param>
		/// <param name="title">The value for <see cref="ProblemDetails.Title" />.</param>
		/// <param name="type">The value for <see cref="ProblemDetails.Type" />.</param>
		/// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
		/// <param name="instance">The value for <see cref="ProblemDetails.Instance" />.</param>
		/// <returns>A task that completes when response has finished being written to.</returns>
		public static Task WriteProblemAsync(
			this HttpContext context,
			string detail = null,
			string instance = null,
			int? statusCode = null,
			string title = null,
			string type = null)
		{
			var problemDetailsFactory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
			var problemDetails = problemDetailsFactory.CreateProblemDetails(
				context,
				statusCode ?? 500,
				title,
				type,
				detail,
				instance);

			return context.ExecuteResultAsync(new ObjectResult(problemDetails)
			{
				StatusCode = problemDetails.Status
			});
		}
		// % protected region % [Customise WriteProblemAsync method here] end

		// % protected region % [Customise ExecuteResultAsync method here] off begin
		/// <summary>
		/// Executes an action result on a http context response. Implementation sourced from
		/// https://kristian.hellang.com/using-mvc-result-executors-in-middleware/
		/// </summary>
		/// <param name="context">The http context to write the response to.</param>
		/// <param name="result">An action result to write to the response.</param>
		/// <typeparam name="TResult">The type of the result to write.</typeparam>
		/// <returns>A task that completes when response has finished being written to.</returns>
		public static Task ExecuteResultAsync<TResult>(this HttpContext context, TResult result)
			where TResult : IActionResult
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			if (result == null)
			{
				return Task.CompletedTask;
			}

			var executor = context.RequestServices.GetRequiredService<IActionResultExecutor<TResult>>();

			var routeData = context.GetRouteData();
			var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

			return executor.ExecuteAsync(actionContext, result);
		}
		// % protected region % [Customise ExecuteResultAsync method here] end

		// % protected region % [Customise IsTwoFactorAuthenticated method here] off begin
		/// <summary>
		/// Checks if the current http context is authenticated by 2 factor auth. Note that if a user did not sign in
		/// with their 2fa token in this session (if they ticked the remember button on their last session), then this
		/// will return false.
		/// </summary>
		/// <param name="httpContext">The http context to check.</param>
		/// <returns>True if the user has been authenticated with 2fa.</returns>
		public static async Task<bool> IsTwoFactorAuthenticated(this HttpContext httpContext)
		{
			var authorizationService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
			var satisfiesPolicy = await authorizationService.AuthorizeAsync(
				httpContext.User,
				httpContext, 
				"TwoFactorEnabled");

			return satisfiesPolicy.Succeeded;
		}
		// % protected region % [Customise IsTwoFactorAuthenticated method here] end

		// % protected region % [Customise StorePrincipal method here] off begin
		/// <summary>
		/// Stores a claims principal in the HttpContext items dictionary.
		/// </summary>
		/// <param name="httpContext">The context to store the principal in.</param>
		/// <param name="principal">The principal to store.</param>
		public static void StorePrincipal(this HttpContext httpContext, ClaimsPrincipal principal)
		{
			httpContext.Items[ClaimsPrincipalKey] = principal;
		}
		// % protected region % [Customise StorePrincipal method here] end

		// % protected region % [Customise GetStoredPrincipal method here] off begin
		/// <summary>
		/// Gets the stored claims principal from the HttpContext items dictionary.
		/// </summary>
		/// <param name="httpContext">The context to get the principal from.</param>
		/// <returns>The stored claims principal or null if there was no principal stored.</returns>
		public static ClaimsPrincipal GetStoredPrincipal(this HttpContext httpContext)
		{
			httpContext.Items.TryGetValue(ClaimsPrincipalKey, out var claimsPrincipal);
			return claimsPrincipal as ClaimsPrincipal;
		}
		// % protected region % [Customise GetStoredPrincipal method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}