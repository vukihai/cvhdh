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
using Firstapp2257.Configuration;
using Firstapp2257.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services
{
	public class RedisConnectionService : IRedisConnectionService, IDisposable
	{
		/// <inheritdoc />
		public bool Enabled { get; }

		/// <inheritdoc />
		public ConnectionMultiplexer Connection { get; set; }

		// % protected region % [Alter constructor here] off begin
		public RedisConnectionService(
			IOptions<RedisConfiguration> configuration,
			ILogger<RedisConnectionService> logger)
		{
			if (!configuration.Value.Enabled)
			{
				return;
			}

			try
			{
				if (configuration.Value.ConfigurationOptions != null)
				{
					Connection = ConnectionMultiplexer.Connect(configuration.Value.ConfigurationOptions);
				}
				else if (IsEnabledFromConnectionString(configuration.Value.Configuration))
				{
					Connection = ConnectionMultiplexer.Connect(configuration.Value.Configuration);
				}
				Enabled = true;
				logger.LogInformation("Connected to Redis instance at {Host}", Connection.Configuration);
			}
			catch (Exception e)
			{
				logger.LogWarning("Unable to connect to Redis: {Message}", e.Message);
				logger.LogInformation("If this is a development environment then consider setting" +
					" Redis:Enabled in appsettings to false for a faster startup time");
				Enabled = false;
			}
		}
		// % protected region % [Alter constructor here] end

		// % protected region % [Alter IsEnabledFromConnectionString here] off begin
		public static bool IsEnabledFromConnectionString(string connectionString)
		{
			return !string.IsNullOrWhiteSpace(connectionString);
		}
		// % protected region % [Alter IsEnabledFromConnectionString here] end

		// % protected region % [Alter Dispose here] off begin
		public void Dispose()
		{
			Connection?.Dispose();
		}
		// % protected region % [Alter Dispose here] end

		// % protected region % [Add any extra fields or methods here] off begin
		// % protected region % [Add any extra fields or methods here] end
	}
}