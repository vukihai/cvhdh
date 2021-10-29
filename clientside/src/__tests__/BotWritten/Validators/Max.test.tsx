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

// % protected region % [Customise MaximumValidatorTheoryData here] off begin
const MaximumValidatorTheoryData = [
	[12, true],
	[11, true],
	[0, true],
	[-12, true],
	[12.1, false],
	[13, false],
	[500, false]
];
// % protected region % [Customise MaximumValidatorTheoryData here] end

// % protected region % [Customise MaxTestNumber here] off begin
const MaxTestNumber = 12;
// % protected region % [Customise MaxTestNumber here] end

// % protected region % [Customise TestModel here] off begin
class TestModel extends Model {
	@Validators.Max(MaxTestNumber)
	testNumber: number
}
// % protected region % [Customise TestModel here] end

// % protected region % [Customise Max Validators tests label here] off begin
describe('Max Validators', () => {
// % protected region % [Customise Max Validators tests label here] end
	// % protected region % [Customise validation to be test label here] off begin
	test.each(MaximumValidatorTheoryData)('we expect %p, validation to be %p', async (inputNumber, isValid) => {
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
					expect(errors).toEqual([`The value is ${inputNumber} which is greater than the required amount of ${MaxTestNumber}`]);
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