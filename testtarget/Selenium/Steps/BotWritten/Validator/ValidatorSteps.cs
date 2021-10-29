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
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using APITests.Attributes.Validators;
using APITests.Factories;
using SeleniumTests.PageObjects.CRUDPageObject;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.Enums;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using RestSharp.Extensions;
using Xunit;
using UrlAttribute = APITests.Attributes.Validators.UrlAttribute;

namespace SeleniumTests.Steps.BotWritten.Validator
{
	[Binding]
	public sealed class ValidatorSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;
		private string _entityName;
		private GenericEntityEditPage _createPage;

		public ValidatorSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}

		[StepDefinition("I navigate to the (.*) create page")]
		public void WhenINavigateToTheCreatePage(string entityName)
		{
			_entityName = entityName;
			var page = new GenericEntityPage(entityName, _contextConfiguration);
			page.Navigate();
			_createPage = page.ClickCreateButton();
		}

		[Then(@"I verify the (.*) Validator for the (.*) attribute for (.*) validation")]
		public void IVerifyAWarningForLiveValidators(ValidatorType validator, string attribute, ValidatorActionType validatorType)
		{
			var factory = new EntityFactory(_entityName);
			var entity = factory.Construct();
			var propertyInfo = entity.GetType().GetProperty(attribute);
			System.Attribute validatorAttribute = validator switch
			{
				ValidatorType.LENGTH => propertyInfo.GetCustomAttribute<MaxLengthAttribute>(),
				ValidatorType.EMAIL => propertyInfo.GetCustomAttribute<EmailAttribute>(),
				ValidatorType.NUMERIC => propertyInfo.GetCustomAttribute<NumericAttribute>(),
				ValidatorType.REQUIRED => propertyInfo.GetCustomAttribute<RequiredAttribute>() ?? new RequiredAttribute(),
				ValidatorType.ALPHA_NUMERIC => propertyInfo.GetCustomAttribute<AlphanumericAttribute>(),
				ValidatorType.URL => propertyInfo.GetCustomAttribute<UrlAttribute>(),
				ValidatorType.UUID => propertyInfo.GetCustomAttribute<UuidAttribute>(),
				_ => throw new ArgumentException($"{validator.ToString()} not found on entity attribute {attribute} ")
			};
			var invalidAttribute = entity.GetInvalidAttribute(propertyInfo, validatorAttribute);
			var createPageDetailsSection = EntityDetailUtils.GetEntityDetailsSection(_entityName, _contextConfiguration);
			createPageDetailsSection.SetInputElement(attribute, invalidAttribute.value?.ToString() ?? "");

			switch (validatorType)
			{
				case ValidatorActionType.ON_SUBMIT:
					_createPage.SubmitButton.Click();
					break;
				case ValidatorActionType.LIVE:
					createPageDetailsSection.GetInputElement(attribute).SendKeys(Keys.Tab);
					break;
			}

			var errors = createPageDetailsSection.GetErrorMessagesForAttribute(attribute);

			if (propertyInfo.GetCustomAttribute<MinLengthAttribute>() != null &&
			    propertyInfo.GetCustomAttribute<MaxLengthAttribute>() != null &&
			    validatorAttribute.GetType() == typeof(MaxLengthAttribute))
			{
				var minLength = propertyInfo.GetCustomAttribute<MinLengthAttribute>().Length;
				var maxLength = propertyInfo.GetCustomAttribute<MaxLengthAttribute>().Length;
				Assert.Contains($"The length of this field is not {minLength} and {maxLength}. Actual Length: {invalidAttribute.value.ToString().Length}", errors);
			}
			else
			{
				switch (validatorAttribute)
				{
					case MaxLengthAttribute attr:
						Assert.Contains($"The length of this field is not less than {attr.Length}. Actual Length: {invalidAttribute.value.ToString().Length}", errors);
						break;
					case EmailAttribute _:
						Assert.Contains($"The value is not a valid email", errors);
						break;
					case NumericAttribute _:
						Assert.Contains("You can only use numbers in this field", errors);
						break;
					case RequiredAttribute _:
						Assert.Contains("This field is required", errors);
						break;
					case UuidAttribute _:
						Assert.Contains("The value is not a valid uuid", errors);
						break;
					case UrlAttribute _:
						Assert.Contains("The value is not a valid url", errors);
						break;
					case AlphanumericAttribute _:
						Assert.Contains("You can only use alphanumeric characters in this field", errors);
						break;
					default:
						throw new Exception($"{validator} is not a valid validator");
				}
			}
		}

		[StepArgumentTransformation]
		public static ValidatorType TransformStringToValidatorTypeEnum(string validatorType)
		{
			return (validatorType.ToLower()) switch
			{
				"length" => ValidatorType.LENGTH,
				"email" => ValidatorType.EMAIL,
				"numeric" => ValidatorType.NUMERIC,
				"required" => ValidatorType.REQUIRED,
				"alphanumeric" => ValidatorType.ALPHA_NUMERIC,
				"url" => ValidatorType.URL,
				"uuid" => ValidatorType.UUID,
				_ => throw new Exception($"{validatorType} enum is not handled"),
			};
		}

		[StepArgumentTransformation]
		public static ValidatorActionType TransformStringToValidatorActionTypeEnum(string validatorActionType)
		{
			switch (validatorActionType.ToLower())
			{
				case "on-submit":
					return ValidatorActionType.ON_SUBMIT;
				case "live":
					return ValidatorActionType.LIVE;
				default:
					throw new Exception($"{validatorActionType} enum is not handled");
			}
		}
	}
}