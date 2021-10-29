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
using Firstapp2257.Services.Interfaces;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Utility
{
	public class ServiceProviderActivator : JobActivator
	{
		private readonly IServiceScopeFactory _scopeFactory;
		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end

		// % protected region % [Customise constructor here] off begin
		public ServiceProviderActivator(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}
		// % protected region % [Customise constructor here] end

		// % protected region % [Customise begin scope here] off begin
		public override JobActivatorScope BeginScope(PerformContext context)
		{
			var serviceScope = _scopeFactory.CreateScope();
			serviceScope.ServiceProvider.GetRequiredService<IPerformContextAccessor>().PerformContext = context;
			return new ServiceProviderActivatorScope(serviceScope);
		}
		// % protected region % [Customise begin scope here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
	public class ServiceProviderActivatorScope : JobActivatorScope
	{
		private readonly IServiceScope _serviceScope;
		// % protected region % [Add any extra scope fields here] off begin
		// % protected region % [Add any extra scope fields here] end

		// % protected region % [Customise scope constructor here] off begin
		public ServiceProviderActivatorScope(IServiceScope serviceScope)
		{
			_serviceScope = serviceScope;
		}
		// % protected region % [Customise scope constructor here] end

		// % protected region % [Customise resolve here] off begin
		public override object Resolve(Type type)
		{
			return _serviceScope.ServiceProvider.GetService(type);
		}
		// % protected region % [Customise resolve here] end

		// % protected region % [Customise scope disposal here] off begin
		public override void DisposeScope()
		{
			_serviceScope.Dispose();
		}
		// % protected region % [Customise scope disposal here] end

		// % protected region % [Add any extra scope methods here] off begin
		// % protected region % [Add any extra scope methods here] end
	}
}