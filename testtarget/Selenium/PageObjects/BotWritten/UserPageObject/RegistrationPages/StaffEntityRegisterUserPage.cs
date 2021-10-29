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
using APITests.EntityObjects.Models;
using OpenQA.Selenium;
using SeleniumTests.PageObjects.Components;
using SeleniumTests.Setup;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.PageObjects.BotWritten.UserPageObjects
{
	public class RegisterStaffEntityUserPage : RegisterUserBasePage
	{
		public override string Url => baseUrl + "/login";
		public IWebElement FirstNameInput => FindElementExt("FirstNameInput");
		public IWebElement LastNameInput => FindElementExt("LastNameInput");

		public RegisterStaffEntityUserPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			InitializeSelectors();
		}

		private void InitializeSelectors()
		{
			selectorDict.Add("FirstNameInput", (selector: "div.firstName > input", type: SelectorType.CSS));
			selectorDict.Add("LastNameInput", (selector: "div.lastName > input", type: SelectorType.CSS));
		}
		public override void Register (UserBaseEntity entity)
		{
			FillRegistrationDetails(entity.EmailAddress, entity.Password);
			FirstNameInput.SendKeys(entity.ToDictionary()["firstName"]);
			LastNameInput.SendKeys(entity.ToDictionary()["lastName"]);
			RegisterButton.Click();
		}
	}
}
