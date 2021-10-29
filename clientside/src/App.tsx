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
import Cookies from 'js-cookie';
import Admin from './Views/Admin';
import Frontend from './Views/Frontend';
import { Route, Router, Switch } from 'react-router';
import { Provider } from 'mobx-react';
import { store, StoreContext } from 'Models/Store';
import { ApolloProvider, ApolloClient, InMemoryCache, HttpLink, from, ApolloLink } from '@apollo/client';
import { SERVER_URL } from 'Constants';
import { isServerError } from 'Util/GraphQLUtils';
import { configure } from 'mobx';
import { createBrowserHistory } from 'history';
import { ToastContainer } from 'react-toastify';
import GlobalModal from './Views/Components/Modal/GlobalModal';
import { configureAuthenticator2fa } from 'Services/TwoFactor/Authenticator';
import { TwoFactorContext, TwoFactorMethods } from 'Services/TwoFactor/Common';
import { configureEmail2fa } from 'Services/TwoFactor/Email';
import { onError } from '@apollo/client/link/error';
import { logout } from 'Util/NavigationUtils';
// % protected region % [Add extra page imports here] off begin
// % protected region % [Add extra page imports here] end

export default class App extends React.Component {
	twoFactorMethods: TwoFactorMethods = {};

	constructor(props: any, context: any) {
		super(props, context);
		store.routerHistory = createBrowserHistory();

		store.apolloClient = new ApolloClient({
			link: from([
				this.apolloXsrfLink,
				this.apolloErrorLink,
				new HttpLink({ uri: `${SERVER_URL}/api/graphql` }),
			]),
			cache: new InMemoryCache(),
		});

		// All state changes should be run in an action so enforce that
		configure({ enforceActions: 'observed' });

		// For cross platform mobile responsiveness
		const defineViewportHeight = () => {
			let vh = window.innerHeight * 0.01;
			document.documentElement.style.setProperty('--vh', `${vh}px`);
		};
		defineViewportHeight();
		window.addEventListener('resize', () => defineViewportHeight());

		// % protected region % [Customise default two factor methods here] off begin
		configureAuthenticator2fa(this.twoFactorMethods);
		configureEmail2fa(this.twoFactorMethods);
		// % protected region % [Customise default two factor methods here] end

		// % protected region % [Add extra constructor logic here] off begin
		// % protected region % [Add extra constructor logic here] end

	};

	public render() {
		// % protected region % [Override render here] off begin
		return (
			<ApolloProvider client={store.apolloClient}>
				<TwoFactorContext.Provider value={this.twoFactorMethods}>
					<StoreContext.Provider value={store}>
						<Provider store={store}>
							<>
								<Router history={store.routerHistory}>
									<>
										<ToastContainer className="frontend" />
									</>
									<Switch>
										<Route path="/admin" component={Admin} />
										<Route path="/" component={Frontend} />
									</Switch>
								</Router>
								<GlobalModal ref={ref => ref ? store.modal = ref : undefined} />
							</>
						</Provider>
					</StoreContext.Provider>
				</TwoFactorContext.Provider>
			</ApolloProvider>
		);
		// % protected region % [Override render here] end
	};

	/**
	 * Request handler for the apollo client
	 */
	private apolloXsrfLink = new ApolloLink((operation, next) => {
		operation.setContext({
			headers: {
				'X-XSRF-TOKEN': Cookies.get('XSRF-TOKEN'),
			},
		});
		return next(operation);
	});

	/**
	 * Error Handler for the apollo client
	 */
	private apolloErrorLink = onError(({ networkError }) => {
		if (isServerError(networkError) && networkError.statusCode === 401) {
			logout(store.routerHistory.location.pathname);
		}
	});

	// % protected region % [Add extra methods here] off begin
	// % protected region % [Add extra methods here] end
}
