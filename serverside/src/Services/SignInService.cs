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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Firstapp2257.Exceptions;
using Firstapp2257.Models;
using Firstapp2257.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

// % protected region % [Customise Authorization Library imports here] off begin
using OpenIddict.Abstractions;
using OpenIddict.Server;
// % protected region % [Customise Authorization Library imports here] end

// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services
{
	public class SignInService : ISignInService
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IUserService _userService;
		private readonly IOptions<IdentityOptions> _identityOptions;
		private readonly IHttpContextAccessor _httpContextAccessor;

		// % protected region % [Add any extra class fields here] off begin
		// % protected region % [Add any extra class fields here] end

		public SignInService(
			// % protected region % [Add any constructor arguments here] off begin
			// % protected region % [Add any constructor arguments here] end
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			IUserService userService,
			IOptions<IdentityOptions> identityOptions,
			IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_userService = userService;
			_identityOptions = identityOptions;
			_httpContextAccessor = httpContextAccessor;
			// % protected region % [Add any constructor logic here] off begin
			// % protected region % [Add any constructor logic here] end
		}

		// % protected region % [Alter CheckCredentials here] off begin
		/// <inheritdoc />
		public async Task<User> CheckCredentials(
			string username,
			string password,
			bool lockoutOnFailure = true,
			bool validateLockout = true,
			bool validateNotAllowed = true)
		{
			var user = await _userManager.FindByNameAsync(username);

			if (user == null)
			{
				throw new InvalidUserPasswordException();
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);

			if (validateLockout && result.IsLockedOut)
			{
				throw new InvalidUserPasswordException("This account has been locked");
			}

			if (validateNotAllowed && result.IsNotAllowed)
			{
				throw new InvalidUserPasswordException("This account is not allowed to log in");
			}

			if (!result.Succeeded)
			{
				throw new InvalidUserPasswordException("Invalid username or password");
			}

			return user;
		}
		// % protected region % [Alter CheckCredentials here] end

		// % protected region % [Alter Exchange here] off begin
		/// <inheritdoc />
		public async Task<AuthenticationTicket> Exchange(OpenIdConnectRequest request)
		{
			if (request.IsPasswordGrantType())
			{
				var user = await CheckCredentials(request.Username, request.Password);

				if (user.TwoFactorEnabled)
				{
					throw new InvalidOperationException("Cannot create tokens for users with two factor enabled");
				}

				// Create a new authentication ticket.
				var ticket = await CreateTicketAsync(request, user);

				return ticket;
			}

			if (request.IsRefreshTokenGrantType())
			{

				if (_httpContextAccessor.HttpContext == null)
				{
					throw new InvalidOperationException("Not in context of HTTP request");
				}

				var info = await _httpContextAccessor.HttpContext
					.AuthenticateAsync(OpenIddictServerDefaults.AuthenticationScheme);

				var user = await _userManager.GetUserAsync(info.Principal);
				if (user == null)
				{
					throw new InvalidUserPasswordException();
				}

				if (!await _signInManager.CanSignInAsync(user))
				{
					throw new InvalidUserPasswordException();
				}

				return await CreateTicketAsync(request, user);
			}

			throw new InvalidGrantTypeException();
		}
		// % protected region % [Alter Exchange here] end

		// % protected region % [Alter CreateTicketAsync here] off begin
		/// <summary>
		/// Creates a ticket for an OpenId connect request
		/// </summary>
		/// <param name="request">The OpenId connect request</param>
		/// <param name="user">The user to provide a ticket for</param>
		/// <returns>The OpenId ticket</returns>
		private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, User user)
		{
			// Create a new ClaimsPrincipal containing the claims that
			// will be used to create an id_token, a token or a code.
			var principal = await _userService.CreateUserPrincipal(
				user,
				OpenIdConnectConstants.Schemes.Bearer);

			// Create a new authentication ticket holding the user identity.
			var ticket = new AuthenticationTicket(principal,
				new AuthenticationProperties(),
				OpenIddictServerDefaults.AuthenticationScheme);

			// The scopes that are provided to every request
			var defaultScopes = new List<string>
			{
				OpenIdConnectConstants.Scopes.OpenId,
				OpenIddictConstants.Scopes.Roles,
			};
			// These are additional scopes that the client can also explicitly request
			var allowedScopes = new List<string>
			{
				OpenIdConnectConstants.Scopes.Email,
				OpenIdConnectConstants.Scopes.Profile,
				OpenIdConnectConstants.Scopes.OfflineAccess,
			};
			ticket.SetScopes(defaultScopes.Concat(allowedScopes.Intersect(request.GetScopes())));

			ticket.SetResources("resource-server");

			ticket.SetAccessTokenLifetime(new TimeSpan(7, 0, 0, 0));

			// Note: by default, claims are NOT automatically included in the access and identity tokens.
			// To allow OpenIddict to serialize them, you must attach them a destination, that specifies
			// whether they should be included in access tokens, in identity tokens or in both.

			foreach (var claim in ticket.Principal.Claims)
			{
				// Never include the security stamp in the access and identity tokens, as it's a secret value.
				if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
				{
					continue;
				}

				var destinations = new List<string>
				{
					OpenIdConnectConstants.Destinations.AccessToken
				};

				// Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
				// The other claims will only be added to the access_token, which is encrypted when using the default format.
				if ((claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
					(claim.Type == OpenIdConnectConstants.Claims.Email && ticket.HasScope(OpenIdConnectConstants.Scopes.Email)) ||
					(claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles)))
				{
					destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
				}

				claim.SetDestinations(destinations);
			}

			return ticket;
		}
		// % protected region % [Alter CreateTicketAsync here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}