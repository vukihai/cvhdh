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
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Firstapp2257.Controllers;
using Firstapp2257.Models;
using Firstapp2257.Services;
using Firstapp2257.Services.Interfaces;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServersideTests.Helpers;
using ServersideTests.Helpers.Services;
using TestDataLib;
using Xunit;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace ServersideTests.Tests.Integration.BotWritten
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Unit")]
	public class ResetPasswordTests
	{
		// % protected region % [Add custom traits here] off begin
		// % protected region % [Add custom traits here] end
		// % protected region % [Customize ResetPasswordTest here] off begin
		[Fact]
		public async Task ResetPasswordTest()
		{
			using var host = ServerBuilder.CreateServer(new ServerBuilderOptions
			{
				ConfigureServices = (services, _) =>
				{
					services.AddScoped<IEmailService, TestEmailService>();
				}
			});

			// Seed a new user to reset the password of
			var userName = DataUtils.RandString(10, CharType.ALPHANUMERIC_ONLY);
			var password = Guid.NewGuid().ToString();
			using (var scope = host.Services.CreateScope())
			{
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
				var result = await userManager.CreateAsync(new User
				{
					Email = DataUtils.RandEmail(),
					UserName = userName,
					EmailConfirmed = true,
				}, password);

				Assert.True(result.Succeeded);
			}
			// Verify that the user can log in with their current password
			using (var scope = host.Services.CreateScope())
			{
				var controller = scope.ServiceProvider.GetTestController<AuthorizationController>();
				Assert.True(await CanLogin(controller, userName, password));
			}

			// Request a reset password email and read the token
			string token;
			using (var scope = host.Services.CreateScope())
			{
				var controller = scope.ServiceProvider.GetTestController<AccountController>();
				await controller.ResetPasswordRequest(new AccountController.UsernameModel
				{
					Username = userName
				});
				// Get the sent reset token
				var emailService = (TestEmailService) scope.ServiceProvider.GetRequiredService<IEmailService>();
				token = ReadResetToken(emailService.SentEmails.First());
			}

			// Reset the password
			var newPassword = Guid.NewGuid().ToString();
			using (var scope = host.Services.CreateScope())
			{
				var controller = scope.ServiceProvider.GetTestController<AccountController>();
				var result = await controller.ResetPassword(new AccountController.ResetPasswordModel
				{
					Username = userName,
					Password = newPassword,
					Token = token,
				});

				Assert.IsType<OkResult>(result);
			}

			// Verify that we can login with the new password
			using (var scope = host.Services.CreateScope())
			{
				var controller = scope.ServiceProvider.GetTestController<AuthorizationController>();
				Assert.True(await CanLogin(controller, userName, newPassword));
			}
			// Also verify that the old password can't be used
			using (var scope = host.Services.CreateScope())
			{
				var controller = scope.ServiceProvider.GetTestController<AuthorizationController>();
				Assert.False(await CanLogin(controller, userName, password));
			}
		}
		// % protected region % [Customize ResetPasswordTest here] end

		// % protected region % [Customize ReadResetToken method here] off begin
		private static string ReadResetToken(EmailEntity email)
		{
			var doc = new HtmlDocument();
			doc.LoadHtml(email.Body);
			var linkTag = doc.GetElementbyId("reset-link");
			var href = linkTag.ChildAttributes("href").First();
			var uri = new Uri(href.Value);
			var query = HttpUtility.ParseQueryString(uri.Query);
			return query["token"];
		}
		// % protected region % [Customize ReadResetToken method here] end

		// % protected region % [Customize CanLogin method here] off begin
		private static async Task<bool> CanLogin(AuthorizationController controller, string userName, string password)
		{
			var result = await controller.Login(new LoginDetails
			{
				Username = userName,
				Password = password,
			});

			return result.GetType() == typeof(OkObjectResult);
		}
		// % protected region % [Customize CanLogin method here] end

		// % protected region % [Add any additional methods here] off begin
		// % protected region % [Add any additional methods here] end
	}
}