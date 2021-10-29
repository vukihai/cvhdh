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
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeleniumTests.Enums;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.ViewModels.Components.Common.Internal
{
	public class InternalCombobox : Component
	{
		// % protected region % [Add class properties here] off begin
		// % protected region % [Add class properties here] end

		// % protected region % [Override constructor here] off begin
		protected InternalCombobox(By selector, ContextConfiguration contextConfiguration) : base(selector, contextConfiguration)
		{

		}
		// % protected region % [Override constructor here] end

		// % protected region % [Override WaitForDropdownOptions here] off begin
		internal void WaitForDropdownOptions()
		{
			WaitUtils.elementState(ContextConfiguration.WebDriverWait, By.XPath("//*/div[@aria-expanded='true']"),ElementState.EXISTS);
		}
		// % protected region % [Override WaitForDropdownOptions here] end

		// % protected region % [Override WaitForOptionValue here] off begin
		internal void WaitForOptionValue(By selector, string value)
		{
			ContextConfiguration.WebDriverWait.Until(driver =>
			{
				try
				{
					var options = driver.FindElements(new ByChained(selector, By.XPath($".//div[@role='option']/span")));
					return options.Any(x => x.Text.ToLower().Equals(value.ToLower()) || x.FindElement(By.XPath("./..")).GetAttribute("data-id").Equals(value.ToLower())) || options.Count == 1;
				}
				catch (Exception)
				{
					return false;
				}
			});
		}
		// % protected region % [Override WaitForOptionValue here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}