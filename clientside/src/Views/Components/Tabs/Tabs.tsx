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
import { observable, action, computed } from 'mobx';
import classNames from 'classnames';
import { Button, Display } from '../Button/Button';
// % protected region % [Add any extra index imports here] off begin
// % protected region % [Add any extra index imports here] end

// % protected region % [Override ITabConfig here] off begin
export interface ITabConfig {
	name: string,
	component: React.ReactNode,
	key: string,
	className?: string
};
// % protected region % [Override ITabConfig here] end

// % protected region % [Override ITabsProps here] off begin
export interface ITabsProps {
	tabs: Array<ITabConfig>;
	className?: string;
	innerProps?: React.DetailedHTMLProps<React.HTMLAttributes<HTMLElement>, HTMLElement>;
	defaultTab?: number;
	currentTab?: number;
	onTabClicked?: (tabIndex: number, event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
}
// % protected region % [Override ITabsProps here] end

// % protected region % [Override ITabHeaderProps here] off begin
interface ITabHeaderProps {
	tabState: { currentTab: number };
	tabIdx: number;
	tabChanged?: (() => void);
	onTabClicked?: (tabIndex: number, event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
}
// % protected region % [Override ITabHeaderProps here] end

// % protected region % [Add any extra interfaces here] off begin
// % protected region % [Add any extra interfaces here] end

class TabHeader extends React.Component<ITabHeaderProps, any> {
	// % protected region % [Override TabHeader render here] off begin
	public render() {
		return <Button display={Display.Text} onClick={this.onTabClicked}>{this.props.children}</Button>;
	}
	// % protected region % [Override TabHeader render here] end

	// % protected region % [Override TabHeader onTabClicked here] off begin
	@action
	private onTabClicked = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
		if(this.props.tabState.currentTab !== this.props.tabIdx && this.props.tabChanged){
			this.props.tabChanged();
		}
		this.props.tabState.currentTab = this.props.tabIdx;
		if (this.props.onTabClicked) {
			this.props.onTabClicked(this.props.tabIdx, event);
		}
	}
	// % protected region % [Override TabHeader onTabClicked here] end
	// % protected region % [Add any extra TabHeader functions here] off begin
	// % protected region % [Add any extra TabHeader functions here] end
}

@observer
class Tabs extends React.Component<ITabsProps, any> {
	// % protected region % [Override Tabs tabState here] off begin
	@observable
	public tabState = {
		currentTab: 0,
	};
	// % protected region % [Override Tabs tabState here] end

	// % protected region % [Override get currentTab here] off begin
	@computed
	public get currentTab() {
		return this.props.currentTab || this.tabState.currentTab;
	}
	// % protected region % [Override get currentTab here] end

	// % protected region % [Override Tabs constructor here] off begin
	constructor(props: ITabsProps, context: any) {
		super(props, context);
		if (this.props.defaultTab) {
			this.tabState.currentTab = this.props.defaultTab;
		}
	}
	// % protected region % [Override Tabs constructor here] end

	// % protected region % [Add any extra Tabs functions here] off begin
	// % protected region % [Add any extra Tabs functions here] end

	// % protected region % [Override Tabs render here] off begin
	public render(){
		return (
			<>
				<nav {...this.props.innerProps} className={'tabs ' + (this.props.className ? this.props.className : '')}>
					<ul>
						{this.props.tabs.map((tab, idx) => {
							return (
								<li key={tab.key} className={'tab-header' + (this.currentTab === idx ? ' selected' : '')}>
									<TabHeader tabState={this.tabState} tabIdx={idx} onTabClicked={this.props.onTabClicked}>{tab.name}</TabHeader>
								</li>
							);
						})}
					</ul>
				</nav>
				<div className={classNames(this.props.tabs[this.currentTab].className)}>
					{this.props.tabs[this.currentTab].component}
				</div>
			</>
		);
	}
	// % protected region % [Override Tabs render here] end
}

// % protected region % [Override export Tabs here] off begin
export default Tabs;
// % protected region % [Override export Tabs here] end