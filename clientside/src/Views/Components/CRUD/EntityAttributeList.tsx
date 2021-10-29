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
import * as queryString from "querystring";
import _ from 'lodash';
import { isApolloError } from '@apollo/client';
import Spinner from 'Views/Components/Spinner/Spinner';
import { Model } from 'Models/Model';
import { RouteComponentProps } from 'react-router';
import { observer } from 'mobx-react';
import { Button, Display } from '../Button/Button';
import { store } from 'Models/Store';
import FormErrors from './FormErrors';
import { observable, action, computed } from 'mobx';
import { IEntityValidationErrors } from 'Validators/Util';
import alert from 'Util/ToastifyUtils';
import { SecurityService } from 'Services/SecurityService';
import { IAcl } from 'Models/Security/IAcl';
import { Form } from '../Form/Form';
import {AttributeFormMode, EntityFormMode} from '../Helpers/Common';
import { EntityFormLayout } from '../EntityFormLayout/EntityFormLayout';
import { AttributeCRUDOptions } from 'Models/CRUDOptions';
// % protected region % [Add extra page imports here] off begin
// % protected region % [Add extra page imports here] end

const VALIDATION_ERROR = "Some of the fields are missing or invalid, please check your form.";

export interface IEntityAttributeBehaviour {
	name: string;
	behaviour: AttributeFormMode
}

interface IEntityCreateProps<T extends Model> extends RouteComponentProps {
	model: T;
	modelType: { acls?: IAcl[], new(...args: any[]): T };
	formMode: EntityFormMode;
	title: string;
	sectionClassName: string;
	customFields?: React.ReactNode;
	customRelationPath?: any;
	attributeBehaviours?: Array<IEntityAttributeBehaviour>;
	/** Function to mutate the attribute options before it is rendered */
	mutateOptions?: (model: Model | Model[], options: AttributeCRUDOptions[], formMode: EntityFormMode) => AttributeCRUDOptions[];
	/** Function to call when saving the entity. If this is not provided then model.saveFromCrud is called instead. */
	saveFn?: (entity: T, formMode: EntityFormMode) => Promise<void>;
}

// % protected region % [Add extra implementation here] off begin
// % protected region % [Add extra implementation here] end

@observer
class EntityAttributeList<T extends Model> extends React.Component<IEntityCreateProps<T>> {
	@observable
	private _generalFormError: React.ReactNode;
	private validationPath: {} = {};
	private hasSubmittedOnce: boolean;

	@observable
	private showSpinner = false;

	@computed
	private get generalFormError(): React.ReactNode{
		if(this.props.model.hasValidationError){
			return VALIDATION_ERROR;
		}
		return this._generalFormError;
	};

	private set generalFormError(value: React.ReactNode) {
		this._generalFormError = value;
	}

	@observable
	private detailedFormError: React.ReactNode;

	@observable
	private fieldErrors?: IEntityValidationErrors = {};

	public render() {
		// % protected region % [Modify Entity attribute render] off begin
		const { title, modelType, formMode, customFields, sectionClassName } = this.props;
		return (
			<div className="crud-component">
				{this.showSpinner && <Spinner />}
				<section className={sectionClassName}>
					<FormErrors error={this.generalFormError} detailedErrors={this.detailedFormError} />
					{SecurityService.canUpdate(modelType) && formMode === EntityFormMode.VIEW
						? (
							<div className="crud__header">
								<h2>{title}</h2>
								<Button className="edit" display={Display.Outline} onClick={this.onEdit}>Edit</Button>
							</div>
						)
					: <h2>{title}</h2>}
					<Form
						submitButton={(formMode === EntityFormMode.CREATE && SecurityService.canCreate(modelType)) 
										|| (formMode === EntityFormMode.EDIT && SecurityService.canUpdate(modelType))}
						cancelButton={true}
						onSubmit={this.onSubmit}
						onCancel={this.onCancel}
					>

						{this.renderEntityFormLayout()}
						{customFields}

					</Form>
				</section>
			</div>
		);
		// % protected region % [Modify Entity attribute render] end
	}

	// % protected region % [Customize EntityFormLayout here] off begin
	protected renderEntityFormLayout = (): React.ReactNode => {
		const { model, formMode, attributeBehaviours, mutateOptions } = this.props;
		return (
			<EntityFormLayout
				model={model}
				getErrorsForAttribute={model.getErrorsForAttribute}
				formMode={formMode}
				onAttributeAfterChange={this.onAttributeAfterChange}
				onAttributeChangeAndBlur={this.onAttributeChangeAndBlur}
				attributeBehaviours={attributeBehaviours}
				mutateOptions={mutateOptions}
			/>
		);
	};
	// % protected region % [Customize EntityFormLayout here] end

	protected onSubmit = (event: React.FormEvent<HTMLFormElement>) => {
		event.preventDefault();

		const { model } = this.props;
		const modelName = model.getModelDisplayName();
		model.clearErrors();

		model.validate(this.props.customRelationPath).then(action(() => {
			if (model.hasValidationError) {
				this.hasSubmittedOnce = true;
				return;
			}

			const saveResult = new Promise<void>((resolve, reject) => {
				if (this.props.saveFn) {
					this.props.saveFn(model, this.props.formMode)
						.then(resolve)
						.catch(reject);
				} else {
					model.saveFromCrud(this.props.formMode)
						.then(resolve)
						.catch((error: Error) => {
							if (isApolloError(error)) {
								if (error.graphQLErrors.length > 0) {
									reject(error.graphQLErrors.map((x: {message: string}) => x.message));
								} else if (
									error.networkError !== null
									&& 'result' in error.networkError
									&& error.networkError.result.errors
									&& error.networkError.result.errors.length > 0
								) {
									reject(error.networkError.result.errors.map((x: {message: string}) => x.message));
								} else {
									reject([error.message]);
								}
							} else {
								reject([error.message]);
							}
						});
				}
			});

			this.showSpinner = true;
			return saveResult
				.then(action(result => {
					this.showSpinner = false;
					const actionDone = this.props.formMode === 'create'
						? 'added'
						: (this.props.formMode === 'edit' ? 'edited' : '');

					console.log(`Successfully ${actionDone} ${modelName}`, result);
					alert(`Successfully ${actionDone} ${modelName}`, 'success');

					const { redirect } = queryString.parse(this.props.location.search.substring(1));

					if (redirect && !Array.isArray(redirect)) {
						store.routerHistory.push(redirect.replace('{id}', model.id));
					} else {
						store.routerHistory.goBack();
					}
				}))
				.catch(action((errors: readonly string[]) => {
					this.showSpinner = false;

					if (errors && errors.length) {
						const uniqueErrors = _.uniq(errors).join(', ');
						let detailedError = 'Could not save entity';
						alert(uniqueErrors, 'error');

						this.setErrors(detailedError, {}, uniqueErrors);
					} else {
						this.setErrors('Could not save entity', {});
						console.error('Crud Save Error', errors);
					}
				}));
			})
		);
	}

	@action
	private setErrors(generalError: React.ReactNode, errors?: IEntityValidationErrors, detailedErrors?: React.ReactNode) {
		this.generalFormError = generalError;
		this.fieldErrors = errors;
		this.detailedFormError = detailedErrors;
	}

	@action
	private clearErrors() {
		this.generalFormError = "";
		this.fieldErrors = {};
	}

	private onCancel = (event: React.MouseEvent<Element, MouseEvent>) => {
		store.routerHistory.goBack();
	}

	private onEdit = (event: React.MouseEvent<Element, MouseEvent>) => {
		this.props.history.push(`../Edit/${this.props.model.id}`);
	}

	private onAttributeAfterChange = (attributeName: string) => {
		const { model } = this.props;
		if(!!this.validationPath[attributeName] || this.hasSubmittedOnce){
			if(this.props.model.getErrorsForAttribute(attributeName).length > 0){
				model.validate(this.hasSubmittedOnce ? this.props.customRelationPath : this.validationPath).then(() => {
					if (!model.hasValidationError) {
						this.clearErrors();
					}
				});
			}
		}
	}

	private onAttributeChangeAndBlur = (attributeName: string) => {
		const { model } = this.props;
		this.validationPath = {...this.validationPath, [`${attributeName}`]: true};
		model.validate(this.hasSubmittedOnce ? this.props.customRelationPath : this.validationPath).then(() => {
			if (!model.hasValidationError) {
				this.clearErrors();
			}
		});
	}

	// % protected region % [Add extra methods here] off begin
	// % protected region % [Add extra methods here] end
}

export default EntityAttributeList;