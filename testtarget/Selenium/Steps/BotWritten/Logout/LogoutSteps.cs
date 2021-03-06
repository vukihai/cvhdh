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
using FluentAssertions;
using SeleniumTests.PageObjects.BotWritten.UserPageObjects;
using SeleniumTests.Setup;
using TechTalk.SpecFlow;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.Steps.BotWritten.Logout
{
	[Binding]
	public class LogoutSteps  : BaseStepDefinition
	{
		// % protected region % [Customize LogoutSteps fields here] off begin
		private readonly ContextConfiguration _contextConfiguration;
		private readonly LogoutPage _logoutPage;
		// % protected region % [Customize LogoutSteps fields here] end

		public LogoutSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_logoutPage = new LogoutPage(_contextConfiguration);
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}

		[Then(@"I am redirected to the login page")]
		public void ThenIamRedirectedToTheLoginPage()
		{
			_driver.Url.Should().BeOneOf(
				// % protected region % [Add any additional valid logout URLs here] off begin
				// % protected region % [Add any additional valid logout URLs here] end
				$"{_baseUrl}/login?redirect=/",
				$"{_baseUrl}/"
			);
		}

		[StepDefinition("I am logged out of the site")]
		public void Logout()
		{
			_logoutPage.Navigate();

		}		
	}
	// % protected region % [Add any further steps here] off begin
	// % protected region % [Add any further steps here] end
}