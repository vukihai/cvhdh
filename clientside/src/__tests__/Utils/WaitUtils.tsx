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
import { pollingInterval, pollingTimeout } from '__tests__/TestConfig/TestConfig';
import { ReactWrapper } from 'enzyme';
// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

// % protected region % [Customise sleep here] off begin
export const sleep = (milliseconds: number) => {
	return new Promise(resolve => setTimeout(resolve, milliseconds))
};
// % protected region % [Customise sleep here] end

// % protected region % [Customise pollComponent here] off begin
export const pollComponent = async (component: ReactWrapper<any, any>, assertion: () => boolean) => {
	let pollCount = 0;
	while (pollCount < pollingTimeout/pollingInterval) {
		component.update();
		if (assertion()) {
			return;
		}
		await sleep(pollingInterval);
		pollCount++;
	}
	throw 'Assertion Failed';
};
// % protected region % [Customise pollComponent here] end

// % protected region % [Customise placeholder test here] off begin
// Add placeholder test so yarn test doesn't throw empty test file exception
describe('Place Holder', function () {
	it('placeholder', () => {
		expect(1).toEqual(1);
	})
});
// % protected region % [Customise placeholder test here] end

// % protected region % [Add extra methods here] off begin
// % protected region % [Add extra methods here] end