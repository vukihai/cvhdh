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
	public class AssessmentsEntityDetailSection : BasePage, IEntityDetailSection
	{
		private readonly IWait<IWebDriver> _driverWait;
		private readonly IWebDriver _driver;
		private readonly bool _isFastText;
		private readonly ContextConfiguration _contextConfiguration;

		// reference elements
		private static By AssessmentNotessElementBy => By.XPath("//*[contains(@class, 'assessmentNotes')]//div[contains(@class, 'dropdown__container')]/a");
		private static By AssessmentNotessInputElementBy => By.XPath("//*[contains(@class, 'assessmentNotes')]/div/input");
		private static By StudentsIdElementBy => By.XPath("//*[contains(@class, 'students')]//div[contains(@class, 'dropdown__container')]");
		private static By StudentsIdInputElementBy => By.XPath("//*[contains(@class, 'students')]/div/input");
		private static By CommentssElementBy => By.XPath("//*[contains(@class, 'comments')]//div[contains(@class, 'dropdown__container')]/a");
		private static By CommentssInputElementBy => By.XPath("//*[contains(@class, 'comments')]/div/input");

		//FlatPickr Elements
		private DateTimePickerComponent StartDateElement => new DateTimePickerComponent(_contextConfiguration, "startDate");
		private DateTimePickerComponent EndDateElement => new DateTimePickerComponent(_contextConfiguration, "endDate");

		//Attribute Headers
		private readonly AssessmentsEntity _assessmentsEntity;

		//Attribute Header Titles
		private IWebElement StartDateHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Start Date']"));
		private IWebElement EndDateHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='End Date']"));
		private IWebElement SummaryHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Summary']"));
		private IWebElement RecommendationsHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='Recommendations']"));

		// Datepickers
		public IWebElement CreateAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.created > input[type='date']"));
		public IWebElement ModifiedAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.modified > input[type='date']"));

		public AssessmentsEntityDetailSection(ContextConfiguration contextConfiguration, AssessmentsEntity assessmentsEntity = null) : base(contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			_driverWait = contextConfiguration.WebDriverWait;
			_isFastText = contextConfiguration.SeleniumSettings.FastText;
			_contextConfiguration = contextConfiguration;
			_assessmentsEntity = assessmentsEntity;

			InitializeSelectors();
			// % protected region % [Add any extra construction requires] off begin

			// % protected region % [Add any extra construction requires] end
		}

		// initialise all selectors and grouping them with the selector type which is used
		private void InitializeSelectors()
		{
			// Attribute web elements
			selectorDict.Add("SummaryElement", (selector: "//div[contains(@class, 'summary')]//input", type: SelectorType.XPath));
			selectorDict.Add("RecommendationsElement", (selector: "//div[contains(@class, 'recommendations')]//input", type: SelectorType.XPath));

			// Reference web elements
			selectorDict.Add("AssessmentnotesElement", (selector: ".input-group__dropdown.assessmentNotess > .dropdown.dropdown__container", type: SelectorType.CSS));
			selectorDict.Add("StudentsElement", (selector: ".input-group__dropdown.studentsId > .dropdown.dropdown__container", type: SelectorType.CSS));
			selectorDict.Add("CommentsElement", (selector: ".input-group__dropdown.commentss > .dropdown.dropdown__container", type: SelectorType.CSS));

			// Datepicker
			selectorDict.Add("CreateAtDatepickerField", (selector: "//div[contains(@class, 'created')]/input", type: SelectorType.XPath));
			selectorDict.Add("ModifiedAtDatepickerField", (selector: "//div[contains(@class, 'modified')]/input", type: SelectorType.XPath));
		}

		//outgoing Reference web elements
		//get the input path as set by the selector library
		private IWebElement StudentsElement => FindElementExt("StudentsElement");

		//Attribute web Elements
		private IWebElement SummaryElement => FindElementExt("SummaryElement");
		private IWebElement RecommendationsElement => FindElementExt("RecommendationsElement");

		// Return an IWebElement that can be used to sort an attribute.
		public IWebElement GetHeaderTile(string attribute)
		{
			return attribute switch
			{
				"Start Date" => StartDateHeaderTitle,
				"End Date" => EndDateHeaderTitle,
				"Summary" => SummaryHeaderTitle,
				"Recommendations" => RecommendationsHeaderTitle,
				_ => throw new Exception($"Cannot find header tile {attribute}"),
			};
		}

		// Return an IWebElement for an attribute input
		public IWebElement GetInputElement(string attribute)
		{
			switch (attribute)
			{
				case "StartDate":
					return StartDateElement.DateTimePickerElement;
				case "EndDate":
					return EndDateElement.DateTimePickerElement;
				case "Summary":
					return SummaryElement;
				case "Recommendations":
					return RecommendationsElement;
				default:
					throw new Exception($"Cannot find input element {attribute}");
			}
		}

		public void SetInputElement(string attribute, string value)
		{
			switch (attribute)
			{
				case "StartDate":
					if (DateTime.TryParse(value, out var startDateValue))
					{
						SetStartDate(startDateValue);
					}
					break;
				case "EndDate":
					if (DateTime.TryParse(value, out var endDateValue))
					{
						SetEndDate(endDateValue);
					}
					break;
				case "Summary":
					SetSummary(value);
					break;
				case "Recommendations":
					SetRecommendations(value);
					break;
				default:
					throw new Exception($"Cannot find input element {attribute}");
			}
		}

		private By GetErrorAttributeSectionAsBy(string attribute)
		{
			return attribute switch
			{
				"StartDate" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.startDate > div > p"),
				"EndDate" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.endDate > div > p"),
				"Summary" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.summary > div > p"),
				"Recommendations" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.recommendations > div > p"),
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
			SetStartDate(_assessmentsEntity.StartDate);
			SetEndDate(_assessmentsEntity.EndDate);
			SetSummary(_assessmentsEntity.Summary);
			SetRecommendations(_assessmentsEntity.Recommendations);

			if (_assessmentsEntity.AssessmentNotesIds != null)
			{
				SetAssessmentNotess(_assessmentsEntity.AssessmentNotesIds.Select(x => x.ToString()));
			}
			SetStudentsId(_assessmentsEntity.StudentsId?.ToString());
			if (_assessmentsEntity.CommentsIds != null)
			{
				SetCommentss(_assessmentsEntity.CommentsIds.Select(x => x.ToString()));
			}
			// % protected region % [Configure entity application here] end
		}

		public List<Guid> GetAssociation(string referenceName)
		{
			switch (referenceName)
			{
				case "assessmentnotes":
					return GetAssessmentNotess();
				case "students":
					return new List<Guid>() {GetStudentsId()};
				case "comments":
					return GetCommentss();
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
		private void SetCommentss(IEnumerable<string> ids)
		{
			WaitUtils.elementState(_driverWait, CommentssInputElementBy, ElementState.VISIBLE);
			var commentssInputElement = _driver.FindElementExt(CommentssInputElementBy);

			foreach(var id in ids)
			{
				commentssInputElement.SendKeys(id);
				WaitForDropdownOptions();
				commentssInputElement.SendKeys(Keys.Return);
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
		private Guid GetStudentsId()
		{
			WaitUtils.elementState(_driverWait, StudentsIdElementBy, ElementState.VISIBLE);
			var studentsIdElement = _driver.FindElementExt(StudentsIdElementBy);
			return new Guid(studentsIdElement.GetAttribute("data-id"));
		}
		private List<Guid> GetCommentss()
		{
			var guids = new List<Guid>();
			WaitUtils.elementState(_driverWait, CommentssElementBy, ElementState.VISIBLE);
			var commentssElement = _driver.FindElements(CommentssElementBy);

			foreach(var element in commentssElement)
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

		private void SetStartDate (DateTime? value)
		{
			if (value is DateTime datetimeValue)
			{
				StartDateElement.SetDate(datetimeValue);
			}
		}

		private DateTime? GetStartDate =>
			Convert.ToDateTime(StartDateElement.DateTimePickerElement.Text);
		private void SetEndDate (DateTime? value)
		{
			if (value is DateTime datetimeValue)
			{
				EndDateElement.SetDate(datetimeValue);
			}
		}

		private DateTime? GetEndDate =>
			Convert.ToDateTime(EndDateElement.DateTimePickerElement.Text);
		private void SetSummary (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "summary", value, _isFastText);
			SummaryElement.SendKeys(Keys.Tab);
			SummaryElement.SendKeys(Keys.Escape);
		}

		private String GetSummary =>
			SummaryElement.Text;

		private void SetRecommendations (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "recommendations", value, _isFastText);
			RecommendationsElement.SendKeys(Keys.Tab);
			RecommendationsElement.SendKeys(Keys.Escape);
		}

		private String GetRecommendations =>
			RecommendationsElement.Text;


		// % protected region % [Add any additional getters and setters of web elements] off begin
		// % protected region % [Add any additional getters and setters of web elements] end
	}
}