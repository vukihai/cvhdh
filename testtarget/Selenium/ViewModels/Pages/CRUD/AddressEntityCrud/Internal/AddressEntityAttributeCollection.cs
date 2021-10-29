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

namespace SeleniumTests.ViewModels.Pages.CRUD.AddressEntityCrud.Internal
{
	public class AddressEntityAttributeCollection : Page
	{
		// % protected region % [Override class properties here] off begin
		public AttributeTextField Name => new(By.CssSelector("div.name"), ContextConfiguration);
		public AttributeTextField Unit => new(By.CssSelector("div.unit"), ContextConfiguration);
		public AttributeTextField AddressLine1 => new(By.CssSelector("div.addressLine1"), ContextConfiguration);
		public AttributeTextField AddressLine2 => new(By.CssSelector("div.addressLine2"), ContextConfiguration);
		public AttributeTextField Suburb => new(By.CssSelector("div.suburb"), ContextConfiguration);
		public AttributeTextField Postcode => new(By.CssSelector("div.postcode"), ContextConfiguration);
		public AttributeTextField City => new(By.CssSelector("div.city"), ContextConfiguration);
		public AttributeTextField Country => new(By.CssSelector("div.country"), ContextConfiguration);
		public AttributeReferenceMultiCombobox StaffsIds => new(By.CssSelector("div.staffss"), ContextConfiguration);
		public AttributeReferenceMultiCombobox StudentsIds => new(By.CssSelector("div.studentss"), ContextConfiguration);
		public AttributeReferenceMultiCombobox FormPageIds => new(By.CssSelector("div.formPages"), ContextConfiguration);
		// % protected region % [Override class properties here] end

		// % protected region % [Override constructor here] off begin
		public AddressEntityAttributeCollection(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
		}
		// % protected region % [Override constructor here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}