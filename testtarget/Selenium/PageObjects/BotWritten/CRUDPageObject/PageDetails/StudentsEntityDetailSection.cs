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
	public class StudentsEntityDetailSection : BasePage, IEntityDetailSection
	{
		private readonly IWait<IWebDriver> _driverWait;
		private readonly IWebDriver _driver;
		private readonly bool _isFastText;
		private readonly ContextConfiguration _contextConfiguration;

		// reference elements
		private static By AchievementssElementBy => By.XPath("//*[contains(@class, 'achievements')]//div[contains(@class, 'dropdown__container')]/a");
		private static By AchievementssInputElementBy => By.XPath("//*[contains(@class, 'achievements')]/div/input");
		private static By AssessmentssElementBy => By.XPath("//*[contains(@class, 'assessments')]//div[contains(@class, 'dropdown__container')]/a");
		private static By AssessmentssInputElementBy => By.XPath("//*[contains(@class, 'assessments')]/div/input");
		private static By AddressIdElementBy => By.XPath("//*[contains(@class, 'address')]//div[contains(@class, 'dropdown__container')]");
		private static By AddressIdInputElementBy => By.XPath("//*[contains(@class, 'address')]/div/input");

		//FlatPickr Elements
		private DateTimePickerComponent EnrolmentStartElement => new DateTimePickerComponent(_contextConfiguration, "enrolmentStart");
		private DateTimePickerComponent EnrolmentEndElement => new DateTimePickerComponent(_contextConfiguration, "enrolmentEnd");

		//Attribute Headers
		private readonly StudentsEntity _studentsEntity;

		//Attribute Header Titles
		private IWebElement FirstNameHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='First Name']"));
		private IWebElement LastNameHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Last Name']"));
		private IWebElement ContactNumberHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Contact Number']"));
		private IWebElement EmailHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Email']"));
		private IWebElement EnrolmentStartHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Enrolment Start']"));
		private IWebElement EnrolmentEndHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Enrolment End']"));
		private IWebElement NameHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Name']"));

		// Datepickers
		public IWebElement CreateAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.created > input[type='date']"));
		public IWebElement ModifiedAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.modified > input[type='date']"));

		public StudentsEntityDetailSection(ContextConfiguration contextConfiguration, StudentsEntity studentsEntity = null) : base(contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			_driverWait = contextConfiguration.WebDriverWait;
			_isFastText = contextConfiguration.SeleniumSettings.FastText;
			_contextConfiguration = contextConfiguration;
			_studentsEntity = studentsEntity;

			InitializeSelectors();
			// % protected region % [Add any extra construction requires] off begin
			// % protected region % [Add any extra construction requires] end
		}

		// initialise all selectors and grouping them with the selector type which is used
		private void InitializeSelectors()
		{
			// Attribute web elements
			selectorDict.Add("FirstNameElement", (selector: "//div[contains(@class, 'firstName')]//input", type: SelectorType.XPath));
			selectorDict.Add("LastNameElement", (selector: "//div[contains(@class, 'lastName')]//input", type: SelectorType.XPath));
			selectorDict.Add("ContactNumberElement", (selector: "//div[contains(@class, 'contactNumber')]//input", type: SelectorType.XPath));
			selectorDict.Add("EmailElement", (selector: "//div[contains(@class, 'email')]//input", type: SelectorType.XPath));

			// Reference web elements
			selectorDict.Add("AchievementsElement", (selector: ".input-group__dropdown.achievementss > .dropdown.dropdown__container", type: SelectorType.CSS));
			selectorDict.Add("AssessmentsElement", (selector: ".input-group__dropdown.assessmentss > .dropdown.dropdown__container", type: SelectorType.CSS));
			selectorDict.Add("AddressElement", (selector: ".input-group__dropdown.addressId > .dropdown.dropdown__container", type: SelectorType.CSS));

			// Form Entity specific web Element
			selectorDict.Add("NameElement", (selector: "div.name > input", type: SelectorType.CSS));

			// Datepicker
			selectorDict.Add("CreateAtDatepickerField", (selector: "//div[contains(@class, 'created')]/input", type: SelectorType.XPath));
			selectorDict.Add("ModifiedAtDatepickerField", (selector: "//div[contains(@class, 'modified')]/input", type: SelectorType.XPath));
		}

		//outgoing Reference web elements
		//get the input path as set by the selector library
		private IWebElement AddressElement => FindElementExt("AddressElement");

		//Attribute web Elements
		private IWebElement FirstNameElement => FindElementExt("FirstNameElement");
		private IWebElement LastNameElement => FindElementExt("LastNameElement");
		private IWebElement ContactNumberElement => FindElementExt("ContactNumberElement");
		private IWebElement EmailElement => FindElementExt("EmailElement");
		private IWebElement NameElement => FindElementExt("NameElement");

		// Return an IWebElement that can be used to sort an attribute.
		public IWebElement GetHeaderTile(string attribute)
		{
			return attribute switch
			{
				"First Name" => FirstNameHeaderTitle,
				"Last Name" => LastNameHeaderTitle,
				"Contact Number" => ContactNumberHeaderTitle,
				"Email" => EmailHeaderTitle,
				"Enrolment Start" => EnrolmentStartHeaderTitle,
				"Enrolment End" => EnrolmentEndHeaderTitle,
				"Name" => NameHeaderTitle,
				_ => throw new Exception($"Cannot find header tile {attribute}"),
			};
		}

		// Return an IWebElement for an attribute input
		public IWebElement GetInputElement(string attribute)
		{
			switch (attribute)
			{
				case "Name":
					return NameElement;
				case "FirstName":
					return FirstNameElement;
				case "LastName":
					return LastNameElement;
				case "ContactNumber":
					return ContactNumberElement;
				case "Email":
					return EmailElement;
				case "EnrolmentStart":
					return EnrolmentStartElement.DateTimePickerElement;
				case "EnrolmentEnd":
					return EnrolmentEndElement.DateTimePickerElement;
				default:
					throw new Exception($"Cannot find input element {attribute}");
			}
		}

		public void SetInputElement(string attribute, string value)
		{
			switch (attribute)
			{
				case "Name":
					SetName(value);
					break;
				case "FirstName":
					SetFirstName(value);
					break;
				case "LastName":
					SetLastName(value);
					break;
				case "ContactNumber":
					SetContactNumber(value);
					break;
				case "Email":
					SetEmail(value);
					break;
				case "EnrolmentStart":
					if (DateTime.TryParse(value, out var enrolmentStartValue))
					{
						SetEnrolmentStart(enrolmentStartValue);
					}
					break;
				case "EnrolmentEnd":
					if (DateTime.TryParse(value, out var enrolmentEndValue))
					{
						SetEnrolmentEnd(enrolmentEndValue);
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
				"Name" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "//div[contains(@class, 'name')]"),
				"FirstName" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.firstName > div > p"),
				"LastName" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.lastName > div > p"),
				"ContactNumber" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.contactNumber > div > p"),
				"Email" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.email > div > p"),
				"EnrolmentStart" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.enrolmentStart > div > p"),
				"EnrolmentEnd" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.enrolmentEnd > div > p"),
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
			SetName(_studentsEntity.Name);
			SetFirstName(_studentsEntity.FirstName);
			SetLastName(_studentsEntity.LastName);
			SetContactNumber(_studentsEntity.ContactNumber);
			SetEmail(_studentsEntity.Email);
			SetEnrolmentStart(_studentsEntity.EnrolmentStart);
			SetEnrolmentEnd(_studentsEntity.EnrolmentEnd);

			if (_studentsEntity.AchievementsIds != null)
			{
				SetAchievementss(_studentsEntity.AchievementsIds.Select(x => x.ToString()));
			}
			if (_studentsEntity.AssessmentsIds != null)
			{
				SetAssessmentss(_studentsEntity.AssessmentsIds.Select(x => x.ToString()));
			}
			SetAddressId(_studentsEntity.AddressId?.ToString());
			// % protected region % [Configure entity application here] end
		}

		public List<Guid> GetAssociation(string referenceName)
		{
			switch (referenceName)
			{
				case "achievements":
					return GetAchievementss();
				case "assessments":
					return GetAssessmentss();
				case "address":
					return new List<Guid>() {GetAddressId()};
				default:
					throw new Exception($"Cannot find association type {referenceName}");
			}
		}

		// set associations
		private void SetAchievementss(IEnumerable<string> ids)
		{
			WaitUtils.elementState(_driverWait, AchievementssInputElementBy, ElementState.VISIBLE);
			var achievementssInputElement = _driver.FindElementExt(AchievementssInputElementBy);

			foreach(var id in ids)
			{
				achievementssInputElement.SendKeys(id);
				WaitForDropdownOptions();
				achievementssInputElement.SendKeys(Keys.Return);
			}
		}

		private void SetAssessmentss(IEnumerable<string> ids)
		{
			WaitUtils.elementState(_driverWait, AssessmentssInputElementBy, ElementState.VISIBLE);
			var assessmentssInputElement = _driver.FindElementExt(AssessmentssInputElementBy);

			foreach(var id in ids)
			{
				assessmentssInputElement.SendKeys(id);
				WaitForDropdownOptions();
				assessmentssInputElement.SendKeys(Keys.Return);
			}
		}

		private void SetAddressId(string id)
		{
			if (id == "") { return; }
			WaitUtils.elementState(_driverWait, AddressIdInputElementBy, ElementState.VISIBLE);
			var addressIdInputElement = _driver.FindElementExt(AddressIdInputElementBy);

			if (id != null)
			{
				addressIdInputElement.SendKeys(id);
				WaitForDropdownOptions();
				WaitUtils.elementState(_driverWait, By.XPath($"//*/div[@role='option']/span[text()='{id}']"), ElementState.EXISTS);
				addressIdInputElement.SendKeys(Keys.Return);
			}
		}

		// get associations
		private List<Guid> GetAchievementss()
		{
			var guids = new List<Guid>();
			WaitUtils.elementState(_driverWait, AchievementssElementBy, ElementState.VISIBLE);
			var achievementssElement = _driver.FindElements(AchievementssElementBy);

			foreach(var element in achievementssElement)
			{
				guids.Add(new Guid (element.GetAttribute("data-id")));
			}
			return guids;
		}
		private List<Guid> GetAssessmentss()
		{
			var guids = new List<Guid>();
			WaitUtils.elementState(_driverWait, AssessmentssElementBy, ElementState.VISIBLE);
			var assessmentssElement = _driver.FindElements(AssessmentssElementBy);

			foreach(var element in assessmentssElement)
			{
				guids.Add(new Guid (element.GetAttribute("data-id")));
			}
			return guids;
		}
		private Guid GetAddressId()
		{
			WaitUtils.elementState(_driverWait, AddressIdElementBy, ElementState.VISIBLE);
			var addressIdElement = _driver.FindElementExt(AddressIdElementBy);
			return new Guid(addressIdElement.GetAttribute("data-id"));
		}

		// wait for dropdown to be displaying options
		private void WaitForDropdownOptions()
		{
			var xpath = "//*/div[@aria-expanded='true']";
			var elementBy = WebElementUtils.GetElementAsBy(SelectorPathType.XPATH, xpath);
			WaitUtils.elementState(_driverWait, elementBy,ElementState.EXISTS);
		}

		private void SetFirstName (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "firstName", value, _isFastText);
			FirstNameElement.SendKeys(Keys.Tab);
			FirstNameElement.SendKeys(Keys.Escape);
		}

		private String GetFirstName =>
			FirstNameElement.Text;

		private void SetLastName (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "lastName", value, _isFastText);
			LastNameElement.SendKeys(Keys.Tab);
			LastNameElement.SendKeys(Keys.Escape);
		}

		private String GetLastName =>
			LastNameElement.Text;

		private void SetContactNumber (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "contactNumber", value, _isFastText);
			ContactNumberElement.SendKeys(Keys.Tab);
			ContactNumberElement.SendKeys(Keys.Escape);
		}

		private String GetContactNumber =>
			ContactNumberElement.Text;

		private void SetEmail (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "email", value, _isFastText);
			EmailElement.SendKeys(Keys.Tab);
			EmailElement.SendKeys(Keys.Escape);
		}

		private String GetEmail =>
			EmailElement.Text;

		private void SetEnrolmentStart (DateTime? value)
		{
			if (value is DateTime datetimeValue)
			{
				EnrolmentStartElement.SetDate(datetimeValue);
			}
		}

		private DateTime? GetEnrolmentStart =>
			Convert.ToDateTime(EnrolmentStartElement.DateTimePickerElement.Text);
		private void SetEnrolmentEnd (DateTime? value)
		{
			if (value is DateTime datetimeValue)
			{
				EnrolmentEndElement.SetDate(datetimeValue);
			}
		}

		private DateTime? GetEnrolmentEnd =>
			Convert.ToDateTime(EnrolmentEndElement.DateTimePickerElement.Text);

		// Set Name for form entity
		private void SetName (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "name", value, _isFastText);
			NameElement.SendKeys(Keys.Tab);
		}

		private String GetName => NameElement.Text;
		// % protected region % [Add any additional getters and setters of web elements] off begin
		// % protected region % [Add any additional getters and setters of web elements] end
	}
}