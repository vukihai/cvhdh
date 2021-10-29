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
using FluentAssertions;
using Firstapp2257.Controllers.Entities;
using Firstapp2257.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServersideTests.Helpers;
using ServersideTests.Helpers.EntityFactory;
using Xunit;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Add any additional imports here] off begin
// % protected region % [Add any additional imports here] end

namespace ServersideTests.Tests.Integration.BotWritten
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Unit")]
	public class CrudTests : IDisposable
	{
		private readonly IHost _host;
		private readonly Firstapp2257DBContext _database;
		private readonly IServiceScope _scope;
		private readonly IServiceProvider _serviceProvider;
		// % protected region % [Add any additional members here] off begin
		// % protected region % [Add any additional members here] end

		public CrudTests()
		{
			// % protected region % [Configure constructor here] off begin
			_host = ServerBuilder.CreateServer();
			_scope = _host.Services.CreateScope();
			_serviceProvider = _scope.ServiceProvider;
			_database = _serviceProvider.GetRequiredService<Firstapp2257DBContext>();
			// % protected region % [Configure constructor here] end
		}
		public void Dispose()
		{
			// % protected region % [Configure dispose here] off begin
			_host?.Dispose();
			_database?.Dispose();
			_scope?.Dispose();
			// % protected region % [Configure dispose here] end
		}

		// % protected region % [Add custom Achievements Entity traits here] off begin
		// % protected region % [Add custom Achievements Entity traits here] end
		// % protected region % [Customise Achievements Entity crud tests here] off begin
		[Fact]
		public async void AchievementsEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<AchievementsEntityController>();
			var entities = new EntityFactory<AchievementsEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Achievements Entity crud tests here] end
		// % protected region % [Add custom Address Entity traits here] off begin
		// % protected region % [Add custom Address Entity traits here] end
		// % protected region % [Customise Address Entity crud tests here] off begin
		[Fact]
		public async void AddressEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<AddressEntityController>();
			var entities = new EntityFactory<AddressEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Address Entity crud tests here] end
		// % protected region % [Add custom Students Entity traits here] off begin
		// % protected region % [Add custom Students Entity traits here] end
		// % protected region % [Customise Students Entity crud tests here] off begin
		[Fact]
		public async void StudentsEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<StudentsEntityController>();
			var entities = new EntityFactory<StudentsEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Students Entity crud tests here] end
		// % protected region % [Add custom Assessment Notes Entity traits here] off begin
		// % protected region % [Add custom Assessment Notes Entity traits here] end
		// % protected region % [Customise Assessment Notes Entity crud tests here] off begin
		[Fact]
		public async void AssessmentNotesEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<AssessmentNotesEntityController>();
			var entities = new EntityFactory<AssessmentNotesEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Assessment Notes Entity crud tests here] end
		// % protected region % [Add custom Assessments Entity traits here] off begin
		// % protected region % [Add custom Assessments Entity traits here] end
		// % protected region % [Customise Assessments Entity crud tests here] off begin
		[Fact]
		public async void AssessmentsEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<AssessmentsEntityController>();
			var entities = new EntityFactory<AssessmentsEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Assessments Entity crud tests here] end
		// % protected region % [Add custom Comments Entity traits here] off begin
		// % protected region % [Add custom Comments Entity traits here] end
		// % protected region % [Customise Comments Entity crud tests here] off begin
		[Fact]
		public async void CommentsEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<CommentsEntityController>();
			var entities = new EntityFactory<CommentsEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Comments Entity crud tests here] end
		// % protected region % [Add custom Staff Entity traits here] off begin
		// % protected region % [Add custom Staff Entity traits here] end
		// % protected region % [Customise Staff Entity crud tests here] off begin
		[Fact]
		public async void StaffEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<StaffEntityController>();
			var entities = new EntityFactory<StaffEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Staff Entity crud tests here] end
		// % protected region % [Add custom Achievements Submission Entity traits here] off begin
		// % protected region % [Add custom Achievements Submission Entity traits here] end
		// % protected region % [Customise Achievements Submission Entity crud tests here] off begin
		[Fact]
		public async void AchievementsSubmissionEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<AchievementsSubmissionEntityController>();
			var entities = new EntityFactory<AchievementsSubmissionEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Achievements Submission Entity crud tests here] end
		// % protected region % [Add custom Address Submission Entity traits here] off begin
		// % protected region % [Add custom Address Submission Entity traits here] end
		// % protected region % [Customise Address Submission Entity crud tests here] off begin
		[Fact]
		public async void AddressSubmissionEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<AddressSubmissionEntityController>();
			var entities = new EntityFactory<AddressSubmissionEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Address Submission Entity crud tests here] end
		// % protected region % [Add custom Students Submission Entity traits here] off begin
		// % protected region % [Add custom Students Submission Entity traits here] end
		// % protected region % [Customise Students Submission Entity crud tests here] off begin
		[Fact]
		public async void StudentsSubmissionEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<StudentsSubmissionEntityController>();
			var entities = new EntityFactory<StudentsSubmissionEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Students Submission Entity crud tests here] end
		// % protected region % [Add custom Achievements Entity Form Tile Entity traits here] off begin
		// % protected region % [Add custom Achievements Entity Form Tile Entity traits here] end
		// % protected region % [Customise Achievements Entity Form Tile Entity crud tests here] off begin
		[Fact]
		public async void AchievementsEntityFormTileEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<AchievementsEntityFormTileEntityController>();
			var entities = new EntityFactory<AchievementsEntityFormTileEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Achievements Entity Form Tile Entity crud tests here] end
		// % protected region % [Add custom Address Entity Form Tile Entity traits here] off begin
		// % protected region % [Add custom Address Entity Form Tile Entity traits here] end
		// % protected region % [Customise Address Entity Form Tile Entity crud tests here] off begin
		[Fact]
		public async void AddressEntityFormTileEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<AddressEntityFormTileEntityController>();
			var entities = new EntityFactory<AddressEntityFormTileEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Address Entity Form Tile Entity crud tests here] end
		// % protected region % [Add custom Students Entity Form Tile Entity traits here] off begin
		// % protected region % [Add custom Students Entity Form Tile Entity traits here] end
		// % protected region % [Customise Students Entity Form Tile Entity crud tests here] off begin
		[Fact]
		public async void StudentsEntityFormTileEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<StudentsEntityFormTileEntityController>();
			var entities = new EntityFactory<StudentsEntityFormTileEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise Students Entity Form Tile Entity crud tests here] end

	// % protected region % [Add any additional tests here] off begin
	// % protected region % [Add any additional tests here] end
	}
}