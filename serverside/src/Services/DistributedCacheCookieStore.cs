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
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Firstapp2257.Models;
using Firstapp2257.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services
{
	/// <summary>
	/// Distributed cache implementation of <see cref="ICookieStore"/>.
	/// </summary>
	public class DistributedCacheCookieStore : ICookieStore
	{
		// % protected region % [Customise constants here] off begin
		private const string CachePrefix = "LoginCookies";
		// % protected region % [Customise constants here] end

		// % protected region % [Customise class fields here] off begin
		private readonly IDistributedCache _distributedCache;
		// % protected region % [Customise class fields here] end

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end

		// % protected region % [Customise the constructor method here] off begin
		public DistributedCacheCookieStore(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache;
		}
		// % protected region % [Customise the constructor method here] end

		// % protected region % [Customise StoreCookieData method here] off begin
		/// <inheritdoc />
		public async Task StoreCookieData(
			string tokenId,
			string userId,
			CookieData cookieData,
			CookieStorageOptions options,
			CancellationToken cancellationToken = default)
		{
			cookieData.Id = tokenId;
			await _distributedCache.SetAsync(
				GetCacheId(tokenId, userId),
				JsonSerializer.SerializeToUtf8Bytes(cookieData),
				new DistributedCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
					SlidingExpiration = options.SlidingExpiration,
				},
				cancellationToken);
		}
		// % protected region % [Customise StoreCookieData method here] end

		// % protected region % [Customise GetCookieData method here] off begin
		/// <inheritdoc />
		public async Task<CookieData> GetCookieData(
			string tokenId,
			string userId,
			CancellationToken cancellationToken = default)
		{
			var cookieData = await _distributedCache.GetAsync(GetCacheId(tokenId, userId), cancellationToken);
			return cookieData is not null
				? JsonSerializer.Deserialize<CookieData>(cookieData)
				: null;
		}
		// % protected region % [Customise GetCookieData method here] end

		// % protected region % [Customise RemoveCookieData method here] off begin
		/// <inheritdoc />
		public async Task RemoveCookieData(
			string tokenId,
			string userId,
			CancellationToken cancellationToken = default)
		{
			await _distributedCache.RemoveAsync(GetCacheId(tokenId, userId), cancellationToken);
		}
		// % protected region % [Customise RemoveCookieData method here] end

		// % protected region % [Customise GetCacheId method here] off begin
		private static string GetCacheId(string tokenId, string userId)
		{
			return $"{CachePrefix}:{userId}:{tokenId}";
		}
		// % protected region % [Customise GetCacheId method here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}