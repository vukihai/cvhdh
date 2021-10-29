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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using EntityObject.Enums;
using APITests.Classes;
using APITests.Attributes;
using APITests.Attributes.Validators;
using RestSharp;
using TestDataLib;

namespace APITests.EntityObjects.Models
{
	public class StudentsEntity : BaseEntity
	{
		// Form Name
		public string Name { get; set; }
		// 
		[Required]
		[EntityAttribute]
		public String FirstName { get; set; }
		// 
		[Required]
		[EntityAttribute]
		public String LastName { get; set; }
		// 
		[EntityAttribute]
		public String ContactNumber { get; set; }
		// 
		[EntityAttribute]
		public String Email { get; set; }
		// 
		[EntityAttribute]
		public DateTime? EnrolmentStart { get; set; }
		// 
		[EntityAttribute]
		public DateTime? EnrolmentEnd { get; set; }

		/// <summary>
		/// Outgoing one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.Achievements"/>
		public List<Guid> AchievementsIds { get; set; }
		public ICollection<AchievementsEntity> Achievementss { get; set; }

		/// <summary>
		/// Outgoing one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.Assessments"/>
		public List<Guid> AssessmentsIds { get; set; }
		public ICollection<AssessmentsEntity> Assessmentss { get; set; }

		/// <summary>
		/// Incoming one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.Address"/>
		public Guid? AddressId { get; set; }

		/// <summary>
		/// Outgoing one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.FormPage"/>
		public List<Guid> FormPageIds { get; set; }
		public ICollection<StudentsEntityFormTileEntity> FormPages { get; set; }


		public StudentsEntity()
		{
			EntityName = "StudentsEntity";
			InitialiseReferences();
		}

		public StudentsEntity(ConfigureOptions option)
		{
			Configure(option);
			InitialiseReferences();
		}

		public override void Configure(ConfigureOptions option)
		{
			switch (option)
			{
				case ConfigureOptions.CREATE_ATTRIBUTES_AND_REFERENCES:
					SetValidEntityAttributes();
					SetValidEntityAssociations();
					break;
				case ConfigureOptions.CREATE_ATTRIBUTES_ONLY:
					SetValidEntityAttributes();
					break;
				case ConfigureOptions.CREATE_REFERENCES_ONLY:
					SetValidEntityAssociations();
					break;
				case ConfigureOptions.CREATE_INVALID_ATTRIBUTES:
					break;
				case ConfigureOptions.CREATE_INVALID_ATTRIBUTES_VALID_REFERENCES:
					SetValidEntityAssociations();
					break;
			}
		}

		private void InitialiseReferences()
		{
			References.Add(new Reference
			{
				EntityName = "AddressEntity",
				OppositeName = "Address",
				Name = "Students",
				Optional = true,
				Type = ReferenceType.ONE,
				OppositeType = ReferenceType.MANY
			});
		}

		public override (int min, int max) GetLengthValidatorMinMax(string attribute)
		{
			switch(attribute)
			{
				default:
					throw new Exception($"{attribute} does not exist or does not have a length validator");
			}
		}

		// % protected region % [Customize GetInvalidMutatedJsons here] off begin
		/// <summary>
		/// Returns a list of invalid/mutated jsons and expected errors. The expected errors are the errors that
		/// should be returned when trying to use the invalid/mutated jsons in a create api request.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<(string error, RestSharp.JsonObject jsonObject)> GetInvalidMutatedJsons()
		{
			return GetInvalidEntities<StudentsEntity>()
				.Select(x => (x.error, x.entity.ToJson()));
		}
		// % protected region % [Customize GetInvalidMutatedJsons here] end

		public override Dictionary<string, string> ToDictionary()
		{
			var entityVar = new Dictionary<string, string>()
			{
				{"id" , Id.ToString()},
				{"name" , Name},
				{"firstName" , FirstName},
				{"lastName" , LastName},
				{"contactNumber" , ContactNumber},
				{"email" , Email},
				{"enrolmentStart" ,((DateTime)EnrolmentStart).ToIsoString()},
				{"enrolmentEnd" ,((DateTime)EnrolmentEnd).ToIsoString()},
			};

			if (AddressId != default)
			{
				entityVar["addressId"] = AddressId.ToString();
			}

			return entityVar;
		}

		// % protected region % [Customize ToJson here] off begin
		public override RestSharp.JsonObject ToJson()
		{
			var entityVar = new RestSharp.JsonObject
			{
				["id"] = Id,
				["name"] = Name,
			};
			if(FirstName != null) 
			{
				entityVar["firstName"] = FirstName.ToString();
			}
			if(LastName != null) 
			{
				entityVar["lastName"] = LastName.ToString();
			}
			if(ContactNumber != null) 
			{
				entityVar["contactNumber"] = ContactNumber.ToString();
			}
			if(Email != null) 
			{
				entityVar["email"] = Email.ToString();
			}
			if(EnrolmentStart != null) 
			{
				entityVar["enrolmentStart"] = EnrolmentStart?.ToString("s");
			}
			if(EnrolmentEnd != null) 
			{
				entityVar["enrolmentEnd"] = EnrolmentEnd?.ToString("s");
			}
			if (AchievementsIds != default)
			{
				entityVar["achievementss"] = Achievementss.Select(x => x.ToJson());
			}
			if (AssessmentsIds != default)
			{
				entityVar["assessmentss"] = Assessmentss.Select(x => x.ToJson());
			}
			if (AddressId  != default)
			{
				entityVar["addressId"] = AddressId.ToString();
			}
			if (FormPageIds != default)
			{
				entityVar["formPages"] = FormPages.Select(x => x.ToJson());
			}

			return entityVar;
		}
		// % protected region % [Customize ToJson here] end


		public override void SetReferences (Dictionary<string, ICollection<Guid>> entityReferences)
		{
			foreach (var (key, guidCollection) in entityReferences)
			{
				switch (key)
				{
					case "AddressId":
						ReferenceIdDictionary.Add("AddressId", guidCollection.FirstOrDefault());
						SetOneReference(key, guidCollection.FirstOrDefault());
						break;
					default:
						throw new Exception($"{key} not valid reference key");
				}
			}
		}

		private void SetOneReference (string key, Guid guid)
		{
			switch (key)
			{
				case "AddressId":
					AddressId = guid;
					break;
				default:
					throw new Exception($"{key} not valid reference key");
			}
		}

		private void SetManyReference (string key, ICollection<Guid> guids)
		{
			switch (key)
			{
				default:
					throw new Exception($"{key} not valid reference key");
			}
		}

		public override List<Guid> GetManyToManyReferences (string reference)
		{
			switch (reference)
			{
				default:
					throw new Exception($"{reference} not valid many to many reference key");
			}
		}

		private List<RestSharp.JsonObject> FormatManyToManyJsonList(string key, List<Guid> values)
		{
			var manyToManyList = new List<RestSharp.JsonObject>();
			values?.ForEach(x => manyToManyList.Add(new RestSharp.JsonObject {[key] = x }));
			return manyToManyList;
		}

		/// <summary>
		/// Gets an entity that violates the validators of its attributes,
		/// if any attributes have a validator to violate.
		/// </summary>
		// TODO needs some warning if trying to get an invalid entity, and the entity
		// attributes don't actually have any validators to violate.
		public static StudentsEntity GetEntity(bool isValid, string fixedValue = null)
		{
			if (isValid && !string.IsNullOrEmpty(fixedValue))
			{
				return GetValidEntity(fixedValue);
			}
			return isValid ? GetValidEntity() : GetInvalidEntity<StudentsEntity>().entity;
		}

		/// <summary>
		/// Created parents entities and set the association id's of this entity
		/// to those of the created parents.
		/// </summary>
		private void SetValidEntityAssociations()
		{
		}

		/// <summary>
		/// Gets an entity with attributes that conform to any attribute validators.
		/// </summary>
		private void SetValidEntityAttributes()
		{
			// % protected region % [Override generated entity attributes here] off begin
			Name = Guid.NewGuid().ToString();
			PopulateAttributes();
			// % protected region % [Override generated entity attributes here] end
		}

		/// <summary>
		/// Gets an entity with attributes that conform to any attribute validators.
		/// </summary>
		public static StudentsEntity GetValidEntity(string fixedStrValue = null)
		{
			var studentsEntity = new StudentsEntity
			{
				Name = Guid.NewGuid().ToString(),
			};
			studentsEntity.PopulateAttributes();
			// % protected region % [Customize valid entity before return here] off begin
			// % protected region % [Customize valid entity before return here] end

			return studentsEntity;
		}

		public override Guid Save()
		{
			return SaveThroughGraphQl(this);
		}
	}
}