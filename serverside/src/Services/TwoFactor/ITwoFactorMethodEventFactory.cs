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
using System.Collections.Generic;
using System.Threading.Tasks;
using Firstapp2257.Models;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services.TwoFactor
{
	public interface ITwoFactorMethodEventFactory
	{
		// % protected region % [Customise GetTwoFactorMethodEvents method here] off begin
		/// <summary>
		/// Gets a two factor method event class for the given method type.
		/// </summary>
		/// <param name="method">The method type.</param>
		/// <returns>An events class for the given method type.</returns>
		/// <exception cref="System.InvalidOperationException">
		/// If the two factor method is invalid.
		/// </exception>
		ITwoFactorMethodEvents GetTwoFactorMethodEvents(string method);
		// % protected region % [Customise GetTwoFactorMethodEvents method here] end

		// % protected region % [Customise PickTwoFactorMethod list overload method here] off begin
		/// <summary>
		/// Picks a two factor method for a user from a list of methods. Only methods that have a transport method
		/// registered can be picked as a method.
		/// </summary>
		/// <param name="user">The user to pick the transport for.</param>
		/// <param name="methods">The list of methods to pick from.</param>
		/// <returns>The type of two factor method.</returns>
		string? PickTwoFactorMethod(User user, IList<string> methods);
		// % protected region % [Customise PickTwoFactorMethod list overload method here] end

		// % protected region % [Customise PickTwoFactorMethodAsync method here] off begin
		/// <summary>
		/// Picks a two factor method for a user from their valid methods. Only methods that have a transport method
		/// registered can be picked as a method.
		/// </summary>
		/// <param name="user">The user to pick the transport for.</param>
		/// <returns>The type of two factor method.</returns>
		Task<string?> PickTwoFactorMethodAsync(User user);
		// % protected region % [Customise PickTwoFactorMethodAsync method here] end

		// % protected region % [Customise GetConfigurableTwoFactorMethods method here] off begin
		/// <summary>
		/// Gets the two factor methods that are available to be configured for a user.
		/// </summary>
		/// <param name="user">The user to get the methods for.</param>
		/// <returns>A list of two factor methods to be configured.</returns>
		public Task<IEnumerable<string>> GetConfigurableTwoFactorMethods(User user);
		// % protected region % [Customise GetConfigurableTwoFactorMethods method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}
