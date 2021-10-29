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
using System.Reflection;
using APITests.Attributes;
using APITests.Attributes.Validators;
using APITests.Extensions;
using APITests.Setup;
using APITests.Utils;
using EntityObject.Enums;
using Newtonsoft.Json.Linq;
using RestSharp;
using TestDataLib;
using UrlAttribute = APITests.Attributes.Validators.UrlAttribute;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace APITests.EntityObjects.Models
{
	public class Reference
	{
		public string Name { get; set; }
		public string OppositeName { get; set; }
		public string EntityName { get; set; }
		public bool Optional { get; set; }
		public ReferenceType Type { get; set; }
		public ReferenceType OppositeType { get; set; }
	}

	public class Attribute
	{
		public string Name { get; set; }
		public bool IsRequired { get; set;}
	}

	public abstract class BaseEntity
	{
		public abstract void Configure(ConfigureOptions option);
		public abstract Guid Save();
		public abstract Dictionary<string, string> ToDictionary();
		public abstract List<Guid> GetManyToManyReferences (string reference);
		public abstract void SetReferences (Dictionary<string, ICollection<Guid>> entityReferences);
		public abstract RestSharp.JsonObject ToJson();
		public abstract (int min, int max) GetLengthValidatorMinMax(string attribute);
		public abstract IEnumerable<(string error, JsonObject jsonObject)> GetInvalidMutatedJsons();
		public ICollection<Reference> References = new List<Reference>();
		public ICollection<BaseEntity> ParentEntities = new List<BaseEntity>();
		public Guid Id = Guid.NewGuid();
		public DateTime Created = DateTime.Now;
		public DateTime Modified = DateTime.Now;
		public Dictionary<string, Guid?> ReferenceIdDictionary { get; set;} = new Dictionary<string, Guid?>();
		public string EntityName { get; set; }
		public virtual bool HasFile { get; set; } = false;
		private readonly StartupTestFixture _configure = new StartupTestFixture();

		internal Guid SaveThroughGraphQl(BaseEntity model)
		{
			var api = new WebApi(_configure);
			var query = QueryBuilder.CreateEntityQueryBuilder(new List<BaseEntity>{model});
			api.ConfigureAuthenticationHeaders();

			IRestResponse response;
			if (model is IFileContainingEntity fileContainingEntity)
			{
				var headers = new Dictionary<string, string>{{"Content-Type", "multipart/form-data"}};
				var files = fileContainingEntity.GetFiles().Where(file => file != null);;
				var param = new Dictionary<string, object>
				{
					{"operationName", query["operationName"]},
					{"variables", query["variables"]},
					{"query", query["query"]}
				};
				response = api.Post($"/api/graphql", param, headers, DataFormat.None, files);
			}
			else
			{
				response = api.Post($"/api/graphql", query);
			}

			var content = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
			var entityId = ((JObject) content)?["data"]?[query["operationName"]]?[0]?["id"]?.ToString();
			return entityId != null ? Guid.Parse(entityId) : Id;
		}

		/// <summary>
		/// Gets all of the properties for an entity with the [EntityAttribute] annotation.
		/// </summary>
		/// <returns>List<PropertyInfo></returns>
		public List<PropertyInfo> GetEntityAttributeProperties()
		{
			return GetType()
				.GetProperties()
				.Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(EntityAttribute)))
				.ToList();
		}
		/// <summary>
		/// Sets an invalid value for a property on an entity.
		/// </summary>
		/// <param name="attributeName">The property/attribute to set the value for</param>
		/// <param name="validatorType">The validator to violate</param>
		/// <returns>expected error for the invalid value</returns>
		/// <exception cref="ArgumentException"></exception>
		private string SetInvalidAttribute(string attributeName, Type validatorType)
		{
			var propertyInfo = GetType().GetProperty(attributeName);

			if (propertyInfo == null)
			{
				throw new ArgumentException($"The attribute name {attributeName} is invalid for entity {EntityName}");
			}

			var validator = propertyInfo.GetCustomAttribute(validatorType);
			if (validator == null)
			{
				throw new ArgumentException($"The validator type {validatorType.Name} is invalid for attribute {attributeName} on entity {EntityName}");
			}

			var (value, error) = GetInvalidAttribute(propertyInfo, validator);
			propertyInfo.SetValue(this, value);
			return error;
		}
		/// <summary>
		/// Gets a version of the entity with one of the attributes set to an invalid value
		/// </summary>
		/// <typeparam name="T">Entity Type</typeparam>
		/// <returns>A T entity along with an error string for the invalid attribute</returns>
		/// <exception cref="Exception"></exception>
		protected static (T entity, string error) GetInvalidEntity<T>()
			where T : BaseEntity, new()
		{
			var entity = new T();

			var attribute = entity.GetEntityAttributeProperties()
				.FirstOrDefault(x =>
					x.CustomAttributes.Any(a => a.AttributeType.BaseType == typeof(ValidationAttribute)));

			if (attribute == null)
			{
				throw new Exception($"The entity {entity.EntityName} does not have any validators");
			}

			var validator =
				attribute.CustomAttributes.FirstOrDefault(x => x.AttributeType.BaseType == typeof(ValidationAttribute));

			entity.PopulateAttributes();
			var error = entity.SetInvalidAttribute(attribute.Name, validator.AttributeType);

			return (entity, error);
		}
		/// <summary>
		/// Gets all permutations of invalid entity.
		/// </summary>
		/// <typeparam name="T">Entity Type</typeparam>
		/// <returns>An enumarable of tuples. Each tuple contains an entity of type T and an error message</returns>

		protected IEnumerable<(T entity, string error)> GetInvalidEntities<T>()
			where T : BaseEntity, new()
		{
			var entityList = new List<(T entity, string error)>();
			var attributes = GetEntityAttributeProperties()
				.Where(x => x.CustomAttributes.Any(a => a.AttributeType.BaseType == typeof(ValidationAttribute)))
				.ToList();

			foreach (var attribute in attributes)
			{
				var validators =
					attribute.CustomAttributes.Where(x => x.AttributeType.BaseType == typeof(ValidationAttribute));
				foreach (var validator in validators)
				{
					var entity = new T();
					entity.PopulateAttributes();
					var error = entity.SetInvalidAttribute(attribute.Name, validator.AttributeType);
					entityList.Add((entity, error));
				}
			}
			return entityList;
		}
		/// <summary>
		/// Get an invalid property for an attribute, given a property and a attribute/validator
		/// </summary>
		/// <param name="property">The property to get an invalid value for</param>
		/// <param name="attribute">The attribute/validator attached to the property to violate</param>
		/// <returns>A value (object)_</returns>
		/// <exception cref="Exception"></exception>
		public (object value, string errorMessage) GetInvalidAttribute(PropertyInfo property, System.Attribute attribute)
		{
			if (attribute.GetType() == typeof(RequiredAttribute))
			{
				return (null, $"The {property.Name} field is required");
			}

			if (property.PropertyType == typeof(string))
			{
				return attribute switch
				{
					EmailAttribute _ => (DataUtils.RandString(), $"{property.Name} is not a valid email"),
					UrlAttribute _ => (DataUtils.RandString(), $"{property.Name} is not a valid url"),
					UuidAttribute _ => (DataUtils.RandString(), $"{property.Name} is not a valid Uuid"),
					NumericAttribute attr => (DataUtils.RandString(), $"{property.Name} is not a valid numeric string"),
					AlphanumericAttribute attr => (DataUtils.RandEmail(), $"{property.Name} is not a valid alpha-numeric string"),
					MinLengthAttribute attr => (DataUtils.RandString(attr.Length - 1), $"The field {property.Name} must be a string or array type with a minimum length of \\u0027{attr.Length}\\u0027"),
					MaxLengthAttribute attr => (DataUtils.RandString(attr.Length + 1), $"The field {property.Name} must be a string or array type with a maximum length of \\u0027{attr.Length}\\u0027"),
					_ => throw new Exception("Invalid validator passed to attribute generator"),
				};
			}
			else if (property.PropertyType == typeof(double?))
			{
				return attribute switch
				{
					RangeAttribute attr => (DataUtils.RandDouble((int) attr.Maximum + 1, (int) attr.Maximum + 10), $"The field {property.Name} must be between {attr.Minimum} and {attr.Maximum}"),
					_ => throw new Exception("Invalid validator passed to attribute generator"),
				};
			}
			else if (property.PropertyType == typeof(int?))
			{
				return attribute switch
				{
					RangeAttribute attr => (DataUtils.RandInt((int) attr.Maximum + 1, (int) attr.Maximum + 10), $"The field {property.Name} must be between {attr.Minimum} and {attr.Maximum}"),
					_ => throw new Exception("Invalid validator passed to attribute generator"),
				};
			}
			throw new Exception("Invalid property and validator passed to attribute generator");
		}
		/// <summary>
		/// Sets valid values for all properties on an Entity with the [EntityAttribute] tag.
		/// </summary>
		protected void PopulateAttributes()
		{
			var properties = GetEntityAttributeProperties();

			foreach (var property in properties)
			{
				// string
				if (property.PropertyType == typeof(string))
				{
					// get length validator value if they exists
					var minLength = property.GetCustomAttribute<MinLengthAttribute>()?.Length ?? 1;
					var maxLength = property.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? minLength + 10;

					// these validators can have 'length' optionally added to them
					if (property.HasAttribute(typeof(NumericAttribute)))
					{
						property.SetValue(this, DataUtils.RandNumericString(minLength, maxLength));
					}
					else if (property.HasAttribute(typeof(AlphanumericAttribute)))
					{
						property.SetValue(this, DataUtils.RandAlphanumericString(minLength, maxLength));
					}

					// these validators can only have the 'required' validator in addition
					else if (property.HasAttribute(typeof(EmailAttribute)))
					{
						property.SetValue(this, DataUtils.RandEmail());
					}
					else if (property.HasAttribute(typeof(UuidAttribute)) || property.HasAttribute(typeof(UniqueAttribute)))
					{
						property.SetValue(this, Guid.NewGuid().ToString());
					}
					else if (property.HasAttribute(typeof(UrlAttribute)))
					{
						property.SetValue(this, DataUtils.RandUrl());
					}

					// none of the above validators
					else
					{
						property.SetValue(this, DataUtils.RandString());
					}
				}
				// numeric
				else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?) ||
				         property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
				{
					// get length validator value if they exists
					var minRange = ((int?) property.GetCustomAttribute<RangeAttribute>()?.Minimum) ?? 1;
					var maxRange = ((int?) property.GetCustomAttribute<RangeAttribute>()?.Maximum) ?? minRange + 10;

					if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
					{
						property.SetValue(this, DataUtils.RandInt(minRange, maxRange));
					}
					else if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
					{
						property.SetValue(this, DataUtils.RandDouble(minRange, maxRange));
					}
				}
				// boolean
				else if (property.PropertyType == typeof(Boolean) || property.PropertyType == typeof(Boolean?))
				{
					property.SetValue(this, DataUtils.RandBool());
				}
				// guid
				else if (property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?))
				{
					property.SetValue(this, Guid.NewGuid());
				}
				// datetime
				else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
				{
					property.SetValue(this, DateTime.Now);
				}
			}
		}

		public enum ConfigureOptions
		{
			CREATE_ATTRIBUTES_AND_REFERENCES,
			CREATE_ATTRIBUTES_ONLY,
			CREATE_REFERENCES_ONLY,
			CREATE_INVALID_ATTRIBUTES,
			CREATE_INVALID_ATTRIBUTES_VALID_REFERENCES
		}
	}
}