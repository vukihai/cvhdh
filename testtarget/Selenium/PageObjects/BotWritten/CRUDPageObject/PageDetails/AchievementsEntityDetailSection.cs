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
	public class AchievementsEntityDetailSection : BasePage, IEntityDetailSection
	{
		private readonly IWait<IWebDriver> _driverWait;
		private readonly IWebDriver _driver;
		private readonly bool _isFastText;
		private readonly ContextConfiguration _contextConfiguration;

		// reference elements
		private static By StudentsIdElementBy => By.XPath("//*[contains(@class, 'students')]//div[contains(@class, 'dropdown__container')]");
		private static By StudentsIdInputElementBy => By.XPath("//*[contains(@class, 'students')]/div/input");

		//FlatPickr Elements
		private DateTimePickerComponent AchievementDateElement => new DateTimePickerComponent(_contextConfiguration, "achievementDate");

		//Attribute Headers
		private readonly AchievementsEntity _achievementsEntity;

		//Attribute Header Titles
		private IWebElement AchievementDateHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Achievement Date']"));
		private IWebElement AchievementDetailsHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Achievement Details']"));
		private IWebElement AchievementTypeHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Achievement Type']"));
		private IWebElement NameHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Name']"));

		// Datepickers
		public IWebElement CreateAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.created > input[type='date']"));
		public IWebElement ModifiedAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.modified > input[type='date']"));

		public AchievementsEntityDetailSection(ContextConfiguration contextConfiguration, AchievementsEntity achievementsEntity = null) : base(contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			_driverWait = contextConfiguration.WebDriverWait;
			_isFastText = contextConfiguration.SeleniumSettings.FastText;
			_contextConfiguration = contextConfiguration;
			_achievementsEntity = achievementsEntity;

			InitializeSelectors();
			// % protected region % [Add any extra construction requires] off begin
			// % protected region % [Add any extra construction requires] end
		}

		// initialise all selectors and grouping them with the selector type which is used
		private void InitializeSelectors()
		{
			// Attribute web elements
			selectorDict.Add("AchievementDetailsElement", (selector: "//div[contains(@class, 'achievementDetails')]//input", type: SelectorType.XPath));
			selectorDict.Add("AchievementTypeElement", (selector: "//div[contains(@class, 'achievementType')]//input", type: SelectorType.XPath));

			// Reference web elements
			selectorDict.Add("StudentsElement", (selector: ".input-group__dropdown.studentsId > .dropdown.dropdown__container", type: SelectorType.CSS));

			// Form Entity specific web Element
			selectorDict.Add("NameElement", (selector: "div.name > input", type: SelectorType.CSS));

			// Datepicker
			selectorDict.Add("CreateAtDatepickerField", (selector: "//div[contains(@class, 'created')]/input", type: SelectorType.XPath));
			selectorDict.Add("ModifiedAtDatepickerField", (selector: "//div[contains(@class, 'modified')]/input", type: SelectorType.XPath));
		}

		//outgoing Reference web elements
		//get the input path as set by the selector library
		private IWebElement StudentsElement => FindElementExt("StudentsElement");

		//Attribute web Elements
		private IWebElement AchievementDetailsElement => FindElementExt("AchievementDetailsElement");
		private IWebElement AchievementTypeElement => FindElementExt("AchievementTypeElement");
		private IWebElement NameElement => FindElementExt("NameElement");

		// Return an IWebElement that can be used to sort an attribute.
		public IWebElement GetHeaderTile(string attribute)
		{
			return attribute switch
			{
				"Achievement Date" => AchievementDateHeaderTitle,
				"Achievement Details" => AchievementDetailsHeaderTitle,
				"Achievement Type" => AchievementTypeHeaderTitle,
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
				case "AchievementDate":
					return AchievementDateElement.DateTimePickerElement;
				case "AchievementDetails":
					return AchievementDetailsElement;
				case "AchievementType":
					return AchievementTypeElement;
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
				case "AchievementDate":
					if (DateTime.TryParse(value, out var achievementDateValue))
					{
						SetAchievementDate(achievementDateValue);
					}
					break;
				case "AchievementDetails":
					SetAchievementDetails(value);
					break;
				case "AchievementType":
					SetAchievementType((AchievementTypes)Enum.Parse(typeof(AchievementTypes), value));
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
				"AchievementDate" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.achievementDate > div > p"),
				"AchievementDetails" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.achievementDetails > div > p"),
				"AchievementType" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.achievementType > div > p"),
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
			SetName(_achievementsEntity.Name);
			SetAchievementDate(_achievementsEntity.AchievementDate);
			SetAchievementDetails(_achievementsEntity.AchievementDetails);
			SetAchievementType(_achievementsEntity.AchievementType);

			SetStudentsId(_achievementsEntity.StudentsId?.ToString());
			// % protected region % [Configure entity application here] end
		}

		public List<Guid> GetAssociation(string referenceName)
		{
			switch (referenceName)
			{
				case "students":
					return new List<Guid>() {GetStudentsId()};
				default:
					throw new Exception($"Cannot find association type {referenceName}");
			}
		}

		// set associations
		private void SetStudentsId(string id)
		{
			if (id == "") { return; }
			WaitUtils.elementState(_driverWait, StudentsIdInputElementBy, ElementState.VISIBLE);
			var studentsIdInputElement = _driver.FindElementExt(StudentsIdInputElementBy);

			if (id != null)
			{
				studentsIdInputElement.SendKeys(id);
				WaitForDropdownOptions();
				WaitUtils.elementState(_driverWait, By.XPath($"//*/div[@role='option']/span[text()='{id}']"), ElementState.EXISTS);
				studentsIdInputElement.SendKeys(Keys.Return);
			}
		}

		// get associations
		private Guid GetStudentsId()
		{
			WaitUtils.elementState(_driverWait, StudentsIdElementBy, ElementState.VISIBLE);
			var studentsIdElement = _driver.FindElementExt(StudentsIdElementBy);
			return new Guid(studentsIdElement.GetAttribute("data-id"));
		}

		// wait for dropdown to be displaying options
		private void WaitForDropdownOptions()
		{
			var xpath = "//*/div[@aria-expanded='true']";
			var elementBy = WebElementUtils.GetElementAsBy(SelectorPathType.XPATH, xpath);
			WaitUtils.elementState(_driverWait, elementBy,ElementState.EXISTS);
		}

		private void SetAchievementDate (DateTime? value)
		{
			if (value is DateTime datetimeValue)
			{
				AchievementDateElement.SetDate(datetimeValue);
			}
		}

		private DateTime? GetAchievementDate =>
			Convert.ToDateTime(AchievementDateElement.DateTimePickerElement.Text);
		private void SetAchievementDetails (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "achievementDetails", value, _isFastText);
			AchievementDetailsElement.SendKeys(Keys.Tab);
			AchievementDetailsElement.SendKeys(Keys.Escape);
		}

		private String GetAchievementDetails =>
			AchievementDetailsElement.Text;

		private void SetAchievementType (AchievementTypes value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "achievementType", value.ToString(), _isFastText);
		}

		private AchievementTypes GetAchievementType =>
			(AchievementTypes)Enum.Parse(typeof(AchievementTypes), AchievementTypeElement.Text);

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