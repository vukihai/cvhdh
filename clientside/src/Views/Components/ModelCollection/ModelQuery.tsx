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
import { DocumentNode } from 'graphql';
import { Model } from 'Models/Model';
import { getFetchAllConditional, getFetchAllQuery } from 'Util/EntityUtils';
import { isOrCondition } from 'Util/GraphQLUtils';
import { QueryResult, useQuery } from '@apollo/client';
import { useObserver } from 'mobx-react';
// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

export type Comparators = 'contains'
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
	| 'startsWith'
	// % protected region % [Add extra comparators here] off begin
	// % protected region % [Add extra comparators here] end
	;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export interface IOrderByCondition<T> {
	path: string;
	descending?: boolean;
	// % protected region % [Add any extra order by condition fields here] off begin
	// % protected region % [Add any extra order by condition fields here] end
}

export type CaseComparison = 'CURRENT_CULTURE'
	| 'CURRENT_CULTURE_IGNORE_CASE'
	| 'INVARIANT_CULTURE'
	| 'INVARIANT_CULTURE_IGNORE_CASE'
	| 'ORDINAL'
	| 'ORDINAL_IGNORE_CASE'
	// % protected region % [Add extra case comparisons here] off begin
	// % protected region % [Add extra case comparisons here] end
	;

export type CaseComparisonPascalCase = 'CurrentCulture'
	| 'CurrentCultureIgnoreCase'
	| 'InvariantCulture'
	| 'InvariantCultureIgnoreCase'
	| 'Ordinal'
	| 'OrdinalIgnoreCase'
	// % protected region % [Add extra pascal case comparators here] off begin
	// % protected region % [Add extra pascal case comparators here] end
	;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
interface BaseWhereCondition<T> {
	path: string;
	comparison: Comparators;
	value: any;
	negate?: boolean;
	// % protected region % [Add any extra base where condition fields here] off begin
	// % protected region % [Add any extra base where condition fields here] end
}

export interface IWhereCondition<T> extends BaseWhereCondition<T> {
	case?: CaseComparison;
	// % protected region % [Add any extra where condition fields here] off begin
	// % protected region % [Add any extra where condition fields here] end
}

export interface IWhereConditionApi<T> extends BaseWhereCondition<T> {
	case?: CaseComparisonPascalCase;
	// % protected region % [Add any extra api where condition fields here] off begin
	// % protected region % [Add any extra api where condition fields here] end
}

export type HasCondition<T> = { path: keyof T, conditions?: IWhereCondition<T>[][], negate?: boolean };

export interface IModelQueryVariables<T> {
	skip?: number;
	take?: number;
	args?: Array<IWhereCondition<T>>;
	orderBy: Array<IOrderByCondition<T>>;
	ids?: string[];
	// % protected region % [Add any extra model query variables fields here] off begin
	// % protected region % [Add any extra model query variables fields here] end
}

export interface IModelQueryProps<T extends Model, TData = any> {
	children: (result: QueryResult<TData>) => React.ReactNode;
	model: {new(json?: {}): T};
	conditions?: Array<IWhereCondition<T>> | Array<Array<IWhereCondition<T>>>;
	ids?: string[];
	has?: HasCondition<T>[][]
	orderBy?: IOrderByCondition<T> | IOrderByCondition<T>[];
	customQuery?: DocumentNode;
	useListExpands?: boolean;
	expandString?: string;
	perPage: number;
	page: number;
	// % protected region % [Add any extra model query props here] off begin
	// % protected region % [Add any extra model query props here] end
}

function ModelQuery<T extends Model,TData = any>(props: IModelQueryProps<T, TData>) {
	// % protected region % [Customize model query here] off begin
	const { customQuery, children, useListExpands, model, conditions, expandString } = props;

	let fetchAllQuery;
	if (isOrCondition(conditions)) {
		fetchAllQuery = getFetchAllConditional(model, expandString, useListExpands);
	} else {
		fetchAllQuery = getFetchAllQuery(model, expandString, useListExpands);
	}
	const result = useQuery(customQuery ?? fetchAllQuery, {
		fetchPolicy: 'network-only',
		notifyOnNetworkStatusChange: true,
		variables: constructVariables(props),
	});

	return useObserver(() => <>{children(result)}</>);
	// % protected region % [Customize model query here] end
}

function constructVariables<T extends Model,TData = any>(props: IModelQueryProps<T, TData>) {
	// % protected region % [Customize construct variables method here] off begin
	const { conditions, ids, orderBy : orderByProp, page, perPage, has } = props;

	let orderBy: IOrderByCondition<T>[];

	if (orderByProp === undefined) {
		orderBy = [{
			path: new props.model().getDisplayAttribute(),
			descending: false
		}];
	} else if (Array.isArray(orderByProp)) {
		orderBy = [...orderByProp];
	} else {
		orderBy = [orderByProp];
	}

	// Also sort by id so any unstable sorting has another unique value to sort against 
	orderBy.push({ path: 'id' });

	return {
		skip: page * perPage,
		take: perPage,
		args: conditions,
		orderBy,
		has,
		ids,
	};
	// % protected region % [Customize construct variables method here] end
}

// % protected region % [Customize default export here] off begin
export default ModelQuery;
// % protected region % [Customize default export here] end