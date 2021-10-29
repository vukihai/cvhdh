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

// % protected region % [Add further imports here] off begin
// % protected region % [Add further imports here] end

namespace SeleniumTests.PageObjects.BotWritten.UIModeled
{
	public interface IUIModeledComponent
	{
		/// <summary>
		/// For a component modeled in the UI builder in the codebots platform (tile or page)
		/// this method will check that the child components exists on that modeled component.
		/// </summary>
		/// <returns>if all of the child components were found on the UI Modeled Component</returns>
		// % protected region % [Override interface properties here] off begin
		public bool ContainsModeledElements();
		// % protected region % [Override interface properties here] end
	}
}