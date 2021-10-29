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
using SeleniumTests.PageObjects.BotWritten.UserPageObjects;
using SeleniumTests.Setup;
using TechTalk.SpecFlow;
using Xunit;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.Steps.Login
{
	[Binding]
	public class LoginSteps : StepDefinition
	{
		// % protected region % [Override class properties here] off begin
		public readonly LoginPage LoginPage;
		// % protected region % [Override class properties here] end

		// % protected region % [Override constructor here] off begin
		public LoginSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			LoginPage = new LoginPage(ContextConfiguration);
		}
		// % protected region % [Override constructor here] end

		// % protected region % [Customize LoginAsUser logic here] off begin
		[StepDefinition("I login to the site as a user")]
		public void LoginAsUser()
		{
			var userName = ContextConfiguration.SuperUserConfiguration.Username;
			var password = ContextConfiguration.SuperUserConfiguration.Password;
			GivenIAttemptToLogin(userName, password, "success");
		}
		// % protected region % [Customize LoginAsUser logic here] end

		// % protected region % [Override GivenIAttemptToLogin here] off begin
		[StepDefinition(@"I login to the site with username (.*) and password (.*) then I expect login (.*)")]
		public void GivenIAttemptToLogin(string user, string pass, string success)
		{
			LoginPage.Navigate();
			LoginPage.Login(user, pass);
			try
			{
				ContextConfiguration.WebDriverWait.Until(wd => wd.Url == ContextConfiguration.BaseUrl + "/mainmenu");
				Assert.Equal("success", success);
			}
			catch (OpenQA.Selenium.UnhandledAlertException)
			{
				Assert.Equal("failure", success);
			}
			catch (OpenQA.Selenium.WebDriverTimeoutException)
			{
				Assert.Equal(ContextConfiguration.WebDriver.Url, ContextConfiguration.BaseUrl + "/login");
			}
		}
		// % protected region % [Override GivenIAttemptToLogin here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}