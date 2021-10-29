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
using System.Collections.Generic;
using APITests.EntityObjects.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.PageObjects.Components;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.Enums;
using SeleniumTests.PageObjects.BotWritten;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.PageObjects.CRUDPageObject.PageDetails
{
	//This section is a mapping from an entity object to an entity create or detailed view page
	public class AssessmentNotesEntityDetailSection : BasePage, IEntityDetailSection
	{
		private readonly IWait<IWebDriver> _driverWait;
		private readonly IWebDriver _driver;
		private readonly bool _isFastText;
		private readonly ContextConfiguration _contextConfiguration;

		// reference elements
		private static By AssessmentsIdElementBy => By.XPath("//*[contains(@class, 'assessments')]//div[contains(@class, 'dropdown__container')]");
		private static By AssessmentsIdInputElementBy => By.XPath("//*[contains(@class, 'assessments')]/div/input");
		private static By StaffIdElementBy => By.XPath("//*[contains(@class, 'staff')]//div[contains(@class, 'dropdown__container')]");
		private static By StaffIdInputElementBy => By.XPath("//*[contains(@class, 'staff')]/div/input");

		//FlatPickr Elements
		private DateTimePickerComponent SessionDateElement => new DateTimePickerComponent(_contextConfiguration, "sessionDate");

		//Attribute Headers
		private readonly AssessmentNotesEntity _assessmentNotesEntity;

		//Attribute Header Titles
		private IWebElement SessionNotesHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Session Notes']"));
		private IWebElement SessionDateHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Session Date']"));

		// Datepickers
		public IWebElement CreateAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.created > input[type='date']"));
		public IWebElement ModifiedAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.modified > input[type='date']"));

		public AssessmentNotesEntityDetailSection(ContextConfiguration contextConfiguration, AssessmentNotesEntity assessmentNotesEntity = null) : base(contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			_driverWait = contextConfiguration.WebDriverWait;
			_isFastText = contextConfiguration.SeleniumSettings.FastText;
			_contextConfiguration = contextConfiguration;
			_assessmentNotesEntity = assessmentNotesEntity;

			InitializeSelectors();
			// % protected region % [Add any extra construction requires] off begin

			// % protected region % [Add any extra construction requires] end
		}

		// initialise all selectors and grouping them with the selector type which is used
		private void InitializeSelectors()
		{
			// Attribute web elements
			selectorDict.Add("SessionNotesElement", (selector: "//div[contains(@class, 'sessionNotes')]//input", type: SelectorType.XPath));

			// Reference web elements
			selectorDict.Add("AssessmentsElement", (selector: ".input-group__dropdown.assessmentsId > .dropdown.dropdown__container", type: SelectorType.CSS));
			selectorDict.Add("StaffElement", (selector: ".input-group__dropdown.staffId > .dropdown.dropdown__container", type: SelectorType.CSS));

			// Datepicker
			selectorDict.Add("CreateAtDatepickerField", (selector: "//div[contains(@class, 'created')]/input", type: SelectorType.XPath));
			selectorDict.Add("ModifiedAtDatepickerField", (selector: "//div[contains(@class, 'modified')]/input", type: SelectorType.XPath));
		}

		//outgoing Reference web elements
		//get the input path as set by the selector library
		private IWebElement AssessmentsElement => FindElementExt("AssessmentsElement");
		//get the input path as set by the selector library
		private IWebElement StaffElement => FindElementExt("StaffElement");

		//Attribute web Elements
		private IWebElement SessionNotesElement => FindElementExt("SessionNotesElement");

		// Return an IWebElement that can be used to sort an attribute.
		public IWebElement GetHeaderTile(string attribute)
		{
			return attribute switch
			{
				"Session Notes" => SessionNotesHeaderTitle,
				"Session Date" => SessionDateHeaderTitle,
				_ => throw new Exception($"Cannot find header tile {attribute}"),
			};
		}

		// Return an IWebElement for an attribute input
		public IWebElement GetInputElement(string attribute)
		{
			switch (attribute)
			{
				case "SessionNotes":
					return SessionNotesElement;
				case "SessionDate":
					return SessionDateElement.DateTimePickerElement;
				default:
					throw new Exception($"Cannot find input element {attribute}");
			}
		}

		public void SetInputElement(string attribute, string value)
		{
			switch (attribute)
			{
				case "SessionNotes":
					SetSessionNotes(value);
					break;
				case "SessionDate":
					if (DateTime.TryParse(value, out var sessionDateValue))
					{
						SetSessionDate(sessionDateValue);
					}
					break;
				default:
					throw new Exception($"Cannot find input element {attribute}");
			}
		}

		private By GetErrorAttributeSectionAsBy(string attribute)
		{
			return attribute switch
			{
				"SessionNotes" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.sessionNotes > div > p"),
				"SessionDate" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.sessionDate > div > p"),
				_ => throw new Exception($"No such attribute {attribute}"),
			};
		}

		public List<string> GetErrorMessagesForAttribute(string attribute)
		{
			var elementBy = GetErrorAttributeSectionAsBy(attribute);
			WaitUtils.elementState(_driverWait, elementBy, ElementState.VISIBLE);
			var element = _driver.FindElementExt(elementBy);
			var errors = new List<string>(element.Text.Split("\r\n"));
			// remove the item in the list which is the name of the attribute and not an error.
			errors.Remove(attribute);
			return errors;
		}

		public void Apply()
		{
			// % protected region % [Configure entity application here] off begin
			SetSessionNotes(_assessmentNotesEntity.SessionNotes);
			SetSessionDate(_assessmentNotesEntity.SessionDate);

			SetAssessmentsId(_assessmentNotesEntity.AssessmentsId?.ToString());
			SetStaffId(_assessmentNotesEntity.StaffId?.ToString());
			// % protected region % [Configure entity application here] end
		}

		public List<Guid> GetAssociation(string referenceName)
		{
			switch (referenceName)
			{
				case "assessments":
					return new List<Guid>() {GetAssessmentsId()};
				case "staff":
					return new List<Guid>() {GetStaffId()};
				default:
					throw new Exception($"Cannot find association type {referenceName}");
			}
		}

		// set associations
		private void SetAssessmentsId(string id)
		{
			if (id == "") { return; }
			WaitUtils.elementState(_driverWait, AssessmentsIdInputElementBy, ElementState.VISIBLE);
			var assessmentsIdInputElement = _driver.FindElementExt(AssessmentsIdInputElementBy);

			if (id != null)
			{
				assessmentsIdInputElement.SendKeys(id);
				WaitForDropdownOptions();
				WaitUtils.elementState(_driverWait, By.XPath($"//*/div[@role='option']/span[text()='{id}']"), ElementState.EXISTS);
				assessmentsIdInputElement.SendKeys(Keys.Return);
			}
		}
		private void SetStaffId(string id)
		{
			if (id == "") { return; }
			WaitUtils.elementState(_driverWait, StaffIdInputElementBy, ElementState.VISIBLE);
			var staffIdInputElement = _driver.FindElementExt(StaffIdInputElementBy);

			if (id != null)
			{
				staffIdInputElement.SendKeys(id);
				WaitForDropdownOptions();
				WaitUtils.elementState(_driverWait, By.XPath($"//*/div[@role='option']/span[text()='{id}']"), ElementState.EXISTS);
				staffIdInputElement.SendKeys(Keys.Return);
			}
		}

		// get associations
		private Guid GetAssessmentsId()
		{
			WaitUtils.elementState(_driverWait, AssessmentsIdElementBy, ElementState.VISIBLE);
			var assessmentsIdElement = _driver.FindElementExt(AssessmentsIdElementBy);
			return new Guid(assessmentsIdElement.GetAttribute("data-id"));
		}
		private Guid GetStaffId()
		{
			WaitUtils.elementState(_driverWait, StaffIdElementBy, ElementState.VISIBLE);
			var staffIdElement = _driver.FindElementExt(StaffIdElementBy);
			return new Guid(staffIdElement.GetAttribute("data-id"));
		}

		// wait for dropdown to be displaying options
		private void WaitForDropdownOptions()
		{
			var xpath = "//*/div[@aria-expanded='true']";
			var elementBy = WebElementUtils.GetElementAsBy(SelectorPathType.XPATH, xpath);
			WaitUtils.elementState(_driverWait, elementBy,ElementState.EXISTS);
		}

		private void SetSessionNotes (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "sessionNotes", value, _isFastText);
			SessionNotesElement.SendKeys(Keys.Tab);
			SessionNotesElement.SendKeys(Keys.Escape);
		}

		private String GetSessionNotes =>
			SessionNotesElement.Text;

		private void SetSessionDate (DateTime? value)
		{
			if (value is DateTime datetimeValue)
			{
				SessionDateElement.SetDate(datetimeValue);
			}
		}

		private DateTime? GetSessionDate =>
			Convert.ToDateTime(SessionDateElement.DateTimePickerElement.Text);

		// % protected region % [Add any additional getters and setters of web elements] off begin
		// % protected region % [Add any additional getters and setters of web elements] end
	}
}