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
import { IUserResult } from 'Models/Store';
import { buildUrl } from 'Util/FetchUtils';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise Configure2faResponseBase here] off begin
type Configure2faResponseBase = {
	method: string,
}
// % protected region % [Customise Configure2faResponseBase here] end

// % protected region % [Customise ConfigureAuthenticatorResponse here] off begin
export type ConfigureAuthenticatorResponse = Configure2faResponseBase & {
	method: 'Authenticator',
	code: string,
}
// % protected region % [Customise ConfigureAuthenticatorResponse here] end

// % protected region % [Customise ConfigureEmailResponse here] off begin
export type ConfigureEmailResponse = Configure2faResponseBase & {
	method: 'Email',
}
// % protected region % [Customise ConfigureEmailResponse here] end

// % protected region % [Customise Configure2faResponse here] off begin
export type Configure2faResponse = ConfigureAuthenticatorResponse | ConfigureEmailResponse;
// % protected region % [Customise Configure2faResponse here] end

// % protected region % [Customise configure2fa here] off begin
export function configure2fa(userName: string, method: string): Promise<Configure2faResponse> {
	const url = `${SERVER_URL}/api/account/configure-2fa`;
	const body = { method: method, userName: userName };
	return Axios.post<Configure2faResponse>(url, body)
		.then(x => x.data);
}
// % protected region % [Customise configure2fa here] end

// % protected region % [Customise disable2fa here] off begin
export function disable2fa(userName: string) {
	const url = `${SERVER_URL}/api/account/disable-2fa`;
	const body = { userName: userName };
	return Axios.post(url, body);
}
// % protected region % [Customise disable2fa here] end

// % protected region % [Customise valid2faMethods here] off begin
export function valid2faMethods(userName: string): Promise<string[]> {
	const url = buildUrl(`${SERVER_URL}/api/account/valid-2fa`, { userName });
	return Axios.get<string[]>(url).then(x => x.data);
}
// % protected region % [Customise valid2faMethods here] end

// % protected region % [Customise me here] off begin
export function me(): Promise<IUserResult> {
	return Axios.get<IUserResult>(`${SERVER_URL}/api/account/me`)
		.then(x => x.data);
}
// % protected region % [Customise me here] end

// % protected region % [Add any additional methods here] off begin
// % protected region % [Add any additional methods here] end
