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
using System.Threading;
using System.Threading.Tasks;
using Firstapp2257.Helpers;
using Firstapp2257.Models;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Utility
{
	/// <summary>
	/// Hosted service to configure the server before it is started.
	/// </summary>
	public class InitializationHostedService : IHostedService
	{
		private readonly ILogger<AuditLog> _logger;
		private readonly JobStorage _jobStorage;
		private readonly IServiceScopeFactory _serviceScopeFactory;
		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end

		// ReSharper disable once ContextualLoggerProblem
		public InitializationHostedService(
			// % protected region % [Add any extra constructor parameters here] off begin
			// % protected region % [Add any extra constructor parameters here] end
			ILogger<AuditLog> logger,
			JobStorage jobStorage,
			IServiceScopeFactory serviceScopeFactory)
		{
			_logger = logger;
			_jobStorage = jobStorage;
			_serviceScopeFactory = serviceScopeFactory;
			// % protected region % [Add any extra constructor logic here] off begin
			// % protected region % [Add any extra constructor logic here] end
		}
		/// <inheritdoc />
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			// % protected region % [Customise audit configuration method here] off begin
			Audit.Core.Configuration.Setup()
				.UseDynamicProvider(configurator =>
				{
					configurator.OnInsert(audit => AuditUtilities.LogAuditEvent(audit, _logger));
					configurator.OnReplace((_, audit) => AuditUtilities.LogAuditEvent(audit, _logger));
				});
			// % protected region % [Customise audit configuration method here] end

			// % protected region % [Customise job storage initialization here] off begin
			JobStorage.Current = _jobStorage;
			// % protected region % [Customise job storage initialization here] end

			// % protected region % [Customise data seeding here] off begin
			// Create a new service scope since DataSeedHelper is a scoped service
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				await scope.ServiceProvider.GetRequiredService<DataSeedHelper>().Initialize(cancellationToken);
			}
			// % protected region % [Customise data seeding here] end

			// % protected region % [Add any additional start logic here] off begin
			// % protected region % [Add any additional start logic here] end
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			// % protected region % [Add any additional stop logic here] off begin
			// % protected region % [Add any additional stop logic here] end
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}