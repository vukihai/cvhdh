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
import { QuestionType } from 'Forms/Schema/Question';
import { IQuestionTile, QuestionTileOptionsProps } from '../QuestionTile';
import TileOptions from '../TileOptions';
import { QuestionComponent, QuestionComponentProps } from 'Forms/Questions/QuestionComponent';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

@observer
export class ListStatementTileOptions extends Component<QuestionTileOptionsProps> {
	// % protected region % [Add extra options class properties here] off begin
	// % protected region % [Add extra options class properties here] end

	public render() {
		// % protected region % [Customize options render here] off begin
		const { question, schema } = this.props;

		return (
			<TileOptions question={question} schema={schema} hasShowConditions hasOptions/>
		);
		// % protected region % [Customize options render here] end
	}
}

export interface IListStatementTileProps<T> extends QuestionComponentProps<T> {
	options: {values: {value: string}[]};
	// % protected region % [Add extra props here] off begin
	// % protected region % [Add extra props here] end
}

@observer
export class ListStatementTile<T> extends QuestionComponent<T, IListStatementTileProps<T>> implements IQuestionTile {
	// % protected region % [Customize static form vars here] off begin
	static displayName = 'List';

	static questionType: QuestionType = 'list';

	static compareFunction = undefined;

	static validateFunction = undefined;

	static optionsMenu = ListStatementTileOptions;

	static conditionOptions = undefined;

	static validatorOptions = undefined;

	static stylingOptions = undefined;
	// % protected region % [Customize static form vars here] end

	// % protected region % [Add extra class properties here] off begin
	// % protected region % [Add extra class properties here] end

	public render() {
		// % protected region % [Customize render here] off begin
		const { title, options, className } = this.props;

		return (
			<>
				<p>{title}</p>
				<div className={className}>
					<ul className="form-list">
						{options && options.values && options.values.map((o, i) => {
							return <li className="form-list-item" key={i}>{o.value}</li>;
						})}
					</ul>
				</div>
			</>
		);
		// % protected region % [Customize render here] end
	}
}

// % protected region % [Add extra methods here] off begin
// % protected region % [Add extra methods here] end
