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
import _ from 'lodash';
import { ApolloError, ApolloQueryResult, OperationVariables } from '@apollo/client';
import Collection, { ICollectionListProps } from '../Collection/Collection';
import { Model } from 'Models/Model';
import { action, observable, runInAction } from 'mobx';
import { observer } from 'mobx-react';
import { Button, ICbButtonProps } from '../Button/Button';
import { lowerCaseFirst } from 'Util/StringUtils';
import { DocumentNode } from 'graphql';
import Spinner from '../Spinner/Spinner';
import { ICollectionHeaderProps } from '../Collection/CollectionHeaders';
import ModelQuery, { IWhereCondition } from './ModelQuery';
import ModelAPIQuery, { ApiQueryParams } from './ModelAPIQuery';
import { ICollectionFilterPanelProps, IFilter } from '../Collection/CollectionFilterPanel';
import { isOrCondition } from 'Util/GraphQLUtils';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

type refetchFunc<TData> = (variables?: OperationVariables) => Promise<ApolloQueryResult<TData>>;

export interface IModelCollectionProps<T extends Model> extends ICollectionListProps<T> {
	model: { new(json?: {}): T };
	perPage?: number;
	conditions?: Array<IWhereCondition<T>> | Array<Array<IWhereCondition<T>>>;
	getMoreParams?: (filters?: Array<IFilter<T>>, filterApplied?: boolean) => ApiQueryParams;
	ids?: string[];
	orderBy?: IOrderByCondition<T>;
	customQuery?: DocumentNode;
	hidePagination?: boolean;
	customSpinner?: JSX.Element;
	customButtons?: Array<{
		label: string,
		className?: string,
		onClick?: (models: T[]) => void | Promise<void>,
		buttonProps?: ICbButtonProps,
	}>;
	isApiQuery?: boolean;
	url?: string;
	searchStr?: string;
	filters?: Array<IFilter<T>>;
	// % protected region % [Add any extra ICollectionListProps here] off begin
	// % protected region % [Add any extra ICollectionListProps here] end
}

// TODO: Remove this definition of this interface - was causing errors on build time without it
interface IOrderByCondition<T> {
	path: string;
	descending?: boolean;
}

/**
 * Collection Component that displays a list of models in a collection with support for pagination
 */
@observer
export class ModelCollection<T extends Model> extends React.Component<IModelCollectionProps<T>> {
	// @observable
	private models: T[];

	@observable
	private filterConfig: ICollectionFilterPanelProps<T>;

	@observable
	private filterApplied: boolean = false;

	@observable
	private filters: Array<IFilter<T>> = [];

	@observable
	private orderBy: IOrderByCondition<T> | undefined;

	@observable
	private searchStr?: string;

	@observable
	private pageNo = 0;

	private get perPage() {
		return this.props.perPage ?? 20;
	}

	public refetch: refetchFunc<T> | (() => void) = () => undefined;

	// % protected region % [Override constructor here] off begin
	constructor(props: IModelCollectionProps<T>, context: any) {
		super(props, context);
		// Order by defaults to the prop
		runInAction(() => {
			if (props.orderBy) {
				this.orderBy = props.orderBy;
			}
		});
		this.filterConfig = {
			filters: this.getFilters(),
			onClearFilter: this.onClearFilter,
			onApplyFilter: this.onApplyFilter,
			onFilterChanged: this.onFilterChanged
		}
	}
	// % protected region % [Override constructor here] end

	public render() {
		const model = new this.props.model();

		const headers: Array<ICollectionHeaderProps<T>> = this.props.headers.map(header => {
			if (!header.transformItem) {
				header = {
					...header,
					sortable: true,
					sortClicked: () => {
						if (this.orderBy && this.orderBy.path === header.name) {
							if (this.orderBy.descending) {
								const descending = !this.orderBy.descending;
								runInAction(() => this.orderBy = { path: header.name, descending });
							} else if (!this.orderBy.descending) {
								runInAction(() => this.orderBy = undefined);
							}
							return this.orderBy;
						} else {
							runInAction(() => this.orderBy = { path: header.name, descending: true });
							return this.orderBy;
						}
					},
				};
			}

			if (!header.sortClicked) {
				header = {
					...header,
					sortClicked: () => {
						if (this.orderBy && this.orderBy.path === header.name) {
							if (this.orderBy.descending) {
								const descending = !this.orderBy.descending;
								runInAction(() => this.orderBy = { path: header.name, descending });
							} else if (!this.orderBy.descending) {
								runInAction(() => this.orderBy = undefined);
							}
							return this.orderBy;
						} else {
							runInAction(() => this.orderBy = { path: header.name, descending: true });
							return this.orderBy;
						}
					},
				};
			}

			return header;
		});

		const { customQuery, ids, customSpinner, model: modelConstruct, url } = this.props;

		let conditions = this.props.conditions;

		let filterConditions = undefined;
		if (this.filterApplied) {
			filterConditions = new Model().getFilterConditions(this.filterConfig);
		}

		if (filterConditions && !!filterConditions.length) {
			if (conditions === undefined && filterConditions === undefined) {
				conditions = undefined;
			}
			if (isOrCondition(conditions)) {
				conditions = [...conditions, ...filterConditions.map(x => {
					if (Array.isArray(x)) {
						return x;
					} else {
						return [x];
					}
				})];
			} else {
				conditions = [
					...(conditions ? conditions.map(x => [x]) : []),
					...filterConditions.map(x => {
						if (Array.isArray(x)) {
							return x;
						} else {
							return [x];
						}
					})]
				;
			}
		}

		// % protected region % [Override render collection here] off begin
		const renderCollection = (loading: boolean, data?: any, error?: ApolloError | string) => {
			if (error) {
				return <h2>An unexpected error occurred:</h2>;
			}

			this.models = [];

			const modelName = model.getModelName();
			if (!!data && data[lowerCaseFirst(modelName) + 's']) {
				this.models = data[lowerCaseFirst(modelName) + 's'].map((e: any) => new this.props.model(e));
			}

			let totalRecords = 0;

			const countName = `count${modelName}s`;
			if (!!data && data[countName]) {
				// Extract pagination details and update total number
				totalRecords = (data[countName]['number']);
			}

			return (
				<>
					{loading && <Spinner/>}
					{this.props.customButtons && this.props.customButtons.map((button, i) => {
						const onClick = () => {
							if (button.onClick) {
								button.onClick(this.models);
							}
						};
						return <Button
							key={i}
							className={button.className}
							onClick={onClick}
							{...button.buttonProps}>
							{button.label}
						</Button>;
					})}
					<Collection
						{...this.props}
						headers={headers}
						collection={this.models}
						orderBy={this.orderBy}
						hidePagination={this.props.hidePagination}
						pageNo={this.pageNo}
						totalRecords={totalRecords}
						perPage={this.perPage}
						onPageChange={this.onPageChange}
						menuFilterConfig={this.filterConfig}
					/>
				</>
			);
		}
		// % protected region % [Override render collection here] end

		if (this.props.isApiQuery) {
			return (
				<ModelAPIQuery
					url={url || ''}
					moreParams={this.props.getMoreParams ? this.props.getMoreParams(this.filterConfig.filters, this.filterApplied) : undefined}
					ids={ids}
					searchStr={this.props.searchStr}
					pageNo={this.pageNo}
					perPage={this.perPage}
					model={modelConstruct}
					orderBy={this.orderBy}>
					{({ loading, success, error, data, refetch }) => {
						this.refetch = refetch;
						return renderCollection(loading, data, error);
					}}
				</ModelAPIQuery>
			);
		} else {
			return (
				<ModelQuery
					conditions={conditions}
					ids={ids}
					page={this.pageNo}
					perPage={this.perPage}
					customQuery={customQuery}
					model={modelConstruct}
					orderBy={this.orderBy}>
					{({ loading, error, data, refetch }) => {
						this.refetch = refetch;
						return renderCollection(loading, data, error);
					}}
				</ModelQuery>
			);
		}

	}

	// % protected region % [Override getFilters method here] off begin
	protected getFilters = (): Array<IFilter<T>> => {
		return [..._.cloneDeep(this.props.filters) || []];
	}
	// % protected region % [Override getFilters method here] end

	// % protected region % [Override onClearFilter method here] off begin
	@action
	protected onClearFilter = () => {
		this.filterConfig.filters = this.getFilters();
		this.filterApplied = false;
	};
	// % protected region % [Override onClearFilter method here] end

	// % protected region % [Override onApplyFilter method here] off begin
	@action
	protected onApplyFilter = () => {
		this.filterApplied = true;
	};
	// % protected region % [Override onApplyFilter method here] end

	// % protected region % [Override onFilterChanged method here] off begin
	@action
	protected onFilterChanged = () => {
		this.filterApplied = false;
	};
	// % protected region % [Override onFilterChanged method here] end

	// % protected region % [Override onPageChange method here] off begin
	@action
	protected onPageChange = (pageNo: number) => {
		this.pageNo = pageNo;
	}
	// % protected region % [Override onPageChange method here] end
}