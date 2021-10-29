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
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Firstapp2257.Models;
using Firstapp2257.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Firstapp2257.Utility;
using Microsoft.AspNetCore.Identity;

namespace ServersideTests.Helpers
{
	public class ServerBuilderOptions
	{
		public delegate Action<IServiceProvider, DbContextOptionsBuilder> DbContextOptionsFunc(
			ServerBuilderOptions options,
			HostBuilderContext context);

		/// <summary>
		/// The name of the in memory database to use
		/// </summary>
		// % protected region % [Configure DatabaseName default] off begin
		public string DatabaseName { get; set; } = Path.GetRandomFileName();
		// % protected region % [Configure DatabaseName default] end

		/// <summary>
		/// Should the data seed helper be called to initialise data
		/// This will create the user super@example.com for testing
		/// </summary>
		// % protected region % [Configure InitialiseData default] off begin
		public bool InitialiseData { get; set; } = true;
		// % protected region % [Configure InitialiseData default] end

		/// <summary>
		/// The claims principal used to represent the testing user
		/// </summary>
		// % protected region % [Configure UserPrincipal default] off begin
		public Func<IServiceProvider, Task<ClaimsPrincipal>> UserPrincipalFactory { get; set; } = async sp =>
		{
			var userManager = sp.GetRequiredService<UserManager<User>>();
			var userService = sp.GetRequiredService<IUserService>();
			return await userService.CreateUserPrincipal(await userManager.FindByNameAsync("super@example.com"));
		};
		// % protected region % [Configure UserPrincipal default] end

		/// <summary>
		/// Configuration function for the database for the tests
		/// </summary>
		// % protected region % [Configure DatabaseOptions default] off begin
		public DbContextOptionsFunc DatabaseOptions { get; set; } = (builderOptions, hostBuilder) => (serviceProvider, options) =>
		{
			options.UseInMemoryDatabase(builderOptions.DatabaseName);
			options.ReplaceService<IQueryCompiler, CrudQueryCompiler>();
			options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
			options.UseOpenIddict<Guid>();
		};
		// % protected region % [Configure DatabaseOptions default] end

		/// <summary>
		/// Configure any additional services for the tests
		/// </summary>
		// % protected region % [Configure ConfigureServices default] off begin
		public Action<IServiceCollection, ServerBuilderOptions> ConfigureServices { get; set; } = null;
		// % protected region % [Configure ConfigureServices default] end

		/// <summary>
		/// Method that will run before any data seeding.
		/// </summary>
		// % protected region % [Configure BeforeDataSeeding default] off begin
		public Func<IHost, Task> BeforeDataSeeding { get; set; } = _ => Task.CompletedTask;
		// % protected region % [Configure BeforeDataSeeding default] end

		// % protected region % [Add any additional server builder options here] off begin
		// % protected region % [Add any additional server builder options here] end
	}
}