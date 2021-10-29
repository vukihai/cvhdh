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
	public class CommentsEntityDetailSection : BasePage, IEntityDetailSection
	{
		private readonly IWait<IWebDriver> _driverWait;
		private readonly IWebDriver _driver;
		private readonly bool _isFastText;
		private readonly ContextConfiguration _contextConfiguration;

		// reference elements
		private static By AssessmentsIdElementBy => By.XPath("//*[contains(@class, 'assessments')]//div[contains(@class, 'dropdown__container')]");
		private static By AssessmentsIdInputElementBy => By.XPath("//*[contains(@class, 'assessments')]/div/input");

		//FlatPickr Elements
		private DateTimePickerComponent CommentDateElement => new DateTimePickerComponent(_contextConfiguration, "commentDate");

		//Attribute Headers
		private readonly CommentsEntity _commentsEntity;

		//Attribute Header Titles
		private IWebElement CommentDateHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Comment Date']"));
		private IWebElement CommentDescriptionHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Comment Description']"));

		// Datepickers
		public IWebElement CreateAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.created > input[type='date']"));
		public IWebElement ModifiedAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.modified > input[type='date']"));

		public CommentsEntityDetailSection(ContextConfiguration contextConfiguration, CommentsEntity commentsEntity = null) : base(contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			_driverWait = contextConfiguration.WebDriverWait;
			_isFastText = contextConfiguration.SeleniumSettings.FastText;
			_contextConfiguration = contextConfiguration;
			_commentsEntity = commentsEntity;

			InitializeSelectors();
			// % protected region % [Add any extra construction requires] off begin

			// % protected region % [Add any extra construction requires] end
		}

		// initialise all selectors and grouping them with the selector type which is used
		private void InitializeSelectors()
		{
			// Attribute web elements
			selectorDict.Add("CommentDescriptionElement", (selector: "//div[contains(@class, 'commentDescription')]//input", type: SelectorType.XPath));

			// Reference web elements
			selectorDict.Add("AssessmentsElement", (selector: ".input-group__dropdown.assessmentsId > .dropdown.dropdown__container", type: SelectorType.CSS));

			// Datepicker
			selectorDict.Add("CreateAtDatepickerField", (selector: "//div[contains(@class, 'created')]/input", type: SelectorType.XPath));
			selectorDict.Add("ModifiedAtDatepickerField", (selector: "//div[contains(@class, 'modified')]/input", type: SelectorType.XPath));
		}

		//outgoing Reference web elements
		//get the input path as set by the selector library
		private IWebElement AssessmentsElement => FindElementExt("AssessmentsElement");

		//Attribute web Elements
		private IWebElement CommentDescriptionElement => FindElementExt("CommentDescriptionElement");

		// Return an IWebElement that can be used to sort an attribute.
		public IWebElement GetHeaderTile(string attribute)
		{
			return attribute switch
			{
				"Comment Date" => CommentDateHeaderTitle,
				"Comment Description" => CommentDescriptionHeaderTitle,
				_ => throw new Exception($"Cannot find header tile {attribute}"),
			};
		}

		// Return an IWebElement for an attribute input
		public IWebElement GetInputElement(string attribute)
		{
			switch (attribute)
			{
				case "CommentDate":
					return CommentDateElement.DateTimePickerElement;
				case "CommentDescription":
					return CommentDescriptionElement;
				default:
					throw new Exception($"Cannot find input element {attribute}");
			}
		}

		public void SetInputElement(string attribute, string value)
		{
			switch (attribute)
			{
				case "CommentDate":
					if (DateTime.TryParse(value, out var commentDateValue))
					{
						SetCommentDate(commentDateValue);
					}
					break;
				case "CommentDescription":
					SetCommentDescription(value);
					break;
				default:
					throw new Exception($"Cannot find input element {attribute}");
			}
		}

		private By GetErrorAttributeSectionAsBy(string attribute)
		{
			return attribute switch
			{
				"CommentDate" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.commentDate > div > p"),
				"CommentDescription" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.commentDescription > div > p"),
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
			SetCommentDate(_commentsEntity.CommentDate);
			SetCommentDescription(_commentsEntity.CommentDescription);

			SetAssessmentsId(_commentsEntity.AssessmentsId?.ToString());
			// % protected region % [Configure entity application here] end
		}

		public List<Guid> GetAssociation(string referenceName)
		{
			switch (referenceName)
			{
				case "assessments":
					return new List<Guid>() {GetAssessmentsId()};
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

		// get associations
		private Guid GetAssessmentsId()
		{
			WaitUtils.elementState(_driverWait, AssessmentsIdElementBy, ElementState.VISIBLE);
			var assessmentsIdElement = _driver.FindElementExt(AssessmentsIdElementBy);
			return new Guid(assessmentsIdElement.GetAttribute("data-id"));
		}

		// wait for dropdown to be displaying options
		private void WaitForDropdownOptions()
		{
			var xpath = "//*/div[@aria-expanded='true']";
			var elementBy = WebElementUtils.GetElementAsBy(SelectorPathType.XPATH, xpath);
			WaitUtils.elementState(_driverWait, elementBy,ElementState.EXISTS);
		}

		private void SetCommentDate (DateTime? value)
		{
			if (value is DateTime datetimeValue)
			{
				CommentDateElement.SetDate(datetimeValue);
			}
		}

		private DateTime? GetCommentDate =>
			Convert.ToDateTime(CommentDateElement.DateTimePickerElement.Text);
		private void SetCommentDescription (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "commentDescription", value, _isFastText);
			CommentDescriptionElement.SendKeys(Keys.Tab);
			CommentDescriptionElement.SendKeys(Keys.Escape);
		}

		private String GetCommentDescription =>
			CommentDescriptionElement.Text;


		// % protected region % [Add any additional getters and setters of web elements] off begin
		// % protected region % [Add any additional getters and setters of web elements] end
	}
}