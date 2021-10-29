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
import SecuredPage from 'Views/Components/Security/SecuredPage';
import { Combobox } from 'Views/Components/Combobox/Combobox';
import { observer } from 'mobx-react';
import { RouteComponentProps } from 'react-router';
import { observable } from 'mobx';
import { getFrontendNavLinks } from 'Views/FrontendNavLinks';
import Navigation, { Orientation } from 'Views/Components/Navigation/Navigation';

// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export interface MainMenuPageProps extends RouteComponentProps {
	// % protected region % [Add any extra props here] off begin
	// % protected region % [Add any extra props here] end
}

@observer
// % protected region % [Add any customisations to default class definition here] off begin
class MainMenuPage extends React.Component<MainMenuPageProps> {
// % protected region % [Add any customisations to default class definition here] end

	@observable
	model = {
		myProfile: '',
	}

	// % protected region % [Add class properties here] off begin
	// % protected region % [Add class properties here] end

	render() {
		// % protected region % [Add logic before rendering contents here] off begin
		// % protected region % [Add logic before rendering contents here] end

		let contents = (
			<SecuredPage>
				{
				// % protected region % [Alter navigation here] off begin
				}
				<Navigation
					linkGroups={getFrontendNavLinks(this.props)}
					orientation={Orientation.VERTICAL}
					match={this.props.match}
					location={this.props.location}
					history={this.props.history}
					staticContext={this.props.staticContext}
				/>
				{
				// % protected region % [Alter navigation here] end
				}
				<div className="body-content">
					<div className="layout__vertical">
						<div className="layout__horizontal">
							<Combobox
								model={this.model}
								modelProperty="myProfile"
								label="My profile"
								options={[
									{display: 'Thông tin cá nhân', value: 'ThôngTinCáNhân'},
									{display: 'Đổi mật khẩu', value: 'ĐổiMậtKhẩu'},
									{display: 'Đăng xuất', value: 'ĐăngXuất'},
								]} 
							/>
							<a href="">
								sửa thông tin
							</a>
							<a href="">
								assignment
							</a>
							<a href="">
								thông tin học viên
							</a>
							<p>
								Trang Chủ
							</p>
						</div>
					</div>
					<div className="layout__vertical">
						<div className="layout__vertical">
							<p>
								List assignment
							</p>
							<ol>
								<li>assignment 1</li>
								<li>assignment 2</li>
								<li>assignment 3</li>
							</ol>
						</div>
					</div>
				</div>
			</SecuredPage>
		);

		// % protected region % [Override contents here] off begin
		// % protected region % [Override contents here] end

		return contents;
	}
}

// % protected region % [Override export here] off begin
export default MainMenuPage;
// % protected region % [Override export here] end
