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
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.ViewModels.Components.Common.Internal;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.ViewModels.Components.Common
{
	public class Combobox : InternalCombobox
	{
		public string Value
		{
			get => GetValue();
			set => SetValue(value);
		}

		// % protected region % [Add class properties here] off begin
		// % protected region % [Add class properties here] end

		// % protected region % [Override constructor here] off begin
		public Combobox(By selector, ContextConfiguration contextConfiguration) : base(selector, contextConfiguration)
		{
		}
		// % protected region % [Override constructor here] end

		// % protected region % [Override GetValue here] off begin
		private string GetValue()
		{
			return ContextConfiguration.WebDriver.FindElementExt(new ByChained(Selector, By.CssSelector("div.text"))).Text;
		}
		// % protected region % [Override GetValue here] end

		// % protected region % [Override SetValue here] off begin
		private void SetValue(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			var element = ContextConfiguration.WebDriver.FindElementExt(new ByChained(Selector, By.CssSelector("input")));
			element.SendKeys(value);
			WaitForDropdownOptions();
			WaitForOptionValue(Selector, value);
			element.SendKeys(Keys.Return);
		}
		// % protected region % [Override SetValue here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}