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
import { SERVER_URL } from 'Constants';
import GraphiQL, { Fetcher } from 'graphiql';
import Axios, { AxiosError } from 'axios';
import { FetcherResultPayload } from '@graphiql/toolkit/src/create-fetcher/types';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

const graphiQLFetcher: Fetcher = (graphQLParams, opts) => {
	// % protected region % [Customise GraphiQL fetcher here] off begin
	return Axios.post(`${SERVER_URL}/api/graphql`, graphQLParams, { headers: opts?.headers })
		.then((x: FetcherResultPayload) => x.data)
		.catch((x: AxiosError<FetcherResultPayload>) => x.response?.data);
	// % protected region % [Customise GraphiQL fetcher here] end
}

export default function GraphiQl() {
	// % protected region % [Customise GraphiQL component here] off begin
	return (
		<div className="graphiql-content-container body-content">
			<GraphiQL fetcher={graphiQLFetcher} />
		</div>
	);
	// % protected region % [Customise GraphiQL component here] end
}