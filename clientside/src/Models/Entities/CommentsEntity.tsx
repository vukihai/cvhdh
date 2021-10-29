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
import moment from 'moment';
import { action, observable } from 'mobx';
import { Model, IModelAttributes, attribute, entity } from 'Models/Model';
import {IOrderByCondition} from 'Views/Components/ModelCollection/ModelQuery';
import * as Models from 'Models/Entities';
import { CRUD } from '../CRUDOptions';
import * as AttrUtils from "Util/AttributeUtils";
import { IAcl } from 'Models/Security/IAcl';
import {
	getCreatedModifiedCrudOptions,
} from 'Util/EntityUtils';
import { VisitorsCommentsEntity } from 'Models/Security/Acl/VisitorsCommentsEntity';
import { StaffCommentsEntity } from 'Models/Security/Acl/StaffCommentsEntity';
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import {SuperAdministratorScheme} from '../Security/Acl/SuperAdministratorScheme';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

export interface ICommentsEntityAttributes extends IModelAttributes {
	commentDate: Date;
	commentDescription: string;

	assessmentsId?: string;
	assessments?: Models.AssessmentsEntity | Models.IAssessmentsEntityAttributes;
	// % protected region % [Add any custom attributes to the interface here] off begin
	// % protected region % [Add any custom attributes to the interface here] end
}

// % protected region % [Customise your entity metadata here] off begin
@entity('CommentsEntity', 'Comments')
// % protected region % [Customise your entity metadata here] end
export default class CommentsEntity extends Model implements ICommentsEntityAttributes {
	public static acls: IAcl[] = [
		new SuperAdministratorScheme(),
		new VisitorsCommentsEntity(),
		new StaffCommentsEntity(),
		// % protected region % [Add any further ACL entries here] off begin
		// % protected region % [Add any further ACL entries here] end
	];

	/**
	 * Fields to exclude from the JSON serialization in create operations.
	 */
	public static excludeFromCreate: string[] = [
		// % protected region % [Add any custom create exclusions here] off begin
		// % protected region % [Add any custom create exclusions here] end
	];

	/**
	 * Fields to exclude from the JSON serialization in update operations.
	 */
	public static excludeFromUpdate: string[] = [
		// % protected region % [Add any custom update exclusions here] off begin
		// % protected region % [Add any custom update exclusions here] end
	];

	/**
	 * The default order by field when the collection is loaded .
	 */
	public get orderByField(): IOrderByCondition<Model> | undefined {
		// % protected region % [Modify the order by field here] off begin
		return {
			path: 'commentDate',
			descending: true,
		};
		// % protected region % [Modify the order by field here] end
	}

	// % protected region % [Modify props to the crud options here for attribute 'Comment Date'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Comment Date',
		displayType: 'datepicker',
		order: 10,
		headerColumn: true,
		searchable: true,
		searchFunction: 'equal',
		searchTransform: AttrUtils.standardiseDate,
	})
	public commentDate: Date;
	// % protected region % [Modify props to the crud options here for attribute 'Comment Date'] end

	// % protected region % [Modify props to the crud options here for attribute 'Comment Description'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Comment Description',
		displayType: 'textfield',
		order: 20,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public commentDescription: string;
	// % protected region % [Modify props to the crud options here for attribute 'Comment Description'] end

	@observable
	@attribute()
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Assessments'] off begin
		name: 'Assessments',
		displayType: 'reference-combobox',
		order: 30,
		referenceTypeFunc: () => Models.AssessmentsEntity,
		// % protected region % [Modify props to the crud options here for reference 'Assessments'] end
	})
	public assessmentsId?: string;
	@observable
	@attribute({isReference: true})
	public assessments: Models.AssessmentsEntity;

	// % protected region % [Add any custom attributes to the model here] off begin
	// % protected region % [Add any custom attributes to the model here] end

	// eslint-disable-next-line @typescript-eslint/no-useless-constructor
	constructor(attributes?: Partial<ICommentsEntityAttributes>) {
		// % protected region % [Add any extra constructor logic before calling super here] off begin
		// % protected region % [Add any extra constructor logic before calling super here] end

		super(attributes);

		// % protected region % [Add any extra constructor logic after calling super here] off begin
		// % protected region % [Add any extra constructor logic after calling super here] end
	}

	/**
	 * Assigns fields from a passed in JSON object to the fields in this model.
	 * Any reference objects that are passed in are converted to models if they are not already.
	 * This function is called from the constructor to assign the initial fields.
	 */
	@action
	public assignAttributes(attributes?: Partial<ICommentsEntityAttributes>) {
		// % protected region % [Override assign attributes here] off begin
		super.assignAttributes(attributes);

		if (attributes) {
			if (attributes.commentDate !== undefined) {
				if (attributes.commentDate === null) {
					this.commentDate = attributes.commentDate;
				} else {
					this.commentDate = moment(attributes.commentDate).toDate();
				}
			}
			if (attributes.commentDescription !== undefined) {
				this.commentDescription = attributes.commentDescription;
			}
			if (attributes.assessmentsId !== undefined) {
				this.assessmentsId = attributes.assessmentsId;
			}
			if (attributes.assessments !== undefined) {
				if (attributes.assessments === null) {
					this.assessments = attributes.assessments;
				} else {
					if (attributes.assessments instanceof Models.AssessmentsEntity) {
						this.assessments = attributes.assessments;
						this.assessmentsId = attributes.assessments.id;
					} else {
						this.assessments = new Models.AssessmentsEntity(attributes.assessments);
						this.assessmentsId = this.assessments.id;
					}
				}
			}
			// % protected region % [Override assign attributes here] end

			// % protected region % [Add any extra assign attributes logic here] off begin
			// % protected region % [Add any extra assign attributes logic here] end
		}
	}

	/**
	 * Additional fields that are added to GraphQL queries when using the
	 * the managed model APIs.
	 */
	// % protected region % [Customize Default Expands here] off begin
	public defaultExpands = `
		assessments {
			${Models.AssessmentsEntity.getAttributes().join('\n')}
		}
	`;
	// % protected region % [Customize Default Expands here] end

	/**
	 * The save method that is called from the admin CRUD components.
	 */
	// % protected region % [Customize Save From Crud here] off begin
	public async saveFromCrud(formMode: EntityFormMode) {
		const relationPath = {
		};
		return this.save(
			relationPath,
			{
				options: [
					{
						key: 'mergeReferences',
						graphQlType: '[String]',
						value: [
						]
					},
				],
			}
		);
	}
	// % protected region % [Customize Save From Crud here] end

	/**
	 * Returns the string representation of this entity to display on the UI.
	 */
	public getDisplayName() {
		// % protected region % [Customise the display name for this entity] off begin
		return this.id;
		// % protected region % [Customise the display name for this entity] end
	}


	// % protected region % [Add any further custom model features here] off begin
	// % protected region % [Add any further custom model features here] end
}

// % protected region % [Modify the create and modified CRUD attributes here] off begin
/*
 * Retrieve the created and modified CRUD attributes for defining the CRUD views and decorate the class with them.
 */
const [ createdAttr, modifiedAttr ] = getCreatedModifiedCrudOptions();
CRUD(createdAttr)(CommentsEntity.prototype, 'created');
CRUD(modifiedAttr)(CommentsEntity.prototype, 'modified');
// % protected region % [Modify the create and modified CRUD attributes here] end
