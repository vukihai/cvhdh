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
import React, { FormEventHandler } from 'react';
import {
	InitialSetupArguments,
	LoginArguments,
	TwoFactorConfiguration,
	TwoFactorMethods
} from 'Services/TwoFactor/Common';
import { useAsync } from 'Hooks/useAsync';
import Spinner from 'Views/Components/Spinner/Spinner';
import { useLocalStore } from 'mobx-react';
import { TextField } from 'Views/Components/TextBox/TextBox';
import { Button } from 'Views/Components/Button/Button';
import { validate2faCode } from 'Services/Api/AuthorizationService';
import QRCode from 'qrcode.react';
import { APPLICATION_NAME } from 'Constants';
import alertToast from 'Util/ToastifyUtils';
import alert from 'Util/ToastifyUtils';
import { Checkbox } from 'Views/Components/Checkbox/Checkbox';
import { logout } from 'Util/NavigationUtils';
import { useLocation } from 'react-router';
import { ButtonGroup } from 'Views/Components/Button/ButtonGroup';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise constants here] off begin
export const AuthenticatorTokenType = 'Authenticator';
const GoogleAuthenticatorLink = 'https://support.google.com/accounts/answer/1066447';
const AuthyLink = 'https://authy.com/';
// % protected region % [Customise constants here] end

// % protected region % [Customise buildOtpUrl here] off begin
export function buildOtpUrl(application: string, userName: string, secret: string, issuer: string): string {
	const escapedApp = encodeURIComponent(application);
	const escapedUser = encodeURIComponent(userName);
	const escapedIssuer = encodeURIComponent(issuer);
	return `otpauth://totp/${escapedApp}:${escapedUser}?secret=${secret}&issuer=${escapedIssuer}&digits=6`;
}
// % protected region % [Customise buildOtpUrl here] end

// % protected region % [Customise formatCode here] off begin
function formatCode(code: string): string {
	let result = '';
	for (let i = 0; i < code.length; i += 4) {
		result += code.substr(i, 4) + ' ';
	}
	return result.trim();
}
// % protected region % [Customise formatCode here] end

// % protected region % [Customise AuthenticatorInitialSetupProps here] off begin
export interface AuthenticatorInitialSetupProps extends InitialSetupArguments {

}
// % protected region % [Customise AuthenticatorInitialSetupProps here] end

export function AuthenticatorInitialSetup({
	userName,
	loggedInUserName,
	close,
	configurePromise,
	// % protected region % [Add any extra initial setup props here] off begin
	// % protected region % [Add any extra initial setup props here] end
}: AuthenticatorInitialSetupProps) {
	// % protected region % [Customise AuthenticatorInitialSetup hooks here] off begin
	const location = useLocation();
	const response = useAsync(() => configurePromise, []);
	const inputData = useLocalStore(() => ({
		code: '',
	}));
	// % protected region % [Customise AuthenticatorInitialSetup hooks here] end

	// % protected region % [Customise AuthenticatorInitialSetup onSubmit here] off begin
	const onSubmit: FormEventHandler = (e) => {
		e.preventDefault();
		validate2faCode({
			token: inputData.code,
			method: AuthenticatorTokenType,
		})
			.then(() => {
				alertToast('Successfully enabled two factor authentication', 'success');
				close();
			})
			.catch(error => {
				console.error(error);
				alertToast('Invalid authentication code. Please try again.', 'error');
			});
	}
	// % protected region % [Customise AuthenticatorInitialSetup onSubmit here] end

	// % protected region % [Customise AuthenticatorInitialSetup onCloseAndLogout here] off begin
	const onCloseAndLogout = () => {
		logout(location.pathname);
	}
	// % protected region % [Customise AuthenticatorInitialSetup onCloseAndLogout here] end

	// % protected region % [Customise AuthenticatorInitialSetup response loading here] off begin
	if (response.type === 'loading') {
		return <Spinner />;
	}
	// % protected region % [Customise AuthenticatorInitialSetup response loading here] end

	// % protected region % [Customise AuthenticatorInitialSetup response errors here] off begin
	if (response.type === 'error') {
		console.error(response.error);
		return (
			<div>
				<p>There was an error configuring the authenticator application.</p>
				<Button onClick={() => close(true)}>Close</Button>
			</div>
		)
	}
	// % protected region % [Customise AuthenticatorInitialSetup response errors here] end

	// % protected region % [Customise AuthenticatorInitialSetup response success here] off begin
	if (response.data.method === AuthenticatorTokenType) {
		const formattedCode = <pre>{formatCode(response.data.code)}</pre>;
		const qrCode = <QRCode value={buildOtpUrl(APPLICATION_NAME, userName, response.data.code, APPLICATION_NAME)} />;

		if (userName === loggedInUserName) {
			return (
				<div>
					<p>To use this code you have to go through the following steps</p>
					<ol>
						<li>
							Download a two factor authenticator app
							like <a href={GoogleAuthenticatorLink}>Google Authenticator</a> or <a
							href={AuthyLink}>Authy</a>.
						</li>
						<li>
							<p>
								Scan the QR code or enter the following key into your two factor authenticator app.
								Spaces and casing do not matter.
							</p>
							{formattedCode}
							{qrCode}
						</li>
						<li>
							<p>
								Once you have scanned the QR code or input the text above, the your two factor
								authentication app will provide you with a unique code. Enter the code in the
								confirmation box below.
							</p>
							<p>
								If you do not enter the code you will be logged out from the application.
							</p>
							<form onSubmit={onSubmit}>
								<TextField model={inputData} modelProperty="code" label="Authenticator Code"/>
								<ButtonGroup>
									<Button type="submit">Submit</Button>
									<Button onClick={onCloseAndLogout}>Close and logout</Button>
								</ButtonGroup>
							</form>
						</li>
					</ol>
				</div>
			);
		}

		return (
			<div>
				<p>Successfully set up two factor authentication for {userName}</p>
				<ol>
					<li>
						<p>
							Send the QR code or authenticator key to the {userName} so they can log into their
							account.
						</p>
						{formattedCode}
						{qrCode}
					</li>
					<li>
						Ask them to add the authenticator key or QE code to an app
						like <a href={GoogleAuthenticatorLink}>Google Authenticator</a> or <a href={AuthyLink}>Authy</a>.
					</li>
				</ol>
				<Button onClick={() => close()}>Close</Button>
			</div>
		);
	}
	// % protected region % [Customise AuthenticatorInitialSetup response success here] end

	// % protected region % [Customise AuthenticatorInitialSetup errors here] off begin
	console.error('Invalid response for Authenticator configuration', response);
	return <div>Unable to configure</div>
	// % protected region % [Customise AuthenticatorInitialSetup errors here] end
}

// % protected region % [Customise AuthenticatorLoginPageProps here] off begin
export interface AuthenticatorLoginPageProps extends LoginArguments {

}
// % protected region % [Customise AuthenticatorLoginPageProps here] end

export function AuthenticatorLoginPage({
	onTwoFactorSuccess,
	rememberMe,
	// % protected region % [Add any extra login page props here] off begin
	// % protected region % [Add any extra login page props here] end
}: AuthenticatorLoginPageProps) {
	// % protected region % [Customise AuthenticatorLoginPage hooks here] off begin
	const inputData = useLocalStore(() => ({
		code: '',
		rememberTwoFactor: false,
	}));
	// % protected region % [Customise AuthenticatorLoginPage hooks here] end

	// % protected region % [Customise AuthenticatorLoginPage onSubmit here] off begin
	const onSubmit: FormEventHandler = (e) => {
		e.preventDefault();
		validate2faCode({
			token: inputData.code,
			method: AuthenticatorTokenType,
			rememberTwoFactor: inputData.rememberTwoFactor,
			rememberMe: rememberMe,
		})
			.then(onTwoFactorSuccess)
			.catch(error => {
				alert(
					`Two factor error ${error}`,
					'error',
				);
			});
	}
	// % protected region % [Customise AuthenticatorLoginPage onSubmit here] end

	// % protected region % [Customise AuthenticatorLoginPage render here] off begin
	return (
		<div className="body-content">
			<form onSubmit={onSubmit}>
				<h2>Verify Login</h2>
				<div>
					Please get the auth code from your phone.
					Please enter that code and press continue to finish logging in.
				</div>
				<TextField model={inputData} modelProperty="code"/>
				<Checkbox model={inputData} modelProperty={'rememberTwoFactor'} label="Skip this step for 14 days" />
				<Button type="submit">Submit</Button>
			</form>
		</div>
	);
	// % protected region % [Customise AuthenticatorLoginPage render here] end
}

export const AuthenticatorTwoFactorConfiguration: TwoFactorConfiguration = {
	// % protected region % [Customise onInitialSetup here] off begin
	onInitialSetup: args => ({
		content: <AuthenticatorInitialSetup {...args} />,
		required: true,
	}),
	// % protected region % [Customise onInitialSetup here] end
	// % protected region % [Customise onRemove here] off begin
	onRemove: () => ({
		content: undefined,
		required: false,
	}),
	// % protected region % [Customise onRemove here] end
	// % protected region % [Customise onLogin here] off begin
	onLogin: args => ({
		content: <AuthenticatorLoginPage {...args} />,
		required: true,
	}),
	// % protected region % [Customise onLogin here] end
	// % protected region % [Add any extra config fields here] off begin
	// % protected region % [Add any extra config fields here] end
}

// % protected region % [Customise configureAuthenticator2fa here] off begin
export function configureAuthenticator2fa(methods: TwoFactorMethods) {
	methods[AuthenticatorTokenType] = AuthenticatorTwoFactorConfiguration;
}
// % protected region % [Customise configureAuthenticator2fa here] end

// % protected region % [Add any additional methods here] off begin
// % protected region % [Add any additional methods here] end
