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
	getCreatedModifiedCrudOptions,
} from 'Util/EntityUtils';
import { VisitorsStudentsEntity } from 'Models/Security/Acl/VisitorsStudentsEntity';
import { StaffStudentsEntity } from 'Models/Security/Acl/StaffStudentsEntity';
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import { FormEntityData, FormEntityDataAttributes, getAllVersionsFn, getPublishedVersionFn } from 'Forms/FormEntityData';
import { FormVersion } from 'Forms/FormVersion';
import { fetchFormVersions, fetchPublishedVersion } from 'Forms/Forms';
import {SuperAdministratorScheme} from '../Security/Acl/SuperAdministratorScheme';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

export interface IStudentsEntityAttributes extends IModelAttributes, FormEntityDataAttributes {
	name: string;
	firstName: string;
	lastName: string;
	contactNumber: string;
	email: string;
	enrolmentStart: Date;
	enrolmentEnd: Date;

	achievementss: Array<Models.AchievementsEntity | Models.IAchievementsEntityAttributes>;
	assessmentss: Array<Models.AssessmentsEntity | Models.IAssessmentsEntityAttributes>;
	addressId?: string;
	address?: Models.AddressEntity | Models.IAddressEntityAttributes;
	formPages: Array<Models.StudentsEntityFormTileEntity | Models.IStudentsEntityFormTileEntityAttributes>;
	// % protected region % [Add any custom attributes to the interface here] off begin
	// % protected region % [Add any custom attributes to the interface here] end
}

// % protected region % [Customise your entity metadata here] off begin
@entity('StudentsEntity', 'Students')
// % protected region % [Customise your entity metadata here] end
export default class StudentsEntity extends Model implements IStudentsEntityAttributes, FormEntityData  {
	public static acls: IAcl[] = [
		new SuperAdministratorScheme(),
		new VisitorsStudentsEntity(),
		new StaffStudentsEntity(),
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
			path: 'lastName',
			descending: false,
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

	// % protected region % [Modify props to the crud options here for attribute 'First Name'] off begin
	@Validators.Required()
	@observable
	@attribute()
	@CRUD({
		name: 'First Name',
		displayType: 'textfield',
		order: 20,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public firstName: string;
	// % protected region % [Modify props to the crud options here for attribute 'First Name'] end

	// % protected region % [Modify props to the crud options here for attribute 'Last Name'] off begin
	@Validators.Required()
	@observable
	@attribute()
	@CRUD({
		name: 'Last Name',
		displayType: 'textfield',
		order: 30,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public lastName: string;
	// % protected region % [Modify props to the crud options here for attribute 'Last Name'] end

	// % protected region % [Modify props to the crud options here for attribute 'Contact Number'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Contact Number',
		displayType: 'textfield',
		order: 40,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public contactNumber: string;
	// % protected region % [Modify props to the crud options here for attribute 'Contact Number'] end

	// % protected region % [Modify props to the crud options here for attribute 'Email'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Email',
		displayType: 'textfield',
		order: 50,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public email: string;
	// % protected region % [Modify props to the crud options here for attribute 'Email'] end

	// % protected region % [Modify props to the crud options here for attribute 'Enrolment Start'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Enrolment Start',
		displayType: 'datepicker',
		order: 60,
		headerColumn: true,
		searchable: true,
		searchFunction: 'equal',
		searchTransform: AttrUtils.standardiseDate,
	})
	public enrolmentStart: Date;
	// % protected region % [Modify props to the crud options here for attribute 'Enrolment Start'] end

	// % protected region % [Modify props to the crud options here for attribute 'Enrolment End'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Enrolment End',
		displayType: 'datepicker',
		order: 70,
		searchable: true,
		searchFunction: 'equal',
		searchTransform: AttrUtils.standardiseDate,
	})
	public enrolmentEnd: Date;
	// % protected region % [Modify props to the crud options here for attribute 'Enrolment End'] end

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
	@attribute({isReference: true})
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Achievements'] off begin
		name: "Achievementss",
		displayType: 'reference-multicombobox',
		order: 80,
		referenceTypeFunc: () => Models.AchievementsEntity,
		referenceResolveFunction: makeFetchOneToManyFunc({
			relationName: 'achievementss',
			oppositeEntity: () => Models.AchievementsEntity,
		}),
		// % protected region % [Modify props to the crud options here for reference 'Achievements'] end
	})
	public achievementss: Models.AchievementsEntity[] = [];

	@observable
	@attribute({isReference: true})
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Assessments'] off begin
		name: "Assessmentss",
		displayType: 'reference-multicombobox',
		order: 90,
		referenceTypeFunc: () => Models.AssessmentsEntity,
		referenceResolveFunction: makeFetchOneToManyFunc({
			relationName: 'assessmentss',
			oppositeEntity: () => Models.AssessmentsEntity,
		}),
		// % protected region % [Modify props to the crud options here for reference 'Assessments'] end
	})
	public assessmentss: Models.AssessmentsEntity[] = [];

	@observable
	@attribute()
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Address'] off begin
		name: 'Address',
		displayType: 'reference-combobox',
		order: 100,
		referenceTypeFunc: () => Models.AddressEntity,
		// % protected region % [Modify props to the crud options here for reference 'Address'] end
	})
	public addressId?: string;
	@observable
	@attribute({isReference: true})
	public address: Models.AddressEntity;

	@observable
	@attribute({isReference: true})
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Form Page'] off begin
		name: "Form Pages",
		displayType: 'hidden',
		order: 110,
		referenceTypeFunc: () => Models.StudentsEntityFormTileEntity,
		disableDefaultOptionRemoval: true,
		referenceResolveFunction: makeFetchOneToManyFunc({
			relationName: 'formPages',
			oppositeEntity: () => Models.StudentsEntityFormTileEntity,
		}),
		// % protected region % [Modify props to the crud options here for reference 'Form Page'] end
	})
	public formPages: Models.StudentsEntityFormTileEntity[] = [];

	// % protected region % [Add any custom attributes to the model here] off begin
	// % protected region % [Add any custom attributes to the model here] end

	// eslint-disable-next-line @typescript-eslint/no-useless-constructor
	constructor(attributes?: Partial<IStudentsEntityAttributes>) {
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
	public assignAttributes(attributes?: Partial<IStudentsEntityAttributes>) {
		// % protected region % [Override assign attributes here] off begin
		super.assignAttributes(attributes);

		if (attributes) {
			if (attributes.firstName !== undefined) {
				this.firstName = attributes.firstName;
			}
			if (attributes.lastName !== undefined) {
				this.lastName = attributes.lastName;
			}
			if (attributes.contactNumber !== undefined) {
				this.contactNumber = attributes.contactNumber;
			}
			if (attributes.email !== undefined) {
				this.email = attributes.email;
			}
			if (attributes.enrolmentStart !== undefined) {
				if (attributes.enrolmentStart === null) {
					this.enrolmentStart = attributes.enrolmentStart;
				} else {
					this.enrolmentStart = moment(attributes.enrolmentStart).toDate();
				}
			}
			if (attributes.enrolmentEnd !== undefined) {
				if (attributes.enrolmentEnd === null) {
					this.enrolmentEnd = attributes.enrolmentEnd;
				} else {
					this.enrolmentEnd = moment(attributes.enrolmentEnd).toDate();
				}
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
			if (attributes.achievementss !== undefined && Array.isArray(attributes.achievementss)) {
				for (const model of attributes.achievementss) {
					if (model instanceof Models.AchievementsEntity) {
						this.achievementss.push(model);
					} else {
						this.achievementss.push(new Models.AchievementsEntity(model));
					}
				}
			}
			if (attributes.assessmentss !== undefined && Array.isArray(attributes.assessmentss)) {
				for (const model of attributes.assessmentss) {
					if (model instanceof Models.AssessmentsEntity) {
						this.assessmentss.push(model);
					} else {
						this.assessmentss.push(new Models.AssessmentsEntity(model));
					}
				}
			}
			if (attributes.addressId !== undefined) {
				this.addressId = attributes.addressId;
			}
			if (attributes.address !== undefined) {
				if (attributes.address === null) {
					this.address = attributes.address;
				} else {
					if (attributes.address instanceof Models.AddressEntity) {
						this.address = attributes.address;
						this.addressId = attributes.address.id;
					} else {
						this.address = new Models.AddressEntity(attributes.address);
						this.addressId = this.address.id;
					}
				}
			}
			if (attributes.formPages !== undefined && Array.isArray(attributes.formPages)) {
				for (const model of attributes.formPages) {
					if (model instanceof Models.StudentsEntityFormTileEntity) {
						this.formPages.push(model);
					} else {
						this.formPages.push(new Models.StudentsEntityFormTileEntity(model));
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
		achievementss {
			${Models.AchievementsEntity.getAttributes().join('\n')}
		}
		assessmentss {
			${Models.AssessmentsEntity.getAttributes().join('\n')}
		}
		address {
			${Models.AddressEntity.getAttributes().join('\n')}
		}
		formPages {
			${Models.StudentsEntityFormTileEntity.getAttributes().join('\n')}
		}
	`;
	// % protected region % [Customize Default Expands here] end

	/**
	 * The save method that is called from the admin CRUD components.
	 */
	// % protected region % [Customize Save From Crud here] off begin
	public async saveFromCrud(formMode: EntityFormMode) {
		const relationPath = {
			achievementss: {},
			assessmentss: {},
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
							'achievementss',
							'assessmentss',
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
		return Models.StudentsSubmissionEntity;
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
CRUD(createdAttr)(StudentsEntity.prototype, 'created');
CRUD(modifiedAttr)(StudentsEntity.prototype, 'modified');
// % protected region % [Modify the create and modified CRUD attributes here] end
