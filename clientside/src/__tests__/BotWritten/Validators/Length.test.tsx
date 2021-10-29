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

// % protected region % [Customise MinimumLengthValidatorTheoryData here] off begin
// TODO: add a rand string library to replace these hard coded strings below.
const MinimumLengthValidatorTheoryData = [
	["Star Wars", true],
	["Star Wars The Empire Strikes Back", true],
	["Star W", true],
	["Star", false],
	["", false],
];
// % protected region % [Customise MinimumLengthValidatorTheoryData here] end

// % protected region % [Customise MaximumLengthValidatorTheoryData here] off begin
const MaximumLengthValidatorTheoryData = [
	["Star Wars", true],
	["Star Wars Th", true],
	["Star W", true],
	["Star", true],
	["", true],
	["Star Wars The", false],
	["Star Wars The Empire Strikes Back", false],
];
// % protected region % [Customise MaximumLengthValidatorTheoryData here] end

// % protected region % [Customise MinimumMaximumLengthValidatorTheoryData here] off begin
const MinimumMaximumLengthValidatorTheoryData = [
	["Star Wars", true],
	["Star Wars Th", true],
	["Star W", true],
	["Star", false],
	["", false],
	["Star Wars The", false],
	["Star Wars The Empire Strikes Back", false],
];
// % protected region % [Customise MinimumMaximumLengthValidatorTheoryData here] end

// % protected region % [Customise Min and Max here] off begin
const MinTestNumber = 6;
const MaxTestNumber = 12;
// % protected region % [Customise Min and Max here] end

// % protected region % [Customise TestModel here] off begin
class TestModel extends Model {
	@Validators.Length(MinTestNumber, MaxTestNumber)
	testString: string
	@Validators.Length(MinTestNumber)
	minimumString: string
	@Validators.Length(undefined, MaxTestNumber)
	maximumString: string
}
// % protected region % [Customise TestModel here] end

// % protected region % [Customise Length Validators tests label here] off begin
describe('Length Validators', () => {
// % protected region % [Customise Length Validators tests label here] end
	// % protected region % [Customise min and max validation test label here] off begin
	test.each(MinimumMaximumLengthValidatorTheoryData)('we expect %p, min and max validation to be %p', async (inputString, isValid) => {
	// % protected region % [Customise min and max validation test label here] end
		// % protected region % [Customise min and max validation test here] off begin
		expect.assertions(1);
		if (typeof inputString !== 'string' || typeof isValid !== 'boolean') {
			throw "Invalid test arguments";
		}

		let testModel = new TestModel()
		testModel.testString = inputString

		await testModel.validate().then(x => {
			const errors = testModel.getErrorsForAttribute("testString");
			if (isValid) {
				expect(errors).toEqual([]);
			} else {
				expect(errors).toEqual([`The length of this field is not ${MinTestNumber} and ${MaxTestNumber}. Actual Length: ${inputString.length}`]);
			}
		});
		// % protected region % [Customise min and max validation test here] end
	});

	// % protected region % [Customise min validation test label here] off begin
	test.each(MinimumLengthValidatorTheoryData)('we expect %p, min validation to be %p', async (inputString, isValid) => {
	// % protected region % [Customise min validation test label here] end
		// % protected region % [Customise min validation test here] off begin
		expect.assertions(1);
		if (typeof inputString !== 'string' || typeof isValid !== 'boolean') {
			throw "Invalid test arguments";
		}

		let testModel = new TestModel()
		testModel.minimumString = inputString

		await testModel.validate().then(x => {
			const errors = testModel.getErrorsForAttribute("minimumString");
			if (isValid) {
				expect(errors).toEqual([]);
			} else {
				expect(errors).toEqual([`The length of this field is not greater than ${MinTestNumber}. Actual Length: ${inputString.length}`]);
			}
		});
		// % protected region % [Customise min validation test here] end
	});

	// % protected region % [Customise  max validation test label here] off begin
	test.each(MaximumLengthValidatorTheoryData)('we expect %p, max validation to be %p', async (inputString, isValid) => {
	// % protected region % [Customise  max validation test label here] end
		// % protected region % [Customise  max validation test here] off begin
		expect.assertions(1);
		if (typeof inputString !== 'string' || typeof isValid !== 'boolean') {
			throw "Invalid test arguments";
		}

		let testModel = new TestModel()
		testModel.maximumString = inputString

		await testModel.validate().then(x => {
			const errors = testModel.getErrorsForAttribute("maximumString");
			if (isValid) {
				expect(errors).toEqual([]);
			} else {
				expect(errors).toEqual([`The length of this field is not less than ${MaxTestNumber}. Actual Length: ${inputString.length}`]);
			}
		});
		// % protected region % [Customise  max validation test here] end
	});
	// % protected region % [Add any extra tests here] off begin
	// % protected region % [Add any extra tests here] end
});

// % protected region % [Add any extra content here] off begin
// % protected region % [Add any extra content here] end