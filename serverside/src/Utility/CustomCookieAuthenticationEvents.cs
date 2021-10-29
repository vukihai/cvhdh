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
using System.Net;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using Firstapp2257.Configuration;
using Firstapp2257.Helpers;
using Firstapp2257.Models;
using Firstapp2257.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Utility
{
	/// <summary>
	/// Customised cookie events for login cookies. This service will store login cookies in a cookie store and use the
	/// cookie store to validate weather the cookie is valid.
	/// </summary>
	public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
	{
		// % protected region % [Customise constants here] off begin
		public const string ClaimTokenId = "user_token_id";
		// % protected region % [Customise constants here] end

		// % protected region % [Customise class fields here] off begin
		private readonly ICookieStore _cookieStore;
		private readonly CookieAuthenticationOptions _cookieAuthenticationOptions;
		private readonly CookieConfiguration _cookieConfiguration;
		private readonly ILogger<CustomCookieAuthenticationEvents> _logger;
		// % protected region % [Customise class fields here] end

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end

		// % protected region % [Customise the constructor method here] off begin
		public CustomCookieAuthenticationEvents(
			ICookieStore cookieStore,
			IOptionsSnapshot<CookieAuthenticationOptions> cookieAuthenticationOptions,
			IOptions<CookieConfiguration> cookieConfiguration,
			ILogger<CustomCookieAuthenticationEvents> logger)
		{
			_cookieStore = cookieStore;
			_cookieAuthenticationOptions = cookieAuthenticationOptions.Get(
				IdentityConstants.ApplicationScheme);
			_cookieConfiguration = cookieConfiguration.Value;
			_logger = logger;
		}
		// % protected region % [Customise the constructor method here] end

		// % protected region % [Customise SigningIn method here] off begin
		/// <inheritdoc />
		public override async Task SigningIn(CookieSigningInContext context)
		{
			var userId = context.Principal?.Identity?.Name;
			if (userId == null)
			{
				_logger.LogWarning("Attempting to log in user with null user id", context.Principal);
			}

			context.HttpContext.StorePrincipal(context.Principal);

			var tokenId = context.Principal.GetClaim(ClaimTokenId);
			await _cookieStore.StoreCookieData(
				tokenId,
				userId,
				new CookieData
				{
					InitialIp = context.HttpContext.Connection.RemoteIpAddress?.ToString(),
					InitialPort = context.HttpContext.Connection.RemotePort,
					IssueDateTime = DateTime.UtcNow,
					IssuingContextId = context.HttpContext.TraceIdentifier
				},
				new CookieStorageOptions
				{
					AbsoluteExpirationRelativeToNow = _cookieAuthenticationOptions.Cookie.Expiration,
					SlidingExpiration = _cookieAuthenticationOptions.ExpireTimeSpan,
				});

			await base.SigningIn(context);
		}
		// % protected region % [Customise SigningIn method here] end

		// % protected region % [Customise SigningOut method here] off begin
		/// <inheritdoc />
		public override async Task SigningOut(CookieSigningOutContext context)
		{
			var tokenId = context.HttpContext.User.GetClaim(ClaimTokenId);
			var userId = context.HttpContext.User.Identity?.Name;
			await _cookieStore.RemoveCookieData(tokenId, userId);
			await base.SigningOut(context);
		}
		// % protected region % [Customise SigningOut method here] end

		// % protected region % [Customise ValidatePrincipal method here] off begin
		/// <inheritdoc />
		public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
		{
			if (!await ValidateStoredCookie(context))
			{
				context.RejectPrincipal();
			}

			await ValidateSecurityStamp(context);
		}
		// % protected region % [Customise ValidatePrincipal method here] end

		// % protected region % [Customise RedirectToLogin method here] off begin
		/// <inheritdoc />
		public override async Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
		{
			// Upon finding any errors, sign the scheme out and report back to the clientside
			await context.HttpContext.SignOutAsync(context.Scheme.Name);
			await context.HttpContext.WriteProblemAsync(
				"You need to log in to continue",
				statusCode: (int)HttpStatusCode.Unauthorized);
		}
		// % protected region % [Customise RedirectToLogin method here] end

		// % protected region % [Customise ValidateStoredCookie method here] off begin
		/// <summary>
		/// Validates the provided cookie against the cookie store. No validation will occur if stored cookie validation
		/// has been disabled.
		/// </summary>
		/// <param name="context">The validation context for this request.</param>
		/// <returns>A task that completes with a true if the cookie is valid or false otherwise.</returns>
		protected async Task<bool> ValidateStoredCookie(CookieValidatePrincipalContext context)
		{
			// If stored cookie validation is disabled then immediately succeed
			if (!_cookieConfiguration.StoredCookieValidationEnabled)
			{
				return true;
			}

			var tokenId = context.Principal?.GetClaim(ClaimTokenId);
			var userId = context.Principal?.Identity?.Name;

			// If there is no token id then reject
			if (string.IsNullOrWhiteSpace(tokenId))
			{
				return false;
			}

			try
			{
				var storedValue = await _cookieStore.GetCookieData(tokenId, userId);
				var cookieDisabled = storedValue == null;

				// If there is no cookie in the store to match the token user pair then reject.
				if (cookieDisabled)
				{
					return false;
				}
			}
			catch (Exception e)
			{
				// Log and reject on any errors
				_logger.LogError("Error in cookie validation {Error}", e);
				return false;
			}

			return true;
		}
		// % protected region % [Customise ValidateStoredCookie method here] end

		// % protected region % [Customise ValidateSecurityStamp method here] off begin
		/// <summary>
		/// Validates a cookies security stamp claim if security stamp validation is enabled.
		/// </summary>
		/// <param name="context">The validation context for this request.</param>
		protected async Task ValidateSecurityStamp(CookieValidatePrincipalContext context)
		{
			if (_cookieConfiguration.CookieSecurityStampValidationEnabled)
			{
				await SecurityStampValidator.ValidatePrincipalAsync(context);
			}
		}
		// % protected region % [Customise ValidateSecurityStamp method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}