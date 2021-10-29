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
using System.Globalization;
using OpenQA.Selenium;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.ViewModels.Components.Common
{
	public class TimePicker : Component
	{
		public DateTime Value
		{
			get => GetValue();
			set => SetValue(value);
		}

		// % protected region % [Override elements here] off begin
		public readonly By HourElement = By.CssSelector("div.flatpickr-calendar.open .numInput.flatpickr-hour");
		public readonly By MinuteElement = By.CssSelector("div.flatpickr-calendar.open .numInput.flatpickr-minute");
		public readonly By AmPmElement = By.CssSelector("div.flatpickr-calendar.open .flatpickr-am-pm");
		// % protected region % [Override elements here] end

		// % protected region % [Add class properties here] off begin
		// % protected region % [Add class properties here] end

		// % protected region % [Override constructor here] off begin
		public TimePicker(By selector, ContextConfiguration contextConfiguration) : base(selector, contextConfiguration)
		{
		}
		// % protected region % [Override constructor here] end

		// % protected region % [Override GetValue here] off begin
		private DateTime GetValue()
		{
			var timeString = ContextConfiguration.WebDriver.FindElementExt(Selector).Text;
			return string.IsNullOrEmpty(timeString) 
				? default 
				: DateTime.ParseExact(timeString, "H:mm", null, DateTimeStyles.None);
		}
		// % protected region % [Override GetValue here] end

		// % protected region % [Override SetValue here] off begin
		private void SetValue(DateTime value)
		{
			ContextConfiguration.WebDriver.FindElementExt(Selector).Click();
			KeyboardUtils.SendIntAsDigits(value.Hour, ContextConfiguration.WebDriver.FindElementExt(HourElement));
			ContextConfiguration.WebDriver.FindElementExt(MinuteElement).Click();
			KeyboardUtils.SendIntAsDigits(value.Minute, ContextConfiguration.WebDriver.FindElementExt(MinuteElement));
		}
		// % protected region % [Override SetValue here] end

		// % protected region % [Override Close here] off begin
		public void Close() => ContextConfiguration.WebDriver.FindElementExt(AmPmElement).SendKeys(Keys.Enter);
		// % protected region % [Override Close here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}