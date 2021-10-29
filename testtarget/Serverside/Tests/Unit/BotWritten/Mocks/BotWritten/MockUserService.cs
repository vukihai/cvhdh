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
using Firstapp2257.Configuration;
using Firstapp2257.Models;
using Firstapp2257.Services;
using Firstapp2257.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using RazorLight;

namespace ServersideTests.Mocks
{
	public class MockUserService: Mock<UserService>
	{
		// % protected region % [Configure MockUserService here] off begin
		public MockUserService(
			IOptions<IdentityOptions> identityOptions,
			IUserClaimsPrincipalFactory<User> claimsPrincipalFactory,
			IServiceProvider serviceProvider,
			UserManager<User> userManager,
			RoleManager<Group> roleManager,
			RazorLightEngine razorLightEngine,
			IBackgroundJobService backgroundJobService,
			IOptions<ServerSettings> serverSettings) :
			base(
				identityOptions,
				claimsPrincipalFactory,
				serviceProvider,
				userManager,
				roleManager,
				razorLightEngine,
				backgroundJobService,
				serverSettings)
		{

		}
		// % protected region % [Configure MockUserService here] end

		// % protected region % [Configure GetMockUserService here] off begin
		public static MockUserService GetMockUserService(
			IOptions<IdentityOptions> identityOptions = null,
			IUserClaimsPrincipalFactory<User> claimsPrincipalFactory = null,
			IServiceProvider serviceProvider = null,
			UserManager<User> userManager = null,
			RoleManager<Group> roleManager = null,
			RazorLightEngine razorLightEngine = null,
			IBackgroundJobService backgroundJobService = null,
			IOptions<ServerSettings> serverSettings = null)
		{
			return new MockUserService(
				identityOptions ?? new Mock<IOptions<IdentityOptions>>().Object,
				claimsPrincipalFactory ?? new Mock<IUserClaimsPrincipalFactory<User>>().Object,
				serviceProvider ?? new Mock<IServiceProvider>().Object,
				userManager ?? MockUserManager.GetMockUserManager().Object,
				roleManager ?? MockRoleManager.GetMockRoleManager().Object,
				razorLightEngine ?? new RazorLightEngine(new Mock<IEngineHandler>().Object),
				backgroundJobService ?? new Mock<IBackgroundJobService>().Object,
				serverSettings ?? new Mock<IOptions<ServerSettings>>().Object);
		}
		// % protected region % [Configure GetMockUserService here] end
	}
}
