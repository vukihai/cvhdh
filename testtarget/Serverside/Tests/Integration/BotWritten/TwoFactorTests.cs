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
using Firstapp2257.Controllers;
using Firstapp2257.Models;
using Firstapp2257.Models.Internal.Identity;
using Firstapp2257.Services.Interfaces;
using Firstapp2257.Services.TwoFactor;
using Firstapp2257.Services.TwoFactor.Methods;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServersideTests.Helpers;
using ServersideTests.Helpers.EntityFactory;
using ServersideTests.Helpers.Services;
using Xunit;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace ServersideTests.Tests.Integration.BotWritten
{
	public class TwoFactorTests : IDisposable
	{
		private const string TestUserName = "test_user@example.com";

		private readonly IHost _host;
		private readonly IServiceScopeFactory _scopeFactory;

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end

		// % protected region % [Customize constructor here] off begin
		public TwoFactorTests()
		{
			_host = ServerBuilder.CreateServer(new ServerBuilderOptions
			{
				ConfigureServices = (services, options) =>
				{
					services.AddScoped<IEmailService, TestEmailService>();
				}
			});
			_scopeFactory = _host.Services.GetRequiredService<IServiceScopeFactory>();
		}
		// % protected region % [Customize constructor here] end

		// % protected region % [Customize Dispose here] off begin
		public void Dispose()
		{
			_host.Dispose();
		}
		// % protected region % [Customize Dispose here] end

		// % protected region % [Add custom VerifyEmailTwoFactorLogin traits here] off begin
		// % protected region % [Add custom VerifyEmailTwoFactorLogin traits here] end
		// % protected region % [Customize VerifyEmailTwoFactorLogin here] off begin
		/// <summary>
		/// Validate that an email two factor login generates an email with the correct code.
		/// </summary>
		[Fact]
		public async Task VerifyEmailTwoFactorLogin()
		{
			// Create a test user
			var user = await CreateTestUser(new User
			{
				UserName = TestUserName,
				Email = TestUserName,
			}, TokenOptions.DefaultEmailProvider);

			// Validate the two factor code that is sent in the email
			using var scope = _scopeFactory.CreateScope();
			var emailEvents = scope.ServiceProvider.GetRequiredService<EmailTwoFactorMethodEvents>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var emailService = (TestEmailService)scope.ServiceProvider.GetRequiredService<IEmailService>();

			var token = await userManager.GenerateUserTokenAsync(
				user,
				TokenOptions.DefaultEmailProvider,
				TwoFactorConstants.TokenPurpose);
			await emailEvents.OnLogin(user, token);

			emailService.SentEmails.Should().HaveCount(1);
			emailService.SentEmails.First().Body.Should().Contain($"<span id=\"token\">{token}</span>");
		}
		// % protected region % [Customize VerifyEmailTwoFactorLogin here] end

		// % protected region % [Customize UserTheoryData method here] off begin
		public static TheoryData<User> UserTheoryData() {
			return new()
			{
				CreateUserData<StaffEntity>(),
			};
		}
		// % protected region % [Customize UserTheoryData method here] end

		// % protected region % [Add custom VerifyValidTwoFactorEndpointResponse traits here] off begin
		// % protected region % [Add custom VerifyValidTwoFactorEndpointResponse traits here] end
		// % protected region % [Customize VerifyValidTwoFactorEndpointResponse here] off begin
		/// <summary>
		/// Validates that the endpoint to get the two factor methods for a user gets the correct methods.
		/// </summary>
		/// <param name="userData">A user to validate two factor methods for.</param>
		/// <typeparam name="T">The type of the user to test against.</typeparam>
		[Theory]
		[MemberData(nameof(UserTheoryData))]
		public async Task VerifyValidTwoFactorEndpointResponse<T>(T userData)
			where T : User, new()
		{
			// Create a test user
			var user = await CreateTestUser(userData);

			// Get the valid 2 factor methods from the endpoint
			using var scope = _scopeFactory.CreateScope();
			var accountController = scope.ServiceProvider.GetRequiredService<AccountController>();
			var validMethodResponse = await accountController.GetTwoFactorMethods(user.UserName);

			// Validate the response
			var response = validMethodResponse.Should().BeAssignableTo<OkObjectResult>();
			var methods = response.Subject.Value.Should().BeAssignableTo<IEnumerable<string>>();

			var methodList = methods.Subject.ToList();
			methodList.Should().HaveCountGreaterOrEqualTo(2);
			methodList.Should().Contain(i => i == TokenOptions.DefaultEmailProvider);
			methodList.Should().Contain(i => i == TokenOptions.DefaultAuthenticatorProvider);
		}
		// % protected region % [Customize VerifyValidTwoFactorEndpointResponse here] end

		// % protected region % [Customize UserMethodTheoryData here] off begin
		public static TheoryData<User, string> UserMethodTheoryData()
		{
			return new()
			{
				{ CreateUserData<StaffEntity>(), TokenOptions.DefaultEmailProvider },
				{ CreateUserData<StaffEntity>(), TokenOptions.DefaultAuthenticatorProvider },
			};
		}
		// % protected region % [Customize UserMethodTheoryData here] end

		// % protected region % [Add custom VerifyConfigureTwoFactorEndpoint traits here] off begin
		// % protected region % [Add custom VerifyConfigureTwoFactorEndpoint traits here] end
		// % protected region % [Customize VerifyConfigureTwoFactorEndpoint here] off begin
		/// <summary>
		/// Validates the functionality of the configure two factor endpoint.
		/// </summary>
		/// <param name="userData">A user to validate two factor methods for.</param>
		/// <param name="method">The two factor method to configure.</param>
		/// <typeparam name="T">The type of the user to test against.</typeparam>
		[Theory]
		[MemberData(nameof(UserMethodTheoryData))]
		public async Task VerifyConfigureTwoFactorEndpoint<T>(T userData, string method)
			where T : User, new()
		{
			// Create a test user
			var user = await CreateTestUser(userData);

			// Configure 2 factor for the provided method type
			using var scope = _scopeFactory.CreateScope();
			var accountController = scope.ServiceProvider.GetRequiredService<AccountController>();
			var validMethodResponse = await accountController.ConfigureTwoFactorAuthentication(new AccountController.ConfigureTwoFactorModel
			{
				Method = method,
				UserName = user.UserName,
			});
			// Validate the results
			var response = validMethodResponse.Should().BeAssignableTo<OkObjectResult>();
			var methods = response.Subject.Value.Should().BeAssignableTo<TwoFactorConfiguringResponse>();
			methods.Subject.Method.Should().Be(method);
		}
		// % protected region % [Customize VerifyConfigureTwoFactorEndpoint here] end

		// % protected region % [Add custom VerifyDisableTwoFactorEndpoint traits here] off begin
		// % protected region % [Add custom VerifyDisableTwoFactorEndpoint traits here] end
		// % protected region % [Customize VerifyDisableTwoFactorEndpoint here] off begin
		/// <summary>
		/// Validates the functionality of the disable two factor endpoint.
		/// </summary>
		/// <param name="userData">A user to validate two factor methods for.</param>
		/// <param name="method">The two factor method to test disabling of.</param>
		/// <typeparam name="T">The type of the user to test against.</typeparam>
		[Theory]
		[MemberData(nameof(UserMethodTheoryData))]
		public async Task VerifyDisableTwoFactorEndpoint<T>(T userData, string method)
			where T : User, new()
		{
			// Create a test user
			var user = await CreateTestUser(userData, method);

			// Configure 2 factor for the provided method type
			using (var scope = _scopeFactory.CreateScope())
			{
				var database = scope.ServiceProvider.GetRequiredService<Firstapp2257DBContext>();

				var testUser = await database.Users.AsNoTracking().FirstAsync(u => u.UserName == user.UserName);
				testUser.PreferredTwoFactorMethod.Should().Be(method);
				testUser.TwoFactorEnabled.Should().Be(true);
			}

			// Now disable the 2 factor method
			using (var scope = _scopeFactory.CreateScope())
			{
				var accountController = scope.ServiceProvider.GetRequiredService<AccountController>();
				await accountController.DisableTwoFactorAuthentication(new AccountController.DisableTwoFactorModel
				{
					UserName = user.UserName
				});
			}

			using (var scope = _scopeFactory.CreateScope())
			{
				var database = scope.ServiceProvider.GetRequiredService<Firstapp2257DBContext>();
				var testUser = await database.Users.FirstAsync(u => u.UserName == user.UserName);
				testUser.PreferredTwoFactorMethod.Should().Be(null);
				testUser.TwoFactorEnabled.Should().Be(false);
			}
		}
		// % protected region % [Customize VerifyDisableTwoFactorEndpoint here] end

		// % protected region % [Customize CreateTestUser here] off begin
		private async Task<User> CreateTestUser(User user, string method = null)
		{
			// Create the user in the database
			using var scope = _scopeFactory.CreateScope();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

			user.EmailConfirmed = true;
			await userManager.CreateAsync(user, "password");

			if (method != null)
			{
				var twoFactorMethodEventsFactory = scope.ServiceProvider.GetRequiredService<ITwoFactorMethodEventFactory>();
				var methodEvents = twoFactorMethodEventsFactory.GetTwoFactorMethodEvents(method);
				await methodEvents.OnConfiguring(user);
			}
			return user;
		}
		// % protected region % [Customize CreateTestUser here] end

		// % protected region % [Customize CreateUserData here] off begin
		private static T CreateUserData<T>()
			where T : User, new()
		{
			return new EntityFactory<T>(1)
				.UseAttributes()
				.UseReferences()
				.Generate()
				.First();
		}
		// % protected region % [Customize CreateUserData here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}