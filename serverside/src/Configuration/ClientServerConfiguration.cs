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
	/// <summary>
	/// Configuration options for the SPA clientside.
	/// </summary>
	public class ClientServerConfiguration
	{
		/// <summary>
		/// If set to true then then the application will proxy the Reactbot application from the url defined in
		/// <see cref="ProxyServerAddress"/>. Otherwise the server will start the application at the location defined
		/// at <see cref="ClientSourcePath"/>.
		/// </summary>
		public bool UseProxyServer { get; set; } = false;

		/// <summary>
		/// The url to proxy Reactbot from.
		/// </summary>
		public string ProxyServerAddress { get; set; } = "http://localhost:3000";

		/// <summary>
		/// The source path to start an in process Reactbot application from.
		/// </summary>
		public string ClientSourcePath { get; set; } = "../../clientside";

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end
	}
}