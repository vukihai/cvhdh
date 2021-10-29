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
import { TextField } from 'Views/Components/TextBox/TextBox';
import { DisplayType } from 'Views/Components/Models/Enums';
import { RadioButtonGroup } from 'Views/Components/RadioButton/RadioButtonGroup';
import { observable, runInAction } from 'mobx';
import { Checkbox } from 'Views/Components/Checkbox/Checkbox';
import { TextArea } from 'Views/Components/TextArea/TextArea';
import { DatePicker } from 'Views/Components/DatePicker/DatePicker';
import { DateRangePicker } from 'Views/Components/DateRangePicker/DateRangePicker';
import { DateTimePicker } from 'Views/Components/DateTimePicker/DateTimePicker';
import { DateTimeRangePicker } from 'Views/Components/DateTimeRangePicker/DateTimeRangePicker';
import { TimePicker } from 'Views/Components/TimePicker/TimePicker';
import { Combobox } from 'Views/Components/Combobox/Combobox';
import { MultiCombobox } from 'Views/Components/Combobox/MultiCombobox';
import { CheckboxGroup } from 'Views/Components/Checkbox/CheckboxGroup';
import Collection from 'Views/Components/Collection/Collection';
import { Breadcrumbs } from 'Views/Components/Breadcrumbs/Breadcrumbs';
import { ContextMenu } from 'Views/Components/ContextMenu/ContextMenu';
import { contextMenu } from 'react-contexify';
import { ButtonGroup, Alignment, Sizing } from 'Views/Components/Button/ButtonGroup';
import Modal from 'Views/Components/Modal/Modal';
import { confirmModal, alertModal } from 'Views/Components/Modal/ModalUtils';
import alert from 'Util/ToastifyUtils';
import { NumberTextField } from 'Views/Components/NumberTextBox/NumberTextBox';
import Tabs from 'Views/Components/Tabs/Tabs';
import { Button, Colors, Display, Sizes } from 'Views/Components/Button/Button';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export interface StyleguidePageProps extends RouteComponentProps {
	// % protected region % [Add any extra props here] off begin
	// % protected region % [Add any extra props here] end
}

@observer
// % protected region % [Add any customisations to default class definition here] off begin
class StyleguidePage extends React.Component<StyleguidePageProps> {
// % protected region % [Add any customisations to default class definition here] end

	@observable
	private model = {
		email: 'admin@example.com',
		date: new Date(),
		range: [new Date(), new Date()],
	};
	@observable
	private modelEmpty = {};
	@observable
	private collectionSearch = '';
	@observable
	private collectionData = [
		{ id: "1", title: "Hello 1", description: "world 1" },
		{ id: "2", title: "Hello 2", description: "world 2" },
		{ id: "3", title: "Hello 3", description: "world 3" },
		{ id: "4", title: "Hello 4", description: "world 4" },
		{ id: "5", title: "Hello 5", description: "world 5" },
	];
	@observable
	private modalState = {
		firstOpen: false,
	};

	// % protected region % [Add class properties here] off begin
	// % protected region % [Add class properties here] end

	render() {

		// % protected region % [Add logic before rendering contents here] off begin
		// % protected region % [Add logic before rendering contents here] end

		let contents = (
			<div className="tile">
				<div className="style-guide">
					<div className="elements" >
						<h3>Headings</h3>
						<h1>Heading 1</h1>
						<h2>Heading 2</h2>
						<h3>Heading 3</h3>
						<h4>Heading 4</h4>
						<h5>Heading 5</h5>
						<h6>Heading 6</h6>



						<h3>Text Tags</h3>
						<p className="txt-body">Just the regular old body text within the application. Just the regular old body text within the application. Just the regular old body text within the application. Just the regular old body text within the application. Just the regular old body text within the application. Just the regular old body text within the application. Just the regular old body text within the application.</p>
						<p className="txt-sm-body">The smaller body text within the application. The smaller body text within the application. The smaller body text within the application. The smaller body text within the application. The smaller body text within the application. The smaller body text within the application. The smaller body text within the application. The smaller body text within the application. The smaller body text within the application. The smaller body text within the application. The smaller body text within the application.</p>
						<p className="txt-lg-body">The larger body text within the application. The larger body text within the application. The larger body text within the application. The larger body text within the application. The larger body text within the application. The larger body text within the application. The larger body text within the application. The larger body text within the application. The larger body text within the application.</p>
						<p className="txt-italics">The text which will be italics within the application. The text which will be italics within the application. The text which will be italics within the application. The text which will be italics within the application. The text which will be italics within the application. The text which will be italics within the application. The text which will be italics within the application.</p>
						<p className="txt-bold">The text which will be bold within the application. The text which will be bold within the application. The text which will be bold within the application. The text which will be bold within the application. The text which will be bold within the application. The text which will be bold within the application. The text which will be bold within the application.</p>

						<h3>Lists</h3>
						<ol>
							<li>Ordered Item</li>
							<li>Ordered Item</li>
							<ul>
								<li>Unordered Item</li>
								<li>Unordered Item</li>
							</ul>
							<li>Ordered Item</li>
							<li>Ordered Item
							<ol>
									<li>Ordered Item</li>
									<li>Ordered Item</li>
								</ol>
							</li>
						</ol>

						<ul>
							<li>Unordered Item</li>
							<li>Unordered Item</li>
							<ul>
								<li>Unordered Item</li>
								<li>Unordered Item</li>
							</ul>
							<li>Unordered Item</li>
							<li>Unordered Item</li>
						</ul>

						<ul className="list-unstyled">
							<li>Unstyled Item</li>
							<li>Unstyled Item</li>
							<ul className="list-unstyled">
								<li>Indented Unstyled Item</li>
								<li>Indented Unstyled Item</li>
								<li>Indented Unstyled Item</li>
							</ul>
							<li>Unstyled Item</li>
							<li>Unstyled Item</li>
						</ul>

						<h3>Links</h3>
						<div>
							<a href="elements" className="link-sm" >Small Link</a>
						</div>
						<div>
							<a href="elements">Medium Link</a>
						</div>
						<div>
							<a href="elements" className="link-md" >Large Link</a>
						</div>
						<div>
							<a href="elements" className="link-italics" >Italics Link</a>
						</div>
						<div>
							<a href="elements" className="link-bold" >Bold Link</a>
						</div>
						<div>
							<a href="elements" className="link-rm-txt-dec" >No Style Link</a>
						</div>

						<h3>Text Field</h3>
						<TextField model={this.model} modelProperty="email"
							id='IdEmail'
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="General Textfield"
							labelVisible={true}
						>
						</TextField>
						<TextField model={this.model} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Required Textfield"
							isRequired={true}
						>
						</TextField>
						<TextField model={this.model} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Not Required Text Field"
							isRequired={false}
						>
						</TextField>
						<TextField model={this.model} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Disabled Textfield"
							isDisabled={true}
						>
						</TextField>
						<TextField model={this.model} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Readonly Textfield"
							isReadOnly={true}
						>
						</TextField>
						<TextField model={this.model} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Textfield with Tooltip"
							tooltip={"Toolitp for Email"}
						>
						</TextField>
						<TextField model={this.modelEmpty} modelProperty="email"
							name='otherExample'
							className="class-other"
							displayType={DisplayType.BLOCK}
							label="Placeholder for Textfield"
							placeholder={'Placeholder for Textfield would go here'}
						>
						</TextField>
						<TextField model={this.modelEmpty} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Clearable Textfield"
							clickToClear={true}
						>
						</TextField>
						<NumberTextField
							model={this.model}
							modelProperty="mynumber"
							label="Number Text Field" />
						<NumberTextField
							model={this.model}
							modelProperty="mynumber"
							label="Clearable Number Text Field"
							clickToClear={true} />
						<TextField model={this.modelEmpty} modelProperty="email"
							name='noLabelTextfield'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="No Label Textfield"
							placeholder="No Label Textfield"
							labelVisible={false}
						>
						</TextField>
						<TextField model={this.modelEmpty} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.INLINE}
							label="Inline Textfield"
						>
						</TextField>
						<TextField model={this.modelEmpty} modelProperty="email"
							name='NameEmail'
							className="class-email"
							label="Static Textfield"
							staticInput={true}
						>
						</TextField>

						<h3>TextArea</h3>
						<TextArea model={this.model} modelProperty="email"
							id='IdEmail'
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="General Textarea"
							labelVisible={true}
						>
						</TextArea>
						<TextArea model={this.model} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Required Textarea"
							isRequired={true}
						>
						</TextArea>
						<TextArea model={this.model} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Not Required Textarea"
							isRequired={false}
						>
						</TextArea>
						<TextArea model={this.model} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Disabled Textarea"
							isDisabled={true}
						>
						</TextArea>
						<TextArea model={this.model} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Readonly Textarea"
							isReadOnly={true}
						>
						</TextArea>
						<TextArea model={this.model} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Textarea with Tooltip"
							tooltip={"Toolitp for Email"}
						>
						</TextArea>
						<TextArea model={this.modelEmpty} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Textarea with Placeholder"
							placeholder={'Placeholder for Email'}
						>
						</TextArea>
						<TextArea model={this.modelEmpty} modelProperty="email"
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="Label Email"
							placeholder="Textarea without Label"
							labelVisible={false}
						>
						</TextArea>

						<h3>Dropdown</h3>
						<Combobox model={this.model}
							modelProperty="DropdownModelProperty"
							label={"Regular Dropdown"}
							options={[
								{ display: "Hello", value: "hello" },
								{ display: "World", value: "world" },
								{ display: "Lorem", value: "lorem" },
								{ display: "Ipsum", value: "ipsum" },
							]}
							searchable={false}
						/>

						<Combobox model={this.model}
							modelProperty="DropdownModelProperty"
							label={"Searchable Dropdown"}
							options={[
								{ display: "Hello", value: "hello" },
								{ display: "World", value: "world" },
								{ display: "Lorem", value: "lorem" },
								{ display: "Ipsum", value: "ipsum" },
							]}
						/>

						<MultiCombobox
							model={this.model}
							modelProperty="MutliDropdownProperty"
							label="Multi Dropdown"
							options={[
								{ display: "Hello", value: "hello" },
								{ display: "World", value: "world" },
								{ display: "Lorem", value: "lorem" },
								{ display: "Ipsum", value: "ipsum" },
							]} />

						<Combobox model={this.model}
							modelProperty="DropdownModelProperty"
							label={"Disabed Dropdown"}
							options={[
								{ display: "Hello", value: "hello" },
								{ display: "World", value: "world" },
								{ display: "Lorem", value: "lorem" },
								{ display: "Ipsum", value: "ipsum" },
							]}
							isDisabled={true}
						/>

						<Combobox model={this.model}
							modelProperty="DropdownModelProperty"
							label={"Disabed Dropdown"}
							options={[
								{ display: "Hello", value: "hello" },
								{ display: "World", value: "world" },
								{ display: "Lorem", value: "lorem" },
								{ display: "Ipsum", value: "ipsum" },
								{ display: "maximus", value: "maximus" },
								{ display: "turpi", value: "turpi" },
								{ display: "consectetur", value: "consectetur" },
								{ display: "adipiscing", value: "adipiscing" },
								{ display: "elit", value: "elit" },
								{ display: "Cras", value: "Cras" },
								{ display: "lobortis", value: "lobortis" },
								{ display: "ultrices", value: "ultrices" },
							]}
							inputProps={{ isMulti: true }}
							isDisabled={true}
						/>

						<h3>Radio Buttons</h3>
						<RadioButtonGroup name="group1" model={this.model}
							modelProperty={"RadioButtonMP1"}
							options={[
								{ value: "test1", display: "General" },
								{ value: "test2", display: "General" },
								{ value: "test3", display: "General" },
								{ value: "test4", display: "General" },
							]}
						/>

						<RadioButtonGroup name="group2" model={this.model}
							modelProperty={"RadioButtonMP2"}
							options={[
								{ value: "test11", display: "Disabled" },
								{ value: "test12", display: "Disabled" },
								{ value: "test13", display: "Disabled" },
								{ value: "test14", display: "Disabled" },
							]}
							isDisabled={true}
						/>

						<RadioButtonGroup name="group3" model={this.model}
							modelProperty={"RadioButtonMP3"}
							options={[
								{ value: "test21", display: "Read Only" },
								{ value: "test22", display: "Read Only" },
								{ value: "test23", display: "Read Only" },
								{ value: "test24", display: "Read Only" },
							]}
							isReadOnly={true}
						/>

						<RadioButtonGroup name="group3" model={this.model}
							modelProperty={"RadioButton123"}
							label="Radio Group Label with Tooltip"
							options={[
								{ value: "test211", display: "General" },
								{ value: "test221", display: "General" },
								{ value: "test231", display: "General" },
								{ value: "test241", display: "General" },
							]}
							tooltip="Let me help you friend"
						/>

						<RadioButtonGroup name="group3" model={this.model}
							modelProperty={"RadioButtonMP3"}
							label="Radio Group Label with Readonly Radio Buttons"
							options={[
								{ value: "test21", display: "Read Only" },
								{ value: "test22", display: "Read Only" },
								{ value: "test23", display: "Read Only" },
								{ value: "test24", display: "Read Only" },
							]}
							isReadOnly={true}
						/>

						<h3>Checkbox</h3>
						<CheckboxGroup
							label="Header for this checkbox group">
							<Checkbox model={this.model}
								modelProperty={"hfg"}
								label={"General Checkbox"}
								tooltip={"tooltip!"}
							>
							</Checkbox>
							<Checkbox model={this.model}
								modelProperty={"abc"}
								label={"General Checkbox"}
							>
							</Checkbox>
							<Checkbox model={this.model}
								modelProperty={"qwe"}
								label={"General Checkbox"}
							>
							</Checkbox>
						</CheckboxGroup>

						<Checkbox model={this.model}
							modelProperty={"ghfg"}
							label={"General Checkbox"}
						>
						</Checkbox>

						<Checkbox model={this.model}
							modelProperty={"jhjgh"}
							label={"Disabled Checkbox"}
							isDisabled={true}
						>
						</Checkbox>

						<Checkbox model={this.model}
							modelProperty={"dhdfh"}
							label={"Readonly Checkbox"}
							isReadOnly={true}
						>
						</Checkbox>

						<Checkbox model={this.model}
							modelProperty={"jgfj"}
							label={"Checkbox with Tooltip"}
							tooltip={"tooltip!"}
						>
						</Checkbox>

						<h3>Date Picker</h3>
						<DatePicker model={this.model} modelProperty="date"
							name='NameEmail'
							className="general-date-picker"
							displayType={DisplayType.BLOCK}
							label="General Date Picker"
							labelVisible={true}
						></DatePicker>

						<DatePicker model={this.model} modelProperty="date"
							name='NameEmail'
							className="required-date-picker"
							displayType={DisplayType.BLOCK}
							label="Required Date Picker"
							isRequired={true}
						></DatePicker>

						<DatePicker model={this.model} modelProperty="date"
							name='NameEmail'
							className="disabled-date-picker"
							displayType={DisplayType.BLOCK}
							label="Disabled Date Picker"
							isDisabled={true}
						></DatePicker>

						<DatePicker model={this.model} modelProperty="date"
							name='NameEmail'
							className="readonly-date-picker"
							displayType={DisplayType.BLOCK}
							label="Readonly Date Picker"
							isReadOnly={true}
						></DatePicker>

						<DatePicker model={this.model} modelProperty="date"
							name='NameEmail'
							className="tooltip-date-picker"
							displayType={DisplayType.BLOCK}
							label="Date Picker with Tooltip"
							tooltip={"Toolitp for Email"}
						></DatePicker>

						<DatePicker model={this.modelEmpty} modelProperty="date"
							name='NameEmail'
							className="placeholder-date-picker"
							displayType={DisplayType.BLOCK}
							label="Date Picker with Placeholder"
							placeholder={'15/02/1992'}
						></DatePicker>

						<DatePicker model={this.modelEmpty} modelProperty="date"
							name='NameEmail'
							className="no-label-date-picker"
							displayType={DisplayType.BLOCK}
							label="Date Picker without Label"
							labelVisible={false}
						></DatePicker>

						<h3>Date Range Picker</h3>
						<DateRangePicker model={this.model} modelProperty="range"
							name='NameEmail'
							className="general-date-range-picker"
							displayType={DisplayType.BLOCK}
							label="General Date Range Picker"
							labelVisible={true}
						></DateRangePicker>

						<DateRangePicker model={this.model} modelProperty="range"
							name='NameEmail'
							className="restricted-date-range-picker"
							displayType={DisplayType.BLOCK}
							label="Restricted Date Range Picker"
							labelVisible={true}
							minDate="today"
							maxDate={((new Date().getDate()) + 14).toString()}
						></DateRangePicker>

						<h3>Date-Time Picker</h3>
						<DateTimePicker model={this.model} modelProperty="date"
							name='NameEmail'
							className="date-time-hf-picker"
							displayType={DisplayType.BLOCK}
							label="Date-Time Picker With Human-Friendly Format"
							labelVisible={true}
							humanFriendly={true}
						></DateTimePicker>

						<h3>Date-Time Range Picker</h3>
						<DateTimeRangePicker model={this.model} modelProperty="range"
							name='NameEmail'
							className="general-date-time-range-picker"
							displayType={DisplayType.BLOCK}
							label="General Date-Time Range Picker"
							labelVisible={true}
						></DateTimeRangePicker>

						<h3>Time Picker</h3>
						<TimePicker model={this.model} modelProperty="date"
							name='NameEmail'
							className="general-time-picker"
							displayType={DisplayType.BLOCK}
							label="General Time Picker"
							labelVisible={true}
						>
						</TimePicker>
						<TimePicker model={this.model} modelProperty="date"
							name='NameEmail'
							className="required-time-picker"
							displayType={DisplayType.BLOCK}
							label="Required Time Picker"
							isRequired={true}
						>
						</TimePicker>
						<TimePicker model={this.model} modelProperty="date"
							name='NameEmail'
							className="disabled-time-picker"
							displayType={DisplayType.BLOCK}
							label="Disabled Time Picker"
							isDisabled={true}
						>
						</TimePicker>
						<TimePicker model={this.model} modelProperty="date"
							name='NameEmail'
							className="readonly-time-picker"
							displayType={DisplayType.BLOCK}
							label="Readonly Time Picker"
							isReadOnly={true}
							placeholder="HH:MM"
						>
						</TimePicker>

						<TimePicker model={this.model} modelProperty="date"
							name='NameEmail'
							className="tooltip-time-picker"
							displayType={DisplayType.BLOCK}
							label="24hr Time Picker with Tooltip"
							tooltip={"Toolitp for Email"}
							time_24hr={true}
						>
						</TimePicker>

						<TimePicker model={this.modelEmpty} modelProperty="date"
							name='NameEmail'
							className="placeholder-time-picker"
							displayType={DisplayType.BLOCK}
							humanFriendly={true}
							label="24hr Time Picker with Placeholder and Human-Friendly Formatting"
							placeholder={'24hr Time Picker with Placeholder and Human-Friendly Formatting'}
							time_24hr={true}
						>
						</TimePicker>
						<TimePicker model={this.modelEmpty} modelProperty="date"
							name='NameEmail'
							className="no-label-time-picker"
							displayType={DisplayType.BLOCK}
							label="Time picker without label"
							labelVisible={false}
							placeholder="Time Picker without Label"
						>
						</TimePicker>

						<h3>Input Validation</h3>
						<TextField model={this.model} modelProperty="email"
							id='IdEmail'
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="General Textfield"
							labelVisible={true}
							errors={['There appears to be an error here, should probably do something about that', 'There appears to be an error here, should probably do something about that']}
						>
						</TextField>
						<TextArea model={this.model} modelProperty="email"
							id='IdEmail'
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="General Textarea"
							labelVisible={true}
							errors={['There appears to be an error here, should probably do something about that', 'There appears to be an error here, should probably do something about that']}
						>
						</TextArea>
						<DatePicker model={this.model} modelProperty="email"
							id='IdEmail'
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="General Date Picker"
							labelVisible={true}
							errors={['There appears to be an error here, should probably do something about that', 'There appears to be an error here, should probably do something about that']}
						>
						</DatePicker>
						<TimePicker model={this.model} modelProperty="email"
							id='IdEmail'
							name='NameEmail'
							className="class-email"
							displayType={DisplayType.BLOCK}
							label="General Time Picker"
							labelVisible={true}
							errors={['There appears to be an error here, should probably do something about that', 'There appears to be an error here, should probably do something about that']}
						>
						</TimePicker>


						<RadioButtonGroup name="group1" model={this.model}
							modelProperty={"RBMP"}
							options={[
								{ value: "test41", display: "General" },
								{ value: "test42", display: "General" },
								{ value: "test43", display: "General" },
								{ value: "test44", display: "General" },
							]}
							errors={['There appears to be an error here, should probably do something about that', 'There appears to be an error here, should probably do something about that']}
						/>

						<Checkbox model={this.model}
							modelProperty={"abc"}
							label={"General Checkbox"}
						>
						</Checkbox>
						<Checkbox model={this.model}
							modelProperty={"ghfg"}
							label={"General Checkbox"}
							errors={['There appears to be an error here, should probably do something about that', 'There appears to be an error here, should probably do something about that']}
						>
						</Checkbox>
						<Checkbox model={this.model}
							modelProperty={"abc"}
							label={"General Checkbox"}
						>
						</Checkbox>
						<Checkbox model={this.model}
							modelProperty={"abc"}
							label={"General Checkbox"}
						>
						</Checkbox>

						<h3>Buttons</h3>
						<ButtonGroup alignment={Alignment.HORIZONTAL}>
							<Button className="test" display={Display.Solid}>Solid Button</Button>
							<Button className="test" display={Display.Outline}>Outline Button</Button>
							<Button className="test" display={Display.Text}>Text Button</Button>
						</ButtonGroup>

						<h3>Button Colours</h3>
						<ButtonGroup alignment={Alignment.HORIZONTAL}>
							<Button className="test" display={Display.Solid} colors={Colors.Primary}>Primary Button</Button>
							<Button className="test" display={Display.Solid} colors={Colors.Secondary}>Secondary Button</Button>
							<Button className="test" display={Display.Solid} colors={Colors.Success}>Success Button</Button>
							<Button className="test" display={Display.Solid} colors={Colors.Warning}>Warning Button</Button>
							<Button className="test" display={Display.Solid} colors={Colors.Error}>Error Button</Button>
							<Button className="test" display={Display.Solid} colors={Colors.Primary} disabled={true}>Disabled Button</Button>
						</ButtonGroup>

						<h3>Button Outline Colours</h3>
						<ButtonGroup alignment={Alignment.HORIZONTAL}>
							<Button className="test" display={Display.Outline} colors={Colors.Primary}>Primary Button</Button>
							<Button className="test" display={Display.Outline} colors={Colors.Secondary}>Secondary Button</Button>
							<Button className="test" display={Display.Outline} colors={Colors.Success}>Success Button</Button>
							<Button className="test" display={Display.Outline} colors={Colors.Warning}>Warning Button</Button>
							<Button className="test" display={Display.Outline} colors={Colors.Error}>Error Button</Button>
							<Button className="test" display={Display.Outline} colors={Colors.Primary} disabled={true}>Disabled Button</Button>
						</ButtonGroup>

						<h3>Button Text Colours</h3>
						<ButtonGroup alignment={Alignment.HORIZONTAL}>
							<Button className="test" display={Display.Text} colors={Colors.Primary}>Primary Button</Button>
							<Button className="test" display={Display.Text} colors={Colors.Secondary}>Secondary Button</Button>
							<Button className="test" display={Display.Text} colors={Colors.Success}>Success Button</Button>
							<Button className="test" display={Display.Text} colors={Colors.Warning}>Warning Button</Button>
							<Button className="test" display={Display.Text} colors={Colors.Error}>Error Button</Button>
							<Button className="test" display={Display.Text} colors={Colors.Primary} disabled={true}>Disabled Button</Button>
						</ButtonGroup>

						<h3>Button Sizes</h3>
						<ButtonGroup alignment={Alignment.HORIZONTAL}>
							<Button className="test" display={Display.Solid} colors={Colors.Primary} sizes={Sizes.Small}>Small Button</Button>
							<Button className="test" display={Display.Solid} colors={Colors.Primary} sizes={Sizes.Medium}>Medium Button</Button>
							<Button className="test" display={Display.Solid} colors={Colors.Primary} sizes={Sizes.Large}>Large Button</Button>
						</ButtonGroup>

						<h3>Button Icons</h3>
						<ButtonGroup alignment={Alignment.HORIZONTAL}>
							<Button className="test" display={Display.Solid} colors={Colors.Primary} icon={{ icon: "academy", iconPos: 'icon-left' }}>Icon Before Text</Button>
							<Button className="test" display={Display.Outline} colors={Colors.Success} icon={{ icon: "academy", iconPos: 'icon-right' }}>Icon After Text</Button>
							<Button className="test" display={Display.Outline} colors={Colors.Warning} labelVisible={false} icon={{ icon: "academy", iconPos: 'icon-right' }}>No Text</Button>
						</ButtonGroup>

						<h3>Button Groups</h3>
						<ButtonGroup alignment={Alignment.HORIZONTAL}>
							<Button display={Display.Solid} colors={Colors.Primary}>Horizontal Button Group</Button>
							<Button display={Display.Solid} colors={Colors.Success}>Horizontal Button Group</Button>
							<Button display={Display.Outline} colors={Colors.Secondary}>Horizontal Button Group</Button>
						</ButtonGroup>
						<ButtonGroup alignment={Alignment.VERTICAL}>
							<Button display={Display.Solid} colors={Colors.Primary}>Vertical Button Group</Button>
							<Button display={Display.Solid} colors={Colors.Warning}>Vertical Button Group</Button>
							<Button display={Display.Outline} colors={Colors.Error}>Vertical Button Group</Button>
						</ButtonGroup>
						<ButtonGroup sizing={Sizing.GROW}>
							<Button display={Display.Solid} colors={Colors.Primary}>Grown Button Group</Button>
							<Button display={Display.Solid} colors={Colors.Success}>Grown Button Group</Button>
							<Button display={Display.Outline} colors={Colors.Warning}>Grown Button Group</Button>
						</ButtonGroup>

						<h3>Collection</h3>
						<Collection
							collection={this.collectionData}
							headers={[
								{ displayName: "id", name: "id" },
								{ displayName: "title", name: "title" },
								{ displayName: "description", name: "description" },
							]} />

						<h3>Collection with context menu</h3>
						<Collection
							collection={this.collectionData}
							headers={[
								{ displayName: "id", name: "id" },
								{ displayName: "title", name: "title" },
								{ displayName: "description", name: "description" },
							]}
							actionsMore={[
								{ customItem: 'aaaa', onEntityClick: (event, entity) => { console.log(entity) } },
								{
									groupName: "group1", actions: [
										{ label: 'aaaa', icon: 'edit', iconPos: 'icon-left', onEntityClick: (event, entity) => { console.log(entity) } },
										{ customItem: 'bbbb', onEntityClick: (event, entity) => { console.log(entity) } },
										{
											groupName: "group11", actions: [
												{ label: 'aaaa', icon: 'edit', iconPos: 'icon-left', onEntityClick: (event, entity) => { console.log(entity) } },
												{ customItem: 'bbbb', onEntityClick: (event, entity) => { console.log(entity) } }
											]
										},
									]
								},
								{ customItem: 'bbbb', onEntityClick: (event, entity) => { console.log(entity) } }
							]} />

						<h3>Searchable Collection</h3>
						<Collection
							collection={this
								.collectionData
								.filter(model => this.collectionSearch.trim()
									? model.id === this.collectionSearch.trim()
									: true)
							}
							selectableItems={true}
							onSearchTriggered={searchTerm => runInAction(() => this.collectionSearch = searchTerm)}
							idColumn="id"
							headers={[
								{ displayName: "id", name: "id" },
								{ displayName: "title", name: "title" },
								{ displayName: "description", name: "description" },
							]}
							actions={[
								{ label: 'Alert', action: model => alert(`Model ${model.id} alerted`) }
							]}
							expandList={model => "id: " + model.id + " | title: " + model.title + " | description: " + model.description} />


						<Tabs tabs={[
							{ component: "A", name: "A Tab", key: "a" },
							{ component: "B", name: "B Tab", key: "b" },
							{ component: "C", name: "C Tab", key: "c" }
						]} />

						<h3>Modals & Alerts</h3>

						<div>
							<ButtonGroup alignment={Alignment.HORIZONTAL}>
								<Button className="open-custom-modal" display={Display.Solid} colors={Colors.Primary} onClick={() => runInAction(() => this.modalState.firstOpen = true)}>Open custom modal</Button>
								<Button className="open-confirm-modal" display={Display.Solid} colors={Colors.Primary}
										onClick={() => {
											confirmModal('Big decisions await...', 'Yeah or nah?')
												.then(() => console.log('success'))
												.catch(() => console.log('fail'))
										}}>
									Confirm
								</Button>
								<Button className="open-alert-modal" display={Display.Solid} colors={Colors.Primary}
										onClick={() => {
											alertModal('Alerting!', '💯🔥 ARE YOU ALERTED!? 🔥💯')
										}}>
									Alert
								</Button>
							</ButtonGroup>

							<Modal isOpen={this.modalState.firstOpen} label="An example modal!" onRequestClose={() => runInAction(() => this.modalState.firstOpen = false)}>
								<h4>This is some modal content</h4>
								<p>This is a paragraph!!!</p>
								<i>Wow, an {'<i>'} tag</i>
								<Button onClick={() => runInAction(() => this.modalState.firstOpen = false)}>Close</Button>
								<Button className="show-flash-alert-test"
									onClick={() => alert("Success!", 'success', {})}>Flash Alert Integration Test</Button>
								<Button className="show-dismissible-alert-test"
									onClick={() => alert("Error!", 'error', {})}>Dismissible Alert Integration Test</Button>
							</Modal>
						</div>

						<div>
							<Button display={Display.Solid} colors={Colors.Primary} onClick={(e) => contextMenu.show({ id: 'testing-menu', event: e })}>Click for context menu</Button>
							<ContextMenu
								menuId="testing-menu"
								actions={[
									{ label: "ayy", icon: 'hoist', iconPos: 'icon-left', onClick: () => alert("Clicked") },
									{ label: "ohh", onClick: () => alert("Clicked Again") },
									{
										groupName: "Wow", arrow: 'ramp', actions: [
											{ label: 'A sub item', onClick: () => alert("Clicked Again 1") },
											{ label: 'Another sub item', onClick: () => alert("Clicked Again 2") }
										]
									}
								]}
							/>
						</div>

						<div>
							<ButtonGroup alignment={Alignment.HORIZONTAL}>
								<Button className="show-error-alert" display={Display.Solid} colors={Colors.Error}
									onClick={() => alert("Error!", 'error', { autoClose: false })}>Show Error</Button>
								<Button className="show-info-alert" display={Display.Solid} colors={Colors.Primary}
									onClick={() => alert("Info!", 'info', { autoClose: false, position: 'top-center' })}>Show Info</Button>
								<Button className="show-warning-alert" display={Display.Solid} colors={Colors.Warning}
									onClick={() => alert("Warning!", 'warning', { position: 'top-center' })}>Show Warning</Button>
								<Button className="show-success-alert" display={Display.Solid} colors={Colors.Success}
									onClick={() => alert("Success!", 'success', {})}>Show Success</Button>
							</ButtonGroup>
						</div>

						<h3>Breadcrumbs</h3>
						<Breadcrumbs
							className={'breadcrumbs-1'}
							tags={
								[
									{ label: 'customer groups', link: '#', className: "home-level" },
									{ label: 'customers', link: '#', className: "customers-level" },
									{ label: 'customer1', link: '#', className: "customer-level" },
									{ label: 'edit', link: '#', className: "action-level" }
								]
							}
						/>
					</div >
				</div >
			</div>
		);

		// % protected region % [Override contents here] off begin
		// % protected region % [Override contents here] end

		return contents;
	}
}

// % protected region % [Override export here] off begin
export default StyleguidePage;
// % protected region % [Override export here] end