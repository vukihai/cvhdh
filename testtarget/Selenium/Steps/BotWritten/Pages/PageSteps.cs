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
using SeleniumTests.PageObjects;
using SeleniumTests.PageObjects.BotWritten.UIModeled;
using SeleniumTests.PageObjects.BotWritten.UIModeled.Pages;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using TechTalk.SpecFlow;
using Xunit;
// % protected region % [Add further imports here] off begin
// % protected region % [Add further imports here] end

namespace SeleniumTests.Steps.BotWritten.Pages
{
	[Binding]
	public sealed class PagesSteps  : BaseStepDefinition
	{
		// % protected region % [Override constructor here] off begin
		public PagesSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
		}
		// % protected region % [Override constructor here] end
		// % protected region % [Override WhenINavigateToTheFrontendPage here] off begin
		[StepDefinition("I navigate to the (.*) frontend page")]
		public void WhenINavigateToTheFrontendPage(string pageName)
		{
			var page = GetPage(pageName);
			page.Navigate();
			WaitUtils.waitForPage(_driverWait);
		}
		// % protected region % [Override WhenINavigateToTheFrontendPage here] end
		// % protected region % [Override AssertThatTheContendsOfPageIsCorrect here] off begin
		[StepDefinition("The contents of the (.*) is correct")]
		public void AssertThatTheContendsOfPageIsCorrect(string pageName)
		{
			var page = GetPage(pageName);
			Assert.True(page.ContainsModeledElements());
		}
		// % protected region % [Override AssertThatTheContendsOfPageIsCorrect here] end
		// % protected region % [Override GetPage here] off begin
		private UIModeledPage GetPage(string page)
		{
			return page switch
			{
				"ListStudentPage" => new ListStudentPage(_contextConfiguration),
				"FormEditStudentPage" => new FormEditStudentPage(_contextConfiguration),
				"EditArchivementPage" => new EditArchivementPage(_contextConfiguration),
				"MainMenuPage" => new MainMenuPage(_contextConfiguration),
				_ => throw new Exception("Failed to switch on page name.")
			};
		}
		// % protected region % [Override GetPage here] end
		// % protected region % [Add further step definitions here] off begin
		// % protected region % [Add further step definitions here] end
	}
}