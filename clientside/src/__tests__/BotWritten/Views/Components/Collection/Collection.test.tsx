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
import Collection from 'Views/Components/Collection/Collection'
import { mount } from 'enzyme';
import React from 'react';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise TestCollectionRow here] off begin
export type TestCollectionRow = { id: string, title: string, description: string };
// % protected region % [Customise TestCollectionRow here] end

// % protected region % [Customise TestCollectionProps here] off begin
interface TestCollectionProps{
	data: TestCollectionRow[];
	perPage: number;
}
// % protected region % [Customise TestCollectionProps here] end

// % protected region % [Customise TestCollection here] off begin
function TestCollection(props: TestCollectionProps) {
	return (
		<Collection
			perPage={props.perPage}
			collection={props.data}
			headers={[
				{ displayName: 'id', name: 'id' },
				{ displayName: 'title', name: 'title' },
				{ displayName: 'description', name: 'description' },
			]}
		/>
	);
}
// % protected region % [Customise TestCollection here] end

// % protected region % [Customise data here] off begin
const data = [
	{id: "1", title: "Hello 1", description: "world 1"},
	{id: "2", title: "Hello 2", description: "world 2"},
	{id: "3", title: "Hello 3", description: "world 3"},
	{id: "4", title: "Hello 4", description: "world 4"},
	{id: "5", title: "Hello 5", description: "world 5"},
	{id: "6", title: "Hello 6", description: "world 6"},
	{id: "7", title: "Hello 7", description: "world 7"},
	{id: "8", title: "Hello 8", description: "world 8"},
	{id: "9", title: "Hello 9", description: "world 9"},
	{id: "10", title: "Hello 10", description: "world 10"},
	{id: "11", title: "Hello 11", description: "world 11"},
	{id: "12", title: "Hello 12", description: "world 12"},
	{id: "13", title: "Hello 13", description: "world 13"},
	{id: "14", title: "Hello 14", description: "world 14"},
	{id: "15", title: "Hello 15", description: "world 15"},
	{id: "16", title: "Hello 16", description: "world 16"},
];
// % protected region % [Customise data here] end

// % protected region % [Customise Collection Component test label here] off begin
describe('Collection Component', () => {
// % protected region % [Customise Collection Component test label here] end
	// % protected region % [Customise test data here] off begin
	let totalPages: number;
	let props: TestCollectionProps;
	const perPage = 3;
	// % protected region % [Customise test data here] end

	// % protected region % [Customise beforeEach here] off begin
	beforeEach(() => {
		props = {
			data,
			perPage,
		};
		totalPages = Math.ceil(data.length / perPage);
	})
	// % protected region % [Customise beforeEach here] end

	// % protected region % [Customise correct items displayed test label here] off begin
	it(`Should have the correct items displayed on the first page`, () => {
	// % protected region % [Customise correct items displayed test label here] end
		// % protected region % [Customise correct items displayed test here] off begin
		const component = mount(<TestCollection {...props}/>);
		expect(component.find(".collection__item")).toHaveLength(perPage);
		// % protected region % [Customise correct items displayed test here] end
	});
	// % protected region % [Customise correct number of items displayed test label here] off begin
	it('Should have the correct number of items displayed on the second page', () => {
	// % protected region % [Customise correct number of items displayed test label here] end
		// % protected region % [Customise correct number of items displayed test here] off begin
		const component = mount(<TestCollection {...props}/>);
		expect(component.find('span.pagination__page-number').text()).toEqual( `1 of ${totalPages}`);
		component.find('button.icon-chevron-right').simulate('click');
		expect(component.find('span.pagination__page-number').text()).toEqual( `2 of ${totalPages}`);
		expect(component.find(".collection__item")).toHaveLength(perPage);
		// % protected region % [Customise correct number of items displayed test here] end
	});
	// % protected region % [Customise end on the last page test label here] off begin
	it('Should end on the last page', () => {
	// % protected region % [Customise end on the last page test label here] end
		// % protected region % [Customise end on the last page test here] off begin
		const component = mount(<TestCollection {...props}/>);

		// go to the last page
		for (let index = 0; index < totalPages; index++) {
			component.find('button.icon-chevron-right').simulate('click');
		}

		expect(component.find('span.pagination__page-number').text()).toEqual( `${totalPages} of ${totalPages}`);
		expect(component.find(".collection__item")).toHaveLength(data.length % perPage);
		// % protected region % [Customise end on the last page test here] end
	});
	// % protected region % [Customise not be able to go before the first page test label here] off begin
	it('Should not be able to go before the first page', () => {
	// % protected region % [Customise not be able to go before the first page test label here] end
		// % protected region % [Customise not be able to go before the first page test here] off begin
		const component = mount(<TestCollection {...props}/>);
		// start on page 1 and click three times
		component.find('button.icon-chevron-left').simulate('click');
		component.find('button.icon-chevron-left').simulate('click');
		component.find('button.icon-chevron-left').simulate('click');

		expect(component.find('span.pagination__page-number').text()).toEqual( `1 of ${totalPages}`);
		expect(component.find(".collection__item")).toHaveLength(perPage);
		// % protected region % [Customise not be able to go before the first page test here] end
	});
	// % protected region % [Customise be able to go back pages test label here] off begin
	it('Should be able to go back pages', () => {
	// % protected region % [Customise be able to go back pages test label here] end
		// % protected region % [Customise be able to go back pages test here] off begin
		const component = mount(<TestCollection {...props}/>);
		// start on page 1
		component.find('button.icon-chevron-right').simulate('click');
		component.find('button.icon-chevron-left').simulate('click');
		expect(component.find('span.pagination__page-number').text()).toEqual(`1 of ${totalPages}`);

		// should still be on on the last page
		expect(component.find(".collection__item")).toHaveLength(perPage);
		// % protected region % [Customise be able to go back pages test here] end
	});
	// % protected region % [Customise be able to jump to the last page test label here] off begin
	it('Should be able to jump to the last page', () => {
	// % protected region % [Customise be able to jump to the last page test label here] end
		// % protected region % [Customise be able to jump to the last page test here] off begin
		const component = mount(<TestCollection {...props}/>);
		component.find('button.icon-chevrons-right').simulate('click');
		expect(component.find('span.pagination__page-number').text()).toEqual(`${totalPages} of ${totalPages}`);
		// % protected region % [Customise be able to jump to the last page test here] end
	});
	// % protected region % [Customise be able to jump to the first page test label here] off begin
	it('Should be able to jump to the first page', () => {
	// % protected region % [Customise be able to jump to the first page test label here] end
		// % protected region % [Customise be able to jump to the first page test here] off begin
		const component = mount(<TestCollection {...props}/>);
		component.find('button.icon-chevrons-right').simulate('click');
		component.find('button.icon-chevrons-left').simulate('click');
		expect(component.find('span.pagination__page-number').text()).toEqual(`1 of ${totalPages}`);
		// % protected region % [Customise be able to jump to the first page test here] end
	});

	// % protected region % [Add any extra tests here] off begin
	// % protected region % [Add any extra tests here] end
});

// % protected region % [Add any extra content here] off begin
// % protected region % [Add any extra content here] end