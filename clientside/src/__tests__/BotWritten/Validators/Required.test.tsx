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
import { Model } from 'Models/Model';
import * as Validators from 'Validators';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise RequiredValidatorTheoryData here] off begin
const RequiredValidatorTheoryData = [
	["", false],
	[undefined, false],
	[null, false],
	["Hello Wordl!!", true]
];
// % protected region % [Customise RequiredValidatorTheoryData here] end

// % protected region % [Customise TestModel here] off begin
class TestModel extends Model {
	@Validators.Required()
	testName: string | null | undefined | boolean
}
// % protected region % [Customise TestModel here] end

// % protected region % [Customise Required Validators test label here] off begin
describe('Required Validators', () => {
// % protected region % [Customise Required Validators test label here] end
	// % protected region % [Customise Required Validators validation to be test label here] off begin
	//@ts-ignore
	test.each(RequiredValidatorTheoryData)('we expect %p, validation to be %p', async (inputString, isValid) => {
	// % protected region % [Customise Required Validators validation to be test label here] end
		// % protected region % [Customise Required Validators validation to be test here] off begin
		expect.assertions(1);

		let testModel = new TestModel()
		testModel.testName = inputString

		await testModel.validate().then(x => {
			const errors = testModel.getErrorsForAttribute("testName");

			if (isValid) {
				expect(errors).toEqual([]);
			} else {
				expect(errors).toEqual(["This field is required"]);
			}
		});
		// % protected region % [Customise Required Validators validation to be test here] end
	});
	// % protected region % [Add any extra tests here] off begin
	// % protected region % [Add any extra tests here] end
});

// % protected region % [Add any extra content here] off begin
// % protected region % [Add any extra content here] end