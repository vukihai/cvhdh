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

using SeleniumTests.Setup;
// % protected region % [Add further imports here] off begin
// % protected region % [Add further imports here] end

namespace SeleniumTests.PageObjects.BotWritten.Admin
{
	public class AdminPage : BasePage 
	{
		// % protected region % [Override Class Properties here] off begin
		public override string Url => baseUrl + "/admin";
		// % protected region % [Override Class Properties here] end
		// % protected region % [Override class constructor here] off begin
		public AdminPage(ContextConfiguration currentContext) : base(currentContext)
		{
		}
		// % protected region % [Override class constructor here] end
		// % protected region % [Add extra methods here] off begin
		// % protected region % [Add extra methods here] end
	}
}