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
using Firstapp2257.Configuration;
using Firstapp2257.Services.Interfaces;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services
{
	public class XsrfService : IXsrfService
	{
		private const string TokenName = "XSRF-TOKEN";

		private readonly IAntiforgery _antiforgery;
		private readonly ServerSettings _serverSettings;
		private readonly CookieAuthenticationOptions _cookieAuthenticationOptions;

		// % protected region % [Add any extra class fields here] off begin
		// % protected region % [Add any extra class fields here] end

		public XsrfService(
			// % protected region % [Add any extra constructor arguments here] off begin
			// % protected region % [Add any extra constructor arguments here] end
			IAntiforgery antiforgery,
			IOptions<ServerSettings> serverSettings,
			IOptionsSnapshot<CookieAuthenticationOptions> cookieAuthenticationOptions)
		{
			_antiforgery = antiforgery;
			_serverSettings = serverSettings.Value;
			_cookieAuthenticationOptions = cookieAuthenticationOptions.Get(
				IdentityConstants.ApplicationScheme);
			// % protected region % [Add any extra constructor logic here] off begin
			// % protected region % [Add any extra constructor logic here] end
		}

		// % protected region % [customize exchange signature] off begin
		/// <inheritdoc />
		public void AddXsrfToken(HttpContext context)
		{
			if (string.IsNullOrEmpty(context.User.Identity?.Name))
			{
				return;
			}

			var tokens = _antiforgery.GetAndStoreTokens(context);

			var date = DateTime.UtcNow.Add(_cookieAuthenticationOptions.ExpireTimeSpan);

			context.Response.Cookies.Append(
				TokenName,
				tokens.RequestToken!,
				new CookieOptions
				{
					HttpOnly = false,
					Expires = new DateTimeOffset(date, TimeSpan.FromHours(0)),
					Secure = _serverSettings.IsHttps,
					SameSite = SameSiteMode.Strict,
				});
		}
		// % protected region % [customize exchange signature] end

		// % protected region % [Customise AddXsrfToken overload here] off begin
		/// <inheritdoc />
		public void AddXsrfToken(HttpContext context, ClaimsPrincipal userClaim)
		{
			var existingClaim = context.User;
			context.User = userClaim;

			AddXsrfToken(context);

			context.User = existingClaim;
		}
		// % protected region % [Customise AddXsrfToken overload here] end

		// % protected region % [Customise RemoveXsrfToken overload here] off begin
		public void RemoveXsrfToken(HttpContext context)
		{
			var date = DateTime.UtcNow.Add(_cookieAuthenticationOptions.ExpireTimeSpan);
			context.Response.Cookies.Delete(TokenName, new CookieOptions
			{
				HttpOnly = false,
				Expires = new DateTimeOffset(date, TimeSpan.FromHours(0)),
				Secure = _serverSettings.IsHttps,
				SameSite = SameSiteMode.Strict,
			});
		}
		// % protected region % [Customise RemoveXsrfToken overload here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}