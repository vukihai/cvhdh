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
using AspNet.Security.OpenIdConnect.Primitives;
using Firstapp2257.Exceptions;
using Firstapp2257.Models;
using Microsoft.AspNetCore.Authentication;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services.Interfaces
{
	public interface ISignInService
	{
		/// <summary>
		/// Check the username and password of a user. This method does not verify two factor authentication requirements.
		/// </summary>
		/// <param name="username">The username of the user</param>
		/// <param name="password">The password of the user</param>
		/// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails</param>
		/// <param name="validateLockout">Whether to check if the email has been confirmed</param>
		/// <param name="validateNotAllowed">Whether to check if the account is not allowed to log in.</param>
		/// <returns>On success returns the user object, on failure throws an exception</returns>
		/// <exception cref="InvalidUserPasswordException">On the username or password being invalid</exception>
		Task<User> CheckCredentials(
			string username,
			string password,
			bool lockoutOnFailure = true,
			bool validateLockout = true,
			bool validateNotAllowed = true);
		/// <summary>
		/// Creates a authentication ticket to identify a user
		/// </summary>
		/// <param name="request">The OpenId request for the user</param>
		/// <returns>The authentication ticket for the user</returns>
		/// <exception cref="InvalidUserPasswordException">Thrown when an invalid username or password is provided</exception>
		/// <exception cref="InvalidGrantTypeException">Thrown when an invalid OpenId grant type is provided</exception>
		Task<AuthenticationTicket> Exchange(OpenIdConnectRequest request);

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end
	}
}