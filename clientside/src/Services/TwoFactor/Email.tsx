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
import {
	InitialSetupArguments,
	LoginArguments,
	TwoFactorConfiguration,
	TwoFactorMethods,
} from 'Services/TwoFactor/Common';
import React, { FormEventHandler } from 'react';
import { useLocalStore } from 'mobx-react';
import { validate2faCode } from 'Services/Api/AuthorizationService';
import alert from 'Util/ToastifyUtils';
import { TextField } from 'Views/Components/TextBox/TextBox';
import { Button } from 'Views/Components/Button/Button';
import { Checkbox } from 'Views/Components/Checkbox/Checkbox';
import alertToast from 'Util/ToastifyUtils';
import { useAsync } from 'Hooks/useAsync';
import Spinner from 'Views/Components/Spinner/Spinner';
import If from 'Views/Components/If/If';
import { ButtonGroup } from 'Views/Components/Button/ButtonGroup';
import { logout } from 'Util/NavigationUtils';
import { useLocation } from 'react-router';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise constants here] off begin
export const EmailTokenType = "Email";
// % protected region % [Customise constants here] end

// % protected region % [Customise EmailInitialSetupProps here] off begin
export interface EmailInitialSetupProps extends InitialSetupArguments {

}
// % protected region % [Customise EmailInitialSetupProps here] end

export function EmailInitialSetup({
	close,
	configurePromise,
	loggedInUserName,
	userName,
	// % protected region % [Add any extra initial setup props here] off begin
	// % protected region % [Add any extra initial setup props here] end
}: EmailInitialSetupProps) {
	// % protected region % [Customise EmailInitialSetup hooks here] off begin
	const location = useLocation();
	const response = useAsync(() => configurePromise, []);
	const inputData = useLocalStore(() => ({
		code: ''
	}));
	// % protected region % [Customise EmailInitialSetup hooks here] end

	// % protected region % [Customise EmailInitialSetup onSubmit here] off begin
	const onSubmit: FormEventHandler = e => {
		e.preventDefault();
		validate2faCode({
			token: inputData.code,
			method: EmailTokenType,
		})
			.then(() => {
				alertToast('Successfully enabled two factor authentication', 'success');
				close();
			})
			.catch(() => {
				alertToast('Invalid two factor code', 'error');
			});
	}
	// % protected region % [Customise EmailInitialSetup onSubmit here] end

	// % protected region % [Customise EmailInitialSetup onCloseAndLogout here] off begin
	const onCloseAndLogout = () => {
		logout(location.pathname);
	}
	// % protected region % [Customise EmailInitialSetup onCloseAndLogout here] end

	// % protected region % [Customise EmailInitialSetup onClose here] off begin
	const onClose = () => {
		close();
	}
	// % protected region % [Customise EmailInitialSetup onClose here] end

	// % protected region % [Customise EmailInitialSetup loading here] off begin
	if (response.type === 'loading') {
		return <Spinner />;
	}
	// % protected region % [Customise EmailInitialSetup loading here] end

	// % protected region % [Customise EmailInitialSetup errors here] off begin
	if (response.type === 'error') {
		console.error(response.error);
		return (
			<div>
				<p>There was an error configuring email authentication.</p>
				<Button onClick={() => close(true)}>Close</Button>
			</div>
		)
	}
	// % protected region % [Customise EmailInitialSetup errors here] end

	// % protected region % [Customise EmailInitialSetup render here] off begin
	return (
		<div>
			<h4>Email Authentication</h4>

			<If condition={userName === loggedInUserName}>
				<p>
					You now have two factor authentication configured for your account. To finish the setup process,
					you will have been sent an email with a code. Enter the code in the confirmation box below.
				</p>
				<p>
					If you do not enter the code you will be logged out from the application.
				</p>
				<form onSubmit={onSubmit}>
					<TextField
						model={inputData}
						modelProperty="code"
						label="Authenticator Code"
					/>
					<ButtonGroup>
						<Button type="submit">Submit</Button>
						<Button onClick={onCloseAndLogout}>Close and logout</Button>
					</ButtonGroup>
				</form>
			</If>

			<If condition={userName !== loggedInUserName}>
				<p>The user {userName} now has two factor authentication enabled for their account.</p>
				<Button onClick={onClose}>Close</Button>
			</If>
		</div>
	);
	// % protected region % [Customise EmailInitialSetup render here] end
}

// % protected region % [Customise EmailTwoFactorPageProps here] off begin
export interface EmailTwoFactorPageProps extends LoginArguments {

}
// % protected region % [Customise EmailTwoFactorPageProps here] end

export function EmailTwoFactorPage({
	onTwoFactorSuccess,
	rememberMe
}: EmailTwoFactorPageProps) {
	// % protected region % [Customise EmailTwoFactorPage hooks here] off begin
	const inputData = useLocalStore(() => ({
		code: '',
		rememberTwoFactor: false,
	}));
	// % protected region % [Customise EmailTwoFactorPage hooks here] end

	// % protected region % [Customise EmailTwoFactorPage onSubmit here] off begin
	const onSubmit: FormEventHandler = (e) => {
		e.preventDefault();
		validate2faCode({
			method: EmailTokenType,
			token: inputData.code,
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
	// % protected region % [Customise EmailTwoFactorPage onSubmit here] end

	// % protected region % [Customise EmailTwoFactorPage render here] off begin
	return (
		<div className="body-content">
			<form onSubmit={onSubmit}>
				<h2>Verify Login</h2>
				<div>
					You will have been sent an email with a code.
					Please enter that code and press continue to finish logging in.
				</div>
				<TextField model={inputData} modelProperty="code"/>
				<Checkbox model={inputData} modelProperty={'rememberTwoFactor'} label="Skip this step for 14 days" />
				<Button type="submit">Submit</Button>
			</form>
		</div>
	);
	// % protected region % [Customise EmailTwoFactorPage render here] end
}


export const EmailTwoFactorConfiguration: TwoFactorConfiguration = {
	// % protected region % [Customise onInitialSetup here] off begin
	onInitialSetup: args => ({
		content: <EmailInitialSetup {...args} />,
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
		content: <EmailTwoFactorPage {...args} />,
		required: true,
	}),
	// % protected region % [Customise onLogin here] end
	// % protected region % [Add any extra config fields here] off begin
	// % protected region % [Add any extra config fields here] end
}

// % protected region % [Customise configureEmail2fa here] off begin
export function configureEmail2fa(methods: TwoFactorMethods) {
	methods[EmailTokenType] = EmailTwoFactorConfiguration;
}
// % protected region % [Customise configureEmail2fa here] end

// % protected region % [Add any additional methods here] off begin
// % protected region % [Add any additional methods here] end
