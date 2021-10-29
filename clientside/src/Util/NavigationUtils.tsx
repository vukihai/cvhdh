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
import { buildUrl } from 'Util/FetchUtils';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise logout here] off begin
/**
 * Logs a user out of the application by navigating to the logout endpoint.
 * @param redirect Redirect url for the user to be navigated to after the logout is completed.
 * @param clean If true then all cookies will be purged in the logout process, otherwise only login related cookies will
 * be.
 */
export function logout(redirect?: string, clean?: boolean) {
	const params: { [key: string]: string } = {};
	if (redirect) params.redirect = redirect;
	if (clean) params.clean = 'true';
	window.location.href = buildUrl('/api/authorization/logout', params);
}
// % protected region % [Customise logout here] end