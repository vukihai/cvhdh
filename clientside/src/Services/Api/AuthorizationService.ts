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
import Axios from 'axios';
import { SERVER_URL } from 'Constants';
import axios from 'axios';
import { IUserResult } from 'Models/Store';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise validate2faCodeArgs here] off begin
export type validate2faCodeArgs = {
	token: string,
	method: string,
	rememberMe?: boolean,
	rememberTwoFactor?: boolean,
}
// % protected region % [Customise validate2faCodeArgs here] end

// % protected region % [Customise validate2faCode here] off begin
export function validate2faCode({
	token,
	method,
	rememberMe,
	rememberTwoFactor,
}: validate2faCodeArgs) {
	const body = { token, method };

	if (rememberMe !== undefined) body['rememberMe'] = rememberMe.toString();
	if (rememberTwoFactor !== undefined) body['rememberTwoFactor'] = rememberTwoFactor.toString();

	return Axios.post(`${SERVER_URL}/api/authorization/validate-2fa`, body);
}
// % protected region % [Customise validate2faCode here] end

// % protected region % [Customise TwoFactorLoginResponse here] off begin
export type TwoFactorLoginResponse = {
	type: '2fa-required',
	method: string,
	data: any,
}
// % protected region % [Customise TwoFactorLoginResponse here] end

// % protected region % [Customise LoginResponse here] off begin
export type LoginResponse = IUserResult | TwoFactorLoginResponse
// % protected region % [Customise LoginResponse here] end

// % protected region % [Customise login here] off begin
export function login(username: string, password: string, rememberMe?: boolean): Promise<LoginResponse> {
	const postData = {
		username: username,
		password: password,
	};

	if (rememberMe !== undefined) {
		postData['rememberMe'] = rememberMe;
	}

	return axios.post<LoginResponse>(`${SERVER_URL}/api/authorization/login`, postData)
		.then(x => x.data);
}
// % protected region % [Customise login here] end

// % protected region % [Add any additional methods here] off begin
// % protected region % [Add any additional methods here] end
