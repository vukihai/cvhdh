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
import { AttributeCRUDOptions } from 'Models/CRUDOptions';
import { Model, IModelType } from 'Models/Model';
import { RouteComponentProps } from 'react-router';
import EntityAttributeList from './EntityAttributeList';
import { EntityFormMode } from '../Helpers/Common'
import { getFetchSingleQuery, getModelDisplayName, getModelName } from 'Util/EntityUtils';
import { lowerCaseFirst } from 'Util/StringUtils';
import { useQuery } from '@apollo/client';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

interface IEntityEditProps<T extends Model> extends RouteComponentProps<IEntityEditRouteParams> {
	modelType: IModelType<T>;
	formMode: EntityFormMode;
	/** Function to mutate the attribute options before it is rendered */
	mutateOptions?: (model: Model | Model[], options: AttributeCRUDOptions[], formMode: EntityFormMode) => AttributeCRUDOptions[];
	/** Function to call when saving the entity. If this is not provided then model.saveFromCrud is called instead. */
	saveFn?: (entity: T, formMode: EntityFormMode) => Promise<void>;
	// % protected region % [Add any extra props here] off begin
	// % protected region % [Add any extra props here] end
}

interface IEntityEditRouteParams {
	id?: string;
	// % protected region % [Add any extra route params here] off begin
	// % protected region % [Add any extra route params here] end
}

function EntityEdit<T extends Model>(props: IEntityEditProps<T>) {
	// % protected region % [Customise setup here] off begin
	const { modelType, formMode, match, mutateOptions, saveFn } = props;
	const query = getFetchSingleQuery(modelType);
	const modelName = getModelDisplayName(modelType);
	const dataReturnName = lowerCaseFirst(getModelName(modelType));

	const title = `${formMode === 'create' ? 'Create' : (formMode === 'edit' ? 'Edit' : 'View')} ${modelName}`;
	const sectionClassName = 'crud__' + formMode;
	const options = { title, sectionClassName };
	// % protected region % [Customise setup here] end

	// % protected region % [Customise route errors here] off begin
	if (match.params.id === null) {
		throw new Error('Expected id of model to fetch for edit');
	}
	// % protected region % [Customise route errors here] end

	// % protected region % [Customise query here] off begin
	const { loading, error, data } = useQuery(query, {
		fetchPolicy: 'network-only',
		variables: { "args": [{ "path": "id", "comparison": "equal", "value": match.params.id }] }
	});
	// % protected region % [Customise query here] end

	// % protected region % [Customise loading here] off begin
	if (loading) {
		return <div>Loading {modelName}...</div>;
	}
	// % protected region % [Customise loading here] end

	// % protected region % [Customise error handling here] off begin
	if (error) {
		return <div>Error Loading {modelName}</div>;
	}
	// % protected region % [Customise error handling here] end

	// % protected region % [Customise render here] off begin
	return (
		<EntityAttributeList
			{...props}
			model={new modelType(data[dataReturnName])}
			{...options}
			formMode={formMode}
			modelType={modelType}
			mutateOptions={mutateOptions}
			saveFn={saveFn}
		/>
	);
	// % protected region % [Customise render here] end
}

// % protected region % [Customise default export here] off begin
export default EntityEdit;
// % protected region % [Customise default export here] end
