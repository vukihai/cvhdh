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
import * as React from 'react';
import Axios from 'axios';
import FileUpload from 'Views/Components/FileUpload/FileUpload';
import { Model } from 'Models/Model';
import { IAttributeProps } from 'Views/Components/CRUD/Attributes/IAttributeProps';
import { action, observable } from 'mobx';
import { observer } from 'mobx-react';
import { SERVER_URL } from 'Constants';
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import { FileUploadPreview, UploadPreview } from 'Views/Components/FileUpload/UploadPreview';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export interface AttributeFileProps<T extends Model> extends IAttributeProps<T> {
	fileAttribute: string;
	imageOnly?: boolean;
	maxFileSize?: number;
	// % protected region % [Add any extra AttributeFileProps fields here] off begin
	// % protected region % [Add any extra AttributeFileProps fields here] end
}

interface FileMetadata {
	id: string;
	created: string;
	modified: string;
	fileName: string;
	contentType: string;
	length: number;
	// % protected region % [Add any extra FileMetadata fields here] off begin
	// % protected region % [Add any extra FileMetadata fields here] end
}

@observer
export default class AttributeFile<T extends Model> extends React.Component<AttributeFileProps<T>> {
	protected readonly initialFileId?: string;

	@observable
	protected fileMetadata?: FileMetadata;

	// % protected region % [Add any extra AttributeFile fields here] off begin
	// % protected region % [Add any extra AttributeFile fields here] end

	// % protected region % [Customise onFetchSucceeded here] off begin
	@action
	protected onFetchSucceeded = (metadata: FileMetadata) => {
		this.fileMetadata = metadata;
	};
	// % protected region % [Customise onFetchSucceeded here] end

	// % protected region % [Customise onAfterDelete here] off begin
	@action
	protected onAfterDelete = () => {
		this.props.model[this.props.options.attributeName] = undefined;
	};
	// % protected region % [Customise onAfterDelete here] end

	// % protected region % [Customise constructor here] off begin
	constructor(props: AttributeFileProps<T>) {
		super(props);

		if (this.props.model[this.props.options.attributeName]) {
			this.initialFileId = this.props.model[this.props.options.attributeName];
		}
	}
	// % protected region % [Customise constructor here] end

	// % protected region % [Customise loadFile here] off begin
	protected loadFile = () => {
		const fileId = this.props.model[this.props.options.attributeName];
		if (fileId) {
			Axios.get(`${SERVER_URL}/api/files/metadata/${fileId}`)
				.then(x => x.data)
				.then(metadata => this.onFetchSucceeded(metadata));
		}
	};
	// % protected region % [Customise loadFile here] end

	// % protected region % [Customise componentDidMount here] off begin
	public componentDidMount() {
		// For view or edit mode load the initial file from the server
		switch (this.props.formMode) {
			case EntityFormMode.VIEW:
			case EntityFormMode.EDIT:
				this.loadFile();
		}
	}
	// % protected region % [Customise componentDidMount here] end

	// % protected region % [Customise render here] off begin
	public render() {
		const {
			fileAttribute,
			isReadonly,
			imageOnly,
			model,
			isRequired,
			onAfterChange,
			className,
			errors,
			options,
			maxFileSize,
		} = this.props;

		return <FileUpload
			preview={(file, onDelete) => {
				if (!file && model[options.attributeName]) {
					return <UploadPreview
						download
						fileUrl={`${SERVER_URL}/api/files/${this.initialFileId}${imageOnly ? '' : '?download=true'}`}
						onDelete={onDelete}
						imagePreview={imageOnly}
						fileName={this.fileMetadata?.fileName}/>;
				}

				if (file) {
					return <FileUploadPreview
						fileBlob={file}
						onDelete={onDelete}
						imagePreview={imageOnly}
						fileName={file.name}/>;
				}

				return null;
			}}
			model={model}
			modelProperty={fileAttribute}
			imageUpload={imageOnly}
			label={options.displayName}
			errors={errors}
			className={className}
			isReadOnly={isReadonly}
			isRequired={isRequired}
			onAfterChange={onAfterChange}
			onAfterDelete={this.onAfterDelete}
			maxFileSize={maxFileSize}
		/>;
	}
	// % protected region % [Customise render here] end
}

@observer
export class FileListPreview extends React.Component<{ url: string }> {
	@observable
	private fileName?: string = undefined;

	// % protected region % [Customise FileListPreview setFileName here] off begin
	@action
	private setFileName = (metadata: FileMetadata) => {
		this.fileName = metadata.fileName;
	};
	// % protected region % [Customise FileListPreview setFileName here] end
	// % protected region % [Customise FileListPreview componentDidMount here] off begin
	public componentDidMount() {
		Axios.get(`${SERVER_URL}/api/files/metadata/${this.props.url}`)
			.then(x => x.data)
			.then(this.setFileName);
	}
	// % protected region % [Customise FileListPreview componentDidMount here] end
	// % protected region % [Customise FileListPreview render here] off begin
	public render() {
		return <a
			href={`${SERVER_URL}/api/files/${this.props.url}?download=true`}
			target="_blank"
			rel="noopener noreferrer"
			className="btn btn--icon icon-download icon-right">
			{this.fileName ?? 'Download'}
		</a>
	}
	// % protected region % [Customise FileListPreview render here] end
}