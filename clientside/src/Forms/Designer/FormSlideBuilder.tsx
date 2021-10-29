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
import * as React from 'react'
import { action, observable } from 'mobx';
import { observer } from 'mobx-react';
import { FormVersion } from '../FormVersion';
import { Question, Slide } from '../Schema/Question';
import { FormDesignerContents } from './FormDesignerContents';
import { FormDesignerSidebar } from './FormDesignerSidebar';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

interface FormDesignerEnabled {
	designMode: true;
	designerState: FormDesignerState;
	// % protected region % [Add any extra FormDesignerEnabled fields here] off begin
	// % protected region % [Add any extra FormDesignerEnabled fields here] end
}

interface FormDesignerDisabled {
	designMode: false;
	// % protected region % [Add any extra FormDesignerEnabled fields here] off begin
	// % protected region % [Add any extra FormDesignerEnabled fields here] end
}

// % protected region % [Modify form designer context here] off begin
export type FormDesignerContextOptions = FormDesignerEnabled | FormDesignerDisabled;
export const FormDesignerContext = React.createContext<FormDesignerContextOptions>({ designMode: false });
// % protected region % [Modify form designer context here] end

export interface FormSlideBuilderProps {
	formVersion: FormVersion;
	className?: string;
	// % protected region % [Add any extra FormSlideBuilderProps fields here] off begin
	// % protected region % [Add any extra FormSlideBuilderProps fields here] end
}

export class FormDesignerState {
	// % protected region % [Modify form designer state fields here] off begin
	@observable
	public mode: 'view' | 'edit' | 'edit-slide' = 'view';

	@observable
	public selectedQuestion?: Question;

	@observable
	public selectedSlide?: Slide;
	// % protected region % [Modify form designer state fields here] end

	// % protected region % [Add any extra form designer state methods or fields here] off begin
	// % protected region % [Add any extra form designer state methods or fields here] end

	// % protected region % [Modify form designer state reset method here] off begin
	@action
	public reset = () => {
		this.mode = 'view';
		this.selectedQuestion = undefined;
		this.selectedSlide = undefined;
	}
	// % protected region % [Modify form designer state reset method here] end
}

@observer
export class FormSlideBuilder extends React.Component<FormSlideBuilderProps> {
	// % protected region % [Modify form slide builder fields here] off begin
	@observable
	private submissionData = {};

	@observable
	private designerState = new FormDesignerState();
	// % protected region % [Modify form slide builder fields here] end

	// % protected region % [Add any extra form form slide builder methods or fields here] off begin
	// % protected region % [Add any extra form form slide builder methods or fields here] end

	// % protected region % [Modify form slide builder fields here] off begin
	public render() {
		return (
			<FormDesignerContext.Provider value={{ designMode: true, designerState: this.designerState }}>
				<section aria-label="Form Slide Builder" className="slide-builder">
					<FormDesignerSidebar
						schema={this.props.formVersion.formData}
						designerState={this.designerState}
					/>
					<FormDesignerContents
						schema={this.props.formVersion.formData}
						model={this.submissionData}
						designerState={this.designerState}
					/>
				</section>
			</FormDesignerContext.Provider>
		);
	}
	// % protected region % [Modify form slide builder fields here] end
}