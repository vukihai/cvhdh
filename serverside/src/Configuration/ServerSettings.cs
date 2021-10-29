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
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Configuration
{
	public class ServerSettings
	{
		public const string SectionName = "ServerSettings";
		/// <summary>
		/// This url is the url that is provided in the forgot password email.
		/// Changing this url will not alter the URL the application is served on.
		/// To change that alter Properties/launchSettings.json instead.
		/// </summary>
		public string ServerUrl { get; set; }

		/// <summary>
		/// Is the web server configured to use HTTPS when it is running. This should be set to true if the client
		/// device receives a HTTPS page, so if this application is only sending HTTP but an ingress in front of it is
		/// serving HTTPS then this should still be enabled.
		/// </summary>
		public bool IsHttps { get; set; } = false;

		// % protected region % [Add any extra configuration properties here] off begin
		// % protected region % [Add any extra configuration properties here] end
	}
}