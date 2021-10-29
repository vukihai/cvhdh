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
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Configuration
{
	public class CookieConfiguration
	{
		/// <summary>
		/// Is storing claims from the UserClaims and RoleClaims tables enabled.
		/// </summary>
		public bool RuntimeClaimsEnabled { get; set; } = false;

		/// <summary>
		/// If this is enabled then cookies will be validated against the cookie store to check if they are enabled.
		/// </summary>
		public bool StoredCookieValidationEnabled { get; set; } = true;

		/// <summary>
		/// If this is enabled then cookies will be periodically validated against the security stamp of the user that
		/// has issued them.
		/// </summary>
		public bool CookieSecurityStampValidationEnabled { get; set; } = true;

		/// <summary>
		/// Interval in which the security stamp stored in the principal is compared against the user in minutes.
		/// Set this to 0 to check every request. In an appsettings file this can be configured by settings a string
		/// span value such as '2:30:00'. For more information see:
		/// https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings
		/// https://blog.ploeh.dk/2019/11/25/timespan-configuration-values-in-net-core/
		/// </summary>
		public TimeSpan CookieSecurityStampValidationInterval { get; set; } = TimeSpan.FromMinutes(1);

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end
	}
}