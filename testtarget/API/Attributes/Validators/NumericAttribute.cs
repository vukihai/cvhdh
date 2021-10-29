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
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace APITests.Attributes.Validators
{
	public class NumericAttribute: ValidationAttribute
	{
		// % protected region % [Override NumericRegex here] off begin
		public readonly string NumericRegex =
			@"^[0-9]*$";
		// % protected region % [Override NumericRegex here] end
		// % protected region % [Override ValidationResult here] off begin
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var dispayName = validationContext.DisplayName;
			var stringValue = value != null? value.ToString(): "";
			if (string.IsNullOrEmpty(stringValue))
			{
				return ValidationResult.Success;
			}
			var match = Regex.Match(stringValue, NumericRegex);
			return match.Success ? ValidationResult.Success : new ValidationResult($"{dispayName} is not a valid numeric string");
		}
		// % protected region % [Override ValidationResult here] end
	}
}