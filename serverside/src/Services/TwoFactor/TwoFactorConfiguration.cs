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
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services.TwoFactor
{
	public class TwoFactorMethodDescriptor
	{
		/// <summary>
		/// The type of the two factor method events class. This class must be registered in the dependency injection
		/// container as it will be constructed from the service provider.
		/// </summary>
		public Type MethodType { get; set; }

		/// <summary>
		/// The priority of the 2 factor method. If a user has no preferred two factor method and multiple methods
		/// configured, then they will use the method with the highest priority.
		/// </summary>
		public int Priority { get; set; }

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end
	}

	/// <summary>
	/// Configuration class for two factor methods.
	/// </summary>
	public class TwoFactorConfiguration : Dictionary<string, TwoFactorMethodDescriptor>
	{
		// % protected region % [Customise AddTwoFactorMethod method here] off begin
		/// <summary>
		/// Adds a two factor event method to the global configuration.
		/// </summary>
		/// <param name="key">
		/// The key for the method. This should be the same as the key for the token provider in the user manager.
		/// </param>
		/// <param name="priority">The priority of the two factor method.</param>
		/// <typeparam name="T">The type of the two factor event method.</typeparam>
		public void AddTwoFactorMethod<T>(string key, int? priority = null)
		{
			Add(key, new TwoFactorMethodDescriptor
			{
				MethodType = typeof(T),
				Priority = priority ?? 0,
			});
		}
		// % protected region % [Customise AddTwoFactorMethod method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}
