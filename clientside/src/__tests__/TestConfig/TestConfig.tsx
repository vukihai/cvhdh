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
import { store } from 'Models/Store';
import { SERVER_URL } from 'Constants';
import { ApolloClient, ApolloQueryResult, InMemoryCache, NetworkStatus, QueryOptions } from '@apollo/client';
// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

// % protected region % [Customise config here] off begin
export const pollingInterval = 100;
export const pollingTimeout = 1000;

let storedGraphqlResponses: {[key: string]: {}} = {};
// % protected region % [Customise config here] end

// % protected region % [Customise setupGraphQlMocking here] off begin
export function setupGraphQlMocking(graphqlResponses: {[key: string]: {}}) {
	storedGraphqlResponses = {...storedGraphqlResponses, ...graphqlResponses};

	store.apolloClient = new ApolloClient({
		uri: `${SERVER_URL}/api/graphql`,
		cache: new InMemoryCache(),
	});

	store.apolloClient.query = (options: QueryOptions): Promise<ApolloQueryResult<any>> => {
		return new Promise<ApolloQueryResult<any>>((resolve, reject) => {
			const queryName = options.query.definitions[0]?.['name']?.['value'];
			if (!queryName){
				return reject('Query name undefined');
			}
			const graphqlResponse = storedGraphqlResponses[queryName];
			if (!graphqlResponse){
				return reject('Mocked response is undefined');
			}
			return resolve({
				data: graphqlResponse,
				loading: false,
				networkStatus: NetworkStatus.ready
			})
		})
	};
}
// % protected region % [Customise setupGraphQlMocking here] end

// % protected region % [Customise placeholder test here] off begin
// Add placeholder test so yarn test doesn't throw empty test file exception
describe('Place Holder', function () {
	it('placeholder', () => {
		expect(1).toEqual(1);
	})
});
// % protected region % [Customise placeholder test here] end

// % protected region % [Add extra methods here] off begin
// % protected region % [Add extra methods here] end
