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
import { createContext } from 'react';
import { History } from 'history';
import { action, computed, observable } from 'mobx';
import { IGlobalModal } from 'Views/Components/Modal/GlobalModal';
import { ApolloClient } from '@apollo/client';
// % protected region % [Add any extra store imports here] off begin
// % protected region % [Add any extra store imports here] end

// % protected region % [Change the group return result as needed] off begin
export interface IGroupResult {
	name: string;
	hasBackendAccess: boolean;
}
// % protected region % [Change the group return result as needed] end

// % protected region % [Change The user return result as needed] off begin
export interface IUserResult {
	type: 'user-data';
	id: string;
	userName: string;
	email: string;
	groups: IGroupResult[];
}
// % protected region % [Change The user return result as needed] end

export interface IStore {
	/**
	 * The current location in the application
	 */
	appLocation: 'frontend' | 'admin';

	/**
	 * The router history object for React Router
	 */
	routerHistory: History;

	/**
	 * The client for Apollo
	 */
	apolloClient: ApolloClient<{}>;

	/**
	 * The global modal that is stored in the app and can be called imperatively
	 */
	modal: IGlobalModal;

	/**
	 * This signifies weather we are logged in or not
	 * Only ever set this value to true if there is a value set in this.token
	 */
	readonly loggedIn: boolean;

	/**
	 * The user Id of the logged-in user
	 */
	readonly userId: string | undefined;

	/**
	 * The user name of the logged in user
	 */
	readonly userName: string | undefined;

	/**
	 * The email of the current logged in user
	 */
	readonly email: string | undefined;

	/**
	 * The groups that the logged in user are a part of
	 */
	readonly userGroups: IGroupResult[];

	/**
	 * Does this user have access to the backend admin views
	 */
	readonly hasBackendAccess: boolean;

	/**
	 * Is the frontend in edit mode
	 */
	frontendEditMode: boolean;

	/**
	 * Sets the current logged in user in the store
	 * @param userResult
	 */
	setLoggedInUser(userResult: IUserResult): void;

	/**
	 * Clears the logged in user data from the store
	 */
	clearLoggedInUser(): void;

	// % protected region % [Add any extra store interface methods or properties here] off begin
	// % protected region % [Add any extra store interface methods or properties here] end
}

/**
 * A global singleton store that contains a global state of data
 */
export class Store implements IStore {
	@observable
	user?: IUserResult;

	@observable
	appLocation: 'frontend' | 'admin' = 'frontend';

	routerHistory: History;

	apolloClient: ApolloClient<{}>;

	modal: IGlobalModal;

	@computed
	public get loggedIn() {
		// % protected region % [Customise the loggedIn getter here] off begin
		return this.user !== undefined;
		// % protected region % [Customise the loggedIn getter here] end
	}

	@computed
	public get userId(): string | undefined {
		// % protected region % [Customise the userId getter here] off begin
		return this.user ? this.user.id : undefined;
		// % protected region % [Customise the userId getter here] end
	};

	@computed
	public get userName(): string | undefined {
		// % protected region % [Customise the user name getter here] off begin
		return this.user?.userName;
		// % protected region % [Customise the user name getter here] end
	}

	@computed
	public get email(): string | undefined {
		// % protected region % [Customise the email getter here] off begin
		return this.user ? this.user.email : undefined;
		// % protected region % [Customise the email getter here] end
	}

	@computed
	public get userGroups(): IGroupResult[] {
		// % protected region % [Customise the userGroups getter here] off begin
		if (this.user) {
			return [...this.user.groups];
		}
		return [];
		// % protected region % [Customise the userGroups getter here] end
	};

	@computed
	public get hasBackendAccess() {
		// % protected region % [Customise the hasBackendAccess getter here] off begin
		if (this.user) {
			return this.user.groups.some(ug => ug.hasBackendAccess);
		}
		return false;
		// % protected region % [Customise the hasBackendAccess getter here] end
	};

	@observable
	public frontendEditMode = false;

	@action
	public setLoggedInUser(userResult: IUserResult) {
		// % protected region % [Customise the setLoggedInUser here] off begin
		this.user = userResult;
		// % protected region % [Customise the setLoggedInUser here] end
	}

	@action
	public clearLoggedInUser() {
		// % protected region % [Customise the clearLoggedInUser here] off begin
		this.user = undefined;
		// % protected region % [Customise the clearLoggedInUser here] end
	}

	// % protected region % [Add any extra store methods or properties here] off begin
	// % protected region % [Add any extra store methods or properties here] end
}

export const store: IStore = new Store();
export const StoreContext = createContext<IStore>(store);
