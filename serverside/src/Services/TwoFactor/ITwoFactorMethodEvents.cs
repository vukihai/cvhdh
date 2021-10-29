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
using System.Threading.Tasks;
using Firstapp2257.Models;
using Firstapp2257.Models.Internal.Identity;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services.TwoFactor
{
	/// <summary>
	/// A two factor method event is a class that is used for configuring the events for a two factor method.
	/// </summary>
	public interface ITwoFactorMethodEvents
	{
		// % protected region % [Customise OnLogin method here] off begin
		/// <summary>
		/// An event that is fired when logging in.
		/// </summary>
		/// <remarks>
		/// This event would normally be used to send a token to the user.
		/// </remarks>
		/// <param name="user">The user to perform the login event for.</param>
		/// <param name="token">The two factor token for the user.</param>
		/// <returns></returns>
		public Task<object> OnLogin(User user, string token);
		// % protected region % [Customise OnLogin method here] end

		// % protected region % [Customise OnConfiguring method here] off begin
		/// <summary>
		/// An event that is fired when the user wants to configure this two factor method.
		/// </summary>
		/// <remarks>
		/// This event would normally be used to configure set up/reset a users 2 factor key.
		/// </remarks>
		/// <param name="user"></param>
		/// <returns></returns>
		public Task<TwoFactorConfiguringResponse> OnConfiguring(User user);
		// % protected region % [Customise OnConfiguring method here] end

		// % protected region % [Customise OnRemoveMethod method here] off begin
		/// <summary>
		/// An event that is fired when the user wants to remove two factor authentication. This method is 
		/// </summary>
		/// <param name="user">The user to disable two factor for.</param>
		/// <returns>A task that is completed when the two factor method is removed.</returns>
		public Task OnRemoveMethod(User user);
		// % protected region % [Customise OnRemoveMethod method here] end

		// % protected region % [Customise CanConfigureMethod method here] off begin
		/// <summary>
		/// Can a user configure this two factor method.
		/// </summary>
		/// <remarks>
		/// An example of not being able to configure a method is email two factor auth will not be available if the
		/// user does not have a confirmed email.
		/// </remarks>
		/// <param name="user">The user to check the configuration against.</param>
		/// <returns>A task that contains a bool determining if the method can be configured.</returns>
		public Task<bool> CanConfigureMethod(User user);
		// % protected region % [Customise CanConfigureMethod method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}
