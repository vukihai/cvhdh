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
using SeleniumTests.ViewModels.Pages.CRUD.AddressSubmissionEntityCrud;
using TechTalk.SpecFlow;
using Xunit;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.Steps.CRUD.AddressSubmissionEntityCrud
{
	[Binding]
	public class AddressSubmissionEntityViewSteps : StepDefinition
	{
		// % protected region % [Override class properties here] off begin
		public ReadAddressSubmissionEntityPage ReadAddressSubmissionEntityPage => new(ContextConfiguration);
		// % protected region % [Override class properties here] end

		// % protected region % [Override constructor here] off begin
		public AddressSubmissionEntityViewSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
		}
		// % protected region % [Override constructor here] end
		// % protected region % [Override AssertViewingAnEntityonTheViewPage here] off begin
		[StepDefinition("I assert that I am viewing a AddressSubmissionEntity on the AddressSubmissionEntity view page")]
		public void AssertViewingAnEntityonTheViewPage()
		{
			ReadAddressSubmissionEntityPage.GetValues();
			ContextConfiguration.WebDriverWait.Until(_ => ContextConfiguration.WebDriver.Url.Trim('/').Contains(ReadAddressSubmissionEntityPage.Url));
			Assert.Contains(ReadAddressSubmissionEntityPage.Url, ContextConfiguration.WebDriver.Url.Trim('/'));
		}
		// % protected region % [Override AssertViewingAnEntityonTheViewPage here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}