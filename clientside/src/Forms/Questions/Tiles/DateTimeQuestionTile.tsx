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
import { computed } from 'mobx';
import { observer } from 'mobx-react';
import { QuestionType, ValidatorType } from '../../Schema/Question';
import { IQuestionTile, QuestionTileOptionsProps } from '../QuestionTile';
import { DateTimePicker } from 'Views/Components/DateTimePicker/DateTimePicker';
import TileOptions from '../TileOptions';
import { hasRequiredValidator } from '../../Validators/ValidationUtils';
import CompareDateTime from 'Forms/Conditions/CompareDateTime';
import { ValidateDateTime } from 'Forms/Validators/ValidateDateTime';
import { QuestionComponent, QuestionComponentProps } from 'Forms/Questions/QuestionComponent';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

@observer
export class DateTimeQuestionTileOptions extends Component<QuestionTileOptionsProps> {
	// % protected region % [Add extra options class properties here] off begin
	// % protected region % [Add extra options class properties here] end

	public render() {
		// % protected region % [Customize options render here] off begin
		const { question, schema } = this.props;

		return (
			<TileOptions question={question} schema={schema} hasShowConditions hasValidators hasTooltip />
		);
		// % protected region % [Customize options render here] end
	}
}

export interface IDateTimeQuestionTileProps<T> extends QuestionComponentProps<T> {
	// % protected region % [Add extra props here] off begin
	// % protected region % [Add extra props here] end
}

@observer
export class DateTimeQuestionTile<T> extends QuestionComponent<T, IDateTimeQuestionTileProps<T>> implements IQuestionTile {
	// % protected region % [Customize static form vars here] off begin
	static displayName = 'Date Time';

	static questionType: QuestionType = 'datetime';

	static optionsMenu = DateTimeQuestionTileOptions;

	static compareFunction = CompareDateTime;

	static validateFunction = ValidateDateTime;

	static conditionOptions = [
		{ display: 'Equal', value: 'equal' },
		{ display: 'Greater than', value: 'greaterThan' },
		{ display: 'Less than', value: 'lessThan' },
		{ display: 'Not equal', value: 'notEqual' },
		{ display: 'Greater than or equal', value: 'greaterThanOrEqual' },
		{ display: 'Less than or equal', value: 'lessThanOrEqual' },
	];

	static validatorOptions: { display: string, value: ValidatorType }[] = [
		{ display: 'Required', value: 'required' },
		{ display: 'Past', value: 'past' },
		{ display: 'Future', value: 'future' },
		{ display: 'After', value: 'after' },
		{ display: 'Before', value: 'before' },
		{ display: 'Custom', value: 'custom' },
	];

	static stylingOptions = undefined;

	@computed
	get internalDate() {
		if (this.props.model[this.props.id] === null || this.props.model[this.props.id] === undefined) {
			return null;
		}
		return new Date(this.props.model[this.props.id]);
	}
	set internalDate(value) {
		if (value === null || value === undefined) {
			this.props.model[this.props.id] = null;
		} else {
			this.props.model[this.props.id] = value.toISOString();
		}
	}
	// % protected region % [Customize static form vars here] end

	// % protected region % [Add extra class properties here] off begin
	// % protected region % [Add extra class properties here] end

	public render() {
		// % protected region % [Customize render here] off begin
		const { title, isReadOnly, validators, toolTip, className, checkValidation} = this.props;

		return (
			<DateTimePicker
				model={this}
				modelProperty="internalDate"
				label={title}
				labelVisible
				isReadOnly={isReadOnly}
				tooltip={toolTip}
				isRequired={hasRequiredValidator(validators)}
				onAfterChange={checkValidation}
				className={className}
			/>
		);
		// % protected region % [Customize render here] end
	}
}

// % protected region % [Add extra methods here] off begin
// % protected region % [Add extra methods here] end
