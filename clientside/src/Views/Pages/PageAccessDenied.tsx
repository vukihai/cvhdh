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
import { observer } from 'mobx-react';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { Button, Colors } from 'Views/Components/Button/Button';
import { Alignment, ButtonGroup } from 'Views/Components/Button/ButtonGroup';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise class signature and class properties] off begin
@observer
export default class PageAccessDenied extends React.Component<RouteComponentProps> {
// % protected region % [Customise class signature and class properties] end

	// % protected region % [Add class properties here] off begin
	// % protected region % [Add class properties here] end

	public render() {
		let contents = null;

		// % protected region % [Override contents here] off begin
		contents = (
			<div className='error-info__page'>
				<div className="error-info__elements">
					<h1 className="error-info__heading">403</h1>
					<p className="error-info__info">Oops! Unfortunately you are not allowed to access this page!</p>
					<ButtonGroup alignment={Alignment.HORIZONTAL}>
						<Button colors={Colors.Error} onClick={this.props.history.goBack}>
							Return to Previous Page 
						</Button>
						<Link to="/logout" className="btn btn--solid btn--error">
							Logout
						</Link>
					</ButtonGroup>
				</div>
			</div>
		);
		// % protected region % [Override contents here] end

		return contents;
	}

	// % protected region % [Add class methods here] off begin
	// % protected region % [Add class methods here] end
}

// % protected region % [Add extra features here] off begin
// % protected region % [Add extra features here] end
