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
import FileUpload, { FileUploadProps } from 'Views/Components/FileUpload/FileUpload';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise test model interface here] off begin
interface FileUploadModel {
	file: File;
}
// % protected region % [Customise test model interface here] end

// % protected region % [Customise File Upload Component Tests label here] off begin
describe('File Upload Component Tests', () => {
// % protected region % [Customise File Upload Component Tests label here] end
	// % protected region % [Customise test properties here] off begin
	const defaultFile = new File([], 'nothing.txt');
	let model: FileUploadModel;
	// % protected region % [Customise test properties here] end
	// % protected region % [Customise beforeEach here] off begin
	beforeEach(() => {
		model = observable({
			file: defaultFile,
		});
	});
	// % protected region % [Customise beforeEach here] end

	// % protected region % [Customise test FileComponent here] off begin
	const FileComponent = (props: Partial<FileUploadProps<FileUploadModel>>) => (
		<FileUpload
			{...props}
			model={model}
			modelProperty="file"
		/>
	);
	// % protected region % [Customise test FileComponent here] end

	// % protected region % [Customise valid set test label here] off begin
	it('Should correctly set a file', () => {
	// % protected region % [Customise valid set test label here] end
		// % protected region % [Customise valid set test here] off begin
		const component = mount(<FileComponent/>);
		const testFile = new File(['0'.repeat(1000)], 'test.txt');

		component.find('.file-input__picker').simulate('change', { target: { files: [testFile] } })

		expect(model.file).toBe(testFile)
		// % protected region % [Customise valid set test here] end
	});

	// % protected region % [Customise valid set content type test label here] off begin
	it('Should accept a file with a valid content type', () => {
	// % protected region % [Customise valid set content type test label here] end
		// % protected region % [Customise valid set content type test here] off begin
		const component = mount(<FileComponent contentType="text/plain"/>);
		const testFile = new File(['0'.repeat(1000)], 'test.txt', { type: 'text/plain' });

		component.find('.file-input__picker').simulate('change', { target: { files: [testFile] } })

		expect(model.file).toBe(testFile)
		// % protected region % [Customise valid set content type test here] end
	});

	// % protected region % [Customise invalid set content type test label here] off begin
	it('Should reject a file with an invalid content type', () => {
	// % protected region % [Customise invalid set content type test label here] end
		// % protected region % [Customise invalid set content type test here] off begin
		const component = mount(<FileComponent contentType="image/*"/>);
		const testFile = new File(['0'.repeat(1000)], 'test.txt', { type: 'text/plain' });

		component.find('.file-input__picker').simulate('change', { target: { files: [testFile] } })

		const errorComponent = component.find('.input-group__error-text');
		expect(model.file).toBe(defaultFile);
		expect(errorComponent.contains('Content type text/plain is not valid for image/*')).toBe(true);
		// % protected region % [Customise invalid set content type test here] end
	});

	// % protected region % [Customise invalid file size test label here] off begin
	it('Should display the correct erorr on uploading a file that is too large', () => {
	// % protected region % [Customise invalid file size test label here] end
		// % protected region % [Customise invalid file size test here] off begin
		const component = mount(<FileComponent maxFileSize={100}/>);
		const testFile = new File(['0'.repeat(1000)], 'test.txt');

		component.find('.file-input__picker').simulate('change', { target: { files: [testFile] } })

		const errorComponent = component.find('.input-group__error-text');
		expect(model.file).toBe(defaultFile);
		expect(errorComponent.contains('Maximum allowed file size is 100 Bytes')).toBe(true);
		// % protected region % [Customise invalid file size test here] end
	});

	// % protected region % [Customise multiple error test label here] off begin
	it('Should display multiple errors', () => {
	// % protected region % [Customise multiple error test label here] end
		// % protected region % [Customise multiple error test here] off begin
		const component = mount(<FileComponent maxFileSize={100} contentType="image/*" />);
		const testFile = new File(['0'.repeat(1000)], 'test.txt', { type: 'text/plain' });

		component.find('.file-input__picker').simulate('change', { target: { files: [testFile] } })

		const errorComponent = component.find('.input-group__error-text');
		expect(model.file).toBe(defaultFile);
		expect(errorComponent.contains('Maximum allowed file size is 100 Bytes')).toBe(true);
		expect(errorComponent.contains('Content type text/plain is not valid for image/*')).toBe(true);
		// % protected region % [Customise multiple error test here] end
	});

	// % protected region % [Customise allowed file size test label here] off begin
	it('Should allow a file that is within the size limits', () => {
	// % protected region % [Customise allowed file size test label here] end
		// % protected region % [Customise allowed file size test here] off begin
		const component = mount(<FileComponent maxFileSize={100}/>);
		const testFile = new File(['0'.repeat(10)], 'test.txt');

		component.find('.file-input__picker').simulate('change', { target: { files: [testFile] } })

		expect(model.file).toBe(testFile)
		// % protected region % [Customise allowed file size test here] end
	});
	// % protected region % [Add any extra tests here] off begin
	// % protected region % [Add any extra tests here] end
});

// % protected region % [Add any extra content here] off begin
// % protected region % [Add any extra content here] end