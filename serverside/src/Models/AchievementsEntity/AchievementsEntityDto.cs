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
using Firstapp2257.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Models
{
	public class AchievementsEntityDto : ModelDto<AchievementsEntity>
	{
		public String Name { get; set; }
		// % protected region % [Customise AchievementDate here] off begin
		public DateTime? AchievementDate { get; set; }
		// % protected region % [Customise AchievementDate here] end

		// % protected region % [Customise AchievementDetails here] off begin
		public String AchievementDetails { get; set; }
		// % protected region % [Customise AchievementDetails here] end

		// % protected region % [Customise AchievementType here] off begin
		[JsonProperty("achievementType")]
		[JsonConverter(typeof(StringEnumConverter))]
		public AchievementTypes AchievementType { get; set; }
		// % protected region % [Customise AchievementType here] end


		// % protected region % [Customise StudentsId here] off begin
		public Guid? StudentsId { get; set; }
		// % protected region % [Customise StudentsId here] end

		// % protected region % [Add any extra attributes here] off begin
		// % protected region % [Add any extra attributes here] end

		public AchievementsEntityDto(AchievementsEntity model)
		{
			LoadModelData(model);
			// % protected region % [Add any constructor logic here] off begin
			// % protected region % [Add any constructor logic here] end
		}

		public AchievementsEntityDto()
		{
			// % protected region % [Add any parameterless constructor logic here] off begin
			// % protected region % [Add any parameterless constructor logic here] end
		}

		public override AchievementsEntity ToModel()
		{
			// % protected region % [Add any extra ToModel logic here] off begin
			// % protected region % [Add any extra ToModel logic here] end

			return new AchievementsEntity
			{
				Id = Id,
				Created = Created,
				Modified = Modified,
				Name = Name,
				AchievementDate = AchievementDate,
				AchievementDetails = AchievementDetails,
				AchievementType = AchievementType,
				StudentsId  = StudentsId,
				// % protected region % [Add any extra model properties here] off begin
				// % protected region % [Add any extra model properties here] end
			};
		}

		public override ModelDto<AchievementsEntity> LoadModelData(AchievementsEntity model)
		{
			Id = model.Id;
			Created = model.Created;
			Modified = model.Modified;
			Name = model.Name;
			AchievementDate = model.AchievementDate;
			AchievementDetails = model.AchievementDetails;
			AchievementType = model.AchievementType;
			StudentsId  = model.StudentsId;

			// % protected region % [Add any extra loading data logic here] off begin
			// % protected region % [Add any extra loading data logic here] end

			return this;
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}