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
import React, { useContext, useEffect, useState } from 'react';
import { TwoFactorContext } from 'Services/TwoFactor/Common';
import { useStore } from 'Hooks/useStore';
import { Button } from 'Views/Components/Button/Button';
import { configure2fa, disable2fa, valid2faMethods } from 'Services/Api/AccountService';
import alertToast from 'Util/ToastifyUtils';
import { Alignment, ButtonGroup } from 'Views/Components/Button/ButtonGroup';
import { useAsync } from 'Hooks/useAsync';
import Spinner from 'Views/Components/Spinner/Spinner';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export interface TwoFactorAuthSetupProps {
	userName: string;
	close: (dataUnchanged?: boolean) => void;
	currentlyEnabled: boolean;
	// % protected region % [Add any extra TwoFactorAuthSetupProps fields here] off begin
	// % protected region % [Add any extra TwoFactorAuthSetupProps fields here] end
}

function EnableTwoFactorAuth({ userName, close }: TwoFactorAuthSetupProps) {
	// % protected region % [Customise EnableTwoFactorAuth hooks here] off begin
	const store = useStore();
	const twoFactorMethods = useContext(TwoFactorContext);
	const twoFactorResponse = useAsync(() => valid2faMethods(userName), []);
	const [ selectedMethodType, setSelectedMethodType ] = useState<string | null>(null);
	const [ content, setContent ] = useState<React.ReactNode>();
	// % protected region % [Customise EnableTwoFactorAuth hooks here] end

	// % protected region % [Customise EnableTwoFactorAuth effect hook here] off begin
	useEffect(() => {
		// If the user has not selected a method then do nothing
		if (selectedMethodType === null) {
			return;
		}

		// Call the initial setup
		const method = twoFactorMethods[selectedMethodType].onInitialSetup({
			userName: userName,
			loggedInUserName: store.userName,
			close: close,
			configurePromise: configure2fa(userName, selectedMethodType)
		});

		// If no ui is required call the close callback
		if (!method.required) {
			close();
			return;
		}

		// Otherwise set the ui in the state
		setContent(method.content);
	}, [ selectedMethodType, close, store.userName, twoFactorMethods, userName ]);
	// % protected region % [Customise EnableTwoFactorAuth effect hook here] end

	// If there is no selected method type then prompt the user to select one.
	if (selectedMethodType === null) {
		// % protected region % [Customise EnableTwoFactorAuth response loading here] off begin
		if (twoFactorResponse.type === 'loading') {
			return <Spinner />;
		}
		// % protected region % [Customise EnableTwoFactorAuth response loading here] end

		// % protected region % [Customise EnableTwoFactorAuth response error here] off begin
		if (twoFactorResponse.type === 'error') {
			console.error(twoFactorResponse.error);
			return (
				<div>
					<p>There was an error loading the two factor methods for this user</p>
					<Button onClick={() => close(true)}>Close</Button>
				</div>
			);
		}
		// % protected region % [Customise EnableTwoFactorAuth response error here] end

		// % protected region % [Customise EnableTwoFactorAuth response render here] off begin
		return (
			<div>
				<p>Please select a 2 factor method to use.</p>
				<ButtonGroup alignment={Alignment.VERTICAL}>
					{twoFactorResponse.data.map(x => (
						<Button onClick={() => setSelectedMethodType(x)} key={x}>
							{x}
						</Button>
					))}
				</ButtonGroup>
				<Button onClick={() => close(true)}>Close</Button>
			</div>
		);
		// % protected region % [Customise EnableTwoFactorAuth response render here] end
	}

	// % protected region % [Customise EnableTwoFactorAuth no method render here] off begin
	// Otherwise display the content for the selected two factor method
	// If the method does not have a required ui then the close callback will have been called and this component
	// should be unrendered.
	return <>{content}</>;
	// % protected region % [Customise EnableTwoFactorAuth no method render here] end
}

function DisableTwoFactorAuth({ userName, close }: TwoFactorAuthSetupProps) {
	// % protected region % [Customise DisableTwoFactorAuth hooks here] off begin
	const store = useStore();
	const currentUser = store.userName;
	// % protected region % [Customise DisableTwoFactorAuth hooks here] end
	// % protected region % [Customise DisableTwoFactorAuth onDisable here] off begin
	const onDisable = () => {
		disable2fa(userName)
			.then(() => {
				close();
			})
			.catch(error => {
				console.log(error);
				alertToast('Unable to disable 2fa', 'error');
			});
	}
	// % protected region % [Customise DisableTwoFactorAuth onDisable here] end

	// % protected region % [Customise DisableTwoFactorAuth confirmation message here] off begin
	let message: React.ReactNode;
	if (currentUser !== userName) {
		message = <p>Are you sure you want to disable two factor authentication for {userName}.</p>;
	} else {
		message = <p>Are you sure you want to disable two factor authentication.</p>;
	}
	// % protected region % [Customise DisableTwoFactorAuth confirmation message here] end

	// % protected region % [Customise DisableTwoFactorAuth render here] off begin
	return (
		<div>
			{message}
			<ButtonGroup>
				<Button onClick={onDisable}>Yes</Button>
				<Button onClick={() => close(true)}>No</Button>
			</ButtonGroup>
		</div>
	);
	// % protected region % [Customise DisableTwoFactorAuth render here] end
}

function TwoFactorAuthSetup(props: TwoFactorAuthSetupProps) {
	// % protected region % [Customise TwoFactorAuthSetup here] off begin
	if (props.currentlyEnabled) {
		return <DisableTwoFactorAuth {...props} />;
	}
	return <EnableTwoFactorAuth {...props} />;
	// % protected region % [Customise TwoFactorAuthSetup here] end
}

export default TwoFactorAuthSetup;

// % protected region % [Add any additional methods here] off begin
// % protected region % [Add any additional methods here] end
