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
import {action, computed, observable, runInAction} from 'mobx';
import {Button, Display} from '../Button/Button';
import {observer} from "mobx-react";
import { SERVER_URL } from 'Constants';
import Collection, {ICollectionItemActionProps} from '../Collection/Collection';
import axios from 'axios';
import {SecurityService} from 'Services/SecurityService';
import { StaffEntity, } from 'Models/Entities';
import {RouteComponentProps} from 'react-router';
import {confirmModal} from '../Modal/ModalUtils';
import alert from '../../../Util/ToastifyUtils';
import classNames from 'classnames';
import Modal from '../Modal/Modal';
import {Combobox} from 'Views/Components/Combobox/Combobox';
import {IOrderByCondition} from '../ModelCollection/ModelQuery';
import {ICollectionHeaderProps} from "../Collection/CollectionHeaders";
import { TwoFactorContext, TwoFactorMethods } from 'Services/TwoFactor/Common';
import { useContext } from 'react';
import TwoFactorAuthSetup from 'Views/Components/CRUD/TwoFactorAuthSetup';

interface IEntityEditRouteParams {
	id?: string;
}

interface UserListInternalProps extends RouteComponentProps<IEntityEditRouteParams> {
	methods: TwoFactorMethods;
}

const userTypes = {
	"StaffEntity" : StaffEntity,
};

interface ISearch {
	searchTerm: string;
}

interface ModalOptions {
	display: string;
	value: string;
}

interface ClosedModalState {
	open: false;
}

interface OpenModalState {
	open: boolean;
	user: IUser;
	activate: boolean;
}

type ModalState = OpenModalState | ClosedModalState;

interface IUser {
	id: string;
	email: string;
	discriminator: string;
	emailConfirmed: boolean;
	lockedOut: boolean;
	created: string;
	modified: string;
	owner: string;
	userName: string;
	twoFactorEnabled: boolean;
	twoFactorMethod: string;
}

@observer
class UserListInternal extends React.Component<UserListInternalProps> {
	@observable
	private users: IUser[] = [];

	@observable
	private totalRecords: number;

	@observable
	private pageNo = 0;

	private get perPage() {
		return 10;
	}

	private sortParams : IOrderByCondition<any> = {
		path: "id",
		descending: true,
	};

	@observable
	private search: ISearch = { searchTerm: "" };

	@computed
	private get urlParams() {
		return {
			PageNo: this.pageNo + 1,
			PageSize: this.perPage
		};
	}

	// configure the search parameters that are sent to the server-side in the next request
	@computed
	private get searchParams() {
		const regex = /^[0-9A-F]{8}-[0-9A-F]{4}-[4][0-9A-F]{3}-[89AB][0-9A-F]{3}-[0-9A-F]{12}$/i;
		let params  =
			[

				{comparison: "Like", path: "Email", value: [`%${this.search.searchTerm}%`]},
				{comparison: "Like", path: "discriminator", value: [`%${this.search.searchTerm}%`]}
			];

		if (this.search.searchTerm.match(regex)) {
			return [params.concat({comparison: "Equal", path: "id", value: [`${this.search.searchTerm}`]})];
		}
		return [params];
	}

	@observable
	private modalState = {
		open: false,
	};

	@observable
	private activateModalState : ModalState = {
		open: false,
		user: undefined,
		activate: true,
	};

	@observable
	private authenticatorModalState: {open: boolean, content: React.ReactNode} = {
		open: false,
		content: '',
	};

	public componentDidMount() {
		this.fetchData();
	};

	@action
	private onPageChange = (pageNo: number) => {
		this.pageNo = pageNo;
		this.fetchData();
	}

	private fetchData = () => {
		axios.post(`${SERVER_URL}/api/account/users`, {
			PaginationOptions: this.urlParams,
			SearchConditions: this.searchParams,
			SortConditions: [this.sortParams]
		}).then(this.setData)
	};

	@action
	private setData = (data: any) => {
		this.totalRecords = data.data.countUsers;
		this.users = data.data.users;
	};

	// Gets the actions (Read - Update - Delete) that can be performed on each row in the
	// all user list, depending on security.
	protected getTableActions = (entity: IUser) => {
		const tableActions: Array<ICollectionItemActionProps<IUser>> = [];

		if(userTypes[entity.discriminator] !== undefined){
			if(SecurityService.canRead(userTypes[entity.discriminator])){
				tableActions.push({
					action: (entity: IUser) => {
						this.props.history.push({ pathname: `/admin/${entity.discriminator}/view/${entity.id}` });
					},
					label: "View",
					showIcon: true,
					icon: "look",
					iconPos: 'icon-top',
				});
			}
			if(SecurityService.canUpdate(userTypes[entity.discriminator])){
				tableActions.push({
					action: (entity: IUser) => {
						this.props.history.push({ pathname: `/admin/${entity.discriminator}/edit/${entity.id}` });
					},
					label: "Edit",
					showIcon:  true,
					icon: "edit",
					iconPos: 'icon-top',
				});
			}
			if (SecurityService.canDelete(userTypes[entity.discriminator])) {
				tableActions.push({
					action: (entity: IUser) => {
						confirmModal('Please confirm', "Are you sure you want to delete this item?").then(() => {
							new userTypes[entity.discriminator](entity).delete().then(() => {
								this.fetchData();
								alert('Deleted successfully', 'success');
							}).catch((errorMessage: any) => {
								alert(
									<div className="delete-error">
										<p className="user-error">This record could not be deleted because of an association</p>
										<p className="internal-error-title">Message:</p>
										<p className="internal-error">{errorMessage}</p>
									</div>
									, 'error'
								);
							});
						});
					},
					label: "Delete",
					showIcon: true,
					icon: "bin-full",
					iconPos: 'icon-top',
				});
			}
		}
		return tableActions;
	};

	protected renderCreateButton(): React.ReactNode {
		return (
			<Button
				key="create"
				className={classNames(Display.Solid)}
				icon={{icon: 'create', iconPos: 'icon-right'}}
				buttonProps={{ onClick: () => this.onOpenModal()}}>
				Create new
			</Button>
		);
	}

	@action
	private onOpenModal = () => {
		this.modalState.open = true;
		this.userTypeSelected.selectedUser = null;
	};

	// The modal shown when the user clicks on the 'create' button.
	protected renderCreateModal = () => {
		return (
			<Modal
				isOpen={this.modalState.open}
				label="User Selection Modal"
				onRequestClose={() => runInAction(() => this.modalState.open = false)}>
				<h4>Select user type</h4>
				<p>What type of user would you like to create?</p>
				{this.renderModalCombobox()}
				<div key="actions" className="modal__actions">
					<Button key="confirm" onClick={this.userSelected} display={Display.Solid}>Confirm</Button>
					<Button key="cancel" onClick={this.onCancelModal} display={Display.Outline}>Cancel</Button>
				</div>
			</Modal>
		)
	};

	@action
	protected onCancelModal = () => {
		this.modalState.open = false;
	};

	protected userSelected = () => {
		if (this.userTypeSelected.selectedUser != null) {
			this.props.history.push({ pathname: `/admin/${this.userTypeSelected.selectedUser}/create` })
		}
		else {
			alert('Please select a user type.','warning',{position: 'top-center'})
		}
	};

	private userTypeSelected = {
		selectedUser: null
	};

	protected renderModalCombobox = () => {
		return (
			<Combobox model={this.userTypeSelected}
			modelProperty="selectedUser"
			label="Select user type"
			labelVisible = {false}
			options={this.getModalUserOptions()}
			searchable={false}
		/>
		)
	};

	protected getModalUserOptions = () => {
		const userOptions: ModalOptions[] = [];
		Object.keys(userTypes).forEach(
			u => {
				if (SecurityService.canCreate(userTypes[u])) {
					userOptions.push({display:u, value: u})
				}
			}
		);
		return userOptions;
	};

	// % protected region % [Customise user collection headers here] off begin
	// The column headings shown at the top of the all users list.
	// includes actions for sorting and on clicking the headings.
	private userCollectionHeaders : Array<ICollectionHeaderProps<IUser>> = [
		{
			name: 'discriminator',
			displayName: 'Type',
			sortable: true,
			sortClicked: () => this.onSort('discriminator')
		},
		{
			name: 'email',
			displayName: 'Email',
			sortable: true,
			sortClicked: () => this.onSort('email')},
		{
			name: 'emailConfirmed',
			displayName: 'Activated',
			sortable: true,
			transformItem: model => model.emailConfirmed && !model.lockedOut ? 'True' : 'False',
			sortClicked: () => this.onSort('emailConfirmed')
		},
		{
			name: 'twoFactorEnabled',
			displayName: 'Two Factor Enabled',
			sortable: true,
			transformItem: model => model.twoFactorEnabled ? 'True' : 'False',
			sortClicked: () => this.onSort('twoFactorEnabled'),
		},
		{
			name: 'twoFactorMethod',
			displayName: 'Two Factor Method',
			sortable: false,
			transformItem: model => model.twoFactorMethod ? model.twoFactorMethod : 'None',
		},
	];
	// % protected region % [Customise user collection headers here] end

	public render()  {
		return (
			<>
				{this.renderCreateModal()}
				{this.renderDeactivateUserModal()}
				{this.renderAuthenticatorModal()}
				<Collection
					additionalActions={[this.renderCreateButton()]}
					actions={this.getTableActions}
					collection={this.users}
					onSearchTriggered={this.onSearchTriggered}
					headers={this.userCollectionHeaders}
					onPageChange={this.onPageChange}
					pageNo={this.pageNo}
					perPage={this.perPage}
					totalRecords={this.totalRecords}
					actionsMore={[
						{
							customItem: <Button>Toggle activation</Button>,
							onEntityClick: (event, entity) => this.setActivateUserModalState(entity, true),
						},
						{
							customItem: <Button>Reset password</Button>,
							onEntityClick: (event, entity) => this.resetPassword(entity),
						},
						{
							customItem: <Button>Toggle Two Factor Authentication</Button>,
							onEntityClick: (event, entity) => this.toggle2fa(entity),
						},
					]}/>
			</>
		);
	}

	@action
	protected onSort = (attribute: string) => {
		if (!this.sortParams.descending && this.sortParams.path === attribute){
			this.sortParams = {
				path: "id",
				descending: true
			};
		}
		else{
			let descending = this.sortParams.path === attribute ? !this.sortParams.descending : true;
			this.sortParams = {
				path: attribute,
				descending: descending
			};
		}

		this.fetchData();
		return this.sortParams;
	};

	@action
	protected setActivateUserModalState = (user: IUser | null, open: boolean) => {
		if (open && user) {
			this.activateModalState = {
				open: open,
				user: user,
				activate: !user.emailConfirmed || user.lockedOut
			};
		} else {
			this.activateModalState = { open: false };
		}
	};

	// The modal shown when a user selects 'deactivate user' on an entry
	// in the all user list.
	protected renderDeactivateUserModal = () => {
		if (this.activateModalState.open) {
			const { user, activate } = this.activateModalState;
			return (
				<Modal
					isOpen={this.activateModalState.open}
					label="Deactivate User Modal"
					onRequestClose={() => runInAction(() => this.setActivateUserModalState(null, false))}>
					<h4>{this.activateModalState.activate ? 'Activate' : 'Deactivate'} User</h4>
					<p>Are you sure you wish to {this.activateModalState.activate ? 'activate' : 'deactivate'} {user.email}?</p>
					<div key="actions" className="modal__actions">
						<Button key="confirm"
								onClick={() => activate
									? this.activateUser(user)
									: this.deactivateUser(user)}
								display={Display.Solid}>Confirm</Button>
						<Button key="cancel"
								onClick={() => this.setActivateUserModalState(null, false)}
								display={Display.Outline}>Cancel</Button>
					</div>
				</Modal>
			)
		}

		return <></>;
	};

	protected renderAuthenticatorModal = () => {
		return (
			<Modal
				isOpen={this.authenticatorModalState.open}
				label="Authenticator Modal"
				onRequestClose={() => undefined}
			>
				{this.authenticatorModalState.content}
			</Modal>
		);
	}

	protected deactivateUser = (entity: IUser) => {
		this.setActivateUserModalState(null, false);
		axios
			.post(`${SERVER_URL}/api/account/deactivate`, {Username: entity.email})
			.then(data => {
				alert(`Successfully deactivated ${entity.email}`, 'success');
				this.updateEmailConfirmed(entity, false);
				this.fetchData();
			})
			.catch(data => alert(`Unable to deactivate ${entity.email}`, 'error'))
	};

	protected activateUser = (entity: IUser) => {
		this.setActivateUserModalState(null, false);
		if (!entity.emailConfirmed || entity.lockedOut) {
			axios
				.post(`${SERVER_URL}/api/account/activate`, {Username: entity.email})
				.then(data => {
					alert(`Successfully activated ${entity.email}`, 'success');
					this.updateEmailConfirmed(entity, true);
					this.fetchData();
				})
				.catch(data => {
					alert(`Unable to activate ${entity.email}`, 'error');
					console.error(data);
				})
		} else {
			alert('Account already activated', 'warning')
		}
	};

	@action
	private updateEmailConfirmed = (entity: IUser, status: boolean) => {
		entity.emailConfirmed = status;
	};

	protected resetPassword = (entity: IUser) => axios
		.post(`${SERVER_URL}/api/account/reset-password-request`, {Username: entity.email})
		.then(data => this.onResetPasswordSuccess(entity))
		.catch(data => alert(`${data}`, 'error'));

	protected onResetPasswordSuccess = (entity: IUser) => {
		alert(`Successfully reset password for ${entity.email}`, 'success');
	};

	@action
	protected toggle2fa = (entity: IUser) => {
		this.authenticatorModalState.content = (
			<TwoFactorAuthSetup
				userName={entity.userName}
				currentlyEnabled={entity.twoFactorEnabled}
				close={action((dataUnchanged) => {
					this.authenticatorModalState.open = false
					if (!dataUnchanged) {
						this.fetchData();
					}
				})}
			/>
		);
		this.authenticatorModalState.open = true;
	}

	@action
	protected onSearchTriggered = (searchTerm: string) => {
		this.pageNo = 0;
		this.search.searchTerm = searchTerm.trim();
		this.fetchData();
	}
}

function UserList(props: RouteComponentProps<IEntityEditRouteParams>) {
	const twoFactorMethods = useContext(TwoFactorContext);
	return <UserListInternal {...props} methods={twoFactorMethods} />;
}

export default UserList;
