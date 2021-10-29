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
import * as Models from 'Models/Entities';
import * as Validators from 'Validators';
import { CRUD } from '../CRUDOptions';
import * as AttrUtils from "Util/AttributeUtils";
import { IAcl } from 'Models/Security/IAcl';
import {
	getCreatedModifiedCrudOptions,
} from 'Util/EntityUtils';
import { VisitorsAddressEntity } from 'Models/Security/Acl/VisitorsAddressEntity';
import { StaffAddressEntity } from 'Models/Security/Acl/StaffAddressEntity';
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import { formTiles } from 'Views/Components/CRUD/Attributes/AttributeFormTile';
import {SuperAdministratorScheme} from '../Security/Acl/SuperAdministratorScheme';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

export interface IAddressEntityFormTileEntityAttributes extends IModelAttributes {
	tile: string;

	formId: string;
	form: Models.AddressEntity | Models.IAddressEntityAttributes;
	// % protected region % [Add any custom attributes to the interface here] off begin
	// % protected region % [Add any custom attributes to the interface here] end
}

// % protected region % [Customise your entity metadata here] off begin
@entity('AddressEntityFormTileEntity', 'Address Entity Form Tile')
// % protected region % [Customise your entity metadata here] end
export default class AddressEntityFormTileEntity extends Model implements IAddressEntityFormTileEntityAttributes {
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

	// % protected region % [Modify props to the crud options here for attribute 'Tile'] off begin
	/**
	 * The tile that the form is contained in
	 */
	@Validators.Required()
	@Validators.Unique()
	@observable
	@attribute()
	@CRUD({
		name: 'Tile',
		displayType: 'form-tile',
		order: 10,
		headerColumn: true,
		searchable: true,
		searchFunction: 'like',
		searchTransform: AttrUtils.standardiseString,
		formTileFilterFn: (option) => option.formModel === 'AddressEntity',
		displayFunction: (attribute: string) => {
			const value = formTiles.find(t => t.value === attribute);
			return value ? value.display : '';
		},
	})
	public tile: string;
	// % protected region % [Modify props to the crud options here for attribute 'Tile'] end

	@Validators.Required()
	@observable
	@attribute()
	@CRUD({
		// % protected region % [Modify props to the crud options here for reference 'Form'] off begin
		name: 'Form',
		displayType: 'reference-combobox',
		order: 20,
		referenceTypeFunc: () => Models.AddressEntity,
		// % protected region % [Modify props to the crud options here for reference 'Form'] end
	})
	public formId: string;
	@observable
	@attribute({isReference: true})
	public form: Models.AddressEntity;

	// % protected region % [Add any custom attributes to the model here] off begin
	// % protected region % [Add any custom attributes to the model here] end

	// eslint-disable-next-line @typescript-eslint/no-useless-constructor
	constructor(attributes?: Partial<IAddressEntityFormTileEntityAttributes>) {
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
	public assignAttributes(attributes?: Partial<IAddressEntityFormTileEntityAttributes>) {
		// % protected region % [Override assign attributes here] off begin
		super.assignAttributes(attributes);

		if (attributes) {
			if (attributes.tile !== undefined) {
				this.tile = attributes.tile;
			}
			if (attributes.formId !== undefined) {
				this.formId = attributes.formId;
			}
			if (attributes.form !== undefined) {
				if (attributes.form === null) {
					this.form = attributes.form;
				} else {
					if (attributes.form instanceof Models.AddressEntity) {
						this.form = attributes.form;
						this.formId = attributes.form.id;
					} else {
						this.form = new Models.AddressEntity(attributes.form);
						this.formId = this.form.id;
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
		form {
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
CRUD(createdAttr)(AddressEntityFormTileEntity.prototype, 'created');
CRUD(modifiedAttr)(AddressEntityFormTileEntity.prototype, 'modified');
// % protected region % [Modify the create and modified CRUD attributes here] end
