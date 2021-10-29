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
import React, { useEffect } from "react";
import _ from 'lodash';
import Spinner from 'Views/Components/Spinner/Spinner';
import { gql, useQuery } from '@apollo/client';
import { FormEntityDataAttributes } from "Forms/FormEntityData";
import { NewFormVersionTile } from "Forms/Admin/Version/NewFormVersionTile";
import { IModelAttributes } from "Models/Model";
import { FormVersionTile } from "Forms/Admin/Version/FormVersionTile";
import { lowerCaseFirst } from 'Util/StringUtils';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Override getFormListQuery here] off begin
export function getFormListQuery (formType: string) {
	return gql`query fetch {
		forms: ${formType}s{
			id
			created
			modified
			name
			publishedVersionId
			publishedVersion {
				id
				created
				modified
				version
			}
			formVersions{
				id
				created
				modified
				version
			}
		}
	}`;
}
// % protected region % [Override getFormListQuery here] end

// % protected region % [Override IFormVersionCollectionProps here] off begin
export interface IFormVersionCollectionProps {
	formName: string;
	formDisplayName: string;
	showCreateTile : boolean;
}
// % protected region % [Override IFormVersionCollectionProps here] end

// % protected region % [Override types here] off begin
type responseData = FormEntityDataAttributes & IModelAttributes;
type formResponse = {
	forms: responseData[]
};
// % protected region % [Override types here] end

// % protected region % [Add any extra types here] off begin
// % protected region % [Add any extra types here] end

function FormVersionCollection(props: IFormVersionCollectionProps) {
	// % protected region % [Override initial logic here] off begin
	const { formDisplayName, formName, showCreateTile } = props;

	const { loading, error, data } = useQuery<formResponse>(getFormListQuery(lowerCaseFirst(formName)), {
		fetchPolicy: 'network-only',
	});
	useEffect(() => {
		if (error) {
			console.error(error);
		}
	}, [ error ]);

	const createTile = showCreateTile
		? <NewFormVersionTile formName={formName} formDisplayName={formDisplayName} />
		: null;
	// % protected region % [Override initial logic here] end

	let content: React.ReactNode = null;

	// % protected region % [Override loading logic here] off begin
	if (loading) {
		 content = <Spinner />
	}
	// % protected region % [Override loading logic here] end

	// % protected region % [Override error logic here] off begin
	if (error) {
		content = 'Something went wrong while connecting to the server. The error is ' + JSON.stringify(error);
	}
	// % protected region % [Override error logic here] end

	// % protected region % [Override data logic here] off begin
	if (data) {
		content = data.forms.map(form => {
			const currentVersion = !form.formVersions ? undefined : _.maxBy(form.formVersions, 'version');
			const currentVersionNumber = !currentVersion ? undefined : currentVersion.version;
			const publishedVersionNumber = !form.publishedVersion ? undefined : form.publishedVersion.version;

			if (form.id) {
				return <FormVersionTile
					formVersionName={form.name}
					formEntityName={formName}
					id={form.id}
					key={form.id}
					currentVersion={currentVersionNumber}
					publishedVersion={publishedVersionNumber}
				/>;
			}

			return null;
		});
	}
	// % protected region % [Override data logic here] end

	// % protected region % [Override render logic here] off begin
	return (
		<section className='forms-block-items'>
			{createTile}
			{content}
		</section>
	)
	// % protected region % [Override render logic here] end
}

// % protected region % [Override default export here] off begin
export default FormVersionCollection;
// % protected region % [Override default export here] end
