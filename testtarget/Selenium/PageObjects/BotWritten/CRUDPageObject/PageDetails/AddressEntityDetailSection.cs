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
	public class AddressEntityDetailSection : BasePage, IEntityDetailSection
	{
		private readonly IWait<IWebDriver> _driverWait;
		private readonly IWebDriver _driver;
		private readonly bool _isFastText;
		private readonly ContextConfiguration _contextConfiguration;

		// reference elements
		private static By StaffssElementBy => By.XPath("//*[contains(@class, 'staffs')]//div[contains(@class, 'dropdown__container')]/a");
		private static By StaffssInputElementBy => By.XPath("//*[contains(@class, 'staffs')]/div/input");
		private static By StudentssElementBy => By.XPath("//*[contains(@class, 'students')]//div[contains(@class, 'dropdown__container')]/a");
		private static By StudentssInputElementBy => By.XPath("//*[contains(@class, 'students')]/div/input");

		//FlatPickr Elements

		//Attribute Headers
		private readonly AddressEntity _addressEntity;

		//Attribute Header Titles
		private IWebElement UnitHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Unit']"));
		private IWebElement AddressLine1HeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Address line 1']"));
		private IWebElement AddressLine2HeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Address line 2']"));
		private IWebElement SuburbHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Suburb']"));
		private IWebElement PostcodeHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Postcode']"));
		private IWebElement CityHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='City']"));
		private IWebElement CountryHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Country']"));
		private IWebElement NameHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Name']"));

		// Datepickers
		public IWebElement CreateAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.created > input[type='date']"));
		public IWebElement ModifiedAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.modified > input[type='date']"));

		public AddressEntityDetailSection(ContextConfiguration contextConfiguration, AddressEntity addressEntity = null) : base(contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			_driverWait = contextConfiguration.WebDriverWait;
			_isFastText = contextConfiguration.SeleniumSettings.FastText;
			_contextConfiguration = contextConfiguration;
			_addressEntity = addressEntity;

			InitializeSelectors();
			// % protected region % [Add any extra construction requires] off begin
			// % protected region % [Add any extra construction requires] end
		}

		// initialise all selectors and grouping them with the selector type which is used
		private void InitializeSelectors()
		{
			// Attribute web elements
			selectorDict.Add("UnitElement", (selector: "//div[contains(@class, 'unit')]//input", type: SelectorType.XPath));
			selectorDict.Add("AddressLine1Element", (selector: "//div[contains(@class, 'addressLine1')]//input", type: SelectorType.XPath));
			selectorDict.Add("AddressLine2Element", (selector: "//div[contains(@class, 'addressLine2')]//input", type: SelectorType.XPath));
			selectorDict.Add("SuburbElement", (selector: "//div[contains(@class, 'suburb')]//input", type: SelectorType.XPath));
			selectorDict.Add("PostcodeElement", (selector: "//div[contains(@class, 'postcode')]//input", type: SelectorType.XPath));
			selectorDict.Add("CityElement", (selector: "//div[contains(@class, 'city')]//input", type: SelectorType.XPath));
			selectorDict.Add("CountryElement", (selector: "//div[contains(@class, 'country')]//input", type: SelectorType.XPath));

			// Reference web elements
			selectorDict.Add("StaffsElement", (selector: ".input-group__dropdown.staffss > .dropdown.dropdown__container", type: SelectorType.CSS));
			selectorDict.Add("StudentsElement", (selector: ".input-group__dropdown.studentss > .dropdown.dropdown__container", type: SelectorType.CSS));

			// Form Entity specific web Element
			selectorDict.Add("NameElement", (selector: "div.name > input", type: SelectorType.CSS));

			// Datepicker
			selectorDict.Add("CreateAtDatepickerField", (selector: "//div[contains(@class, 'created')]/input", type: SelectorType.XPath));
			selectorDict.Add("ModifiedAtDatepickerField", (selector: "//div[contains(@class, 'modified')]/input", type: SelectorType.XPath));
		}

		//outgoing Reference web elements

		//Attribute web Elements
		private IWebElement UnitElement => FindElementExt("UnitElement");
		private IWebElement AddressLine1Element => FindElementExt("AddressLine1Element");
		private IWebElement AddressLine2Element => FindElementExt("AddressLine2Element");
		private IWebElement SuburbElement => FindElementExt("SuburbElement");
		private IWebElement PostcodeElement => FindElementExt("PostcodeElement");
		private IWebElement CityElement => FindElementExt("CityElement");
		private IWebElement CountryElement => FindElementExt("CountryElement");
		private IWebElement NameElement => FindElementExt("NameElement");

		// Return an IWebElement that can be used to sort an attribute.
		public IWebElement GetHeaderTile(string attribute)
		{
			return attribute switch
			{
				"Unit" => UnitHeaderTitle,
				"Address line 1" => AddressLine1HeaderTitle,
				"Address line 2" => AddressLine2HeaderTitle,
				"Suburb" => SuburbHeaderTitle,
				"Postcode" => PostcodeHeaderTitle,
				"City" => CityHeaderTitle,
				"Country" => CountryHeaderTitle,
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
				case "Unit":
					return UnitElement;
				case "AddressLine1":
					return AddressLine1Element;
				case "AddressLine2":
					return AddressLine2Element;
				case "Suburb":
					return SuburbElement;
				case "Postcode":
					return PostcodeElement;
				case "City":
					return CityElement;
				case "Country":
					return CountryElement;
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
				case "Unit":
					SetUnit(value);
					break;
				case "AddressLine1":
					SetAddressLine1(value);
					break;
				case "AddressLine2":
					SetAddressLine2(value);
					break;
				case "Suburb":
					SetSuburb(value);
					break;
				case "Postcode":
					int? postcode = null;
					if (int.TryParse(value, out var intPostcode))
					{
						postcode = intPostcode;
					}
					SetPostcode(postcode);
					break;
				case "City":
					SetCity(value);
					break;
				case "Country":
					SetCountry(value);
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
				"Unit" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.unit > div > p"),
				"AddressLine1" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.addressLine1 > div > p"),
				"AddressLine2" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.addressLine2 > div > p"),
				"Suburb" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.suburb > div > p"),
				"Postcode" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.postcode > div > p"),
				"City" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.city > div > p"),
				"Country" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.country > div > p"),
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
			SetName(_addressEntity.Name);
			SetUnit(_addressEntity.Unit);
			SetAddressLine1(_addressEntity.AddressLine1);
			SetAddressLine2(_addressEntity.AddressLine2);
			SetSuburb(_addressEntity.Suburb);
			SetPostcode(_addressEntity.Postcode);
			SetCity(_addressEntity.City);
			SetCountry(_addressEntity.Country);

			if (_addressEntity.StaffsIds != null)
			{
				SetStaffss(_addressEntity.StaffsIds.Select(x => x.ToString()));
			}
			if (_addressEntity.StudentsIds != null)
			{
				SetStudentss(_addressEntity.StudentsIds.Select(x => x.ToString()));
			}
			// % protected region % [Configure entity application here] end
		}

		public List<Guid> GetAssociation(string referenceName)
		{
			switch (referenceName)
			{
				case "staffs":
					return GetStaffss();
				case "students":
					return GetStudentss();
				default:
					throw new Exception($"Cannot find association type {referenceName}");
			}
		}

		// set associations
		private void SetStaffss(IEnumerable<string> ids)
		{
			WaitUtils.elementState(_driverWait, StaffssInputElementBy, ElementState.VISIBLE);
			var staffssInputElement = _driver.FindElementExt(StaffssInputElementBy);

			foreach(var id in ids)
			{
				staffssInputElement.SendKeys(id);
				WaitForDropdownOptions();
				staffssInputElement.SendKeys(Keys.Return);
			}
		}

		private void SetStudentss(IEnumerable<string> ids)
		{
			WaitUtils.elementState(_driverWait, StudentssInputElementBy, ElementState.VISIBLE);
			var studentssInputElement = _driver.FindElementExt(StudentssInputElementBy);

			foreach(var id in ids)
			{
				studentssInputElement.SendKeys(id);
				WaitForDropdownOptions();
				studentssInputElement.SendKeys(Keys.Return);
			}
		}


		// get associations
		private List<Guid> GetStaffss()
		{
			var guids = new List<Guid>();
			WaitUtils.elementState(_driverWait, StaffssElementBy, ElementState.VISIBLE);
			var staffssElement = _driver.FindElements(StaffssElementBy);

			foreach(var element in staffssElement)
			{
				guids.Add(new Guid (element.GetAttribute("data-id")));
			}
			return guids;
		}
		private List<Guid> GetStudentss()
		{
			var guids = new List<Guid>();
			WaitUtils.elementState(_driverWait, StudentssElementBy, ElementState.VISIBLE);
			var studentssElement = _driver.FindElements(StudentssElementBy);

			foreach(var element in studentssElement)
			{
				guids.Add(new Guid (element.GetAttribute("data-id")));
			}
			return guids;
		}

		// wait for dropdown to be displaying options
		private void WaitForDropdownOptions()
		{
			var xpath = "//*/div[@aria-expanded='true']";
			var elementBy = WebElementUtils.GetElementAsBy(SelectorPathType.XPATH, xpath);
			WaitUtils.elementState(_driverWait, elementBy,ElementState.EXISTS);
		}

		private void SetUnit (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "unit", value, _isFastText);
			UnitElement.SendKeys(Keys.Tab);
			UnitElement.SendKeys(Keys.Escape);
		}

		private String GetUnit =>
			UnitElement.Text;

		private void SetAddressLine1 (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "addressLine1", value, _isFastText);
			AddressLine1Element.SendKeys(Keys.Tab);
			AddressLine1Element.SendKeys(Keys.Escape);
		}

		private String GetAddressLine1 =>
			AddressLine1Element.Text;

		private void SetAddressLine2 (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "addressLine2", value, _isFastText);
			AddressLine2Element.SendKeys(Keys.Tab);
			AddressLine2Element.SendKeys(Keys.Escape);
		}

		private String GetAddressLine2 =>
			AddressLine2Element.Text;

		private void SetSuburb (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "suburb", value, _isFastText);
			SuburbElement.SendKeys(Keys.Tab);
			SuburbElement.SendKeys(Keys.Escape);
		}

		private String GetSuburb =>
			SuburbElement.Text;

		private void SetPostcode (int? value)
		{
			if (value is int intValue)
			{
				TypingUtils.InputEntityAttributeByClass(_driver, "postcode", intValue.ToString(), _isFastText);
			}
		}

		private int? GetPostcode =>
			int.Parse(PostcodeElement.Text);

		private void SetCity (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "city", value, _isFastText);
			CityElement.SendKeys(Keys.Tab);
			CityElement.SendKeys(Keys.Escape);
		}

		private String GetCity =>
			CityElement.Text;

		private void SetCountry (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "country", value, _isFastText);
			CountryElement.SendKeys(Keys.Tab);
			CountryElement.SendKeys(Keys.Escape);
		}

		private String GetCountry =>
			CountryElement.Text;


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