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
using System.Threading.Tasks;
using Firstapp2257.Models;
using Firstapp2257.Models.Internal.Identity;
using Microsoft.AspNetCore.Identity;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services.TwoFactor.Methods
{
	/// <summary>
	/// Events class for the authenticator two factor method.
	/// </summary>
	public class AuthenticatorAppTwoFactorMethodEvents : ITwoFactorMethodEvents
	{
		private const string UserStoreLoginProvider = "[AspNetUserStore]";
		private const string AuthenticatorKeyTokenName = "AuthenticatorKey";

		private readonly UserManager<User> _userManager;

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end

		// % protected region % [Alter constructor here] off begin
		public AuthenticatorAppTwoFactorMethodEvents(UserManager<User> userManager)
		{
			_userManager = userManager;
		}
		// % protected region % [Alter constructor here] end

		// % protected region % [Customise OnLogin method here] off begin
		/// <inheritdoc />
		public Task<object> OnLogin(User user, string token)
		{
			return Task.FromResult(null as object);
		}
		// % protected region % [Customise OnLogin method here] end

		// % protected region % [Customise OnConfiguring method here] off begin
		/// <inheritdoc />
		public async Task<TwoFactorConfiguringResponse> OnConfiguring(User user)
		{
			user.PreferredTwoFactorMethod = TokenOptions.DefaultAuthenticatorProvider;
			await _userManager.SetTwoFactorEnabledAsync(user, true);
			await _userManager.ResetAuthenticatorKeyAsync(user);
			var code = await _userManager.GetAuthenticatorKeyAsync(user);
			return new AuthenticatorTwoFactorConfiguringResponse
			{
				Method = TokenOptions.DefaultAuthenticatorProvider,
				Code = code,
			};
		}
		// % protected region % [Customise OnConfiguring method here] end

		// % protected region % [Customise OnRemoveMethod method here] off begin
		/// <inheritdoc />
		public async Task OnRemoveMethod(User user)
		{
			user.PreferredTwoFactorMethod = null;
			await _userManager.SetTwoFactorEnabledAsync(user, false);
			await _userManager.RemoveAuthenticationTokenAsync(
				user,
				UserStoreLoginProvider,
				AuthenticatorKeyTokenName);
		}
		// % protected region % [Customise OnRemoveMethod method here] end

		// % protected region % [Customise CanConfigureMethod method here] off begin
		/// <inheritdoc />
		public async Task<bool> CanConfigureMethod(User user)
		{
			// Can always configure token since a new authenticator key is generated each time the method
			// is configured.
			return true;
		}
		// % protected region % [Customise CanConfigureMethod method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}
