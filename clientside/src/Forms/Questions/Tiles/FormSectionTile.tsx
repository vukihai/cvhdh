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
import React, { Component } from 'react';
import { observer } from 'mobx-react';
import { Contents, QuestionType } from 'Forms/Schema/Question';
import { IQuestionTile, QuestionTileOptionsProps } from '../QuestionTile';
import TileOptions from '../TileOptions';
import { QuestionComponent, QuestionComponentProps } from 'Forms/Questions/QuestionComponent';
import { FormDesignerContents } from '../../Designer/FormDesignerContents';
import { action, computed } from 'mobx';
import { SlideTile } from '../../SlideTile';
import { FormDesignerContext } from 'Forms/Designer/FormSlideBuilder';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

@observer
export class FormSectionTileOptions extends Component<QuestionTileOptionsProps> {
	// % protected region % [Add extra FormSectionTileOptions class properties here] off begin
	// % protected region % [Add extra FormSectionTileOptions class properties here] end

	public render() {
		// % protected region % [Customize FormSectionTileOptions render here] off begin
		const { question, schema } = this.props;

		return (
			<TileOptions question={question} schema={schema} hasShowConditions hasValidators hasTooltip />
		);
		// % protected region % [Customize FormSectionTileOptions render here] end
	}
}

export interface IFormSectionTileProps<T> extends QuestionComponentProps<T> {
	options: { contents: Contents };
	// % protected region % [Add extra props here] off begin
	// % protected region % [Add extra props here] end
}

@observer
export class FormSectionTile<T> extends QuestionComponent<T, IFormSectionTileProps<T>> implements IQuestionTile {
	// % protected region % [Customize static form vars here] off begin
	static displayName = 'Form Section';

	static questionType: QuestionType = 'formSection';

	static optionsMenu = FormSectionTileOptions;

	static validateFunction = undefined;

	static conditionOptions = undefined;

	static stylingOptions = undefined;

	static validatorOptions = undefined;
	// % protected region % [Customize static form vars here] end

	// % protected region % [Customize constructor here] off begin
	constructor(props: IFormSectionTileProps<T>) {
		super(props);
		this.initContents();
	}
	// % protected region % [Customize constructor here] end

	// % protected region % [Customize initContents here] off begin
	@action
	private initContents() {
		this.props.options.contents = this.props.options.contents ?? [];
	}
	// % protected region % [Customize initContents here] end

	// % protected region % [Customize schema here] off begin
	@computed
	private get schema() {
		return [{ name: this.props.title, contents: this.props.options.contents }];
	}
	// % protected region % [Customize schema here] end

	// % protected region % [Customize context declarations here] off begin
	context: React.ContextType<typeof FormDesignerContext>;
	static contextType = FormDesignerContext;
	// % protected region % [Customize context declarations here] end

	// % protected region % [Add extra class properties here] off begin
	// % protected region % [Add extra class properties here] end

	public render() {
		// % protected region % [Customize render here] off begin
		const { title, model, isReadOnly, options } = this.props;

		if (this.context.designMode) {
			return (
				<FormDesignerContents
					schema={this.schema}
					model={model}
					designerState={this.context.designerState}
				/>
			);
		}

		return (
			<SlideTile
				reValidate={this.props.reValidate}
				isReadOnly={isReadOnly}
				model={model}
				schema={this.schema}
				name={title}
				contents={options.contents}
				className="form__section"
			/>
		);
		// % protected region % [Customize render here] end
	}
}

// % protected region % [Add extra methods here] off begin
// % protected region % [Add extra methods here] end