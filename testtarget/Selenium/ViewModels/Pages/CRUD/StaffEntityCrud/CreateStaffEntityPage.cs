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
using APITests.EntityObjects.Models;
using EntityObject.Enums;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using OpenQA.Selenium;
using SeleniumTests.ViewModels.Components.Attribute;
using SeleniumTests.ViewModels.Pages.CRUD.StaffEntityCrud.Internal;
using SeleniumTests.ViewModels.Pages.Crud.Internal;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.ViewModels.Pages.CRUD.StaffEntityCrud
{
	public class CreateStaffEntityPage : StaffEntityAttributeCollection
	{
		// % protected region % [Override class properties here] off begin
		public string Url => ContextConfiguration.BaseUrl + "/admin/staffEntity/create";
		public AttributePassword Password => new(By.CssSelector("div.password"), ContextConfiguration);
		public AttributePassword ConfirmPassword => new(By.CssSelector("div._confirmPassword"), ContextConfiguration);
		public CrudCreateButtons ActionButtons => new(ContextConfiguration);
		// % protected region % [Override class properties here] end

		// % protected region % [Override constructor here] off begin
		public CreateStaffEntityPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
		}
		// % protected region % [Override constructor here] end
		// % protected region % [Override Navigate here] off begin
		public void Navigate()
		{
			ContextConfiguration.WebDriver.GoToUrlExt(ContextConfiguration.WebDriverWait, Url);
		}
		// % protected region % [Override Navigate here] end

		// % protected region % [Override set values here] off begin
		public void SetValues(StaffEntity staffEntity)
		{
			EmailAddress.Value = staffEntity.EmailAddress;
			Password.Value = staffEntity.Password;
			ConfirmPassword.Value = staffEntity.Password;
			FirstName.Value = staffEntity.FirstName;
			LastName.Value = staffEntity.LastName;
			Role.Value = staffEntity.Role.ToString();
			ContactNumber.Value = staffEntity.ContactNumber;
			AssessmentNotesIds.Value = staffEntity.AssessmentNotesIds?.Select(x => x.ToString());
			AddressId.Value = staffEntity.AddressId == null ? string.Empty : staffEntity.AddressId.ToString();
		}
		// % protected region % [Override set values here] end

		// % protected region % [Override get values here] off begin
		public StaffEntity GetValues()
		{
			var staffEntity =  new StaffEntity
			{
				EmailAddress = EmailAddress.Value,
				Password = Password.Value,
				FirstName = FirstName.Value,
				LastName = LastName.Value,
				Role = Role.Value.ToEnum<StaffRoles>(),
				ContactNumber = ContactNumber.Value,
			};

			staffEntity.AssessmentNotesIds = AssessmentNotesIds.Value.Select(Guid.Parse).ToList();;
			if (Guid.TryParse(AddressId.Value, out var addressId)) {
				staffEntity.AddressId = addressId;
			}
			return staffEntity;
		}
		// % protected region % [Override get values here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}