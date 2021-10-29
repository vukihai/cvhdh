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
import { useState, useEffect } from "react";
import { unstable_batchedUpdates } from 'react-dom';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise useAsyncLoading here] off begin
type useAsyncLoading = {
	type: 'loading',
	error: undefined,
	data: undefined,
}
// % protected region % [Customise useAsyncLoading here] end

// % protected region % [Customise useAsyncError here] off begin
type useAsyncError<E> = {
	type: 'error',
	error: E,
	data: undefined,
}
// % protected region % [Customise useAsyncError here] end

// % protected region % [Customise useAsyncData here] off begin
type useAsyncData<T> = {
	type: 'data',
	error: undefined,
	data: T,
}
// % protected region % [Customise useAsyncData here] end

// % protected region % [Customise useAsyncResponse here] off begin
type useAsyncResponse<T, E> = useAsyncLoading | useAsyncError<E> | useAsyncData<T>;
// % protected region % [Customise useAsyncResponse here] end

// % protected region % [Customise useAsync here] off begin
export function useAsync<T = any, E = any>(fn: () => Promise<T>, deps?: readonly any[]): useAsyncResponse<T, E> {
	const [data, setData] = useState<T>();
	const [error, setError] = useState<E>();
	const [type, setType] = useState<'loading' | 'error' | 'data'>('loading');

	useEffect(() => {
		setType('loading');
		fn()
			.then(x => {
				unstable_batchedUpdates(() => {
					setError(undefined);
					setData(x);
					setType('data');
				});
			})
			.catch(x => {
				unstable_batchedUpdates(() => {
					setError(x);
					setData(undefined);
					setType('error');
				});
			})
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [ ...(deps ?? []) ]);

	// Cast to any since we know better than the compiler in this case. Since the function has an explicit return type
	// the function will be externally consistent.
	// eslint-disable-next-line  @typescript-eslint/no-explicit-any
	return { error, data, type } as any;
}
// % protected region % [Customise useAsync here] end
