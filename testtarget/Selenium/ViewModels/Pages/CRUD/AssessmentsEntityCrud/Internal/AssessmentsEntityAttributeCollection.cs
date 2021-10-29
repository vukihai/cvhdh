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
using SeleniumTests.PageObjects.Pages;
using SeleniumTests.Setup;
using SeleniumTests.ViewModels.Components.Attribute;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.ViewModels.Pages.CRUD.AssessmentsEntityCrud.Internal
{
	public class AssessmentsEntityAttributeCollection : Page
	{
		// % protected region % [Override class properties here] off begin
		public AttributeDatePicker StartDate => new(By.CssSelector("div.startDate"), ContextConfiguration);
		public AttributeDatePicker EndDate => new(By.CssSelector("div.endDate"), ContextConfiguration);
		public AttributeTextField Summary => new(By.CssSelector("div.summary"), ContextConfiguration);
		public AttributeTextField Recommendations => new(By.CssSelector("div.recommendations"), ContextConfiguration);
		public AttributeReferenceMultiCombobox AssessmentNotesIds => new(By.CssSelector("div.assessmentNotess"), ContextConfiguration);
		public AttributeReferenceMultiCombobox CommentsIds => new(By.CssSelector("div.commentss"), ContextConfiguration);
		public AttributeReferenceCombobox StudentsId => new(By.CssSelector("div.studentsId"), ContextConfiguration);
		// % protected region % [Override class properties here] end

		// % protected region % [Override constructor here] off begin
		public AssessmentsEntityAttributeCollection(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
		}
		// % protected region % [Override constructor here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}