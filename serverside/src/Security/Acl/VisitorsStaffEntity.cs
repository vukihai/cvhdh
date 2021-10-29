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
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firstapp2257.Models;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Security.Acl
{
	/// <summary>
	/// Security listing for the Visitors group for the StaffEntity scheme
	/// </summary>
	public class VisitorsStaffEntity : IAcl
	{
		public string Group => "Visitors";
		public bool IsVisitorAcl => true;

		public bool GetCreate(User user, IEnumerable<IAbstractModel> models, SecurityContext context)
		{
			// % protected region % [Override create rule contents here here] off begin
			return false;
			// % protected region % [Override create rule contents here here] end
		}

		public Expression<Func<TModel, bool>> GetReadConditions<TModel>(User user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new()
		{
			// % protected region % [Override read rule contents here here] off begin
			return model => true;
			// % protected region % [Override read rule contents here here] end
		}

		public Expression<Func<TModel, bool>> GetUpdateConditions<TModel>(User user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new()
		{
			// % protected region % [Override conditional update rule contents here here] off begin
			return model => false;
			// % protected region % [Override conditional update rule contents here here] end
		}

		public Expression<Func<TModel, bool>> GetDeleteConditions<TModel>(User user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new()
		{
			// % protected region % [Override conditional delete rule contents here here] off begin
			return model => false;
			// % protected region % [Override conditional delete rule contents here here] end
		}

		public bool GetUpdate(User user, IEnumerable<IAbstractModel> models, SecurityContext context)
		{
			// % protected region % [Override update rule contents here here] off begin
			return false;
			// % protected region % [Override update rule contents here here] end
		}

		public bool GetDelete(User user, IEnumerable<IAbstractModel> models, SecurityContext context)
		{
			// % protected region % [Override delete rule contents here here] off begin
			return false;
			// % protected region % [Override delete rule contents here here] end
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			// Acls have no setable properties so if they are the same type then they will be the same
			return GetType() == obj.GetType();
		}

		public override int GetHashCode()
		{
			return GetType().GetHashCode();
		}

		// % protected region % [Add any extra ACL functions here] off begin
		// % protected region % [Add any extra ACL functions here] end
	}
}