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
import { mount } from 'enzyme';
import React from "react";
import { Checkbox } from 'Views/Components/Checkbox/Checkbox';
import { CheckboxGroup } from 'Views/Components/Checkbox/CheckboxGroup';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise Checkbox Group Component test label here] off begin
describe('Checkbox Group Component', () => {
// % protected region % [Customise Checkbox Group Component test label here] end
	// % protected region % [Customise Checkbox Group Component test content label here] off begin
	it('a change event on each checkbox, only affects one of the checkboxes', () => {
	// % protected region % [Customise Checkbox Group Component test content label here] end
		// % protected region % [Customise model here] off begin
		let model = {
			"item1": false,
			"item2": false,
			"item3": false,
		}
		// % protected region % [Customise model here] end

		// % protected region % [Customise CheckboxGroup here] off begin
		let group = <CheckboxGroup
			label="Header for this checkbox group" >
			<Checkbox model={model}
				id="checkbox-1"
				modelProperty={"item1"}
				label={"General Checkbox"}
			>
			</Checkbox>
			<Checkbox model={model}
				id="checkbox-2"
				modelProperty={"item2"}
				label={"General Checkbox"}
			>
			</Checkbox>
			<Checkbox model={model}
				id="checkbox-3"
				modelProperty={"item3"}
				label={"General Checkbox"}
			>
			</Checkbox>
		</CheckboxGroup>;
		// % protected region % [Customise CheckboxGroup here] end

		// % protected region % [Customise mount here] off begin
		const component = mount(group);
		// % protected region % [Customise mount here] end

		// % protected region % [Customise assertions here] off begin
		component.find('#checkbox-1-field').simulate('change', { target: { checked: true } });
		expect(model.item1).toEqual(true);
		expect(model.item2).toEqual(false);
		expect(model.item3).toEqual(false);
		component.find('#checkbox-2-field').simulate('change', { target: { checked: true } });
		expect(model.item1).toEqual(true);
		expect(model.item2).toEqual(true);
		expect(model.item3).toEqual(false);
		component.find('#checkbox-3-field').simulate('change', { target: { checked: true } });
		expect(model.item1).toEqual(true);
		expect(model.item2).toEqual(true);
		expect(model.item3).toEqual(true);
		// % protected region % [Customise assertions here] end
	});
});

// % protected region % [Add any extra content here] off begin
// % protected region % [Add any extra content here] end