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
using EntityObject.Enums;
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
	public class StaffEntityDetailSection : BasePage, IEntityDetailSection
	{
		private readonly IWait<IWebDriver> _driverWait;
		private readonly IWebDriver _driver;
		private readonly bool _isFastText;
		private readonly ContextConfiguration _contextConfiguration;

		// reference elements
		private static By AssessmentNotessElementBy => By.XPath("//*[contains(@class, 'assessmentNotes')]//div[contains(@class, 'dropdown__container')]/a");
		private static By AssessmentNotessInputElementBy => By.XPath("//*[contains(@class, 'assessmentNotes')]/div/input");
		private static By AddressIdElementBy => By.XPath("//*[contains(@class, 'address')]//div[contains(@class, 'dropdown__container')]");
		private static By AddressIdInputElementBy => By.XPath("//*[contains(@class, 'address')]/div/input");

		//FlatPickr Elements

		//Attribute Headers
		private readonly StaffEntity _staffEntity;

		//Attribute Header Titles
		private IWebElement FirstNameHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='First Name']"));
		private IWebElement LastNameHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Last Name']"));
		private IWebElement RoleHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Role']"));
		private IWebElement ContactNumberHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Contact Number']"));

		// User Entity specific web Elements
		private IWebElement UserEmailElement => FindElementExt("UserEmailElement");
		private IWebElement UserPasswordElement => FindElementExt("UserPasswordElement");
		private IWebElement UserConfirmPasswordElement => FindElementExt("UserConfirmPasswordElement");
		// Datepickers
		public IWebElement CreateAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.created > input[type='date']"));
		public IWebElement ModifiedAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.modified > input[type='date']"));

		public StaffEntityDetailSection(ContextConfiguration contextConfiguration, StaffEntity staffEntity = null) : base(contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			_driverWait = contextConfiguration.WebDriverWait;
			_isFastText = contextConfiguration.SeleniumSettings.FastText;
			_contextConfiguration = contextConfiguration;
			_staffEntity = staffEntity;

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
			selectorDict.Add("RoleElement", (selector: "//div[contains(@class, 'role')]//input", type: SelectorType.XPath));
			selectorDict.Add("ContactNumberElement", (selector: "//div[contains(@class, 'contactNumber')]//input", type: SelectorType.XPath));

			// Reference web elements
			selectorDict.Add("AssessmentnotesElement", (selector: ".input-group__dropdown.assessmentNotess > .dropdown.dropdown__container", type: SelectorType.CSS));
			selectorDict.Add("AddressElement", (selector: ".input-group__dropdown.addressId > .dropdown.dropdown__container", type: SelectorType.CSS));

			// User Entity specific web Elements
			selectorDict.Add("UserEmailElement", (selector: "div.email > input", type: SelectorType.CSS));
			selectorDict.Add("UserPasswordElement", (selector: "div.password> input", type: SelectorType.CSS));
			selectorDict.Add("UserConfirmPasswordElement", (selector: "div._confirmPassword > input", type: SelectorType.CSS));

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
		private IWebElement RoleElement => FindElementExt("RoleElement");
		private IWebElement ContactNumberElement => FindElementExt("ContactNumberElement");

		// Return an IWebElement that can be used to sort an attribute.
		public IWebElement GetHeaderTile(string attribute)
		{
			return attribute switch
			{
				"First Name" => FirstNameHeaderTitle,
				"Last Name" => LastNameHeaderTitle,
				"Role" => RoleHeaderTitle,
				"Contact Number" => ContactNumberHeaderTitle,
				_ => throw new Exception($"Cannot find header tile {attribute}"),
			};
		}

		// Return an IWebElement for an attribute input
		public IWebElement GetInputElement(string attribute)
		{
			switch (attribute)
			{
				case "FirstName":
					return FirstNameElement;
				case "LastName":
					return LastNameElement;
				case "Role":
					return RoleElement;
				case "ContactNumber":
					return ContactNumberElement;
				default:
					throw new Exception($"Cannot find input element {attribute}");
			}
		}

		public void SetInputElement(string attribute, string value)
		{
			switch (attribute)
			{
				case "FirstName":
					SetFirstName(value);
					break;
				case "LastName":
					SetLastName(value);
					break;
				case "Role":
					SetRole((StaffRoles)Enum.Parse(typeof(StaffRoles), value));
					break;
				case "ContactNumber":
					SetContactNumber(value);
					break;
				default:
					throw new Exception($"Cannot find input element {attribute}");
			}
		}

		private By GetErrorAttributeSectionAsBy(string attribute)
		{
			return attribute switch
			{
				"FirstName" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.firstName > div > p"),
				"LastName" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.lastName > div > p"),
				"Role" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.role > div > p"),
				"ContactNumber" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.contactNumber > div > p"),
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
			SetFirstName(_staffEntity.FirstName);
			SetLastName(_staffEntity.LastName);
			SetRole(_staffEntity.Role);
			SetContactNumber(_staffEntity.ContactNumber);

			if (_staffEntity.AssessmentNotesIds != null)
			{
				SetAssessmentNotess(_staffEntity.AssessmentNotesIds.Select(x => x.ToString()));
			}
			SetAddressId(_staffEntity.AddressId?.ToString());

			if (_driver.Url == $"{_contextConfiguration.BaseUrl}/admin/staffentity/create")
			{
				SetUserFields(_staffEntity);
			}
			// % protected region % [Configure entity application here] end
		}

		public List<Guid> GetAssociation(string referenceName)
		{
			switch (referenceName)
			{
				case "assessmentnotes":
					return GetAssessmentNotess();
				case "address":
					return new List<Guid>() {GetAddressId()};
				default:
					throw new Exception($"Cannot find association type {referenceName}");
			}
		}

		// set associations
		private void SetAssessmentNotess(IEnumerable<string> ids)
		{
			WaitUtils.elementState(_driverWait, AssessmentNotessInputElementBy, ElementState.VISIBLE);
			var assessmentNotessInputElement = _driver.FindElementExt(AssessmentNotessInputElementBy);

			foreach(var id in ids)
			{
				assessmentNotessInputElement.SendKeys(id);
				WaitForDropdownOptions();
				assessmentNotessInputElement.SendKeys(Keys.Return);
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
		private List<Guid> GetAssessmentNotess()
		{
			var guids = new List<Guid>();
			WaitUtils.elementState(_driverWait, AssessmentNotessElementBy, ElementState.VISIBLE);
			var assessmentNotessElement = _driver.FindElements(AssessmentNotessElementBy);

			foreach(var element in assessmentNotessElement)
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

		private void SetRole (StaffRoles value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "role", value.ToString(), _isFastText);
		}

		private StaffRoles GetRole =>
			(StaffRoles)Enum.Parse(typeof(StaffRoles), RoleElement.Text);
		private void SetContactNumber (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "contactNumber", value, _isFastText);
			ContactNumberElement.SendKeys(Keys.Tab);
			ContactNumberElement.SendKeys(Keys.Escape);
		}

		private String GetContactNumber =>
			ContactNumberElement.Text;

		// set the email, password and confirm password fields
		private void SetUserFields(StaffEntity staffEntity)
		{
			UserEmailElement.SendKeys(staffEntity.EmailAddress);
			UserPasswordElement.SendKeys(staffEntity.Password);
			UserConfirmPasswordElement.SendKeys(staffEntity.Password);
		}

		// % protected region % [Add any additional getters and setters of web elements] off begin
		// % protected region % [Add any additional getters and setters of web elements] end
	}
}