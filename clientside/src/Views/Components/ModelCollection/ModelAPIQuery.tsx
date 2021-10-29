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
import React, { useState } from 'react';
import { useAsync } from 'Hooks/useAsync';
import axios from 'axios';
import { Model } from 'Models/Model';
import { SERVER_URL } from 'Constants';
import { lowerCaseFirst } from 'Util/StringUtils';
import { modelName as modelNameSymbol } from 'Symbols';
import { IWhereCondition } from './ModelQuery';

type Comparators =
	'contains'
	| 'endsWith'
	| 'equal'
	| 'greaterThan'
	| 'greaterThanOrEqual'
	| 'in'
	| 'notIn'
	| 'lessThan'
	| 'lessThanOrEqual'
	| 'like'
	| 'notEqual'
	| 'startsWith';

export interface IOrderByCondition<T> {
	path: string;
	descending?: boolean;
}

export interface IModelAPIQueryVariables<T> {
	skip?: number;
	take?: number;
	args?: Array<IWhereCondition<T>>;
	orderBy: Array<IOrderByCondition<T>>;
	ids?: string[];
}

export interface QueryResult {
	data: any;
	error?: string;
	success?: boolean;
	loading: boolean;
	refetch: () => void;
}

export interface ApiQueryParams {
	[key: string]: Date | boolean | string | number
}

export interface IModelAPIQueryProps<T extends Model, TData = any> {
	children: (result: QueryResult) => React.ReactNode;
	model: { new(json?: {}): T };
	conditions?: Array<IWhereCondition<T>> | Array<Array<IWhereCondition<T>>>;
	moreParams?: ApiQueryParams;
	ids?: string[];
	searchStr?: string;
	orderBy?: IOrderByCondition<T>;
	url: string;
	pageNo: number;
	perPage: number;
}

function constructVariables<T extends Model>(
	model: { new(json?: {}): T },
	pageNo: number,
	perPage: number,
	searchStr?: string,
	orderByProp?: IOrderByCondition<T>,
	moreParams?: ApiQueryParams) {
	let orderBy: IOrderByCondition<T> = { path: new model().getDisplayAttribute(), descending: false };
	if (orderByProp) {
		orderBy = orderByProp;
	}

	return {
		pageNo: (!isNaN(pageNo) && pageNo >= 0) ? (pageNo + 1) : undefined,  // matching the backend pagination which starts from page 1
		pageSize: perPage ?? undefined,
		searchStr: searchStr ?? undefined,
		orderBy: orderBy.path ?? undefined,
		descending: orderBy.descending,
		...moreParams
	};
}

function ModelAPIQuery<T extends Model, TData = any>(props: IModelAPIQueryProps<T, TData>) {
	const {
		model,
		pageNo,
		perPage,
		searchStr,
		orderBy,
		moreParams,
		children,
		conditions,
		ids,
	} = props;

	const modelName: string = props.model[modelNameSymbol];
	const lowerModelName = lowerCaseFirst(modelName);
	const url = props.url ?? `${SERVER_URL}/api/${lowerModelName}`;

	const [ refetch, setRefetch ] = useState(0);

	const { type, error, data } = useAsync(() => {
		const variables = constructVariables(
			model,
			pageNo,
			perPage,
			searchStr,
			orderBy,
			moreParams
		);
		return axios.get<{ data: Array<T>, totalCount: number }>(url, { params: variables })
			.then(x => x.data);
	}, [
		children,
		model,
		conditions,
		moreParams,
		ids,
		searchStr,
		orderBy,
		url,
		pageNo,
		perPage,
		refetch,
	]);

	return (
		<>
			{props.children({
				loading: type === 'loading',
				success: type === 'data',
				error: error,
				refetch: () => setRefetch(old => ++old),
				data: {
					[`${lowerCaseFirst(modelName)}s`]: data ? data.data : [],
					[`count${modelName}s`]: { number: data ? data.totalCount : 0 }
				},
			})}
		</>
	)
}

export default ModelAPIQuery;