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
	makeFetchOneToManyFunc,
	getCreatedModifiedCrudOptions,
} from 'Util/EntityUtils';
import { VisitorsAssessmentsEntity } from 'Models/Security/Acl/VisitorsAssessmentsEntity';
import { StaffAssessmentsEntity } from 'Models/Security/Acl/StaffAssessmentsEntity';
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import {SuperAdministratorScheme} from '../Security/Acl/SuperAdministratorScheme';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

export interface IAssessmentsEntityAttributes extends IModelAttributes {
	startDate: Date;
	endDate: Date;
	summary: string;
	recommendations: string;

	assessmentNotess: Array<Models.AssessmentNotesEntity | Models.IAssessmentNotesEntityAttributes>;
	studentsId?: string;
	students?: Models.StudentsEntity | Models.IStudentsEntityAttributes;
	commentss: Array<Models.CommentsEntity | Models.ICommentsEntityAttributes>;
	// % protected region % [Add any custom attributes to the interface here] off begin
	// % protected region % [Add any custom attributes to the interface here] end
}

// % protected region % [Customise your entity metadata here] off begin
@entity('AssessmentsEntity', 'Assessments')
// % protected region % [Customise your entity metadata here] end
export default class AssessmentsEntity extends Model implements IAssessmentsEntityAttributes {
	public static acls: IAcl[] = [
		new SuperAdministratorScheme(),
		new VisitorsAssessmentsEntity(),
		new StaffAssessmentsEntity(),
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
			path: 'startDate',
			descending: true,
		};
		// % protected region % [Modify the order by field here] end
	}

	// % protected region % [Modify props to the crud options here for attribute 'Start Date'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Start Date',
		displayType: 'datepicker',
		order: 10,
		headerColumn: true,
		searchable: true,
		searchFunction: 'equal',
		searchTransform: AttrUtils.standardiseDate,
	})
	public startDate: Date;
	// % protected region % [Modify props to the crud options here for attribute 'Start Date'] end

	// % protected region % [Modify props to the crud options here for attribute 'End Date'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'End Date',
		displayType: 'datepicker',
		order: 20,
		headerColumn: true,
		searchable: true,
		searchFunction: 'equal',
		searchTransform: AttrUtils.standardiseDate,
	})
	public endDate: Date;
	// % protected region % [Modify props to the crud options here for attribute 'End Date'] end

	// % protected region % [Modify props to the crud options here for attribute 'Summary'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Summary',
		displayType: 'textfield',
		order: 30,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public summary: string;
	// % protected region % [Modify props to the crud options here for attribute 'Summary'] end

	// % protected region % [Modify props to the crud options here for attribute 'Recommendations'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Recommendations',
		displayType: 'textfield',
		order: 40,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public recommendations: string;
	// % protected region % [Modify props to the crud options here for attribute 'Recommendations'] end

	@observable
	@attribute({isReference: true})
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Assessment Notes'] off begin
		name: "Assessment Notess",
		displayType: 'reference-multicombobox',
		order: 50,
		referenceTypeFunc: () => Models.AssessmentNotesEntity,
		referenceResolveFunction: makeFetchOneToManyFunc({
			relationName: 'assessmentNotess',
			oppositeEntity: () => Models.AssessmentNotesEntity,
		}),
		// % protected region % [Modify props to the crud options here for reference 'Assessment Notes'] end
	})
	public assessmentNotess: Models.AssessmentNotesEntity[] = [];

	@observable
	@attribute()
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Students'] off begin
		name: 'Students',
		displayType: 'reference-combobox',
		order: 60,
		referenceTypeFunc: () => Models.StudentsEntity,
		// % protected region % [Modify props to the crud options here for reference 'Students'] end
	})
	public studentsId?: string;
	@observable
	@attribute({isReference: true})
	public students: Models.StudentsEntity;

	@observable
	@attribute({isReference: true})
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Comments'] off begin
		name: "Commentss",
		displayType: 'reference-multicombobox',
		order: 70,
		referenceTypeFunc: () => Models.CommentsEntity,
		referenceResolveFunction: makeFetchOneToManyFunc({
			relationName: 'commentss',
			oppositeEntity: () => Models.CommentsEntity,
		}),
		// % protected region % [Modify props to the crud options here for reference 'Comments'] end
	})
	public commentss: Models.CommentsEntity[] = [];

	// % protected region % [Add any custom attributes to the model here] off begin
	// % protected region % [Add any custom attributes to the model here] end

	// eslint-disable-next-line @typescript-eslint/no-useless-constructor
	constructor(attributes?: Partial<IAssessmentsEntityAttributes>) {
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
	public assignAttributes(attributes?: Partial<IAssessmentsEntityAttributes>) {
		// % protected region % [Override assign attributes here] off begin
		super.assignAttributes(attributes);

		if (attributes) {
			if (attributes.startDate !== undefined) {
				if (attributes.startDate === null) {
					this.startDate = attributes.startDate;
				} else {
					this.startDate = moment(attributes.startDate).toDate();
				}
			}
			if (attributes.endDate !== undefined) {
				if (attributes.endDate === null) {
					this.endDate = attributes.endDate;
				} else {
					this.endDate = moment(attributes.endDate).toDate();
				}
			}
			if (attributes.summary !== undefined) {
				this.summary = attributes.summary;
			}
			if (attributes.recommendations !== undefined) {
				this.recommendations = attributes.recommendations;
			}
			if (attributes.assessmentNotess !== undefined && Array.isArray(attributes.assessmentNotess)) {
				for (const model of attributes.assessmentNotess) {
					if (model instanceof Models.AssessmentNotesEntity) {
						this.assessmentNotess.push(model);
					} else {
						this.assessmentNotess.push(new Models.AssessmentNotesEntity(model));
					}
				}
			}
			if (attributes.studentsId !== undefined) {
				this.studentsId = attributes.studentsId;
			}
			if (attributes.students !== undefined) {
				if (attributes.students === null) {
					this.students = attributes.students;
				} else {
					if (attributes.students instanceof Models.StudentsEntity) {
						this.students = attributes.students;
						this.studentsId = attributes.students.id;
					} else {
						this.students = new Models.StudentsEntity(attributes.students);
						this.studentsId = this.students.id;
					}
				}
			}
			if (attributes.commentss !== undefined && Array.isArray(attributes.commentss)) {
				for (const model of attributes.commentss) {
					if (model instanceof Models.CommentsEntity) {
						this.commentss.push(model);
					} else {
						this.commentss.push(new Models.CommentsEntity(model));
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
		assessmentNotess {
			${Models.AssessmentNotesEntity.getAttributes().join('\n')}
		}
		students {
			${Models.StudentsEntity.getAttributes().join('\n')}
		}
		commentss {
			${Models.CommentsEntity.getAttributes().join('\n')}
		}
	`;
	// % protected region % [Customize Default Expands here] end

	/**
	 * The save method that is called from the admin CRUD components.
	 */
	// % protected region % [Customize Save From Crud here] off begin
	public async saveFromCrud(formMode: EntityFormMode) {
		const relationPath = {
			assessmentNotess: {},
			commentss: {},
		};
		return this.save(
			relationPath,
			{
				options: [
					{
						key: 'mergeReferences',
						graphQlType: '[String]',
						value: [
							'assessmentNotess',
							'commentss',
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
CRUD(createdAttr)(AssessmentsEntity.prototype, 'created');
CRUD(modifiedAttr)(AssessmentsEntity.prototype, 'modified');
// % protected region % [Modify the create and modified CRUD attributes here] end
