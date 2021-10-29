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
import Navigation, { Orientation, ILink } from 'Views/Components/Navigation/Navigation';
import { RouteComponentProps } from 'react-router';
import * as Models from 'Models/Entities';
import { Model } from 'Models/Model';
import { IIconProps } from "Views/Components/Helpers/Common";
import { SecurityService } from "Services/SecurityService";
import { getModelDisplayName } from 'Util/EntityUtils';
import { store } from 'Models/Store';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

interface AdminLink extends IIconProps {
	path: string;
	label: string;
	entity: {new (args: any): Model};
	isMember?: boolean;
	// % protected region % [Add extra AdminLink fields here] off begin
	// % protected region % [Add extra AdminLink fields here] end
}

const getPageLinks = (): AdminLink[] => [
	{
		// % protected region % [Override navigation link for AchievementsEntity here] off begin
		path: '/admin/achievementsentity',
		label: getModelDisplayName(Models.AchievementsEntity),
		entity: Models.AchievementsEntity,
		isMember: false
		// % protected region % [Override navigation link for AchievementsEntity here] end
	},
	{
		// % protected region % [Override navigation link for AddressEntity here] off begin
		path: '/admin/addressentity',
		label: getModelDisplayName(Models.AddressEntity),
		entity: Models.AddressEntity,
		isMember: false
		// % protected region % [Override navigation link for AddressEntity here] end
	},
	{
		// % protected region % [Override navigation link for StudentsEntity here] off begin
		path: '/admin/studentsentity',
		label: getModelDisplayName(Models.StudentsEntity),
		entity: Models.StudentsEntity,
		isMember: false
		// % protected region % [Override navigation link for StudentsEntity here] end
	},
	{
		// % protected region % [Override navigation link for AssessmentNotesEntity here] off begin
		path: '/admin/assessmentnotesentity',
		label: getModelDisplayName(Models.AssessmentNotesEntity),
		entity: Models.AssessmentNotesEntity,
		isMember: false
		// % protected region % [Override navigation link for AssessmentNotesEntity here] end
	},
	{
		// % protected region % [Override navigation link for AssessmentsEntity here] off begin
		path: '/admin/assessmentsentity',
		label: getModelDisplayName(Models.AssessmentsEntity),
		entity: Models.AssessmentsEntity,
		isMember: false
		// % protected region % [Override navigation link for AssessmentsEntity here] end
	},
	{
		// % protected region % [Override navigation link for CommentsEntity here] off begin
		path: '/admin/commentsentity',
		label: getModelDisplayName(Models.CommentsEntity),
		entity: Models.CommentsEntity,
		isMember: false
		// % protected region % [Override navigation link for CommentsEntity here] end
	},
	{
		// % protected region % [Override navigation link for StaffEntity here] off begin
		path: '/admin/staffentity',
		label: getModelDisplayName(Models.StaffEntity),
		entity: Models.StaffEntity,
		isMember: true
		// % protected region % [Override navigation link for StaffEntity here] end
	},
	{
		// % protected region % [Override navigation link for AchievementsSubmissionEntity here] off begin
		path: '/admin/achievementssubmissionentity',
		label: getModelDisplayName(Models.AchievementsSubmissionEntity),
		entity: Models.AchievementsSubmissionEntity,
		isMember: false
		// % protected region % [Override navigation link for AchievementsSubmissionEntity here] end
	},
	{
		// % protected region % [Override navigation link for AddressSubmissionEntity here] off begin
		path: '/admin/addresssubmissionentity',
		label: getModelDisplayName(Models.AddressSubmissionEntity),
		entity: Models.AddressSubmissionEntity,
		isMember: false
		// % protected region % [Override navigation link for AddressSubmissionEntity here] end
	},
	{
		// % protected region % [Override navigation link for StudentsSubmissionEntity here] off begin
		path: '/admin/studentssubmissionentity',
		label: getModelDisplayName(Models.StudentsSubmissionEntity),
		entity: Models.StudentsSubmissionEntity,
		isMember: false
		// % protected region % [Override navigation link for StudentsSubmissionEntity here] end
	},
	{
		// % protected region % [Override navigation link for AchievementsEntityFormTileEntity here] off begin
		path: '/admin/achievementsentityformtileentity',
		label: getModelDisplayName(Models.AchievementsEntityFormTileEntity),
		entity: Models.AchievementsEntityFormTileEntity,
		isMember: false
		// % protected region % [Override navigation link for AchievementsEntityFormTileEntity here] end
	},
	{
		// % protected region % [Override navigation link for StudentsEntityFormTileEntity here] off begin
		path: '/admin/studentsentityformtileentity',
		label: getModelDisplayName(Models.StudentsEntityFormTileEntity),
		entity: Models.StudentsEntityFormTileEntity,
		isMember: false
		// % protected region % [Override navigation link for StudentsEntityFormTileEntity here] end
	},
	// % protected region % [Add any extra page links here] off begin
	// % protected region % [Add any extra page links here] end
];

export default class PageLinks extends React.Component<RouteComponentProps> {
	private filter = (link: AdminLink) => {
		return SecurityService.canRead(link.entity);
	}

	public render() {
		return <Navigation
			className='nav__admin'
			orientation={Orientation.VERTICAL}
			linkGroups={this.getAdminNavLinks()}
			{...this.props} />;
	}

	private getAdminNavLinks = () : ILink[][] => {
		// % protected region % [Add custom logic before all here] off begin
		// % protected region % [Add custom logic before all here] end

		const links = getPageLinks();
		let userLinks = links.filter(link => link.isMember).filter(this.filter);
		let entityLinks = links.filter(link => ! link.isMember).filter(this.filter);

		let linkGroups: ILink[][] = [];

		// % protected region % [Add any custom logic here before groups are made] off begin
		// % protected region % [Add any custom logic here before groups are made] end

		const homeLinkGroup: ILink[] = [
			{ path: '/admin', label: 'Home', icon: 'home', iconPos: 'icon-left' },
			// % protected region % [Updated your home link group here] off begin
			// % protected region % [Updated your home link group here] end
		];
		linkGroups.push(homeLinkGroup);

		const entityLinkGroup: ILink[] = [];
		if (userLinks.length > 0) {
			entityLinkGroup.push(
				{
					path: '/admin/users',
					label: 'Users',
					icon: 'person-group',
					iconPos: 'icon-left',
					subLinks: [
						{path: "/admin/user", label: "All Users"},
						...userLinks.map(link => ({path: link.path, label: link.label}))
					]
				}
			);
		}
		if (entityLinks.length > 0) {
			entityLinkGroup.push(
				{
					path: '/admin/entities',
					label: 'Entities',
					icon: 'list',
					iconPos: 'icon-left',
					subLinks: entityLinks.map(link => {
						return {
							path: link.path,
							label: link.label,
						}
					})
				}
			);
		}
		linkGroups.push(entityLinkGroup);

		// % protected region % [Add any new link groups here before other and bottom] off begin
		// % protected region % [Add any new link groups here before other and bottom] end

		const otherlinkGroup: ILink[] = [];
		// % protected region % [Update the link group for the forms extension here] off begin
		const formsGroups: string[] = ['Staff', 'Super Administrators', 'Visitors'];
		if (store.userGroups.some(ug => formsGroups.some(fg => fg === ug.name))) {
			otherlinkGroup.push(
				{
					path: '/admin/forms',
					label: 'Forms',
					icon: 'forms',
					iconPos: 'icon-left',
					isDisabled: false
				}
			);
		}
		// % protected region % [Update the link group for the forms extension here] end

		// % protected region % [Add any additional links to otherLinkGroup here] off begin
		// % protected region % [Add any additional links to otherLinkGroup here] end

		if (otherlinkGroup.length > 0) {
			linkGroups.push(otherlinkGroup);
		}

		const bottomlinkGroup: ILink[] = [];
		bottomlinkGroup.push(
			// % protected region % [Modify the utility link group here] off begin
			{
				path: '/admin/',
				label: 'Utility',
				icon: 'file',
				iconPos: 'icon-left',
				subLinks: [
					{
						path: '/admin/styleguide',
						label: 'Style Guide',
						useATag: false
					},
					{
						path: '/admin/graphiql',
						label: 'GraphiQL',
						useATag: true,
					},
					{
						path: '/api/hangfire',
						label: 'Hangfire',
						useATag: true,
					},
					{
						path: '/api/swagger',
						label: 'OpenAPI',
						useATag: true,
					},
					{
						path: 'https://gitlab.codebots.dev',
						label: 'Git',
						useATag: true,
					},
				],
			}
			// % protected region % [Modify the utility link group here] end
		);

		// % protected region % [Update the logout link here] off begin
		bottomlinkGroup.push(
			{
				path: '/logout',
				label: 'Logout',
				icon: 'room',
				iconPos: 'icon-left'
			}
		);
		// % protected region % [Update the logout link here] end

		// % protected region % [Add any additional links to bottomlinkGroup here] off begin
		// % protected region % [Add any additional links to bottomlinkGroup here] end

		linkGroups.push(bottomlinkGroup);

		// % protected region % [Modify your link groups here before returning] off begin
		// % protected region % [Modify your link groups here before returning] end

		return linkGroups;
	}

	// % protected region % [Add custom methods here] off begin
	// % protected region % [Add custom methods here] end
}