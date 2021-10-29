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
using Firstapp2257.Configuration;
using Firstapp2257.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Utility
{
	public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Group>
	{
		private readonly bool _runtimeClaimsEnabled;

		// % protected region % [Add any extra class fields here] off begin
		// % protected region % [Add any extra class fields here] end

		public ClaimsPrincipalFactory(
			// % protected region % [Add any extra constructor arguments here] off begin
			// % protected region % [Add any extra constructor arguments here] end
			UserManager<User> userManager,
			RoleManager<Group> roleManager,
			IOptions<IdentityOptions> options,
			IOptions<CookieConfiguration> cookieConfiguration)
			: base(userManager, roleManager, options)
		{
			_runtimeClaimsEnabled = cookieConfiguration.Value.RuntimeClaimsEnabled;
			// % protected region % [Add any extra constructor logic here] off begin
			// % protected region % [Add any extra constructor logic here] end
		}

		// % protected region % [Customise GenerateClaimsAsync method here] off begin
		/// <inheritdoc />
		protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
		{
			var userId = await UserManager.GetUserIdAsync(user);
			var userName = await UserManager.GetUserNameAsync(user);

			var identity = new ClaimsIdentity("Identity.Application",
				Options.ClaimsIdentity.UserNameClaimType,
				Options.ClaimsIdentity.RoleClaimType);

			identity.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
			identity.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, userName));
			identity.AddClaim(new Claim("UserId", user.Id.ToString()));
			identity.AddClaim(
				new Claim(CustomCookieAuthenticationEvents.ClaimTokenId, Guid.NewGuid().ToString()));

			if (UserManager.SupportsUserEmail)
			{
				var email = await UserManager.GetEmailAsync(user);
				if (!string.IsNullOrEmpty(email))
				{
					identity.AddClaim(new Claim(Options.ClaimsIdentity.EmailClaimType, email));
				}
			}

			if (UserManager.SupportsUserSecurityStamp)
			{
				identity.AddClaim(new Claim(Options.ClaimsIdentity.SecurityStampClaimType,
					await UserManager.GetSecurityStampAsync(user)));
			}

			if (_runtimeClaimsEnabled && UserManager.SupportsUserClaim)
			{
				identity.AddClaims(await UserManager.GetClaimsAsync(user));
			}

			if (UserManager.SupportsUserRole)
			{
				var roles = await UserManager.GetRolesAsync(user);
				foreach (var roleName in roles)
				{
					identity.AddClaim(new Claim(Options.ClaimsIdentity.RoleClaimType, roleName));
					if (_runtimeClaimsEnabled && RoleManager.SupportsRoleClaims)
					{
						var role = await RoleManager.FindByNameAsync(roleName);
						if (role != null)
						{
							identity.AddClaims(await RoleManager.GetClaimsAsync(role));
						}
					}
				}
			}

			return identity;
		}
		// % protected region % [Customise GenerateClaimsAsync method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}