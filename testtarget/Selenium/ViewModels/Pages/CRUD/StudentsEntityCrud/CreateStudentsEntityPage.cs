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
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.ViewModels.Pages.CRUD.StudentsEntityCrud.Internal;
using SeleniumTests.ViewModels.Pages.Crud.Internal;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.ViewModels.Pages.CRUD.StudentsEntityCrud
{
	public class CreateStudentsEntityPage : StudentsEntityAttributeCollection
	{
		// % protected region % [Override class properties here] off begin
		public string Url => ContextConfiguration.BaseUrl + "/admin/studentsEntity/create";
		public CrudCreateButtons ActionButtons => new(ContextConfiguration);
		// % protected region % [Override class properties here] end

		// % protected region % [Override constructor here] off begin
		public CreateStudentsEntityPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
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
		public void SetValues(StudentsEntity studentsEntity)
		{
			Name.Value = studentsEntity.Name;
			FirstName.Value = studentsEntity.FirstName;
			LastName.Value = studentsEntity.LastName;
			ContactNumber.Value = studentsEntity.ContactNumber;
			Email.Value = studentsEntity.Email;
			EnrolmentStart.Value = studentsEntity.EnrolmentStart.GetValueOrDefault();
			EnrolmentEnd.Value = studentsEntity.EnrolmentEnd.GetValueOrDefault();
			AchievementsIds.Value = studentsEntity.AchievementsIds?.Select(x => x.ToString());
			AssessmentsIds.Value = studentsEntity.AssessmentsIds?.Select(x => x.ToString());
			AddressId.Value = studentsEntity.AddressId == null ? string.Empty : studentsEntity.AddressId.ToString();
		}
		// % protected region % [Override set values here] end

		// % protected region % [Override get values here] off begin
		public StudentsEntity GetValues()
		{
			var studentsEntity =  new StudentsEntity
			{
				Name = Name.Value,
				FirstName = FirstName.Value,
				LastName = LastName.Value,
				ContactNumber = ContactNumber.Value,
				Email = Email.Value,
				EnrolmentStart = EnrolmentStart.Value,
				EnrolmentEnd = EnrolmentEnd.Value,
			};

			studentsEntity.AchievementsIds = AchievementsIds.Value.Select(Guid.Parse).ToList();;
			studentsEntity.AssessmentsIds = AssessmentsIds.Value.Select(Guid.Parse).ToList();;
			if (Guid.TryParse(AddressId.Value, out var addressId)) {
				studentsEntity.AddressId = addressId;
			}
			return studentsEntity;
		}
		// % protected region % [Override get values here] end

		// % protected region % [Add class methods here] off begin
		// % protected region % [Add class methods here] end
	}
}