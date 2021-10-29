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
import * as Models from "Models/Entities";
import AccordionGroup from 'Views/Components/Accordion/Accordion';
import SecuredPage from "Views/Components/Security/SecuredPage";
import FormVersionCollection from "Forms/Admin/Version/FormVersionCollection";
import { RouteComponentProps } from "react-router";
import { PageWrapper } from "Views/Components/PageWrapper/PageWrapper";
import { SecurityService } from "Services/SecurityService";
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Add any extra types here] off begin
// % protected region % [Add any extra types here] end

// % protected region % [Override FormsLandingPage here] off begin
export default class FormsLandingPage extends React.Component<RouteComponentProps>{
// % protected region % [Override FormsLandingPage here] end

	// % protected region % [Override getAccordionProps here] off begin
	private getAccordionProps = () => {
		const forms = [
			{displayName: 'Address', name: 'AddressEntity', model: Models.AddressEntity},
			{displayName: 'Students', name: 'StudentsEntity', model: Models.StudentsEntity},
			{displayName: 'Achievements', name: 'AchievementsEntity', model: Models.AchievementsEntity},
		];

		return forms
			.filter(f => SecurityService.canRead(f.model))
			.map(f => ({
				name: f.displayName,
				key: f.name,
				component: <FormVersionCollection
					formName={f.name}
					formDisplayName={f.displayName}
					showCreateTile={SecurityService.canCreate(f.model)}/>
			}));
	};
	// % protected region % [Override getAccordionProps here] end

	// % protected region % [Add any extra methods here] off begin
	// % protected region % [Add any extra methods here] end

	// % protected region % [Override render here] off begin
	public render(){
		return (
			<SecuredPage groups={["Staff","Super Administrators","Visitors"]}>
				<PageWrapper {...this.props}>
					<section className={'forms-behaviour forms-behaviour__landing'}>
						<h2>Forms</h2>
						<AccordionGroup accordions={this.getAccordionProps()} />
					</section>
				</PageWrapper>
			</SecuredPage>
		)
	}
	// % protected region % [Override render here] end
}