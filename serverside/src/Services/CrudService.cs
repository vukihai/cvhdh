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
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Firstapp2257.Enums;
using Firstapp2257.Helpers;
using Firstapp2257.Models;
using Firstapp2257.Models.Internal;
using Firstapp2257.Utility;
using Firstapp2257.Models.RegistrationModels;
using Firstapp2257.Services.Interfaces;
using Firstapp2257.Services.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services
{
	public class CrudService : ICrudService, IDisposable
	{
		private readonly IDbContextFactory<Firstapp2257DBContext> _dbContextFactory;
		private readonly UserManager<User> _userManager;
		private readonly ISecurityService _securityService;
		private readonly IIdentityService _identityService;
		private readonly ILogger<CrudService> _logger;
		private readonly IServiceProvider _serviceProvider;
		private readonly IUploadStorageProvider _storageProvider;
		private readonly IAuditService _auditService;

		private readonly List<IDisposable> _disposables = new();

		public CrudService(
			IDbContextFactory<Firstapp2257DBContext> dbContextFactory,
			UserManager<User> userManager,
			ISecurityService securityService,
			IIdentityService identityService,
			ILogger<CrudService> logger,
			IServiceProvider serviceProvider,
			IUploadStorageProvider storageProvider,
			IUserService userService,
			IAuditService auditService)
		{
			_dbContextFactory = dbContextFactory;
			_userManager = userManager;
			_securityService = securityService;
			_identityService = identityService;
			_logger = logger;
			_serviceProvider = serviceProvider;
			_storageProvider = storageProvider;
			_auditService = auditService;
		}

		/// <inheritdoc />
		public IQueryable<T> GetById<T>(Guid id, ReadOptions options = null)
			where T : class, IOwnerAbstractModel, new()
		{
			return Get<T>(options: options).Where(model => model.Id == id);
		}

		/// <inheritdoc />
		public IQueryable<T> Get<T>(
			Pagination pagination = null,
			object auditFields = null,
			ReadOptions options = null)
			where T : class, IOwnerAbstractModel, new()
		{
			options ??= new ReadOptions();
			_identityService.RetrieveUserAsync().Wait();
			Firstapp2257DBContext dbContext;
			if (options.DbContext == null)
			{
				dbContext = _dbContextFactory.CreateDbContext();
				_disposables.Add(dbContext);
			}
			else
			{
				dbContext = options.DbContext;
			}
			var dbSet = dbContext.Set<T>() as IQueryable<T>;

			if (!options.DisableAudit)
			{
				_auditService.CreateReadAudit(
					_identityService.User?.Id.ToString(),
					_identityService.User?.UserName,
					typeof(T).Name,
					auditFields);
			}

			// % protected region % [Do extra things after get] off begin
			// % protected region % [Do extra things after get] end

			if (!options.DisableSecurity)
			{
				dbSet = dbSet.AddReadSecurityFiltering(_identityService, _userManager, dbContext, _serviceProvider);
			}

			return dbSet.AddPagination(pagination);
		}

		/// <inheritdoc />
		public async Task<T> Create<T>(
			T model,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			var result = await Create(new List<T> {model}, options, cancellation);
			return result.First();
		}

		/// <inheritdoc />
		public async Task<ICollection<T>> Create<T>(
			ICollection<T> models,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			await _identityService.RetrieveUserAsync();
			await using var dbContext = _dbContextFactory.CreateDbContext();
			var dbSet = dbContext.Set<T>();

			await using (var transaction = await dbContext.Database.BeginTransactionAsync(cancellation))
			{
				try
				{
					await MergeReferences(models, options, dbContext, cancellation);

					foreach (var model in models)
					{
						// Update is used here so references are properly handled
						dbSet.Update(model);
					}

					// Ensure that we create all of the base entities instead of updating
					var addedEntries = dbContext
						.ChangeTracker
						.Entries()
						.Where(entry => models.Contains(entry.Entity));
					foreach (var entry in addedEntries)
					{
						entry.State = EntityState.Added;
					}

					var fileModels = dbContext
						.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity)
						.ToList();
					if (options?.Files != null)
					{
						await SaveFiles(fileModels, options.Files, dbContext, cancellation);
					}
					else
					{
						ClearFileAttributes(models);
					}

					await AssignModelMetaData(_identityService.User, dbContext, cancellation);
					ValidateModels(dbContext.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity));

					// % protected region % [Do extra things after create] off begin
					// % protected region % [Do extra things after create] end

					var errors = await CheckDbSecurityChanges(dbContext, options);
					if (errors.Any())
					{
						throw new AggregateException(
							errors.Select(error => new InvalidOperationException(error)));
					}

					foreach (var entry in dbContext.ChangeTracker.Entries<IAbstractModel>().ToList())
					{
						await entry.Entity.BeforeSave(entry.State, dbContext, _serviceProvider, cancellation);
					}

					var changes = dbContext
						.ChangeTracker
						.Entries<IAbstractModel>()
						.Select(e => new ChangeState { State = e.State, Entry = e })
						.ToList();
					await dbContext.SaveChangesAsync(cancellation);

					foreach (var change in changes)
					{
						await change.Entry.Entity.AfterSave(
							change.State,
							dbContext,
							_serviceProvider,
							changes,
							cancellation);
					}

					await transaction.CommitAsync(cancellation);

					return models;
				}
				catch (Exception e)
				{
					await transaction.RollbackAsync(cancellation);
					_logger.LogError("Error completing create action - {Error}", e.ToString());
					throw;
				}
			}
		}

		/// <inheritdoc />
		public async Task<T> Update<T>(
			T model,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			var result = await Update(new List<T> {model}, options, cancellation);
			return result.First();
		}

		/// <inheritdoc />
		public async Task<ICollection<T>> Update<T>(
			ICollection<T> models,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			await _identityService.RetrieveUserAsync();
			await using var dbContext = _dbContextFactory.CreateDbContext();
			var dbSet = dbContext.Set<T>();

			await using (var transaction = await dbContext.Database.BeginTransactionAsync(cancellation))
			{
				try
				{
					await MergeReferences(models, options, dbContext, cancellation);

					foreach (var model in models)
					{
						var entry = dbContext.ChangeTracker.Entries<IAbstractModel>()
							.FirstOrDefault(x => x.Entity.Id == model.Id);
						if (entry != null)
						{
							entry.State = EntityState.Detached;
						}
						dbSet.Update(model);
					}

					var fileModels = dbContext
						.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity)
						.ToList();
					if (options?.Files != null)
					{
						await SaveFiles(fileModels, options.Files, dbContext, cancellation);
					}
					else
					{
						ClearFileAttributes(models);
					}

					await AssignModelMetaData(_identityService.User, dbContext, cancellation);
					ValidateModels(dbContext.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity));

					// % protected region % [Do extra things after update] off begin
					// % protected region % [Do extra things after update] end

					var errors = await CheckDbSecurityChanges(dbContext, options);
					if (errors.Any())
					{
						throw new AggregateException(
							errors.Select(error => new InvalidOperationException(error)));
					}

					foreach (var entry in dbContext.ChangeTracker.Entries<IAbstractModel>().ToList())
					{
						await entry.Entity.BeforeSave(entry.State, dbContext, _serviceProvider, cancellation);
					}


					var changes = dbContext
						.ChangeTracker
						.Entries<IAbstractModel>()
						.Select(e => new ChangeState { State = e.State, Entry = e })
						.ToList();
					await dbContext.SaveChangesAsync(cancellation);

					foreach (var change in changes)
					{
						await change.Entry.Entity.AfterSave(
							change.State,
							dbContext,
							_serviceProvider,
							changes,
							cancellation);
					}

					await transaction.CommitAsync(cancellation);

					return models;
				}
				catch (Exception e)
				{
					await transaction.RollbackAsync(cancellation);
					_logger.LogError("Error completing update action - {Error}", e.ToString());
					throw;
				}
			}
		}

		/// <inheritdoc />
		public async Task<BooleanObject> ConditionalUpdate<T>(
			IQueryable<T> models,
			MemberInitExpression updateMemberInitExpression,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			options ??= new UpdateOptions();
			await using var dbContext = _dbContextFactory.CreateDbContext();
			var param = Expression.Parameter(typeof(T), "model");
			var replacer = new ParameterReplacer(param);
			var updateFactory = Expression.Lambda<Func<T, T>>(replacer.Visit(updateMemberInitExpression), param);
			if (!options.DisableSecurity)
			{
				models = models.AddUpdateSecurityFiltering(_identityService, _userManager, dbContext, _serviceProvider);
			}
			await models.UpdateAsync(updateFactory, cancellation);

			// % protected region % [Do extra things after conditional update] off begin
			// % protected region % [Do extra things after conditional update] end

			var errors = await CheckDbSecurityChanges(dbContext, options);
			if (errors.Any())
			{
				throw new AggregateException(errors.Select(error => new InvalidOperationException(error)));
			}
			await dbContext.SaveChangesAsync(cancellation);
			return new BooleanObject { Value = true };
		}

		/// <inheritdoc />
		public async Task<Guid> Delete<T>(
			Guid id,
			CrudOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IAbstractModel
		{
			var result = await Delete<T>(new List<Guid> {id}, options, cancellation);
			return result.First();
		}

		/// <inheritdoc />
		public async Task<ICollection<Guid>> Delete<T>(
			List<Guid> ids,
			CrudOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IAbstractModel
		{
			await _identityService.RetrieveUserAsync();
			await using var dbContext = _dbContextFactory.CreateDbContext();
			var dbSet = dbContext.Set<T>();

			await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellation);
			var models = await dbSet.Where(o => ids.Contains(o.Id)).ToListAsync(cancellation);

			dbSet.RemoveRange(models);

			// % protected region % [Do extra things after delete] off begin
			// % protected region % [Do extra things after delete] end

			var errors = await CheckDbSecurityChanges(dbContext, options);
			if (errors.Any())
			{
				throw new AggregateException(errors.Select(error => new InvalidOperationException(error)));
			}

			await AssignModelMetaData(_identityService.User, dbContext, cancellation);

			foreach (var entry in dbContext.ChangeTracker.Entries<IAbstractModel>().ToList())
			{
				await entry.Entity.BeforeSave(entry.State, dbContext, _serviceProvider);
			}

			var changes = dbContext
				.ChangeTracker
				.Entries<IAbstractModel>()
				.Select(e => new ChangeState { State = e.State, Entry = e })
				.ToList();
			try {
				await dbContext.SaveChangesAsync(cancellation);
			}
			catch (Exception e)
			{
				var exceptionData = e.InnerException.Data;
				if (exceptionData.Contains("Detail"))
				{
					string errorMessage = exceptionData["Detail"].ToString();
					throw new AggregateException(new InvalidOperationException(errorMessage));
				}
				throw new AggregateException(new InvalidOperationException(e.Message));
			}

			foreach (var change in changes)
			{
				await change.Entry.Entity.AfterSave(change.State, dbContext, _serviceProvider, changes);
			}

			transaction.Commit();

			return ids;
		}

		/// <inheritdoc />
		public async Task<BooleanObject> ConditionalDelete<T>(
			IQueryable<T> models,
			CrudOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			options ??= new CrudOptions();
			await using var dbContext = _dbContextFactory.CreateDbContext();
			try
			{
				if (!options.DisableSecurity)
				{
					models = models.AddDeleteSecurityFiltering(
						_identityService,
						_userManager,
						dbContext,
						_serviceProvider);
				}
				await models.DeleteAsync(cancellation);
			}
			catch (Exception e) {
				var exceptionData = e.Data;
				if (exceptionData.Contains("Detail"))
				{
					string errorMessage = exceptionData["Detail"].ToString();
					throw new AggregateException(new InvalidOperationException(errorMessage));
				}
				throw new AggregateException(new InvalidOperationException(e.Message));
			}

			// % protected region % [Do extra things after conditional delete] off begin
			// % protected region % [Do extra things after conditional delete] end

			var errors = await CheckDbSecurityChanges(dbContext, options);
			if (errors.Any())
			{
				throw new AggregateException(errors.Select(error => new InvalidOperationException(error)));
			}
			await dbContext.SaveChangesAsync(cancellation);
			return new BooleanObject { Value = true };
		}

		/// <inheritdoc />
		public async Task<ICollection<User>> CreateUser<TModel, TRegisterModel>(
			ICollection<TRegisterModel> models,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where TModel : User, IOwnerAbstractModel, new()
			where TRegisterModel : IRegistrationModel<TModel>
		{
			// % protected region % [Customise the registration before any action] off begin
			// % protected region % [Customise the registration before any action] end
			await _identityService.RetrieveUserAsync();
			await using var dbContext = _dbContextFactory.CreateDbContext();

			var dbModels = models.Select(m => m.ToModel()).ToList();
			var dbSet = dbContext.Set<TModel>();

			var roles = models.SelectMany(m => m.Groups).ToList();
			var dbRoles = await dbContext.Roles.Where(r => roles.Contains(r.Name)).ToListAsync(cancellation);
			// % protected region % [Customise the registration after initial lookups] off begin
			// % protected region % [Customise the registration after initial lookups] end


			await using (var transaction = await dbContext.Database.BeginTransactionAsync(cancellation))
			{
				try
				{
					await MergeReferences(dbModels, options, dbContext, cancellation);

					var modelPairs = models.Select(model => (model, model.ToModel())).ToList();

					// Create each user
					var createdUsers = new List<User>();
					foreach (var (registrationModel, model) in modelPairs)
					{
						// % protected region % [Customise for each registration model] off begin
						// % protected region % [Customise for each registration model] end

						if (model.Id == Guid.Empty)
						{
							model.Id = Guid.NewGuid();
						}

						foreach (var group in registrationModel.Groups)
						{
							var role = dbRoles.First(r => r.Name == group);
							dbContext.UserRoles.Add(new IdentityUserRole<Guid>{UserId = model.Id, RoleId = role.Id});
						}

						// % protected region % [Customise user model here] off begin
						// % protected region % [Customise user model here] end

						model.UserName ??= model.Email;
						model.PasswordHash = _userManager.PasswordHasher.HashPassword(
							model,
							registrationModel.Password);
						model.ConcurrencyStamp = await _userManager.GenerateConcurrencyStampAsync(model);
						model.NormalizedEmail = _userManager.NormalizeEmail(model.Email);
						model.NormalizedUserName = _userManager.NormalizeName(model.UserName);
						model.EmailConfirmed = true;
						model.SecurityStamp = Guid.NewGuid().ToString();

						// Run the user and password validators over the model
						var validationTasks = _userManager
							.UserValidators
							.Select(v => v.ValidateAsync(_userManager, model))
							.ToList();
						validationTasks.AddRange(_userManager
							.PasswordValidators
							.Select(v => v.ValidateAsync(_userManager, model, registrationModel.Password)));
						var validationResults = await Task.WhenAll(validationTasks);
						var failed = validationResults.Where(r => r.Succeeded == false).ToList();
						if (failed.Any())
						{
							throw new AggregateException(failed
								.SelectMany(f => f.Errors.Select(s => s.Description))
								.Select(s => new InvalidOperationException(s)));
						}

						// % protected region % [Customise user model after validation here] off begin
						// % protected region % [Customise user model after validation here] end

						dbSet.Update(model);
						createdUsers.Add(model);
					}

					// Ensure that we create all of the base entities instead of updating
					var addedEntries = dbContext.ChangeTracker.Entries()
						.Where(entry => createdUsers.Contains(entry.Entity));
					foreach (var entry in addedEntries)
					{
						entry.State = EntityState.Added;
					}

					var fileModels = dbContext
						.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity)
						.ToList();
					if (options?.Files != null)
					{
						await SaveFiles(fileModels, options.Files, dbContext, cancellation);
					}
					else
					{
						ClearFileAttributes(models);
					}

					await AssignModelMetaData(_identityService.User, dbContext, cancellation);
					ValidateModels(dbContext.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity));

					foreach (var user in createdUsers)
					{
						// % protected region % [Add in any custom creates for each created user] off begin
						// % protected region % [Add in any custom creates for each created user] end

						user.Owner = user.Id;

						// % protected region % [Add in any custom creates for each created user after owner assignment] off begin
						// % protected region % [Add in any custom creates for each created user after owner assignment] end
					}

					var errors = await CheckDbSecurityChanges(dbContext, options);
					if (errors.Any())
					{
						throw new AggregateException(
							errors.Select(error => new InvalidOperationException(error)));
					}

					foreach (var entry in dbContext.ChangeTracker.Entries<IAbstractModel>().ToList())
					{
						await entry.Entity.BeforeSave(entry.State, dbContext, _serviceProvider, cancellation);
					}

					var changes = dbContext
						.ChangeTracker
						.Entries<IAbstractModel>()
						.Select(e => new ChangeState { State = e.State, Entry = e })
						.ToList();
					await dbContext.SaveChangesAsync(cancellation);

					foreach (var change in changes)
					{
						await change.Entry.Entity.AfterSave(
							change.State,
							dbContext,
							_serviceProvider,
							changes,
							cancellation);
					}

					await transaction.CommitAsync(cancellation);

					return createdUsers;
				}
				catch (Exception e)
				{
					await transaction.RollbackAsync(cancellation);
					_logger.LogError("Error completing create user action - {Error}", e.ToString());
					throw;
				}
			}
		}

		/// <inheritdoc />
		public async Task<UploadFile> GetFile(
			Guid id,
			CrudOptions options = null,
			CancellationToken cancellation = default)
		{
			// % protected region % [Override GetFile here] off begin
			options ??= new CrudOptions();
			await _identityService.RetrieveUserAsync();
			await using var dbContext = _dbContextFactory.CreateDbContext();

			IQueryable<UploadFile> fileQuery = dbContext.Files;

			if (!options.DisableSecurity)
			{
				var baseParam = Expression.Parameter(typeof(UploadFile), "file");

				var conditions = new Expression[]
				{
					Expression.Constant(false),
				};

				var securityCondition = Expression.Lambda<Func<UploadFile, bool>>(
					ExpressionHelper.OrElse(conditions),
					baseParam);

				fileQuery = fileQuery.Where(securityCondition);
			}

			var file = await fileQuery.FirstOrDefaultAsync(f => f.Id == id, cancellation);

			if (file == null)
			{
				throw new FileNotFoundException("File not found");
			}

			return file;
			// % protected region % [Override GetFile here] end
		}

		private Expression CreateFileSecurityCondition<T>(
			Firstapp2257DBContext dbContext,
			Expression parameter,
			string propertyName)
			where T : IOwnerAbstractModel, new()
		{
			var entityFilter = SecurityService.CreateReadSecurityFilter<T>(
				_identityService,
				_userManager,
				dbContext,
				_serviceProvider);
			var subQueryParam = Expression.PropertyOrField(parameter, propertyName);
			return ParameterReplacer.Replace(
				subQueryParam,
				entityFilter.Parameters.First(),
				entityFilter.Body);
		}

		private async Task SaveFiles<T>(
			IEnumerable<T> models,
			IFormFileCollection files,
			Firstapp2257DBContext dbContext,
			CancellationToken cancellation = default)
		{
			foreach (var model in models)
			{
				var modelType = model.GetType();
				var fileAttrs = ReflectionCache.GetFileAttributes(modelType);

				if (fileAttrs.Count <= 0)
				{
					continue;
				}

				foreach (var attr in fileAttrs)
				{
					if (!(attr.GetValue(model) is Guid fileId))
					{
						continue;
					}

					var file = files.FirstOrDefault(f => f.Name == fileId.ToString());

					if (file == null)
					{
						continue;
					}

					await using var fileStream = file.OpenReadStream();
					var serverFileId = Guid.NewGuid().ToString();
					await _storageProvider.PutAsync(new StoragePutOptions
					{
						Container = modelType.Name,
						FileName = serverFileId,
						Content = fileStream,
						ContentType = file.ContentType,
						CreateContainerIfNotExists = true,
					}, cancellation);

					var dbFile = new UploadFile
					{
						Id = Guid.NewGuid(),
						FileName = file.FileName,
						FileId = serverFileId,
						Length = file.Length,
						ContentType = file.ContentType,
						Container = modelType.Name,
					};

					var existingFile = (await dbContext.Entry(model).GetDatabaseValuesAsync(cancellation))?[attr.Name];

					attr.SetValue(model, dbFile.Id);
					dbContext.Files.Add(dbFile);

					if (existingFile is Guid oldFileId)
					{
						var fileEntry = await dbContext
							.Files
							.FirstOrDefaultAsync(f => f.Id == oldFileId, cancellation);
						if (fileEntry != null)
						{
							dbContext.Files.Remove(fileEntry);
						}
					}
				}
			}
		}

		private static void ClearFileAttributes<T>(IEnumerable<T> models)
		{
			foreach (var model in models)
			{
				var modelType = model.GetType();
				var fileAttrs = ReflectionCache.GetFileAttributes(modelType);

				foreach (var attr in fileAttrs)
				{
					attr.SetValue(model, null);
				}
			}
		}

		private async Task AssignModelMetaData(
			User user,
			Firstapp2257DBContext dbContext,
			CancellationToken cancellation = default)
		{
			foreach (var entry in dbContext.ChangeTracker.Entries<IOwnerAbstractModel>())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.Created = DateTime.UtcNow;
						entry.Entity.Modified = DateTime.UtcNow;
						if (entry.Entity.Owner == Guid.Empty)
						{
							entry.Entity.Owner = user?.Id ?? Guid.Empty;
						}

						// If we haven't been given an id to create against then we need to make a new one
						if (entry.Entity.Id == Guid.Empty)
						{
							entry.Entity.Id = Guid.NewGuid();
						}

						// % protected region % [Add any additional metadata before create here] off begin
						// % protected region % [Add any additional metadata before create here] end

						break;
					case EntityState.Modified:
						// Unset fields we don't want to be changed on update
						entry.Property("Owner").IsModified = false;
						entry.Property("Created").IsModified = false;
						entry.Entity.Modified = DateTime.UtcNow;

						// % protected region % [Add any additional metadata before update here] off begin
						// % protected region % [Add any additional metadata before update here] end

						break;
				}
			}

			// Users have a concurrency stamp so they need to be pulled from the db and have
			// the concurrency stamp applied to each of the objects we save back to the database
			var userEntries = dbContext.ChangeTracker
				.Entries<User>()
				.ToList();

			// When updating users the core object should not ever change
			foreach (var entry in userEntries)
			{
				switch (entry.State)
				{
					case EntityState.Added:
						// A user should always own themselves
						entry.Entity.Owner = entry.Entity.Id;
						break;
					case EntityState.Modified:
						// % protected region % [Adjust modification of user models here] off begin
						var databaseProperties = await entry.GetDatabaseValuesAsync(cancellation);
						var proposedProperties = entry.CurrentValues;

						foreach (var userProperty in GetNonModifiableUserProperties())
						{
							proposedProperties[userProperty] = databaseProperties[userProperty];
						}

						entry.OriginalValues.SetValues(databaseProperties);
						entry.Property("Discriminator").IsModified = false;
						// % protected region % [Adjust modification of user models here] end
						break;
				}
			}

			// % protected region % [Add any metadata here] off begin
			// % protected region % [Add any metadata here] end
		}

		// % protected region % [Configure the modelled groups here] off begin
		/// <summary>
		/// Return the list of fields that should not be modified on the User.cs Entity.
		/// </summary>
		/// <returns></returns>
		private List<string> GetNonModifiableUserProperties()
		{
			return typeof(User)
				.GetProperties()
				.Select(p => p.Name)
				.Where(n => n != "Acls")
				.ToList();
		}
		// % protected region % [Configure the modelled groups here] end

		private static void ValidateModels<T>(IEnumerable<T> models)
		{
			var validationExceptions = models.SelectMany(model =>
			{
				var errors = new List<ValidationResult>();
				model.ValidateObjectFields(errors);
				if (errors.Count > 0)
				{
					return new List<ValidationException>
					{
						new ValidationException(string.Join(
							"; ",
							errors.Select(e => e.ErrorMessage).ToArray()))
					};
				}
				return new List<ValidationException>();
			}).ToList();

			if (validationExceptions.Count > 0)
			{
				throw new AggregateException(validationExceptions);
			}
		}

		private async Task MergeReferences<T>(
			ICollection<T> models,
			UpdateOptions options,
			Firstapp2257DBContext dbContext,
			CancellationToken cancellation = default)
			where T : IOwnerAbstractModel, new()
		{
			if (options == null) return;

			var referencesToMerge = typeof(T)
				.GetProperties()
				.SelectMany(prop => prop
					.GetCustomAttributes(typeof(EntityForeignKey), false)
					.Select(attr => attr as EntityForeignKey))
				.Where(attr => options.MergeReferences != null &&
								options.MergeReferences.Contains(attr?.Name, StringComparer.OrdinalIgnoreCase))
				.ToList();

			if (options.MergeReferences != null)
			{
				foreach (var reference in options.MergeReferences)
				{
					try
					{
						var foreignAttribute = referencesToMerge.First(attr => string
							.Equals(attr?.Name, reference, StringComparison.OrdinalIgnoreCase));
						await models.First().CleanReference(foreignAttribute.Name, models, dbContext, cancellation);
					}
					catch
					{
						// ignored
					}
				}
			}
		}

		private async Task<List<string>> CheckDbSecurityChanges(
			Firstapp2257DBContext dbContext,
			CrudOptions options)
		{
			if (options != null && options.DisableSecurity)
			{
				return new List<string>();
			}
			return await _securityService.CheckDbSecurityChanges(_identityService, dbContext);
		}

		// % protected region % [Add extra functions in crudService] off begin
		// % protected region % [Add extra functions in crudService] end

		private static IEnumerable<string> GetExportProperties<T>()
		{
			var properties = ObjectHelper.GetNonReferenceProperties(typeof(T)).ToArray();
			var propertyNames = properties.Select(p => char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1)).ToList();
			if (propertyNames.Contains("owner"))
			{
				propertyNames.Remove("owner");
			}

			return propertyNames;
		}

		public void Dispose()
		{
			foreach (var disposable in _disposables)
			{
				disposable.Dispose();
			}
		}
	}

	public class PaginationOptions
	{
		/// <summary>
		/// What page to fetch for pagination
		/// </summary>
		public int? PageNo { get; set; }

		/// <summary>
		/// The size of each page for pagination
		/// </summary>
		public int? PageSize { get; set; }
	}

	public class Pagination
	{
		public Pagination() { }

		public Pagination(PaginationOptions options)
		{
			PageNo = options?.PageNo;
			PageSize = options?.PageSize;
		}

		public int? PageSize { get; set; }
		public int? PageNo { get; set; }

		public int? SkipAmount
		{
			get
			{
				if (!isValid())
				{
					return null;
				}
				return (PageNo - 1) * PageSize;
			}
		}

		public bool isValid()
		{
			return PageSize != null && PageSize > 0 && PageNo != null && PageNo > 0;
		}
	}

	public class ExportParameters
	{
		[Required]
		[DataType(DataType.Date)]
		public DateTime FromDate { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime ToDate { get; set; }
	}

	public class CrudOptions
	{
		public bool DisableSecurity { get; set; }
	}

	public class UpdateOptions : CrudOptions
	{
		public IEnumerable<string> MergeReferences { get; set; }
		public IFormFileCollection Files { get; set; }
	}

	public class ReadOptions : CrudOptions
	{
		public bool DisableAudit { get; set; } = false;
		public Firstapp2257DBContext DbContext { get; set; }
	}
}