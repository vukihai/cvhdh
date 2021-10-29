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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServersideTests.Helpers;
using ServersideTests.Helpers.EntityFactory;
using Firstapp2257.Controllers;
using Firstapp2257.Models;
using Microsoft.AspNetCore.Http;
using Xunit;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace ServersideTests.Tests.Integration.BotWritten
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Unit")]
	public class DeactivatedUserTests
	{
		// % protected region % [Add custom StaffEntity traits here] off begin
		// % protected region % [Add custom StaffEntity traits here] end
		// % protected region % [Customize CreateAndValidateUser Test for StaffEntity] off begin
		[Fact]
		public async void StaffEntityDeactivatedLoginTest()
		{
			await CreateAndValidateUser<StaffEntity>();
		}
		// % protected region % [Customize CreateAndValidateUser Test for StaffEntity] end


		// % protected region % [Customize CreateAndValidateUser method here] off begin
		private static async Task CreateAndValidateUser<T>()
			where T : User, new()
		{
			using var host = ServerBuilder.CreateServer();

			var controller = host.Services.GetRequiredService<AuthorizationController>();
			var userManager = host.Services.GetRequiredService<UserManager<User>>();
			var httpContextAccessor = host.Services.GetRequiredService<IHttpContextAccessor>();
			controller.ControllerContext = new ControllerContext
			{
				HttpContext = httpContextAccessor.HttpContext,
			};

			// Create a user with the user manager
			var entity = new EntityFactory<T>()
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.First();

			var id = Guid.NewGuid().ToString();
			entity.UserName = id;
			entity.Email = $"{id}@example.com";
			entity.NormalizedUserName = entity.UserName.ToUpper();
			entity.NormalizedEmail = entity.Email.ToUpper();
			entity.EmailConfirmed = false;
			await userManager.CreateAsync(entity, "password");

			var result = await controller.Login(new LoginDetails
			{
				Username = entity.UserName,
				Password = "password"
			});

			Assert.Equal(typeof(UnauthorizedObjectResult), result.GetType());
		}
		// % protected region % [Customize CreateAndValidateUser method here] end

		// % protected region % [Customize DeactivatedUserTheoryData method here] off begin
		public static TheoryData<User> DeactivatedUserTheoryData() {
			return new()
			{
				new StaffEntity(),
			};
		}
		// % protected region % [Customize DeactivatedUserTheoryData method here] end
		// % protected region % [Add custom traits here] off begin
		// % protected region % [Add custom traits here] end
		// % protected region % [Customize DeactivateEndpointTest here] off begin
		[Theory]
		[MemberData(nameof(DeactivatedUserTheoryData))]
#pragma warning disable xUnit1026
		public async Task DeactivateEndpointTest<T>(T userType)
#pragma warning restore
			where T : User, IAbstractModel, new()
		{
			using var host = ServerBuilder.CreateServer();

			// Create user to test against
			var entity = new EntityFactory<T>()
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.First();

			var userName = Guid.NewGuid().ToString();
			var email = $"{userName}@example.com";
			entity.UserName = userName;
			entity.Email = email;
			entity.EmailConfirmed = true;
			entity.NormalizedUserName = userName.ToUpper();
			entity.NormalizedEmail = email.ToUpper();

			// Save the user to the database
			using (var scope = host.Services.CreateScope())
			{
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
				await userManager.CreateAsync(entity, "password");
			}

			// Test that we can login as the new user
			using (var scope = host.Services.CreateScope())
			{
				var authorizationController = scope.ServiceProvider.GetTestController<AuthorizationController>();
				var initialResult = await authorizationController.Login(new LoginDetails
				{
					Username = entity.UserName,
					Password = "password"
				});
				Assert.Equal(typeof(OkObjectResult), initialResult.GetType());
			}

			// Deactivate the account
			using (var scope = host.Services.CreateScope())
			{
				var accountController = scope.ServiceProvider.GetTestController<AccountController>();
				await accountController.DeactivateUser(new AccountController.UsernameModel
				{
					Username = entity.UserName,
				});
			}

			// Test that we can't login now the account is deactivated
			using (var scope = host.Services.CreateScope())
			{
				var failAuthController = scope.ServiceProvider.GetTestController<AuthorizationController>();
				var deactivatedResult = await failAuthController.Login(new LoginDetails
				{
					Username = entity.UserName,
					Password = "password"
				});
				Assert.Equal(typeof(UnauthorizedObjectResult), deactivatedResult.GetType());
			}
		}
		// % protected region % [Customize DeactivateEndpointTest here] end

		// % protected region % [Add any additional methods here] off begin
		// % protected region % [Add any additional methods here] end
	}
}