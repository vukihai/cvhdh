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
using SeleniumTests.PageObjects;
using SeleniumTests.Setup;
using TechTalk.SpecFlow;
using Xunit;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.Steps.Toast
{
	[Binding]
	public class ToastSteps : StepDefinition
	{
		// % protected region % [Override class properties here] off begin
		// % protected region % [Override class properties here] end

		// % protected region % [Override constructor here] off begin
		public ToastSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
		}
		// % protected region % [Override constructor here] end
		// % protected region % [Override AssertAToasterIsDisplayedWithMessage here] off begin
		[StepDefinition(@"I assert a toaster alert is displayed with the message: (.*)")]
		public void AssertAToasterIsDisplayedWithMessage(string message)
		{
			var toaster = new ToasterAlert(ContextConfiguration);
			ContextConfiguration.WebDriverWait.Until(_ => toaster.ToasterBody.Displayed);
			Assert.Equal(message, toaster.ToasterBody.Text);
		}
		// % protected region % [Override AssertAToasterIsDisplayedWithMessage here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}