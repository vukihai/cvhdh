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
using System.Threading;
using System.Threading.Tasks;
using Firstapp2257.Models;
using Firstapp2257.Models.Internal.Emails;
using Firstapp2257.Models.Internal.Identity;
using Firstapp2257.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using RazorLight;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services.TwoFactor.Methods
{
	/// <summary>
	/// Events class for the email two factor method.
	/// </summary>
	public class EmailTwoFactorMethodEvents : ITwoFactorMethodEvents
	{
		private const string EmailResourceKey = "Firstapp2257.Assets.Emails.TwoFactor";

		private readonly UserManager<User> _userManager;
		private readonly IBackgroundJobService _backgroundJobService;
		private readonly IEmailService _emailService;
		private readonly IRazorLightEngine _razorLightEngine;
		private readonly EmailTokenProvider<User> _emailTokenProvider;
		private readonly IServiceProvider _serviceProvider;

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end

		// % protected region % [Alter constructor here] off begin
		public EmailTwoFactorMethodEvents(
			UserManager<User> userManager,
			IBackgroundJobService backgroundJobService,
			IEmailService emailService,
			IRazorLightEngine razorLightEngine,
			EmailTokenProvider<User> emailTokenProvider,
			IServiceProvider serviceProvider)
		{
			_userManager = userManager;
			_backgroundJobService = backgroundJobService;
			_emailService = emailService;
			_razorLightEngine = razorLightEngine;
			_emailTokenProvider = emailTokenProvider;
			_serviceProvider = serviceProvider;
		}
		// % protected region % [Alter constructor here] end

		// % protected region % [Customise OnLogin method here] off begin
		/// <inheritdoc />
		public async Task<object> OnLogin(User user, string token)
		{
			var userId = await _userManager.GetUserIdAsync(user);
			var userName = await _userManager.GetUserNameAsync(user);
			var email = await _userManager.GetEmailAsync(user);

			_backgroundJobService.StartBackgroundJob<EmailTwoFactorMethodEvents>(
				emailMethod => emailMethod.SendTokenEmail(userId, userName, email, token, default));

			return null;
		}
		// % protected region % [Customise OnLogin method here] end

		// % protected region % [Customise OnConfiguring method here] off begin
		/// <inheritdoc />
		public async Task<TwoFactorConfiguringResponse> OnConfiguring(User user)
		{
			user.PreferredTwoFactorMethod = TokenOptions.DefaultEmailProvider;
			await _userManager.SetTwoFactorEnabledAsync(user, true);
			return new TwoFactorConfiguringResponse
			{
				Method = TokenOptions.DefaultEmailProvider,
			};
		}
		// % protected region % [Customise OnConfiguring method here] end

		// % protected region % [Customise OnRemoveMethod method here] off begin
		/// <inheritdoc />
		public async Task OnRemoveMethod(User user)
		{
			user.PreferredTwoFactorMethod = null;
			await _userManager.SetTwoFactorEnabledAsync(user, false);
		}
		// % protected region % [Customise OnRemoveMethod method here] end

		// % protected region % [Customise CanConfigureMethod method here] off begin
		/// <inheritdoc />
		public async Task<bool> CanConfigureMethod(User user)
		{
			// Can only configure the email method if the token provider can generate tokens
			return await _emailTokenProvider.CanGenerateTwoFactorTokenAsync(_userManager, user);
		}
		// % protected region % [Customise CanConfigureMethod method here] end

		// % protected region % [Customise SendTokenEmail method here] off begin
		/// <summary>
		/// Sends an email to a user containing a two factor token.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="userName">The username of the user.</param>
		/// <param name="email">The email address of the user.</param>
		/// <param name="token">The two factor token for the user to log in with.</param>
		/// <param name="cancellationToken">A cancellation token for the operation.</param>
		public async Task SendTokenEmail(
			string userId,
			string userName,
			string email,
			string token,
			CancellationToken cancellationToken = default)
		{
			await _emailService.SendEmail(new EmailEntity
			{
				To = new []{ email },
				Body = await _razorLightEngine.CompileRenderAsync(EmailResourceKey, new TwoFactorEmailModel
				{
					Email = email,
					UserId = userId,
					UserName = userName,
					Token = token,
					ServiceProvider = _serviceProvider,
				}),
				Subject = "Two Factor Authentication",
			}, cancellationToken);
		}
		// % protected region % [Customise SendTokenEmail method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}
