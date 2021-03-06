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
import { IModelType, Model } from 'Models/Model';
// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

// % protected region % [Customise EntityFactory here] off begin
export class EntityFactory<T extends Model> {
	private _useAttributes: boolean = true;
	private _userReferences: boolean = true;
	private _totalEntities : number = 1;
	constructor(private model: IModelType<T>) {
	}
	public SetAmount = (totalEntities: number) => {
		this._totalEntities = totalEntities;
	};
	public UseAttributes = (enabled? :boolean) : EntityFactory<T> => {
		this._useAttributes = !(enabled === false);
		return this;
	};

	public UseReferences = (enabled? :boolean) : EntityFactory<T> => {
		this._userReferences = !(enabled === false);
		return this;
	};
	public Generate = (): T[] => [ new this.model() ];
}
// % protected region % [Customise EntityFactory here] end

// % protected region % [Customise placeholder test here] off begin
// Add placeholder test so yarn test doesn't throw empty test file exception
describe('Place Holder', function () {
	it('placeholder', () => {
		expect(1).toEqual(1);
	})
});
// % protected region % [Customise placeholder test here] end

// % protected region % [Add extra methods here] off begin
// % protected region % [Add extra methods here] end
