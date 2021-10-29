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
import { action } from 'mobx';
import { observer } from 'mobx-react';
import { Form, Question, Slide } from '../Schema/Question';
import { Button, Display } from 'Views/Components/Button/Button';
import { SlideTile } from '../SlideTile';
import { FormDesignerNewQuestion } from './FormDesignerNewQuestion';
import { ButtonGroup } from 'Views/Components/Button/ButtonGroup';
import { ContextMenu } from 'Views/Components/ContextMenu/ContextMenu';
import { contextMenu } from 'react-contexify';
import { confirmModal } from 'Views/Components/Modal/ModalUtils';
import { FormDesignerState } from 'Forms/Designer/FormSlideBuilder';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

@observer
export class FormDesignerContents extends React.Component<{schema: Form, model: {}, designerState: FormDesignerState}> {
	// % protected region % [Customize getQuestionOptions here] off begin
	private getQuestionOptions = (question: Question, slide: Slide, index: number) => (
		<ButtonGroup>
			<Button
				onClick={() => this.editQuestion(question, slide)}
				icon={{icon: 'edit', iconPos: 'icon-left'}}
				display={Display.Outline}>
				Options
			</Button>
			<Button
				icon={{icon: 'more-horizontal', iconPos: 'icon-left'}}
				display={Display.Outline}
				labelVisible={false}
				onClick={(event) => contextMenu.show({event: event, id: 'forms-question-more-' + question.id})}
				className="icon-only">
				More
			</Button>
			<ContextMenu
				menuId={'forms-question-more-' + question.id}
				actions={[
					{label: 'Move Question Up', onClick: () => this.moveQuestionUp(slide, index)},
					{label: 'Move Question Down', onClick: () => this.moveQuestionDown(slide, index)},
					{label: 'Delete', onClick: () => this.confirmDeleteQuestion(question, slide, index)},
				]} />
		</ButtonGroup>
	);
	// % protected region % [Customize getQuestionOptions here] end

	// % protected region % [Customize moveQuestionUp here] off begin
	@action
	private moveQuestionUp = (slide: Slide, index: number) => {
		if (index > 0) {
			this.props.designerState.reset();
			const question = slide.contents.splice(index, 1);
			slide.contents.splice(index - 1, 0, ...question);
		}
	}
	// % protected region % [Customize moveQuestionUp here] end

	// % protected region % [Customize moveQuestionDown here] off begin
	@action
	private moveQuestionDown = (slide: Slide, index: number) => {
		if (index < slide.contents.length - 1) {
			this.props.designerState.reset();
			const question = slide.contents.splice(index, 1);
			slide.contents.splice(index + 1, 0, ...question);
		}
	}
	// % protected region % [Customize moveQuestionDown here] end

	// % protected region % [Customize confirmDeleteQuestion here] off begin
	private confirmDeleteQuestion = (question: Question, slide: Slide, index: number) => {
		confirmModal('Confirm', 'Do you want to delete this question')
			.then(() => this.deleteQuestion(question, slide, index));
	}
	// % protected region % [Customize confirmDeleteQuestion here] end

	// % protected region % [Customize deleteQuestion here] off begin
	@action
	private deleteQuestion = (question: Question, slide: Slide, index: number) => {
		if (this.props.designerState.selectedQuestion === question) {
			this.props.designerState.reset();
		}
		slide.contents.splice(index, 1);
	}
	// % protected region % [Customize deleteQuestion here] end

	// % protected region % [Customize editQuestion here] off begin
	@action
	private editQuestion = (question: Question, slide: Slide) => {
		this.props.designerState.mode = 'edit';
		this.props.designerState.selectedQuestion = question;
		this.props.designerState.selectedSlide = slide;
	}
	// % protected region % [Customize editQuestion here] end

	// % protected region % [Customize getAfterSlideContent here] off begin
	private getAfterSlideContent = (slide: Slide, index: number) => () => (
		<>
			<FormDesignerNewQuestion
				schema={this.props.schema}
				slide={slide}
				designerState={this.props.designerState}/>
		</>
	);
	// % protected region % [Customize getAfterSlideContent here] end

	// % protected region % [Customize render here] off begin
	public render() {
		return (
			<section aria-label="form-builder" className="form-builder">
				{this.props.schema.map((s, i) => (
					<React.Fragment key={i}>
						<SlideTile
							model={this.props.model}
							schema={this.props.schema}
							name={s.name}
							contents={s.contents}
							disableShowConditions={true}
							isReadOnly={true}
							afterQuestionContent={this.getQuestionOptions}
							afterSlideContent={this.getAfterSlideContent(s, i)} />
					</React.Fragment>
				))}
			</section>
		)
	}
	// % protected region % [Customize render here] end

	// % protected region % [Add any further methods here] off begin
	// % protected region % [Add any further methods here] end
}