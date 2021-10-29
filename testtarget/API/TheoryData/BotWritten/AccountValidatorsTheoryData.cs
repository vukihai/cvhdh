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
using APITests.Factories;
using Xunit;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace APITests.TheoryData.BotWritten
{
	public class PasswordInvalidTheoryData : TheoryData<UserEntityFactory, string, string>
	{
		public const string PasswordMustContainDigitError = "Passwords must have at least one digit ('0'-'9').";
		public const string PasswordMustContainUppercaseError = "Passwords must have at least one uppercase ('A'-'Z').";
		public const string PasswordMustContainNonAlphanumericError = "Passwords must have at least one non alphanumeric character.";
		public const string PasswordLengthError = "Passwords must be at least 6 characters.";

		public PasswordInvalidTheoryData()
		{
			// % protected region % [Add any further password error here] off begin
			// % protected region % [Add any further password error here] end

			// % protected region % [Modify PasswordInvalidTheoryData entities here] off begin
			Add(
				new UserEntityFactory("StaffEntity"),
				"pass",
				PasswordLengthError);
			// % protected region % [Modify PasswordInvalidTheoryData entities here] end

			// % protected region % [Add any further test cases here] off begin
			// % protected region % [Add any further test cases here] end
		}
	}

	public class UsernameInvalidTheoryData : TheoryData<UserEntityFactory, string, string>
	{
		public UsernameInvalidTheoryData()
		{
			// % protected region % [Modify UsernameInvalidTheoryData entities here] off begin
			var InvalidEmailError = "Email is not a valid email";

			Add(
				new UserEntityFactory("StaffEntity"),
				"super@example.com",
				"Username 'super@example.com' is already taken."
			);
			Add(
				new UserEntityFactory("StaffEntity"),
				"super",
				InvalidEmailError
			);
			Add(
				new UserEntityFactory("StaffEntity"),
				"@e.c*@example.com",
				InvalidEmailError
			);
			// % protected region % [Modify UsernameInvalidTheoryData entities here] end
		}
	}
}