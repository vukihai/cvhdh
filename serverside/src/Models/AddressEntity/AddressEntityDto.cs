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
	public class AddressEntityDto : ModelDto<AddressEntity>
	{
		public String Name { get; set; }
		// % protected region % [Customise Unit here] off begin
		public String Unit { get; set; }
		// % protected region % [Customise Unit here] end

		// % protected region % [Customise AddressLine1 here] off begin
		public String AddressLine1 { get; set; }
		// % protected region % [Customise AddressLine1 here] end

		// % protected region % [Customise AddressLine2 here] off begin
		public String AddressLine2 { get; set; }
		// % protected region % [Customise AddressLine2 here] end

		// % protected region % [Customise Suburb here] off begin
		public String Suburb { get; set; }
		// % protected region % [Customise Suburb here] end

		// % protected region % [Customise Postcode here] off begin
		public int? Postcode { get; set; }
		// % protected region % [Customise Postcode here] end

		// % protected region % [Customise City here] off begin
		public String City { get; set; }
		// % protected region % [Customise City here] end

		// % protected region % [Customise Country here] off begin
		public String Country { get; set; }
		// % protected region % [Customise Country here] end


		// % protected region % [Add any extra attributes here] off begin
		// % protected region % [Add any extra attributes here] end

		public AddressEntityDto(AddressEntity model)
		{
			LoadModelData(model);
			// % protected region % [Add any constructor logic here] off begin
			// % protected region % [Add any constructor logic here] end
		}

		public AddressEntityDto()
		{
			// % protected region % [Add any parameterless constructor logic here] off begin
			// % protected region % [Add any parameterless constructor logic here] end
		}

		public override AddressEntity ToModel()
		{
			// % protected region % [Add any extra ToModel logic here] off begin
			// % protected region % [Add any extra ToModel logic here] end

			return new AddressEntity
			{
				Id = Id,
				Created = Created,
				Modified = Modified,
				Name = Name,
				Unit = Unit,
				AddressLine1 = AddressLine1,
				AddressLine2 = AddressLine2,
				Suburb = Suburb,
				Postcode = Postcode,
				City = City,
				Country = Country,
				// % protected region % [Add any extra model properties here] off begin
				// % protected region % [Add any extra model properties here] end
			};
		}

		public override ModelDto<AddressEntity> LoadModelData(AddressEntity model)
		{
			Id = model.Id;
			Created = model.Created;
			Modified = model.Modified;
			Name = model.Name;
			Unit = model.Unit;
			AddressLine1 = model.AddressLine1;
			AddressLine2 = model.AddressLine2;
			Suburb = model.Suburb;
			Postcode = model.Postcode;
			City = model.City;
			Country = model.Country;

			// % protected region % [Add any extra loading data logic here] off begin
			// % protected region % [Add any extra loading data logic here] end

			return this;
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}