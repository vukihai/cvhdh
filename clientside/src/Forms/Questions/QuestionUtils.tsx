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
import React, { ReactNode } from 'react';
import _ from 'lodash';
import { Contents, Question, QuestionType } from '../Schema/Question';
import { CheckboxQuestionTile } from './Tiles/CheckboxQuestionTile';
import { FormStatementTile } from './Tiles/FormStatementTile';
import { IQuestionTileProps, QuestionTile } from './QuestionTile';
import { TextQuestionTile } from './Tiles/TextQuestionTile';
import { NumberQuestionTile } from './Tiles/NumberQuestionTile';
import { DateTimeQuestionTile } from './Tiles/DateTimeQuestionTile';
import { RadioQuestionTile } from './Tiles/RadioQuestionTile';
import { ListStatementTile } from './Tiles/ListStatementTile';
import { DateQuestionTile } from './Tiles/DateQuestionTile';
import { QuestionComponent } from 'Forms/Questions/QuestionComponent';
import { FormSectionTile, IFormSectionTileProps } from './Tiles/FormSectionTile';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

export const questions: Array<typeof QuestionComponent> = [
	TextQuestionTile,
	NumberQuestionTile,
	CheckboxQuestionTile,
	FormStatementTile,
	DateTimeQuestionTile,
	RadioQuestionTile,
	ListStatementTile,
	DateQuestionTile,
	FormSectionTile,
	// % protected region % [Add any extra question types here] off begin
	// % protected region % [Add any extra question types here] end
];

export function getNestedQuestions(contents: Contents): Question[] {
	// % protected region % [Customize getQuestionType here] off begin
	const [ formSections, questions ] = _.partition(contents, x => x.questionType === 'formSection');
	return [
		...questions, 
		..._.flatMap(formSections, x => getNestedQuestions((x as IFormSectionTileProps<unknown>).options.contents))
	]
	// % protected region % [Customize getQuestionType here] end
}

export function getQuestionType(questionType: QuestionType) {
	// % protected region % [Customize getQuestionType here] off begin
	return questions.find(q => q.questionType === questionType);
	// % protected region % [Customize getQuestionType here] end
}

// % protected region % [Customize GetQuestion logic here] off begin
export function getQuestion<T, P extends IQuestionTileProps<T>>(questionProps: P): ReactNode {
	const SelectedQuestion = getQuestionType(questionProps.questionType);

	if (SelectedQuestion !== undefined) {
		return <QuestionTile {...questionProps} selectedQuestion={SelectedQuestion} />;
	}

	return <></>;
}
// % protected region % [Customize GetQuestion logic here] end

// % protected region % [Add any sub question logic here] off begin
// % protected region % [Add any sub question logic here] end
