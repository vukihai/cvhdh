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
#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firstapp2257.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services.TwoFactor
{
	public class TwoFactorMethodEventFactory : ITwoFactorMethodEventFactory
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly UserManager<User> _userManager;
		private readonly IOptions<TwoFactorConfiguration> _configuration;

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end

		// % protected region % [Alter constructor here] off begin
		public TwoFactorMethodEventFactory(
			IServiceProvider serviceProvider,
			UserManager<User> userManager,
			IOptions<TwoFactorConfiguration> configuration)
		{
			_serviceProvider = serviceProvider;
			_userManager = userManager;
			_configuration = configuration;
		}
		// % protected region % [Alter constructor here] end

		// % protected region % [Customise GetTwoFactorMethodEvents method here] off begin
		/// <inheritdoc />
		public ITwoFactorMethodEvents GetTwoFactorMethodEvents(string method)
		{
			if (_configuration.Value.TryGetValue(method, out var eventsDescriptor))
			{
				return (ITwoFactorMethodEvents)_serviceProvider.GetRequiredService(eventsDescriptor.MethodType);
			}

			throw new InvalidOperationException("Invalid 2 factor method");
		}
		// % protected region % [Customise GetTwoFactorMethodEvents method here] end

		// % protected region % [Customise PickTwoFactorMethod method here] off begin
		/// <inheritdoc />
		public string? PickTwoFactorMethod(User user, IList<string> methods)
		{
			// If a user has a preferred method then use that one
			if (!string.IsNullOrWhiteSpace(user.PreferredTwoFactorMethod) && 
				_configuration.Value.ContainsKey(user.PreferredTwoFactorMethod) &&
				methods.Contains(user.PreferredTwoFactorMethod))
			{
				return user.PreferredTwoFactorMethod;
			}

			// Other wise find the valid two factor methods and use the one with the highest priority.
			var validKeys = methods.Intersect(_configuration.Value.Keys);
			return _configuration.Value
				.Where(x => validKeys.Contains(x.Key))
				.OrderByDescending(x => x.Value.Priority)
				.Select(x => x.Key)
				.FirstOrDefault();
		}
		// % protected region % [Customise PickTwoFactorMethod method here] end

		// % protected region % [Customise PickTwoFactorMethodAsync method here] off begin
		/// <inheritdoc />
		public async Task<string?> PickTwoFactorMethodAsync(User user)
		{
			var methods = await _userManager.GetValidTwoFactorProvidersAsync(user);
			return PickTwoFactorMethod(user, methods);
		}
		// % protected region % [Customise PickTwoFactorMethodAsync method here] end

		// % protected region % [Customise GetConfigurableTwoFactorMethods method here] off begin
		/// <inheritdoc />
		public async Task<IEnumerable<string>> GetConfigurableTwoFactorMethods(User user)
		{
			var methods = new List<string>();

			foreach (var (key, value) in _configuration.Value)
			{
				var method = (ITwoFactorMethodEvents) _serviceProvider.GetRequiredService(value.MethodType);
				if (await method.CanConfigureMethod(user))
				{
					methods.Add(key);
				}
			}

			return methods;
		}
		// % protected region % [Customise GetConfigurableTwoFactorMethods method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}
