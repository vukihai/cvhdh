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
using System.Threading;
using System.Threading.Tasks;
using Firstapp2257.Models;
using Firstapp2257.Services.Interfaces;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services
{
	public class Firstapp2257GraphQlContext : Dictionary<string, object>
	{
		public Firstapp2257DBContext DbContext { get; set; }
		public User User { get; set; }
		public IList<string> UserGroups { get; set; }
		public ISecurityService SecurityService { get; set; }
		public UserManager<User> UserManager { get; set; }
		public IUserService UserService { get; set; }
		public ICrudService CrudService { get; set; }
		public IIdentityService IdentityService { get; set; }
		public IServiceProvider ServiceProvider { get; set; }
		public IAuditService AuditService { get; set; }
		public IFormFileCollection Files { get; set; }

		// % protected region % [Add any extra user context fields here] off begin
		// % protected region % [Add any extra user context fields here] end
	}

	public class GraphQlService : IGraphQlService
	{
		private readonly IDocumentExecuter _executer;
		private readonly ISchema _schema;
		private readonly Firstapp2257DBContext _dataContext;
		private readonly ISecurityService _securityService;
		private readonly UserManager<User> _userManager;
		private readonly IUserService _userService;
		private readonly ICrudService _crudService;
		private readonly IIdentityService _identityService;
		private readonly IServiceProvider _serviceProvider;
		private readonly DataLoaderDocumentListener _dataLoaderDocumentListener;
		private readonly IAuditService _auditService;

		// % protected region % [Add any extra class fields here] off begin
		// % protected region % [Add any extra class fields here] end

		public GraphQlService(
			// % protected region % [Add any extra constructor arguments here] off begin
			// % protected region % [Add any extra constructor arguments here] end
			ISchema schema,
			IDocumentExecuter executer,
			Firstapp2257DBContext dataContext,
			ISecurityService securityService,
			UserManager<User> userManager,
			IUserService userService,
			ICrudService crudService,
			IServiceProvider serviceProvider,
			IIdentityService identityService,
			DataLoaderDocumentListener dataLoaderDocumentListener,
			IAuditService auditService)
		{
			_schema = schema;
			_executer = executer;
			_dataContext = dataContext;
			_securityService = securityService;
			_userManager = userManager;
			_userService = userService;
			_crudService = crudService;
			_identityService = identityService;
			_serviceProvider = serviceProvider;
			_dataLoaderDocumentListener = dataLoaderDocumentListener;
			_auditService = auditService;
			// % protected region % [Add any extra constructor logic here] off begin
			// % protected region % [Add any extra constructor logic here] end
		}

		/// <inheritdoc />
		public async Task<ExecutionResult> Execute(
			string query,
			string operationName,
			Inputs variables,
			IFormFileCollection attachments,
			User user,
			CancellationToken cancellation)
		{
			// % protected region % [Override Execute method here] off begin
			await _identityService.RetrieveUserAsync();

			var executionOptions = new ExecutionOptions
			{
				Schema = _schema,
				Query = query,
				OperationName = operationName,
				Inputs = variables,
				UserContext = new Firstapp2257GraphQlContext
				{
					DbContext = _dataContext,
					User = user,
					UserGroups = _identityService.Groups,
					SecurityService = _securityService,
					CrudService = _crudService,
					IdentityService = _identityService,
					UserManager = _userManager,
					UserService = _userService,
					ServiceProvider = _serviceProvider,
					AuditService = _auditService,
					Files = attachments,
				},
				CancellationToken = cancellation,
#if (DEBUG)
				EnableMetrics = true,
#endif
			};

			executionOptions.Listeners.Add(_dataLoaderDocumentListener);

			var result = await _executer.ExecuteAsync(executionOptions)
				.ConfigureAwait(false);

			return result;
			// % protected region % [Override Execute method here] end
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}