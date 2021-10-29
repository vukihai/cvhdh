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
using APITests.EntityObjects.Models;
using APITests.Factories;
using SeleniumTests.PageObjects.CRUDPageObject.PageDetails;
using SeleniumTests.Setup;

namespace SeleniumTests.Factories
{
	internal class EntityDetailFactory
	{
		private readonly ContextConfiguration _contextConfiguration;

		public EntityDetailFactory(ContextConfiguration contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
		}

		public BaseEntity ApplyDetails(string entityName, bool isValid)
		{
			var entityFactory = new EntityFactory(entityName);
			var entity = entityFactory.Construct(isValid);
			entity.Configure(BaseEntity.ConfigureOptions.CREATE_ATTRIBUTES_AND_REFERENCES);
			CreateDetailSection(entityName, entity).Apply();
			return entity;
		}

		public IEntityDetailSection CreateDetailSection(string entityName, BaseEntity entity = null)
		{
			return entityName switch
			{
				"AchievementsEntity" => new AchievementsEntityDetailSection(_contextConfiguration, (AchievementsEntity) entity),
				"AddressEntity" => new AddressEntityDetailSection(_contextConfiguration, (AddressEntity) entity),
				"StudentsEntity" => new StudentsEntityDetailSection(_contextConfiguration, (StudentsEntity) entity),
				"AssessmentNotesEntity" => new AssessmentNotesEntityDetailSection(_contextConfiguration, (AssessmentNotesEntity) entity),
				"AssessmentsEntity" => new AssessmentsEntityDetailSection(_contextConfiguration, (AssessmentsEntity) entity),
				"CommentsEntity" => new CommentsEntityDetailSection(_contextConfiguration, (CommentsEntity) entity),
				"StaffEntity" => new StaffEntityDetailSection(_contextConfiguration, (StaffEntity) entity),
				"AchievementsSubmissionEntity" => new AchievementsSubmissionEntityDetailSection(_contextConfiguration, (AchievementsSubmissionEntity) entity),
				"AddressSubmissionEntity" => new AddressSubmissionEntityDetailSection(_contextConfiguration, (AddressSubmissionEntity) entity),
				"StudentsSubmissionEntity" => new StudentsSubmissionEntityDetailSection(_contextConfiguration, (StudentsSubmissionEntity) entity),
				"AchievementsEntityFormTileEntity" => new AchievementsEntityFormTileEntityDetailSection(_contextConfiguration, (AchievementsEntityFormTileEntity) entity),
				"StudentsEntityFormTileEntity" => new StudentsEntityFormTileEntityDetailSection(_contextConfiguration, (StudentsEntityFormTileEntity) entity),
				_ => throw new Exception($"Cannot find entity type {entityName}"),
			};
		}
	}
}
