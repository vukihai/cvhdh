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
using System.Threading;
using System.Threading.Tasks;
using Firstapp2257.Models;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services.Interfaces
{
	/// <summary>
	/// Storage interface for login cookies.
	/// </summary>
	public interface ICookieStore
	{
		/// <summary>
		/// Store an application cookie.
		/// </summary>
		/// <param name="tokenId">The id of of the cookie.</param>
		/// <param name="userId">The user id of the user who owns the cookie.</param>
		/// <param name="cookieData">Data on the cookie.</param>
		/// <param name="options">Storage options for the cookie.</param>
		/// <param name="cancellationToken">Cancellation token for the operation.</param>
		/// <returns>A task that is completed when the operation is completed.</returns>
		Task StoreCookieData(
			string tokenId,
			string userId,
			CookieData cookieData,
			CookieStorageOptions options,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets data about a cookie for a user.
		/// </summary>
		/// <param name="tokenId">The id of of the cookie get information about.</param>
		/// <param name="userId">The user who owns the cookie.</param>
		/// <param name="cancellationToken">Cancellation token for the operation.</param>
		/// <returns>
		/// A task that is completed when the operation is completed and contains information about the requested
		/// cookie. Will return null if there is no cookie that matches the provided token and user ids.
		/// </returns>
		Task<CookieData> GetCookieData(string tokenId, string userId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Removes data about a cookie.
		/// </summary>
		/// <param name="tokenId">The id of of the cookie to delete.</param>
		/// <param name="userId">The user who owns the cookie.</param>
		/// <param name="cancellationToken">Cancellation token for the operation.</param>
		/// <returns>A task that is completed when the operation is completed.</returns>
		Task RemoveCookieData(string tokenId, string userId, CancellationToken cancellationToken = default);

		// % protected region % [Add any extra fields or methods here] off begin
		// % protected region % [Add any extra fields or methods here] end
	}

	public class CookieStorageOptions
	{
		/// <summary>
		/// Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
		/// This will not extend the entry lifetime beyond the absolute expiration (if set).
		/// </summary>
		public TimeSpan? SlidingExpiration { get; set; }

		/// <summary>
		/// Gets or sets an absolute expiration time, relative to now.
		/// </summary>
		public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

		// % protected region % [Add any extra CookieStorageOptions fields or methods here] off begin
		// % protected region % [Add any extra CookieStorageOptions fields or methods here] end
	}
}