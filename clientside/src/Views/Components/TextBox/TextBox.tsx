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
import * as React from "react";
import { observer } from 'mobx-react';
import { action } from 'mobx';
import If from '../If/If';
import InputWrapper from '../Inputs/InputWrapper';
import * as uuid from 'uuid';
import { DisplayType } from '../Models/Enums';
import InputsHelper from '../Helpers/InputsHelper';

export interface ITextFieldProps<T> {
	model: T;
	modelProperty: string;
	id?: string;
	name?:string;
	className?: string;
	inputClassName?: string;
	defaultToUndefined?: boolean;
	displayType?: DisplayType;
	label?: string;
	labelVisible?: boolean;
	isRequired?: boolean;
	isDisabled?: boolean;
	isReadOnly?: boolean;
	staticInput?: boolean;
	tooltip?: string;
	subDescription?: string;
	inputProps?: React.InputHTMLAttributes<Element>;
	placeholder?: string;
	clickToClear?: boolean;
	autoFocus?: boolean;
	errors?: string | string[];
	onAfterChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
	onChangeAndBlur?: (event: React.ChangeEvent<HTMLInputElement>) => void;
	onClickToClear?: (event: React.MouseEvent<HTMLButtonElement>) => void;
	// % protected region % [Add extra ITextFieldProps here] off begin
	// % protected region % [Add extra ITextFieldProps here] end
}

@observer
export class TextField<T> extends React.Component<ITextFieldProps<T>, any> {
	public static defaultProps = {
		clickToClear: false,
		inputProps: {},
		className: '',
	};
	private  _input?: HTMLInputElement;
	private uuid = uuid.v4();
	private valueWhenFocused: string = '';

	componentDidMount() {
		if(this.props.autoFocus && this._input){
			this._input.focus();
		}
	}

	public render() {
		const {
			model,
			modelProperty,
			name,
			className,
			displayType,
			label,
			isRequired,
			isDisabled,
			isReadOnly,
			staticInput,
			tooltip,
			subDescription,
			clickToClear,
			placeholder,
			errors,
			inputClassName,
			// % protected region % [Add extra props here] off begin
			// % protected region % [Add extra props here] end
		} = this.props;
		const id = this.props.id || this.uuid.toString();
		const fieldId = `${id}-field`;

		const labelVisible = (this.props.labelVisible === undefined) ? true : this.props.labelVisible;
		const ariaLabel = !labelVisible ? label : undefined;
		const ariaDescribedby = InputsHelper.getAriaDescribedBy(id, tooltip, subDescription);
		// % protected region % [Override render return here] off begin
		return (
			<InputWrapper
				id={id}
				inputId={fieldId}
				className={className}
				displayType={displayType}
				isRequired={isRequired}
				staticInput={staticInput}
				tooltip={tooltip}
				subDescription={subDescription}
				label={label}
				labelVisible={labelVisible}
				errors={errors}>
				<input
					className={inputClassName}
					type="text"
					name={name}
					id={fieldId}
					value={model[modelProperty] || model[modelProperty] === 0 ? model[modelProperty] : ''}
					onChange={this.onChange}
					onBlur={this.onBlur}
					onFocus={this.onFocus}
					placeholder={placeholder ? placeholder : (label ? label : undefined)}
					disabled={isDisabled}
					readOnly={staticInput || isReadOnly}
					aria-label={ariaLabel}
					aria-describedby = {ariaDescribedby}
					ref={i => (this._input = i || undefined)}
					{...this.props.inputProps}
				/>
				<If condition={clickToClear}>
					<button className="click-to-clear icon-cross icon-right" onClick={this.onClickToClear} type="button" />
				</If>
			</InputWrapper>
		);
		// % protected region % [Override render return here] end
	}

	@action
	private onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		let value: string | undefined = event.target.value;
		value = (value === '' && this.props.defaultToUndefined) ? undefined : value;
		this.props.model[this.props.modelProperty] = value;
		if (this.props.onAfterChange) {
			this.props.onAfterChange(event);
		}
	}

	@action
	private onFocus = (event: React.ChangeEvent<HTMLInputElement>) => {
		this.valueWhenFocused = event.target.value;
	}

	@action
	private onBlur = (event: React.ChangeEvent<HTMLInputElement>) => {
		if(this.valueWhenFocused !== event.target.value && this.props.onChangeAndBlur){
			this.props.onChangeAndBlur(event);
		}
	}

	@action
	private onClickToClear = (event: React.MouseEvent<HTMLButtonElement>) => {
		if (this.props.onClickToClear) {
			return this.props.onClickToClear(event);
		}
		this.props.model[this.props.modelProperty] = '';
	}
}