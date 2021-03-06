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
import {standardiseInteger, standardiseBoolean} from '../../../Util/AttributeUtils';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise standardIntTheoryData here] off begin
const standardIntTheoryData = [
	["52", {"query": "52"}],
	["52.5", null],
	["NotANumber", null],
	[undefined, null],
	["NaN", null],
];
// % protected region % [Customise standardIntTheoryData here] end

// % protected region % [Customise standardBoolTheoryData here] off begin
const standardBoolTheoryData = [
	["52", null],
	["52.5", null],
	["NotANumber", null],
	[undefined, null],
	["false", {"query": "false"}],
	["NaN", null],
];
// % protected region % [Customise standardBoolTheoryData here] end

// % protected region % [Customise standardise integer function test label here] off begin
describe("Test standardise integer function with different inputs", () => {
// % protected region % [Customise standardise integer function test label here] end
	// % protected region % [Customise standardise integer function test here] off begin
	test.each(standardIntTheoryData)(
		"given %p input we expect to return %p", (inputString, expectedOutput) => {
			if (typeof(inputString) === 'string'){
				expect(standardiseInteger(inputString)).toEqual(expectedOutput);
			} else {
				expect(standardiseInteger(inputString?.query)).toEqual(expectedOutput);
			}
		}
	);
	// % protected region % [Customise standardise integer function test here] end
});

// % protected region % [Customise standardise bool function with different test label here] off begin
describe("Test standardise bool function with different inputs", () => {
// % protected region % [Customise standardise bool function with different test label here] end
	// % protected region % [Customise standardise bool function with different test here] off begin
	test.each(standardBoolTheoryData)(
		"given %p input we expect to return %p", (inputString, expectedOutput) => {
			if (typeof(inputString) === 'string'){
				expect(standardiseBoolean(inputString)).toEqual(expectedOutput);
			} else {
				expect(standardiseInteger(inputString?.query)).toEqual(expectedOutput);
			}
		}
	);
	// % protected region % [Customise standardise bool function with different test here] end
});

// % protected region % [Add any extra content here] off begin
// % protected region % [Add any extra content here] end