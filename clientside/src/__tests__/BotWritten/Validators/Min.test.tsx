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

// % protected region % [Customise MinimumValidatorTheoryData here] off begin
const MinimumValidatorTheoryData = [
	[12, true],
	[11, false],
	[0, false],
	[-12, false],
	[12.1, true],
	[13, true],
	[500, true]
];
// % protected region % [Customise MinimumValidatorTheoryData here] end

// % protected region % [Customise MinTestNumber here] off begin
const MinTestNumber = 12;
// % protected region % [Customise MinTestNumber here] end

// % protected region % [Customise TestModel here] off begin
class TestModel extends Model {
	@Validators.Min(MinTestNumber)
	testNumber: number
}
// % protected region % [Customise TestModel here] end

// % protected region % [Customise Minimum Validators tests label here] off begin
describe('Minimum Validators', () => {
// % protected region % [Customise Minimum Validators tests label here] end
	// % protected region % [Customise validation to be test label here] off begin
	test.each(MinimumValidatorTheoryData)('we expect %p, validation to be %p', async (inputNumber, isValid) => {
	// % protected region % [Customise validation to be test label here] end
		// % protected region % [Customise validation to be test here] off begin
		expect.assertions(1);
		if (typeof (inputNumber) === 'number') {
			let testModel = new TestModel()
			testModel.testNumber = inputNumber

			await testModel.validate().then(x => {
				const errors = testModel.getErrorsForAttribute("testNumber");
				if (isValid) {
					expect(errors).toEqual([]);
				} else {
					expect(errors).toEqual([`The value is ${inputNumber} which is less than the required amount of ${MinTestNumber}`]);
				}
			});
		}
		// % protected region % [Customise validation to be test here] end
	});
	// % protected region % [Add any extra tests here] off begin
	// % protected region % [Add any extra tests here] end
});

// % protected region % [Add any extra content here] off begin
// % protected region % [Add any extra content here] end