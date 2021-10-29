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
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.ViewModels.Components.Common.Internal;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.ViewModels.Components.Attribute
{
	public class AttributeReferenceMultiCombobox : InternalCombobox
	{
		public IEnumerable<string> Value
		{
			get => GetValue();
			set => SetValue(value);
		}

		// % protected region % [Add class properties here] off begin
		// % protected region % [Add class properties here] end

		// % protected region % [Override constructor here] off begin
		public AttributeReferenceMultiCombobox(By selector, ContextConfiguration contextConfiguration) : base(selector, contextConfiguration)
		{
		}
		// % protected region % [Override constructor here] end

		// % protected region % [Override get value here] off begin
		private IEnumerable<string> GetValue()
		{
			try
			{
				return ContextConfiguration.WebDriver
					.FindElementsExt(new ByChained(Selector, By.CssSelector("a")))
					.Select(x => x.GetAttribute("value"));
			}
			catch (WebDriverTimeoutException)
			{
				return new List<string>();
			}
		}
		// % protected region % [Override get value here] end

		// % protected region % [Override get value here] off begin
		private void SetValue(IEnumerable<string> value)
		{
			if (value == null)
			{
				return;
			}
			var element = ContextConfiguration.WebDriver.FindElementExt(new ByChained(Selector, By.CssSelector("input")));
			foreach (var id in value)
			{
				element.SendKeys(id);
				WaitForDropdownOptions();
				element.SendKeys(Keys.Return);
			}
		}
		// % protected region % [Override get value here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}