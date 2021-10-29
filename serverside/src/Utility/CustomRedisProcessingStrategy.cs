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
using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using Firstapp2257.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Utility
{
	public class CustomRedisProcessingStrategy : RedisProcessingStrategy
	{
		// % protected region % [Customise constants here] off begin
		public const string RateLimitingPrefix = "RateLimiting:";
		// % protected region % [Customise constants here] end

		// % protected region % [Customise class fields here] off begin
		private readonly IOptions<RedisConfiguration> _redisConfig;
		// % protected region % [Customise class fields here] end

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end

		// % protected region % [Customise the constructor method here] off begin
		public CustomRedisProcessingStrategy(
			IConnectionMultiplexer connectionMultiplexer,
			IRateLimitConfiguration config,
			IOptions<RedisConfiguration> redisConfig,
			ILogger<RedisProcessingStrategy> logger)
			: base(connectionMultiplexer, config, logger)
		{
			_redisConfig = redisConfig;
		}
		// % protected region % [Customise the constructor method here] end

		// % protected region % [Customise BuildCounterKey here] off begin
		protected override string BuildCounterKey(ClientRequestIdentity requestIdentity, RateLimitRule rule, ICounterKeyBuilder counterKeyBuilder,
			RateLimitOptions rateLimitOptions)
		{
			return _redisConfig.Value.InstanceName +
				RateLimitingPrefix +
				base.BuildCounterKey(requestIdentity, rule, counterKeyBuilder, rateLimitOptions);
		}
		// % protected region % [Customise BuildCounterKey here] end

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}