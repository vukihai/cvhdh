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
// % protected region % [Override flatpicker theme here] off begin
import 'flatpickr/dist/themes/material_green.css'
// % protected region % [Override flatpicker theme here] end
import { DateTimePicker, IDateTimePickerProps } from '../DateTimePicker/DateTimePicker';

/** TimePicker-specific properties. Extend as necessary. */
export interface ITimePickerProps<T> extends IDateTimePickerProps<T> {}

/**
 * TimePicker Component. Wraps DateTimePicker, which in turn wraps Flatpickr.
 * See IDateTimePickerProps for root property definitions. Can pass Flatpickr
 * properties that are not implemented by this interface via this.flatpickrProps.
 */
@observer
export class TimePicker<T> extends React.Component<ITimePickerProps<T>> {

	public render() {

		return (
			<DateTimePicker
				/* The two options below are only applied if the humanReadable
				 * property is set to true on Component instantiation.
				 * Displays the time in 12hr format. Default is 24hr. */
				altFormat="h:i K"
				dateFormat="H:i"
				/* Set the Flatpickr to allow selection of a single time only. */
				mode="time"
				noCalendar={true}
				{...this.props}
			/>
		);
	}
}