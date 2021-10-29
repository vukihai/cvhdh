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
import { action, observable, runInAction } from 'mobx';
import { Model, IModelAttributes, attribute, entity } from 'Models/Model';
import {IOrderByCondition} from 'Views/Components/ModelCollection/ModelQuery';
import * as Models from 'Models/Entities';
import * as Validators from 'Validators';
import { CRUD } from '../CRUDOptions';
import * as AttrUtils from "Util/AttributeUtils";
import { IAcl } from 'Models/Security/IAcl';
import {
	makeFetchOneToManyFunc,
	makeEnumFetchFunction,
	getCreatedModifiedCrudOptions,
} from 'Util/EntityUtils';
import { VisitorsAchievementsEntity } from 'Models/Security/Acl/VisitorsAchievementsEntity';
import { StaffAchievementsEntity } from 'Models/Security/Acl/StaffAchievementsEntity';
import * as Enums from '../Enums';
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import { FormEntityData, FormEntityDataAttributes, getAllVersionsFn, getPublishedVersionFn } from 'Forms/FormEntityData';
import { FormVersion } from 'Forms/FormVersion';
import { fetchFormVersions, fetchPublishedVersion } from 'Forms/Forms';
import {SuperAdministratorScheme} from '../Security/Acl/SuperAdministratorScheme';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

export interface IAchievementsEntityAttributes extends IModelAttributes, FormEntityDataAttributes {
	name: string;
	achievementDate: Date;
	achievementDetails: string;
	achievementType: Enums.achievementTypes;

	studentsId?: string;
	students?: Models.StudentsEntity | Models.IStudentsEntityAttributes;
	formPages: Array<Models.AchievementsEntityFormTileEntity | Models.IAchievementsEntityFormTileEntityAttributes>;
	// % protected region % [Add any custom attributes to the interface here] off begin
	// % protected region % [Add any custom attributes to the interface here] end
}

// % protected region % [Customise your entity metadata here] off begin
@entity('AchievementsEntity', 'Achievements')
// % protected region % [Customise your entity metadata here] end
export default class AchievementsEntity extends Model implements IAchievementsEntityAttributes, FormEntityData  {
	public static acls: IAcl[] = [
		new SuperAdministratorScheme(),
		new VisitorsAchievementsEntity(),
		new StaffAchievementsEntity(),
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
			path: 'achievementDate',
			descending: true,
		};
		// % protected region % [Modify the order by field here] end
	}

	// % protected region % [Modify props to the crud options here for attribute 'Name'] off begin
	@Validators.Required()
	@observable
	@attribute()
	@CRUD({
		name: 'Name',
		displayType: 'textfield',
		order: 10,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public name: string;
	// % protected region % [Modify props to the crud options here for attribute 'Name'] end

	// % protected region % [Modify props to the crud options here for attribute 'Achievement Date'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Achievement Date',
		displayType: 'datepicker',
		order: 20,
		headerColumn: true,
		searchable: true,
		searchFunction: 'equal',
		searchTransform: AttrUtils.standardiseDate,
	})
	public achievementDate: Date;
	// % protected region % [Modify props to the crud options here for attribute 'Achievement Date'] end

	// % protected region % [Modify props to the crud options here for attribute 'Achievement Details'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Achievement Details',
		displayType: 'textfield',
		order: 30,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public achievementDetails: string;
	// % protected region % [Modify props to the crud options here for attribute 'Achievement Details'] end

	// % protected region % [Modify props to the crud options here for attribute 'Achievement Type'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Achievement Type',
		displayType: 'enum-combobox',
		order: 40,
		headerColumn: true,
		searchable: true,
		searchFunction: 'equal',
		searchTransform: (attr: string) => {
			return AttrUtils.standardiseEnum(attr, Enums.achievementTypesOptions);
		},
		enumResolveFunction: makeEnumFetchFunction(Enums.achievementTypesOptions),
		displayFunction: (attribute: Enums.achievementTypes) => Enums.achievementTypesOptions[attribute],
	})
	public achievementType: Enums.achievementTypes;
	// % protected region % [Modify props to the crud options here for attribute 'Achievement Type'] end

	@observable
	@attribute({isReference: true})
	public formVersions: FormVersion[] = [];

	@observable
	@attribute()
	public publishedVersionId?: string;

	@observable
	@attribute({isReference: true})
	public publishedVersion?: FormVersion;

	@observable
	@attribute()
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Students'] off begin
		name: 'Students',
		displayType: 'reference-combobox',
		order: 50,
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
		// % protected region % [Modify props to the crud options here for reference 'Form Page'] off begin
		name: "Form Pages",
		displayType: 'hidden',
		order: 60,
		referenceTypeFunc: () => Models.AchievementsEntityFormTileEntity,
		disableDefaultOptionRemoval: true,
		referenceResolveFunction: makeFetchOneToManyFunc({
			relationName: 'formPages',
			oppositeEntity: () => Models.AchievementsEntityFormTileEntity,
		}),
		// % protected region % [Modify props to the crud options here for reference 'Form Page'] end
	})
	public formPages: Models.AchievementsEntityFormTileEntity[] = [];

	// % protected region % [Add any custom attributes to the model here] off begin
	// % protected region % [Add any custom attributes to the model here] end

	// eslint-disable-next-line @typescript-eslint/no-useless-constructor
	constructor(attributes?: Partial<IAchievementsEntityAttributes>) {
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
	public assignAttributes(attributes?: Partial<IAchievementsEntityAttributes>) {
		// % protected region % [Override assign attributes here] off begin
		super.assignAttributes(attributes);

		if (attributes) {
			if (attributes.achievementDate !== undefined) {
				if (attributes.achievementDate === null) {
					this.achievementDate = attributes.achievementDate;
				} else {
					this.achievementDate = moment(attributes.achievementDate).toDate();
				}
			}
			if (attributes.achievementDetails !== undefined) {
				this.achievementDetails = attributes.achievementDetails;
			}
			if (attributes.achievementType !== undefined) {
				this.achievementType = attributes.achievementType;
			}
			if (attributes.publishedVersionId !== undefined) {
				this.publishedVersionId = attributes.publishedVersionId;
			}
			if (attributes.publishedVersion !== undefined) {
				if (attributes.publishedVersion === null) {
					this.publishedVersion = attributes.publishedVersion;
				} else {
					this.publishedVersion = attributes.publishedVersion;
					this.publishedVersionId = attributes.publishedVersion.id;
					if (typeof attributes.publishedVersion.formData === 'string') {
						this.publishedVersion.formData = JSON.parse(attributes.publishedVersion.formData);
					}
				}
			}
			if (attributes.formVersions !== undefined) {
				this.formVersions.push(...attributes.formVersions);
			}
			if (attributes.name !== undefined) {
				this.name = attributes.name;
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
			if (attributes.formPages !== undefined && Array.isArray(attributes.formPages)) {
				for (const model of attributes.formPages) {
					if (model instanceof Models.AchievementsEntityFormTileEntity) {
						this.formPages.push(model);
					} else {
						this.formPages.push(new Models.AchievementsEntityFormTileEntity(model));
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
		publishedVersion {
			id
			created
			modified
			formData
		}
		students {
			${Models.StudentsEntity.getAttributes().join('\n')}
		}
		formPages {
			${Models.AchievementsEntityFormTileEntity.getAttributes().join('\n')}
		}
	`;
	// % protected region % [Customize Default Expands here] end

	/**
	 * The save method that is called from the admin CRUD components.
	 */
	// % protected region % [Customize Save From Crud here] off begin
	public async saveFromCrud(formMode: EntityFormMode) {
		const relationPath = {
			formPages: {},
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
		return this.name;
		// % protected region % [Customise the display name for this entity] end
	}

	/**
	 * Gets all the versions for this form.
	 */
	public getAllVersions: getAllVersionsFn = (includeSubmissions?, conditions?) => {
		// % protected region % [Modify the getAllVersionsFn here] off begin
		return fetchFormVersions(this, includeSubmissions, conditions)
			.then(d => {
				runInAction(() => this.formVersions = d);
				return d.map(x => x.formData)
			});
		// % protected region % [Modify the getAllVersionsFn here] end
	};

	/**
	 * Gets the published version for this form.
	 */
	public getPublishedVersion: getPublishedVersionFn = includeSubmissions => {
		// % protected region % [Modify the getPublishedVersionFn here] off begin
		return fetchPublishedVersion(this, includeSubmissions)
			.then(d => {
				runInAction(() => this.publishedVersion = d);
				return d ? d.formData : undefined;
			});
		// % protected region % [Modify the getPublishedVersionFn here] end
	};

	/**
	 * Gets the submission entity type for this form.
	 */
	public getSubmissionEntity = () => {
		// % protected region % [Modify the getSubmissionEntity here] off begin
		return Models.AchievementsSubmissionEntity;
		// % protected region % [Modify the getSubmissionEntity here] end
	}


	// % protected region % [Add any further custom model features here] off begin
	// % protected region % [Add any further custom model features here] end
}

// % protected region % [Modify the create and modified CRUD attributes here] off begin
/*
 * Retrieve the created and modified CRUD attributes for defining the CRUD views and decorate the class with them.
 */
const [ createdAttr, modifiedAttr ] = getCreatedModifiedCrudOptions();
CRUD(createdAttr)(AchievementsEntity.prototype, 'created');
CRUD(modifiedAttr)(AchievementsEntity.prototype, 'modified');
// % protected region % [Modify the create and modified CRUD attributes here] end
