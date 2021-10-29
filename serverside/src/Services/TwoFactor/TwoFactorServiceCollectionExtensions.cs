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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services.TwoFactor
{
	public static class TwoFactorServiceCollectionExtensions
	{
		// % protected region % [Customise AddTwoFactorAuthenticationMethod method here] off begin
		/// <summary>
		/// Adds a two factor method to the service collection.
		/// </summary>
		/// <param name="services">The service collection to augment.</param>
		/// <param name="key">
		/// The name of the two factor method. This name must match a token provider in the user manager.
		/// </param>
		/// <param name="priority">
		/// The priority of the two factor method. If a user has no preferred two factor method and multiple methods
		/// configured, then they will use the method with the highest priority.
		/// </param>
		/// <typeparam name="T">The type of the two factor method event class.</typeparam>
		/// <returns>A service collection for chaining methods.</returns>
		public static IServiceCollection AddTwoFactorAuthenticationMethod<T>(
			this IServiceCollection services,
			string key,
			int? priority = null)
			where T : class, ITwoFactorMethodEvents
		{
			services.TryAddScoped<T>();
			services.Configure<TwoFactorConfiguration>(config =>
			{
				config.AddTwoFactorMethod<T>(key, priority);
			});

			return services;
		}
		// % protected region % [Customise AddTwoFactorAuthenticationMethod method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}
