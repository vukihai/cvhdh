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
	public class StudentsEntityDto : ModelDto<StudentsEntity>
	{
		public String Name { get; set; }
		// % protected region % [Customise FirstName here] off begin
		public String FirstName { get; set; }
		// % protected region % [Customise FirstName here] end

		// % protected region % [Customise LastName here] off begin
		public String LastName { get; set; }
		// % protected region % [Customise LastName here] end

		// % protected region % [Customise ContactNumber here] off begin
		public String ContactNumber { get; set; }
		// % protected region % [Customise ContactNumber here] end

		// % protected region % [Customise Email here] off begin
		public String Email { get; set; }
		// % protected region % [Customise Email here] end

		// % protected region % [Customise EnrolmentStart here] off begin
		public DateTime? EnrolmentStart { get; set; }
		// % protected region % [Customise EnrolmentStart here] end

		// % protected region % [Customise EnrolmentEnd here] off begin
		public DateTime? EnrolmentEnd { get; set; }
		// % protected region % [Customise EnrolmentEnd here] end


		// % protected region % [Customise AddressId here] off begin
		public Guid? AddressId { get; set; }
		// % protected region % [Customise AddressId here] end

		// % protected region % [Add any extra attributes here] off begin
		// % protected region % [Add any extra attributes here] end

		public StudentsEntityDto(StudentsEntity model)
		{
			LoadModelData(model);
			// % protected region % [Add any constructor logic here] off begin
			// % protected region % [Add any constructor logic here] end
		}

		public StudentsEntityDto()
		{
			// % protected region % [Add any parameterless constructor logic here] off begin
			// % protected region % [Add any parameterless constructor logic here] end
		}

		public override StudentsEntity ToModel()
		{
			// % protected region % [Add any extra ToModel logic here] off begin
			// % protected region % [Add any extra ToModel logic here] end

			return new StudentsEntity
			{
				Id = Id,
				Created = Created,
				Modified = Modified,
				Name = Name,
				FirstName = FirstName,
				LastName = LastName,
				ContactNumber = ContactNumber,
				Email = Email,
				EnrolmentStart = EnrolmentStart,
				EnrolmentEnd = EnrolmentEnd,
				AddressId  = AddressId,
				// % protected region % [Add any extra model properties here] off begin
				// % protected region % [Add any extra model properties here] end
			};
		}

		public override ModelDto<StudentsEntity> LoadModelData(StudentsEntity model)
		{
			Id = model.Id;
			Created = model.Created;
			Modified = model.Modified;
			Name = model.Name;
			FirstName = model.FirstName;
			LastName = model.LastName;
			ContactNumber = model.ContactNumber;
			Email = model.Email;
			EnrolmentStart = model.EnrolmentStart;
			EnrolmentEnd = model.EnrolmentEnd;
			AddressId  = model.AddressId;

			// % protected region % [Add any extra loading data logic here] off begin
			// % protected region % [Add any extra loading data logic here] end

			return this;
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}