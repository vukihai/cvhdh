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
import React, { useContext } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Route, Switch } from 'react-router';
import LoginPage from 'Views/Pages/Login/LoginPage';
import { TwoFactorContext } from 'Services/TwoFactor/Common';
import queryString from 'querystring';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export interface LoginRoutesProps extends RouteComponentProps {
	// % protected region % [Add any extra props here] off begin
	// % protected region % [Add any extra props here] end
}

function LoginRoutes(props: LoginRoutesProps) {
	// % protected region % [Customise hooks here] off begin
	const twoFactorMethods = useContext(TwoFactorContext);
	// % protected region % [Customise hooks here] end

	// % protected region % [Customise logic here] off begin
	const path = props.match.path === '/' ? '' : props.match.path;
	const twoFactorMethodKeys = Object.keys(twoFactorMethods);
	const { redirect, rememberMe } = queryString.parse(props.location.search.substring(1));
	// % protected region % [Customise logic here] end
	// % protected region % [Customise onTwoFactorSuccess here] off begin
	const onTwoFactorSuccess = () => {
		const redirectString = typeof redirect === 'string'
			? redirect
			: undefined;

		if (redirectString) {
			props.history.push(redirectString);
		} else {
			props.history.push('/');
		}
	}
	// % protected region % [Customise onTwoFactorSuccess here] end

	// % protected region % [Customise render here] off begin
	return (
		<Switch>
			{/* Routes for each two factor page */}
			{twoFactorMethodKeys.map(x => {
				const method = twoFactorMethods[x];
				const onLogin = method.onLogin({
					onTwoFactorSuccess,
					rememberMe: rememberMe === 'true',
				});

				if (onLogin.required) {
					const route = `${path}/2fa/${x.toLocaleLowerCase()}`;
					return (
						<Route key={x} path={route}>
							{onLogin.content}
						</Route>
					);
				}
				return null;
			})}

			{/* Login page route */}
			<Route path={path} exact component={LoginPage} />
		</Switch>
	)
	// % protected region % [Customise render here] end
}

// % protected region % [Override export here] off begin
export default LoginRoutes;
// % protected region % [Override export here] end
