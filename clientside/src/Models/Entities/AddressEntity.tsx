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
import { action, observable, runInAction } from 'mobx';
import { Model, IModelAttributes, attribute, entity } from 'Models/Model';
import * as Models from 'Models/Entities';
import * as Validators from 'Validators';
import { CRUD } from '../CRUDOptions';
import * as AttrUtils from "Util/AttributeUtils";
import { IAcl } from 'Models/Security/IAcl';
import {
	makeFetchOneToManyFunc,
	getCreatedModifiedCrudOptions,
} from 'Util/EntityUtils';
import { VisitorsAddressEntity } from 'Models/Security/Acl/VisitorsAddressEntity';
import { StaffAddressEntity } from 'Models/Security/Acl/StaffAddressEntity';
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import { FormEntityData, FormEntityDataAttributes, getAllVersionsFn, getPublishedVersionFn } from 'Forms/FormEntityData';
import { FormVersion } from 'Forms/FormVersion';
import { fetchFormVersions, fetchPublishedVersion } from 'Forms/Forms';
import {SuperAdministratorScheme} from '../Security/Acl/SuperAdministratorScheme';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

export interface IAddressEntityAttributes extends IModelAttributes, FormEntityDataAttributes {
	name: string;
	unit: string;
	addressLine1: string;
	addressLine2: string;
	suburb: string;
	postcode: number;
	city: string;
	country: string;

	staffss: Array<Models.StaffEntity | Models.IStaffEntityAttributes>;
	studentss: Array<Models.StudentsEntity | Models.IStudentsEntityAttributes>;
	formPages: Array<Models.AddressEntityFormTileEntity | Models.IAddressEntityFormTileEntityAttributes>;
	// % protected region % [Add any custom attributes to the interface here] off begin
	// % protected region % [Add any custom attributes to the interface here] end
}

// % protected region % [Customise your entity metadata here] off begin
@entity('AddressEntity', 'Address')
// % protected region % [Customise your entity metadata here] end
export default class AddressEntity extends Model implements IAddressEntityAttributes, FormEntityData  {
	public static acls: IAcl[] = [
		new SuperAdministratorScheme(),
		new VisitorsAddressEntity(),
		new StaffAddressEntity(),
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

	// % protected region % [Modify props to the crud options here for attribute 'Unit'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Unit',
		displayType: 'textfield',
		order: 20,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public unit: string;
	// % protected region % [Modify props to the crud options here for attribute 'Unit'] end

	// % protected region % [Modify props to the crud options here for attribute 'Address line 1'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Address line 1',
		displayType: 'textfield',
		order: 30,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public addressLine1: string;
	// % protected region % [Modify props to the crud options here for attribute 'Address line 1'] end

	// % protected region % [Modify props to the crud options here for attribute 'Address line 2'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Address line 2',
		displayType: 'textfield',
		order: 40,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public addressLine2: string;
	// % protected region % [Modify props to the crud options here for attribute 'Address line 2'] end

	// % protected region % [Modify props to the crud options here for attribute 'Suburb'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Suburb',
		displayType: 'textfield',
		order: 50,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public suburb: string;
	// % protected region % [Modify props to the crud options here for attribute 'Suburb'] end

	// % protected region % [Modify props to the crud options here for attribute 'Postcode'] off begin
	@Validators.Integer()
	@observable
	@attribute()
	@CRUD({
		name: 'Postcode',
		displayType: 'textfield',
		order: 60,
		headerColumn: true,
		searchable: true,
		searchFunction: 'equal',
		searchTransform: AttrUtils.standardiseInteger,
	})
	public postcode: number;
	// % protected region % [Modify props to the crud options here for attribute 'Postcode'] end

	// % protected region % [Modify props to the crud options here for attribute 'City'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'City',
		displayType: 'textfield',
		order: 70,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public city: string;
	// % protected region % [Modify props to the crud options here for attribute 'City'] end

	// % protected region % [Modify props to the crud options here for attribute 'Country'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Country',
		displayType: 'textfield',
		order: 80,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public country: string;
	// % protected region % [Modify props to the crud options here for attribute 'Country'] end

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
		// % protected region % [Modify props to the crud options here for reference 'Staffs'] off begin
		name: "Staffss",
		displayType: 'reference-multicombobox',
		order: 90,
		referenceTypeFunc: () => Models.StaffEntity,
		referenceResolveFunction: makeFetchOneToManyFunc({
			relationName: 'staffss',
			oppositeEntity: () => Models.StaffEntity,
		}),
		// % protected region % [Modify props to the crud options here for reference 'Staffs'] end
	})
	public staffss: Models.StaffEntity[] = [];

	@observable
	@attribute({isReference: true})
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Students'] off begin
		name: "Studentss",
		displayType: 'reference-multicombobox',
		order: 100,
		referenceTypeFunc: () => Models.StudentsEntity,
		referenceResolveFunction: makeFetchOneToManyFunc({
			relationName: 'studentss',
			oppositeEntity: () => Models.StudentsEntity,
		}),
		// % protected region % [Modify props to the crud options here for reference 'Students'] end
	})
	public studentss: Models.StudentsEntity[] = [];

	@observable
	@attribute({isReference: true})
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Form Page'] off begin
		name: "Form Pages",
		displayType: 'hidden',
		order: 110,
		referenceTypeFunc: () => Models.AddressEntityFormTileEntity,
		disableDefaultOptionRemoval: true,
		referenceResolveFunction: makeFetchOneToManyFunc({
			relationName: 'formPages',
			oppositeEntity: () => Models.AddressEntityFormTileEntity,
		}),
		// % protected region % [Modify props to the crud options here for reference 'Form Page'] end
	})
	public formPages: Models.AddressEntityFormTileEntity[] = [];

	// % protected region % [Add any custom attributes to the model here] off begin
	// % protected region % [Add any custom attributes to the model here] end

	// eslint-disable-next-line @typescript-eslint/no-useless-constructor
	constructor(attributes?: Partial<IAddressEntityAttributes>) {
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
	public assignAttributes(attributes?: Partial<IAddressEntityAttributes>) {
		// % protected region % [Override assign attributes here] off begin
		super.assignAttributes(attributes);

		if (attributes) {
			if (attributes.unit !== undefined) {
				this.unit = attributes.unit;
			}
			if (attributes.addressLine1 !== undefined) {
				this.addressLine1 = attributes.addressLine1;
			}
			if (attributes.addressLine2 !== undefined) {
				this.addressLine2 = attributes.addressLine2;
			}
			if (attributes.suburb !== undefined) {
				this.suburb = attributes.suburb;
			}
			if (attributes.postcode !== undefined) {
				this.postcode = attributes.postcode;
			}
			if (attributes.city !== undefined) {
				this.city = attributes.city;
			}
			if (attributes.country !== undefined) {
				this.country = attributes.country;
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
			if (attributes.staffss !== undefined && Array.isArray(attributes.staffss)) {
				for (const model of attributes.staffss) {
					if (model instanceof Models.StaffEntity) {
						this.staffss.push(model);
					} else {
						this.staffss.push(new Models.StaffEntity(model));
					}
				}
			}
			if (attributes.studentss !== undefined && Array.isArray(attributes.studentss)) {
				for (const model of attributes.studentss) {
					if (model instanceof Models.StudentsEntity) {
						this.studentss.push(model);
					} else {
						this.studentss.push(new Models.StudentsEntity(model));
					}
				}
			}
			if (attributes.formPages !== undefined && Array.isArray(attributes.formPages)) {
				for (const model of attributes.formPages) {
					if (model instanceof Models.AddressEntityFormTileEntity) {
						this.formPages.push(model);
					} else {
						this.formPages.push(new Models.AddressEntityFormTileEntity(model));
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
		staffss {
			${Models.StaffEntity.getAttributes().join('\n')}
		}
		studentss {
			${Models.StudentsEntity.getAttributes().join('\n')}
		}
		formPages {
			${Models.AddressEntityFormTileEntity.getAttributes().join('\n')}
		}
	`;
	// % protected region % [Customize Default Expands here] end

	/**
	 * The save method that is called from the admin CRUD components.
	 */
	// % protected region % [Customize Save From Crud here] off begin
	public async saveFromCrud(formMode: EntityFormMode) {
		const relationPath = {
			staffss: {},
			studentss: {},
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
							'staffss',
							'studentss',
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
		return Models.AddressSubmissionEntity;
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
CRUD(createdAttr)(AddressEntity.prototype, 'created');
CRUD(modifiedAttr)(AddressEntity.prototype, 'modified');
// % protected region % [Modify the create and modified CRUD attributes here] end
