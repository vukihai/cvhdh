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
import React from 'react';
import { mount } from 'enzyme';
import { observable } from 'mobx';
import { Form, Question, Slide } from 'Forms/Schema/Question';
import { FormDesignerContents } from 'Forms/Designer/FormDesignerContents';
import { FormDesignerState } from 'Forms/Designer/FormSlideBuilder';
import { FormTile } from 'Forms/FormTile';
import { Checkbox } from 'Views/Components/Checkbox/Checkbox';
import { TextQuestionTile } from 'Forms/Questions/Tiles/TextQuestionTile';
import { SlideTile } from 'Forms/SlideTile';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise Form section tests label here] off begin
describe('Form section tests', () => {
// % protected region % [Customise Form section tests label here] end
	// % protected region % [Customise display formSection in form designer test label here] off begin
	it('Display formSection in form designer', () => {
	// % protected region % [Customise display formSection in form designer test label here] end
		// % protected region % [Customise display formSection in form designer test here] off begin
		const formSectionContents: Question = {
			questionType: 'formSection',
			id: 'cee8b0d4-403a-47f7-830d-7f4f4e4ae567',
			title: 'Inner Slide',
			options: { contents: [] },
		};
		const schema: Slide[] = [{ name: 'Outer Slide', contents: [formSectionContents] }];

		const model = observable({});
		const dom = mount(<FormDesignerContents
			designerState={new FormDesignerState()}
			model={model}
			schema={schema}
		/>);

		const slides = dom.find(SlideTile);
		const outerSlide = slides.findWhere(x => x.prop('name') === 'Outer Slide').first();
		const innerSlide = slides.findWhere(x => x.prop('name') === 'Inner Slide').first();

		expect(outerSlide.contains(innerSlide.getElement())).toBe(true);
		// % protected region % [Customise display formSection in form designer test here] end
	});

	// % protected region % [Customise nested show condition test label here] off begin
	it('Should respect nested show conditions', () => {
	// % protected region % [Customise nested show condition test label here] end
		// % protected region % [Customise nested show condition test here] off begin
		const checkboxId = '9b1cb863-3867-4825-b699-04baa250a361';
		const formSectionId = 'cee8b0d4-403a-47f7-830d-7f4f4e4ae567';
		const textFieldId = 'dda4c1be-6400-471d-820e-573bc0959b90'

		const schema: Form = [
			{
				name: 'Outer Slide',
				contents: [
					{
						questionType: 'formSection',
						id: formSectionId,
						title: 'Inner Slide',
						options: {
							contents: [
								{
									questionType: 'checkbox',
									id: checkboxId,
									title: 'Condition Box',
								},
							],
						},
					},
					{
						questionType: 'text',
						id: textFieldId,
						title: 'Hidden By default',
						showConditions: [
							{
								path: checkboxId,
								condition: 'equal',
								value: 'true',
							},
						],
					},
				],
			},
		];

		const model = observable({
			[checkboxId]: false,
		});

		const wrapper = mount(<FormTile model={model} schema={schema} />);

		expect(wrapper.find(TextQuestionTile)).toHaveLength(0);
		wrapper.find(Checkbox).find('input').simulate('change', {target: {checked: true}});
		expect(wrapper.find(TextQuestionTile)).toHaveLength(1);
		// % protected region % [Customise nested show condition test here] end
	});
	// % protected region % [Add any extra tests here] off begin
	// % protected region % [Add any extra tests here] end
});

// % protected region % [Add any extra content here] off begin
// % protected region % [Add any extra content here] end