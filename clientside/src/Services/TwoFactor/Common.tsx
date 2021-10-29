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
import React, { createContext } from 'react';
import { Configure2faResponse } from 'Services/Api/AccountService';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export type InitialSetupArguments = {
	userName: string,
	loggedInUserName?: string,
	configurePromise: Promise<Configure2faResponse>,
	close: (dataUnchanged?: boolean) => void,
	// % protected region % [Add any extra InitialSetupArguments fields here] off begin
	// % protected region % [Add any extra InitialSetupArguments fields here] end
};

export type RemoveArguments = {
	userName: string,
	close: (dataUnchanged?: boolean) => void,
	// % protected region % [Add any extra RemoveArguments fields here] off begin
	// % protected region % [Add any extra RemoveArguments fields here] end
};

export type LoginArguments = {
	onTwoFactorSuccess?: () => void,
	rememberMe?: boolean,
	// % protected region % [Add any extra LoginArguments fields here] off begin
	// % protected region % [Add any extra LoginArguments fields here] end
}

export type TwoFactorResponse = {
	content: React.ReactNode,
	required: boolean,
	// % protected region % [Add any extra TwoFactorResponse fields here] off begin
	// % protected region % [Add any extra TwoFactorResponse fields here] end
}

export interface TwoFactorConfiguration {
	onInitialSetup: (args: InitialSetupArguments) => TwoFactorResponse;
	onRemove: (args: RemoveArguments) => TwoFactorResponse;
	onLogin: (args: LoginArguments) => TwoFactorResponse;
	// % protected region % [Add any extra TwoFactorConfiguration fields here] off begin
	// % protected region % [Add any extra TwoFactorConfiguration fields here] end
}

export interface TwoFactorMethods {
	[name: string] : TwoFactorConfiguration;
	// % protected region % [Add any extra TwoFactorMethods fields here] off begin
	// % protected region % [Add any extra TwoFactorMethods fields here] end
}

// % protected region % [Customise buildOtpUrl here] off begin
export const TwoFactorContext = createContext<TwoFactorMethods>({ });
// % protected region % [Customise buildOtpUrl here] end

// % protected region % [Add any additional methods here] off begin
// % protected region % [Add any additional methods here] end
