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
	public class StaffEntityDto : ModelDto<StaffEntity>
	{
		public string Email { get; set; }
		// % protected region % [Customise FirstName here] off begin
		public String FirstName { get; set; }
		// % protected region % [Customise FirstName here] end

		// % protected region % [Customise LastName here] off begin
		public String LastName { get; set; }
		// % protected region % [Customise LastName here] end

		// % protected region % [Customise Role here] off begin
		[JsonProperty("role")]
		[JsonConverter(typeof(StringEnumConverter))]
		public StaffRoles Role { get; set; }
		// % protected region % [Customise Role here] end

		// % protected region % [Customise ContactNumber here] off begin
		public String ContactNumber { get; set; }
		// % protected region % [Customise ContactNumber here] end


		// % protected region % [Customise AddressId here] off begin
		public Guid? AddressId { get; set; }
		// % protected region % [Customise AddressId here] end

		// % protected region % [Add any extra attributes here] off begin
		// % protected region % [Add any extra attributes here] end

		public StaffEntityDto(StaffEntity model)
		{
			LoadModelData(model);
			// % protected region % [Add any constructor logic here] off begin
			// % protected region % [Add any constructor logic here] end
		}

		public StaffEntityDto()
		{
			// % protected region % [Add any parameterless constructor logic here] off begin
			// % protected region % [Add any parameterless constructor logic here] end
		}

		public override StaffEntity ToModel()
		{
			// % protected region % [Add any extra ToModel logic here] off begin
			// % protected region % [Add any extra ToModel logic here] end

			return new StaffEntity
			{
				Id = Id,
				Created = Created,
				Modified = Modified,
				Email = Email,
				FirstName = FirstName,
				LastName = LastName,
				Role = Role,
				ContactNumber = ContactNumber,
				AddressId  = AddressId,
				// % protected region % [Add any extra model properties here] off begin
				// % protected region % [Add any extra model properties here] end
			};
		}

		public override ModelDto<StaffEntity> LoadModelData(StaffEntity model)
		{
			Id = model.Id;
			Created = model.Created;
			Modified = model.Modified;
			Email = model.Email;
			FirstName = model.FirstName;
			LastName = model.LastName;
			Role = model.Role;
			ContactNumber = model.ContactNumber;
			AddressId  = model.AddressId;

			// % protected region % [Add any extra loading data logic here] off begin
			// % protected region % [Add any extra loading data logic here] end

			return this;
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}