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
import { IconPositions } from '../Helpers/Common';
import { observer } from 'mobx-react';
import {action, computed, observable} from 'mobx';
import If from '../If/If';
import CollectionMenu from './CollectionMenu';
import Pagination, { CommonPaginationOptions } from '../Pagination/Pagination';
import { ReactNode } from 'react';
import CollectionRow from './CollectionRow';
import CollectionHeaders, { ICollectionHeaderProps, ICollectionHeaderPropsPrivate } from './CollectionHeaders';
import * as uuid from 'uuid';
import classNames from 'classnames';
import { union, intersectionWith, isEqual, pullAllWith } from 'lodash';
import { IEntityContextMenuActions } from '../EntityContextMenu/EntityContextMenu';
import { ICollectionFilterPanelProps } from './CollectionFilterPanel';
import { IOrderByCondition } from '../ModelCollection/ModelQuery';

// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export type actionFn<T> = (model: T, event: React.MouseEvent<Element, MouseEvent>) => void;
export type bulkActionFn<T> = (models: T[], event: React.MouseEvent<Element, MouseEvent>) => void;
export type expandFn<T> = (model: T) => React.ReactNode | string;
export type showExpandFn<T> = (model: T) => boolean;
export type actionFilterFn<T> = (item: T) => Array<ICollectionItemActionProps<T>>;

export interface ICollectionActionProps<T> {
	/** A label for the action button */
	label: string;
	/** A class for the action button */
	buttonClass?: string;
	/** An icon class for the action button */
	icon?: string;
	/** The position of the icon */
	iconPos?: IconPositions;
	/** Should the icon be displayed */
	showIcon?: boolean;
	/** Should the label be displayed*/
	showLabel?: boolean;
	/** Is the action an additional action, this is displayed in the more menu */
	isAdditional?: boolean;
	/** 
	 * A custom button that is displayed, instead of the one provided by the collection 
	 * Using this will override any and all presentation level options also provided
	 */
	customButton?: (model: T) => React.ReactNode;
	// % protected region % [Add any extra ICollectionActionProps fields here] off begin
	// % protected region % [Add any extra ICollectionActionProps fields here] end
}

export interface ICollectionItemActionProps<T> extends ICollectionActionProps<T> {
	/** The callback function for the action */
	action: actionFn<T>;
}

export interface ICollectionBulkActionProps<T> extends ICollectionActionProps<T> {
	/** The callback function for the action */
	bulkAction: bulkActionFn<T>;
}

export interface ICollectionListProps<T> {
	/** The headers to provide to the collection, this defines the columns that are displayed */
	headers: Array<ICollectionHeaderProps<T>>;
	/** The actions that are displayed for each item on the collection */
	actions?: Array<ICollectionItemActionProps<T>> | actionFilterFn<T>;
	/** More actions that will be put into a context menu with will be triggered by clicking a button on the each row */
	actionsMore?: IEntityContextMenuActions<T>;
	/** The actions that are defined  */
	selectedBulkActions?: Array<ICollectionBulkActionProps<T>>;
	/**
	 * A function that takes a row and returns a component to render in the expand section
	 * If this property exists an expand button is rendered on every row
	 */
	expandList?: expandFn<T>;
	/**
	 * A function that takes a row and returns a boolean, to determine whether to show the expand button
	 */
	showExpandButton?: showExpandFn<T>;
	/** A class name for the collection */
	className?: string;
	/** Pass through for any props to pass to the top level section */
	innerProps?: React.DetailedHTMLProps<React.HTMLAttributes<HTMLElement>, HTMLElement>;
	/** Should items be selectable, if this is true then checkboxes are rendered for each row */
	selectableItems?: boolean;
	/** Should the column headers be hidden */
	hideHeaders?: boolean;
	/** Callback for the search textbox */
	onSearchTriggered?: (searchTerm: string) => void;
	/** Any additional actions that should be displayed in the collection menu */
	additionalActions?: ReactNode[];
	/** A filter function for the data displayed on the collection */
	filter?: (model: T) => boolean;
	/** menu filter config */
	menuFilterConfig?: ICollectionFilterPanelProps<T>;
	/** Callback when the selected checkboxes change */
	itemSelectionChanged?: (checked: boolean, changedItems: T[]) => T[];
	/** Callback for when the select items on all pages button is pressed */
	onCheckedAllPages?: (checked: boolean)=> T[]
	/** Callback for when the deselect items on all pages button is pressed */
	cancelAllSelection?: () => void;
	/** A function to display the total number of selected items, will default to only showing this on this page */
	menuCountFunction?: (selectedItems: T[]) => number;
	/** A function get the selected items on the collection, this overwrite the default selection logic */
	getSelectedItems?: () => T[];
	/**
	 * A on the models that uniquely identifies the model.
	 * If provided will display in the `data-id` property in the dom
	 */
	idColumn?: string;
	/** The data attributs to be inserted into collection row <tr> tag */
	dataFields?: (row: T) => {[key: string]: string};
	/** The default order by condition */
	orderBy?: IOrderByCondition<T> | undefined;
	// % protected region % [Add any extra ICollectionListProps fields here] off begin
	// % protected region % [Add any extra ICollectionListProps fields here] end
}

export interface ICollectionProps<T> extends ICollectionListProps<T>, CommonPaginationOptions {
	/** The collection to display */
	collection: T[];
	/** Should the pagination be hidden */
	hidePagination?: boolean;
	filterOrientationRow?: boolean;
	// % protected region % [Add any extra ICollectionProps fields here] off begin
	// % protected region % [Add any extra ICollectionProps fields here] end
}

/**
 * Displays a collection of items.
 * 
 * By default this collection will handle its state internally. If the pageNo prop is given then the pagination will
 * become controlled and must be handled externally.
 */
@observer
export default class Collection<T> extends React.Component<ICollectionProps<T>> {
	/**
	 * Is the pagination currently controlled. 
	 */
	get isPaginationControlled() {
		return this.props.pageNo !== undefined;
	}

	/**
	 * The currently selected items on the page
	 */
	@observable
	private _selectedItems: T[] = [];
	@computed
	get selectedItems() {
		if (this.props.getSelectedItems) {
			return this.props.getSelectedItems();
		}
		return this._selectedItems;
	}

	/**
	 * The current page number. The first page is page 0.
	 */
	@observable
	private _pageNo: number = 0;
	@computed
	get pageNo() {
		return this.props.pageNo ?? this._pageNo;
	}
	set pageNo(value) {
		// Providing a page number makes this a managed component, so call the appropriate function on the props
		// Otherwise fallback to unmanaged functionality
		if (this.props.pageNo !== undefined) {
			this.props.onPageChange?.(value);
		} else {
			this._pageNo = value;
		}
	}

	/**
	 * The number of items per page
	 */
	get perPage() {
		return this.props.perPage ?? 10;
	}

	/**
	 * The headers of the collection
	 */
	@computed
	get headers(): Array<ICollectionHeaderPropsPrivate<T>> {
		return this.props.headers.map(header => {
			const computedHeader: ICollectionHeaderPropsPrivate<T> = {...header};

			if (typeof header.displayName === 'string') {
				computedHeader.headerName = header.displayName;
			} else if (typeof header.displayName === 'function') {
				computedHeader.headerName = header.displayName(header.name);
			}

			return computedHeader;
		});
	}

	/**
	 * The data to display on the current page.
	 */
	@computed
	get pageData(): T[] {
		const { collection, filter } = this.props;

		let data = collection;
		if (filter) {
			data = collection.filter(filter);
		}

		// If we are an uncontrolled component then handle pagination internally
		if (!this.isPaginationControlled) {
			const pageStartIdx = this.pageNo * this.perPage;
			data = data.slice(pageStartIdx, pageStartIdx + this.perPage);
		}

		return data;
	}

	/**
	 * Should the all items checkbox be selected
	 */
	@computed
	get allChecked() {
		// If the component is uncontrolled then this needs to handle the internal pagination.
		if (!this.isPaginationControlled) {
			const selectedPageItems = intersectionWith(this.selectedItems, this.pageData, isEqual);
			return selectedPageItems.length === this.pageData.length;
		}

		// If the component is controlled then pagination is handled externally. This means that for the all box to be
		// checked, all items in the provided collection are selected.
		const selectedItems = intersectionWith(this.selectedItems, this.props.collection, isEqual);
		return (selectedItems.length === this.props.collection.length) && (this.props.collection.length > 0);
	}

	/**
	 * The total number of items that exist over all pages
	 */
	@computed
	get totalItems() {
		return this.props.totalRecords ?? this.props.collection.length;
	}

	/**
	 * Gets the number of items that have been checked
	 */
	@computed
	get selectedItemsCount() {
		if (this.props.menuCountFunction) {
			return this.props.menuCountFunction(this.selectedItems);
		}
		return this.selectedItems.length;
	}

	/**
	 * Should the select all items on all pages button be shown
	 */
	@computed
	get showSelectAll() {
		return this.allChecked && this.totalItems > this.selectedItemsCount;
	};

	// % protected region % [Override render method of Collection here] off begin
	public render() {
		return (
			<section
				className={classNames('collection-component', this.props.className)}
				data-selected-count={this.selectedItemsCount}
				data-total={this.totalItems}
				{...this.props.innerProps}>
				<CollectionMenu
					selectedItems={this.selectedItems}
					search={!!this.props.onSearchTriggered}
					filterConfig={this.props.menuFilterConfig}
					onSearchTriggered={this.props.onSearchTriggered}
					additionalActions={this.props.additionalActions}
					cancelAllSelection={this.cancelAllSelection}
					showSelectAll={this.showSelectAll}
					onSelectAll={this.checkAllPages}
					totalSelectedItems={this.selectedItemsCount}
					selectedBulkActions={this.props.selectedBulkActions}
					filterOrientationRow={this.props.filterOrientationRow}
					totalRecords={this.totalItems}
				/>
				{this.list()}
				{this.pagination()}
			</section>
		);
	}
	// % protected region % [Override render method of Collection here] end


	/**
	 * The collection pagination component
	 */
	private pagination = () => {
		const { hidePagination, showGoToPageBox } = this.props;
		if (hidePagination) {
			return null;
		}

		return (
			<section className="collection__load">
				<Pagination
					onPageChange={this.onPageChange}
					perPage={this.perPage}
					pageNo={this.pageNo}
					totalRecords={this.totalItems}
					showGoToPageBox={showGoToPageBox ?? false}
				/>
			</section>
		);
	}

	/**
	 * The table list component
	 */
	private list = () => {
		const collectionId = uuid.v4();
		// % protected region % [change any classes for all collectionlists] off begin
		const expandClassName = this.props.expandList ? 'collection__list--expandable' : null;
		const className = classNames('collection__list', expandClassName);
		// % protected region % [change any classes for all collectionlists] end
		return (
			<section aria-label="collection list" className={className}>
				<table>
					{this.header()}
					<tbody>
						{this.row({ id: collectionId })}
					</tbody>
				</table>
			</section>
		);
	}

	// % protected region % [Override method row of Collection here] off begin
	/**
	 * The table row component
	 * @param props Contains the id of the row
	 */
	private row = (props: {id: string}) => {
		return (
			<>
				{this.pageData.map((item, idx) => {
					return (
						<CollectionRow
							selectableItems={this.props.selectableItems}
							item={item}
							headers={this.headers}
							actions={this.props.actions}
							actionsMore={this.props.actionsMore}
							checked={this.selectedItems.some(i => isEqual(i, item))}
							onChecked={this.onRowChecked}
							expandAction={this.props.expandList}
							showExpandButton={this.props.showExpandButton}
							key={this.props.idColumn ? item[this.props.idColumn] : `${idx}-${props.id}`}
							keyValue={this.props.idColumn ? item[this.props.idColumn] : `${idx}-${props.id}`}
							idColumn={this.props.idColumn}
							dataFields={this.props.dataFields}
						/>
					);
				})}
			</>
		);
	}
	// % protected region % [Override method row of Collection here] end

	// % protected region % [Override method header of Collection here] off begin
	/**
	 * The header row component
	 */
	private header = () => {
		return (
			<If condition={!this.props.hideHeaders}>
				<CollectionHeaders<T>
					headers={this.headers}
					selectableItems={this.props.selectableItems}
					actions={this.props.actions}
					allChecked={this.allChecked}
					onCheckedAll={this.onCheckedAll}
					orderBy={this.props.orderBy} />
			</If>
		);
	}
	// % protected region % [Override method header of Collection here] end

	/**
	 * Selects or deselects items from the collection
	 * @param checked Weather to select or deselect the items
	 * @param items The items to change
	 */
	@action
	selectItems = (checked: boolean, items: T[]) => {
		if (this.props.itemSelectionChanged) {
			this.props.itemSelectionChanged(checked, items);
		} else {
			if (checked) {
				this._selectedItems = union(this._selectedItems, items);
			} else {
				pullAllWith(this._selectedItems, items, isEqual)
			}
		}

	}

	/**
	 * The callback for when a row is checked
	 * @param event The checkbox change event
	 * @param checked Weather the checkbox was checked or unchecked
	 * @param checkedItem The item that was checked
	 */
	@action
	onRowChecked = (event: React.ChangeEvent<HTMLInputElement>, checked: boolean, checkedItem: T) => {
		this.selectItems(checked, [checkedItem]);
	}

	/**
	 * Callback when the checkbox in the collection header was changed
	 * @param event The checkbox change event
	 * @param checked Weather the checkbox was checked or unchecked
	 */
	onCheckedAll = (event: React.ChangeEvent<HTMLInputElement>, checked: boolean) => {
		this.selectItems(checked, this.pageData);
	}

	/**
	 * Callback for when the select all pages button was pressed
	 */
	checkAllPages = () => {
		// If the component is managed then all page checks need to be done in the parent component
		if (this.isPaginationControlled) {
			if (this.props.onCheckedAllPages) {
				this.selectItems(true, this.props.onCheckedAllPages(true));
			}
		} else {
			this.selectItems(true, this.props.collection);
		}
	}

	/**
	 * Callback when the cancel selection button was pressed
	 */
	cancelAllSelection = () => {
		this.selectItems(false, this.props.collection);
		if(this.props.cancelAllSelection) {
			this.props.cancelAllSelection();
		}
	}

	/**
	 * Callback for page changes.
	 * @param pageNo The page number to change to.
	 */
	onPageChange = (pageNo: number) => {
		this.pageNo = pageNo;
	}
}