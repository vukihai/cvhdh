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
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Firstapp2257.Models;
using Microsoft.AspNetCore.Identity;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Helpers
{
	/// <summary>
	/// Extension methods for the sign in manager class
	/// </summary>
	public static class SignInManagerExtensions
	{
		private static readonly MethodInfo SignInOrTwoFactorAsyncMethodInfo = typeof(SignInManager<User>)
			.GetMethod("SignInOrTwoFactorAsync", BindingFlags.Instance | BindingFlags.NonPublic);

		// % protected region % [Customise SignInOrTwoFactorAsync method here] off begin
		/// <summary>
		/// Signs in the specified <paramref name="user"/> if <paramref name="bypassTwoFactor"/> is set to false.
		/// Otherwise stores the <paramref name="user"/> for use after a two factor check.
		/// </summary>
		/// <param name="signInManager">The sign in manager to invoke the call on.</param>
		/// <param name="user">The user to sign in,</param>
		/// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
		/// <param name="loginProvider">The login provider to use. Default is null</param>
		/// <param name="bypassTwoFactor">Flag indicating whether to bypass two factor authentication. Default is false</param>
		/// <returns>Returns a <see cref="SignInResult"/></returns>
		public static async Task<SignInResult> SignInOrTwoFactorAsync(
			this SignInManager<User> signInManager,
			User user,
			bool isPersistent,
			string loginProvider = null,
			bool bypassTwoFactor = false)
		{
			// There is a method to do this on the sign in manager, however it is protected. Because of this we need to
			// use reflection to get the method before we can invoke it.

			// Create function parameters
			var userParam = Expression.Parameter(typeof(User), "user");
			var isPersistentParam = Expression.Parameter(typeof(bool), "isPersistent");
			var loginProviderParam = Expression.Parameter(typeof(string), "loginProvider");
			var bypassTwoFactorParam = Expression.Parameter(typeof(bool), "bypassTwoFactor");

			// Create function body
			var body = Expression.Call(
				Expression.Constant(signInManager),
				SignInOrTwoFactorAsyncMethodInfo,
				userParam,
				isPersistentParam,
				loginProviderParam,
				bypassTwoFactorParam);

			// Create expression and compile it to a function
			var func = Expression.Lambda<Func<User, bool, string, bool, Task<SignInResult>>>(
				body,
				userParam,
				isPersistentParam,
				loginProviderParam,
				bypassTwoFactorParam)
				.Compile();

			var result = await func(user, isPersistent, loginProvider, bypassTwoFactor);

			return result;
		}
		// % protected region % [Customise SignInOrTwoFactorAsync method here] end
	}
}