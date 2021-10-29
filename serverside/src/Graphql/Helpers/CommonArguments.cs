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
using Firstapp2257.Models.Internal;
using GraphQL;
using GraphQL.EntityFramework;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Graphql.Helpers
{
	public class CommonArguments
	{
		public Guid? Id { get; set; }
		public List<Guid> Ids { get; set; }
		public List<WhereExpression> Where { get; set; }
		public List<List<WhereExpression>> Conditions { get; set; }
		public int? Skip { get; set; }
		public int? Take { get; set; }
		public List<OrderBy> OrderBy { get; set; }
		public List<List<HasCondition>> Has { get; set; }
		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end

		public CommonArguments()
		{
			// % protected region % [Customise parameterless constructor here] off begin
			// % protected region % [Customise parameterless constructor here] end
		}

		public CommonArguments(IResolveFieldContext context)
		{
			Id = context.GetArgument<Guid?>("id");
			Ids = context.GetArgument<List<Guid>>("ids");
			Where = context.GetArgument<List<WhereExpression>>("where");
			Conditions = context.GetArgument<List<List<WhereExpression>>>("conditions");
			Skip = context.GetArgument<int?>("skip") ?? 0;
			Take = context.GetArgument<int?>("take");
			OrderBy = context.GetArgument<List<OrderBy>>("orderBy");
			Has = context.GetArgument<List<List<HasCondition>>>("has");

			// % protected region % [Customise context constructor here] off begin
			// % protected region % [Customise context constructor here] end
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}