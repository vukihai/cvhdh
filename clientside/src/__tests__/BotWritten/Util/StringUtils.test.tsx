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
import {camelCase, camelCaseIntoWords, lowerCaseFirst, lowerCaseNoSpaces, noSpaces} from '../../../Util/StringUtils';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise setup here] off begin
const camelCasesTheoryData = [
	["star wars", "starWars"],
	["star Wars", "starWars"],
	["starWars", "starwars"],
	["StarWars", "starwars"],
	["STARWARS", "starwars"],
	["star", "star"],
	["star wars a new hope", "starWarsANewHope"],
];
// % protected region % [Customise setup here] end

// % protected region % [Customise camel casing string function test label here] off begin
describe("Test camel casing string function with different inputs", () => {
// % protected region % [Customise camel casing string function test label here] end
	// % protected region % [Customise camel casing string function test here] off begin
	test.each(camelCasesTheoryData)(
		"given %p input we expect to return %p", (inputString, expectedOutput) => {
			expect(camelCase(inputString)).toEqual(expectedOutput);
		}
	);
	// % protected region % [Customise camel casing string function test here] end
});

// % protected region % [Add any extra content here] off begin
// % protected region % [Add any extra content here] end