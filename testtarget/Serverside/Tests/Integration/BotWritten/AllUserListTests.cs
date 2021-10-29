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
using Firstapp2257.Models.Internal;
using Firstapp2257.Services;
using GraphQL.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ServersideTests.Helpers;
using ServersideTests.Helpers.EntityFactory;
using Xunit;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace ServersideTests.Tests.Integration.BotWritten
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Unit")]
	public class AllUserListTests
	{
		// % protected region % [Add custom traits here] off begin
		// % protected region % [Add custom traits here] end
		// % protected region % [Customise GetAllUsersEndpointTest crud tests here] off begin
		[Fact]
		public async Task GetAllUsersEndpointTest()
		{
			using var host = ServerBuilder.CreateServer();

			var users = new User[]
			{
				CreateUser<StaffEntity>(),
			};
			var userIds = users.Select(x => x.Id).ToList();

			// Create the users in the database for the test
			using (var scope = host.Services.CreateScope())
			{
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
				foreach (var user in users)
				{
					await userManager.CreateAsync(user);
				}
			}

			// Assert that these users are correctly listed
			using (var scope = host.Services.CreateScope())
			{
				const int perPage = 20;
				var controller = scope.ServiceProvider.GetTestController<AccountController>();
				var result = await controller.GetUsers(new AccountController.AllUserRequestModel
				{
					PaginationOptions = new PaginationOptions
					{
						PageNo = 1,
						PageSize = perPage,
					},
					SortConditions = new List<OrderBy>
					{
						new()
						{
							Descending = true,
							Path = "id",
						}
					},
					SearchConditions = new []
					{
						users.Select(x => new WhereExpression
						{
							Path = "id",
							Comparison = Comparison.Equal,
							Value = new []{ x.Id.ToString() },
						})
					}
				});
				var resultUserList = result.Users.ToList();

				Assert.Equal(users.Length, result.countUsers);
				Assert.Equal(Math.Min(perPage, users.Length), resultUserList.Count);
				foreach (var resultUser in resultUserList)
				{
					Assert.Contains(resultUser.Id, userIds);
				}
			}
		}
		// % protected region % [Customise GetAllUsersEndpointTest crud tests here] end

		// % protected region % [Customise CreateUser crud tests here] off begin
		private static T CreateUser<T>()
			where T : User, new()
		{
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

			return entity;
		}
		// % protected region % [Customise CreateUser crud tests here] end
	}
}