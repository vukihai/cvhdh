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
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Models
{
	public class AssessmentsEntityDto : ModelDto<AssessmentsEntity>
	{
		// % protected region % [Customise StartDate here] off begin
		public DateTime? StartDate { get; set; }
		// % protected region % [Customise StartDate here] end

		// % protected region % [Customise EndDate here] off begin
		public DateTime? EndDate { get; set; }
		// % protected region % [Customise EndDate here] end

		// % protected region % [Customise Summary here] off begin
		public String Summary { get; set; }
		// % protected region % [Customise Summary here] end

		// % protected region % [Customise Recommendations here] off begin
		public String Recommendations { get; set; }
		// % protected region % [Customise Recommendations here] end


		// % protected region % [Customise StudentsId here] off begin
		public Guid? StudentsId { get; set; }
		// % protected region % [Customise StudentsId here] end

		// % protected region % [Add any extra attributes here] off begin
		// % protected region % [Add any extra attributes here] end

		public AssessmentsEntityDto(AssessmentsEntity model)
		{
			LoadModelData(model);
			// % protected region % [Add any constructor logic here] off begin
			// % protected region % [Add any constructor logic here] end
		}

		public AssessmentsEntityDto()
		{
			// % protected region % [Add any parameterless constructor logic here] off begin
			// % protected region % [Add any parameterless constructor logic here] end
		}

		public override AssessmentsEntity ToModel()
		{
			// % protected region % [Add any extra ToModel logic here] off begin
			// % protected region % [Add any extra ToModel logic here] end

			return new AssessmentsEntity
			{
				Id = Id,
				Created = Created,
				Modified = Modified,
				StartDate = StartDate,
				EndDate = EndDate,
				Summary = Summary,
				Recommendations = Recommendations,
				StudentsId  = StudentsId,
				// % protected region % [Add any extra model properties here] off begin
				// % protected region % [Add any extra model properties here] end
			};
		}

		public override ModelDto<AssessmentsEntity> LoadModelData(AssessmentsEntity model)
		{
			Id = model.Id;
			Created = model.Created;
			Modified = model.Modified;
			StartDate = model.StartDate;
			EndDate = model.EndDate;
			Summary = model.Summary;
			Recommendations = model.Recommendations;
			StudentsId  = model.StudentsId;

			// % protected region % [Add any extra loading data logic here] off begin
			// % protected region % [Add any extra loading data logic here] end

			return this;
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}