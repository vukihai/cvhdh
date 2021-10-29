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

namespace Firstapp2257.Models
{
	/// <summary>
	/// The data about cookies that are stored in the cookie store.
	/// </summary>
	public class CookieData
	{
		/// <summary>
		/// Id of the cookie.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// IP of the client when they were issued the cookie.
		/// </summary>
		public string InitialIp { get; set; }

		/// <summary>
		/// Port of the client when they were issued the cookie.
		/// </summary>
		public int InitialPort { get; set; }

		/// <summary>
		/// Timestamp when the cookie was originally issued. Timezone is UTC.
		/// </summary>
		public DateTime IssueDateTime { get; set; }

		/// <summary>
		/// Trace id of the http context that issued the cookie.
		/// </summary>
		public string IssuingContextId { get; set; }

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end
	}
}