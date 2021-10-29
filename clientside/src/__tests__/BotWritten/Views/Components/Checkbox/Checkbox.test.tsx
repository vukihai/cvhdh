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
import React from "react";
import { Checkbox } from 'Views/Components/Checkbox/Checkbox'
import { mount } from 'enzyme';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise Checkbox Component should append -field test label here] off begin
describe('Checkbox Component', () => {
// % protected region % [Customise Checkbox Component should append -field test label here] end
	// % protected region % [Customise Checkbox Component should append -field test content label here] off begin
	it('should append -field to the id of the checkbox input', () => {
	// % protected region % [Customise Checkbox Component should append -field test content label here] end
		// % protected region % [Customise Checkbox Component should append -field test label here] off begin
		let model = {
			"ticked": true
		}

		const props = {
			model:model,
			modelProperty:"ticked",
			id: "testCheckbox",
			onAfterChecked: () => model.ticked = !model.ticked
		}
		const component = mount(<Checkbox {...props}/>);
		expect(component.find('#testCheckbox-field')).toHaveLength(1);
		// % protected region % [Customise Checkbox Component should append -field test label here] end
	});
});

// % protected region % [Customise Checkbox Component triggers on checked test label here] off begin
describe('Checkbox Component', () => {
// % protected region % [Customise Checkbox Component triggers on checked test label here] end
	// % protected region % [Customise Checkbox Component triggers on checked test content label here] off begin
	it('a change event on the checkbox triggers on checked function', () => {
	// % protected region % [Customise Checkbox Component triggers on checked test content label here] end
		// % protected region % [Customise Checkbox Component triggers on checked test content here] off begin
		let model = {
			"ticked": true,
			"anotherProperty": "no"
		}

		const props = {
			model:model,
			modelProperty:"ticked",
			id: "testCheckbox",
			onAfterChecked: () => model.anotherProperty = "yes"
		}
		const component = mount(<Checkbox {...props}/>);
		component.find('#testCheckbox-field').simulate('change');
		expect(model.anotherProperty).toEqual("yes");
		// % protected region % [Customise Checkbox Component triggers on checked test content here] end
	});
});

// % protected region % [Customise Checkbox Component ticked should be false test label here] off begin
describe('Checkbox Component', () => {
// % protected region % [Customise Checkbox Component ticked should be false test label here] end
	// % protected region % [Customise Checkbox Component ticked should be false test content label here] off begin
	it('the model property is ommitted, ticked should be false', () => {
	// % protected region % [Customise Checkbox Component ticked should be false test content label here] end
		// % protected region % [Customise Checkbox Component ticked should be false test content here] off begin
		let model = {
			"ticked": true
		}

		const props = {
			model:model,
			modelProperty:"",
			id: "testCheckbox",
		}
		const component = mount(<Checkbox {...props}/>);
		expect(component.find('#testCheckbox-field').prop('checked')).toEqual(false);
		// % protected region % [Customise Checkbox Component ticked should be false test content here] end
	});
});

// % protected region % [Customise Checkbox Component StandardBooleanTheoryData here] off begin
const StandardBooleanTheoryData = [
	[true],
	[false],
];
// % protected region % [Customise Checkbox Component StandardBooleanTheoryData here] end

// % protected region % [Customise Checkbox Component we expect checked property test label here] off begin
describe('Checkbox Component', () => {
// % protected region % [Customise Checkbox Component we expect checked property test label here] end
	// % protected region % [Customise Checkbox Component we expect checked property test content label here] off begin
	test.each(StandardBooleanTheoryData)('when the model property is %p, we expect checked property to be %p', (expectedOutput) => {
	// % protected region % [Customise Checkbox Component we expect checked property test content label here] end
		// % protected region % [Customise Checkbox Component we expect checked property test content here] off begin
		let model = {
			"ticked": expectedOutput
		}

		const props = {
			model:model,
			modelProperty:"ticked",
			id: "testCheckbox",
		}
		const component = mount(<Checkbox {...props}/>);
		expect(component.find('#testCheckbox-field').prop('checked')).toEqual(expectedOutput);
		// % protected region % [Customise Checkbox Component we expect checked property test content here] end
	});
});

// % protected region % [Customise Checkbox Component we expect ticked to be test label here] off begin
describe('Checkbox Component', () => {
// % protected region % [Customise Checkbox Component we expect ticked to be test label here] end
	// % protected region % [Customise Checkbox Component we expect ticked to be test content label here] off begin
	test.each(StandardBooleanTheoryData)('when the checkbox is %p from a change, we expect ticked to be %p', (expectedOutput) => {
	// % protected region % [Customise Checkbox Component we expect ticked to be test content label here] end
		// % protected region % [Customise Checkbox Component we expect ticked to be test content here] off begin
		let model = {
			"ticked": true
		}

		const props = {
			model:model,
			modelProperty:"ticked",
			id: "testCheckbox",
			isDisabled: false
		}
		const component = mount(<Checkbox {...props}/>);

		component.find('#testCheckbox-field').simulate('change', {target: {checked: expectedOutput}});
		expect(model.ticked).toEqual(expectedOutput);
		// % protected region % [Customise Checkbox Component we expect ticked to be test content here] end
	});
});

// % protected region % [Add any extra content here] off begin
// % protected region % [Add any extra content here] end