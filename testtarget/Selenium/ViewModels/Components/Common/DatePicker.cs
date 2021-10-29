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
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.ViewModels.Components.Common
{
	public class DatePicker : Component
	{
		// % protected region % [Override class properties here] off begin
		public DateTime Value { get => GetValue(); set => SetDate(value); }
		public readonly By CalendarElement = By.CssSelector(".numInput.cur-year");
		public readonly By YearElement = By.CssSelector("div.flatpickr-calendar.open .numInput.cur-year");
		public readonly By MonthElement = By.CssSelector("div.flatpickr-calendar.open .flatpickr-monthDropdown-months");
		// % protected region % [Override class properties here] end

		// % protected region % [Add class properties here] off begin
		// % protected region % [Add class properties here] end

		// % protected region % [Override constructor here] off begin
		public DatePicker(By selector, ContextConfiguration contextConfiguration) : base(selector, contextConfiguration)
		{
		}
		// % protected region % [Override constructor here] end
		// % protected region % [Override GetValue here] off begin
		private DateTime GetValue()
		{
			var dateStr = ContextConfiguration.WebDriver.FindElementExt(Selector).Text;
			return string.IsNullOrEmpty(dateStr) 
				? default 
				: DateTime.ParseExact(dateStr, "yyyy-MM-dd", null, DateTimeStyles.None);
		}
		// % protected region % [Override GetValue here] end

		// % protected region % [Override SetDate here] off begin
		public void SetDate(DateTime value)
		{
			ContextConfiguration.WebDriver.FindElementExt(Selector).Click();
			SelectCalendarYear(value.Year);
			SelectCalendarMonth(value.Month);
			SelectCalendarDay(value.Day);
			new Actions(ContextConfiguration.WebDriver).SendKeys(Keys.Escape).Perform();
		}
		// % protected region % [Override SetDate here] end

		// % protected region % [Override SelectCalendarYear here] off begin
		internal void SelectCalendarYear(int year)
		{
			var element = ContextConfiguration.WebDriver.FindElementExt(YearElement);
			element.Clear();
			element.Click();
			KeyboardUtils.SendIntAsDigits(year, element);
		}
		// % protected region % [Override SelectCalendarYear here] end

		// % protected region % [Override SelectCalendarMonth here] off begin
		internal void SelectCalendarMonth(int month)
		{
			var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
			var element = ContextConfiguration.WebDriver.FindElementExt(MonthElement);
			var calendarMonths = element.FindElements(By.CssSelector(".flatpickr-monthDropdown-month"));
			element.Click();
			calendarMonths.FirstOrDefault(x => x.Text == monthName)?.Click();
		}
		// % protected region % [Override SelectCalendarMonth here] end

		// % protected region % [Override SelectCalendarDay here] off begin
		internal void SelectCalendarDay(int day)
		{
			var element = ContextConfiguration.WebDriver.FindElementExt(CalendarElement);
			var daysElement = By.XPath("//*[not(contains(@class,'prevMonthDay') or contains(@class,'nextMonthDay')) and (contains(@class,'flatpickr-day'))]");
			var calendarDays = element.FindElements(daysElement);
			calendarDays.FirstOrDefault(x => x.Text == day.ToString())?.Click();
		}
		// % protected region % [Override SelectCalendarDay here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}