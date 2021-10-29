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
import { action, observable } from 'mobx';
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
import { VisitorsStaffEntity } from 'Models/Security/Acl/VisitorsStaffEntity';
import { StaffStaffEntity } from 'Models/Security/Acl/StaffStaffEntity';
import * as Enums from '../Enums';
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import {SuperAdministratorScheme} from '../Security/Acl/SuperAdministratorScheme';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

export interface IStaffEntityAttributes extends IModelAttributes {
	email: string;
	firstName: string;
	lastName: string;
	role: Enums.staffRoles;
	contactNumber: string;

	assessmentNotess: Array<Models.AssessmentNotesEntity | Models.IAssessmentNotesEntityAttributes>;
	addressId?: string;
	address?: Models.AddressEntity | Models.IAddressEntityAttributes;
	// % protected region % [Add any custom attributes to the interface here] off begin
	// % protected region % [Add any custom attributes to the interface here] end
}

// % protected region % [Customise your entity metadata here] off begin
@entity('StaffEntity', 'Staff')
// % protected region % [Customise your entity metadata here] end
export default class StaffEntity extends Model implements IStaffEntityAttributes {
	public static acls: IAcl[] = [
		new SuperAdministratorScheme(),
		new VisitorsStaffEntity(),
		new StaffStaffEntity(),
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
		'email',
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

	// % protected region % [Modify props to the crud options here for attribute 'Email'] off begin
	@Validators.Email()
	@Validators.Required()
	@observable
	@attribute()
	@CRUD({
		name: 'Email',
		displayType: 'displayfield',
		order: 10,
		createFieldType: 'textfield',
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public email: string;
	// % protected region % [Modify props to the crud options here for attribute 'Email'] end

	// % protected region % [Modify props to the crud options here for attribute 'Password'] off begin
	@Validators.Length(6)
	@observable
	@CRUD({
		name: 'Password',
		displayType: 'hidden',
		order: 20,
		createFieldType: 'password',
	})
	public password: string;
	// % protected region % [Modify props to the crud options here for attribute 'Password'] end

	// % protected region % [Modify props to the crud options here for attribute 'Confirm Password'] off begin
	@Validators.Custom('Password Match', (e: string, target: StaffEntity) => {
		return new Promise(resolve => resolve(target.password !== e ? "Password fields do not match" : null))
	})
	@observable
	@CRUD({
		name: 'Confirm Password',
		displayType: 'hidden',
		order: 30,
		createFieldType: 'password',
	})
	public _confirmPassword: string;
	// % protected region % [Modify props to the crud options here for attribute 'Confirm Password'] end

	// % protected region % [Modify props to the crud options here for attribute 'First Name'] off begin
	@Validators.Required()
	@observable
	@attribute()
	@CRUD({
		name: 'First Name',
		displayType: 'textfield',
		order: 40,
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
		order: 50,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public lastName: string;
	// % protected region % [Modify props to the crud options here for attribute 'Last Name'] end

	// % protected region % [Modify props to the crud options here for attribute 'Role'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Role',
		displayType: 'enum-combobox',
		order: 60,
		headerColumn: true,
		searchable: true,
		searchFunction: 'equal',
		searchTransform: (attr: string) => {
			return AttrUtils.standardiseEnum(attr, Enums.staffRolesOptions);
		},
		enumResolveFunction: makeEnumFetchFunction(Enums.staffRolesOptions),
		displayFunction: (attribute: Enums.staffRoles) => Enums.staffRolesOptions[attribute],
	})
	public role: Enums.staffRoles;
	// % protected region % [Modify props to the crud options here for attribute 'Role'] end

	// % protected region % [Modify props to the crud options here for attribute 'Contact Number'] off begin
	@observable
	@attribute()
	@CRUD({
		name: 'Contact Number',
		displayType: 'textfield',
		order: 70,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
	})
	public contactNumber: string;
	// % protected region % [Modify props to the crud options here for attribute 'Contact Number'] end

	@observable
	@attribute({isReference: true})
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Assessment Notes'] off begin
		name: "Assessment Notess",
		displayType: 'reference-multicombobox',
		order: 80,
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
		// % protected region % [Modify props to the crud options here for reference 'Address'] off begin
		name: 'Address',
		displayType: 'reference-combobox',
		order: 90,
		referenceTypeFunc: () => Models.AddressEntity,
		// % protected region % [Modify props to the crud options here for reference 'Address'] end
	})
	public addressId?: string;
	@observable
	@attribute({isReference: true})
	public address: Models.AddressEntity;

	// % protected region % [Add any custom attributes to the model here] off begin
	// % protected region % [Add any custom attributes to the model here] end

	// eslint-disable-next-line @typescript-eslint/no-useless-constructor
	constructor(attributes?: Partial<IStaffEntityAttributes>) {
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
	public assignAttributes(attributes?: Partial<IStaffEntityAttributes>) {
		// % protected region % [Override assign attributes here] off begin
		super.assignAttributes(attributes);

		if (attributes) {
			if (attributes.email !== undefined) {
				this.email = attributes.email;
			}
			if (attributes.firstName !== undefined) {
				this.firstName = attributes.firstName;
			}
			if (attributes.lastName !== undefined) {
				this.lastName = attributes.lastName;
			}
			if (attributes.role !== undefined) {
				this.role = attributes.role;
			}
			if (attributes.contactNumber !== undefined) {
				this.contactNumber = attributes.contactNumber;
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
		address {
			${Models.AddressEntity.getAttributes().join('\n')}
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
		};

		if (formMode === 'create') {
			relationPath['password'] = {};

			if (this.password !== this._confirmPassword) {
				throw Error("Password fields do not match");
			}
		}
		return this.save(
			relationPath,
			{
				graphQlInputType: formMode === 'create'
					? `[${this.getModelName()}CreateInput]`
					: `[${this.getModelName()}Input]`,
				options: [
					{
						key: 'mergeReferences',
						graphQlType: '[String]',
						value: [
							'assessmentNotess',
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
		return this.email;
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
CRUD(createdAttr)(StaffEntity.prototype, 'created');
CRUD(modifiedAttr)(StaffEntity.prototype, 'modified');
// % protected region % [Modify the create and modified CRUD attributes here] end
