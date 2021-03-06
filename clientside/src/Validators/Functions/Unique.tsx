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
import { Symbols } from 'Symbols';
import { initValidators, IModelAttributeValidationError, ErrorType } from '../Util';
import { Model } from 'Models/Model';
import { getFetchAllQuery } from 'Util/EntityUtils';
import { store } from 'Models/Store';
import { modelName as modelNameSymbol } from 'Symbols';
import { lowerCaseFirst } from 'Util/StringUtils';
// % protected region % [Add extra imports and exports here] off begin
// % protected region % [Add extra imports and exports here] end

// % protected region % [Override validate here] off begin
export default function validate() {
	return (target: object, key: string) => {
		initValidators(target, key);
		target[Symbols.validatorMap][key].push('Unique');
		target[Symbols.validator].push(
			(model: Model): Promise<IModelAttributeValidationError | null> => new Promise((resolve) => {
				if(model[key] === null || model[key] === undefined){
					resolve(null);
					return;
				}
				const modelName = lowerCaseFirst(target.constructor[modelNameSymbol]);
				const query = getFetchAllQuery(target.constructor as { new(): Model });
				const variables = { "args": [{ "path": key, "comparison": "equal", "value": model[key] }] };
				store.apolloClient.query({ query: query, fetchPolicy: 'network-only', variables: variables }).then(
					({data}) => {
						if (!!data[`${modelName}s`] && !!data[`${modelName}s`][0] && !!data[`${modelName}s`][0]['id'] && model['id'] !== data[`${modelName}s`][0]['id']) {
							const errorMessage = `This value entered has already been used`;
							resolve({
								errorType: ErrorType.EXISTS,
								errorMessage, 
								attributeName: key, 
								target: model });
						}
						resolve(null);
					},
					() => {
						// if error happens to asynchronous validation, just consider this as valid for now
						resolve(null);
					}
				);
			})
		);
	};
}
// % protected region % [Override validate here] end