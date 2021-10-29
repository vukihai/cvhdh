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
using System;
using System.Globalization;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.ViewModels.Components.Common
{
	public class DateTimePicker : Component
	{
		// % protected region % [Override Value here] off begin
		public DateTime Value
		{
			get => GetValue();
			set => SetValue(value);
		}
		// % protected region % [Override Value here] end

		// % protected region % [Override class properties here] off begin
		internal TimePicker TimePicker { get; set; }
		internal DatePicker DatePicker { get; set; }
		// % protected region % [Override class properties here] end

		// % protected region % [Add class properties here] off begin
		// % protected region % [Add class properties here] end

		// % protected region % [Override constructor here] off begin
		public DateTimePicker(By selector, ContextConfiguration contextConfiguration) : base(selector, contextConfiguration)
		{
			TimePicker = new TimePicker(selector, contextConfiguration);
			DatePicker = new DatePicker(selector, contextConfiguration);
		}
		// % protected region % [Override constructor here] end

		// % protected region % [Override GetValue here] off begin
		internal DateTime GetValue()
		{
			var dateStr = ContextConfiguration.WebDriver.FindElementExt(Selector).Text;
			return string.IsNullOrEmpty(dateStr) 
				? default 
				: DateTime.ParseExact(dateStr, "yyyy-MM-dd H:mm", null, DateTimeStyles.None);
		}
		// % protected region % [Override GetValue here] end

		// % protected region % [Override GetValue here] off begin
		internal void SetValue(DateTime value)
		{
			DatePicker.Value = value;
			TimePicker.Value = value;
			TimePicker.Close();
		}
		// % protected region % [Override GetValue here] end
	}
}