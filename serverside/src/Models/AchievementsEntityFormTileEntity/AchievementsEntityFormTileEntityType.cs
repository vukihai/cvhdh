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
using System.Linq;
using Firstapp2257.Graphql.Helpers;
using Firstapp2257.Helpers;
using Firstapp2257.Services;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Firstapp2257.Models
{
	/// <summary>
	/// The GraphQL type for returning data in GraphQL queries
	/// </summary>
	public class AchievementsEntityFormTileEntityType : ObjectGraphType<AchievementsEntityFormTileEntity>
	{
		public AchievementsEntityFormTileEntityType()
		{
			Description = @"Stores the form entity to form tile mappings";

			// Add model fields to type
			Field(o => o.Id, type: typeof(NonNullGraphType<IdGraphType>));
			Field(o => o.Created, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.Modified, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.Tile, type: typeof(StringGraphType)).Description(@"The tile that the form is contained in");
			// % protected region % [Add any extra GraphQL fields here] off begin
			// % protected region % [Add any extra GraphQL fields here] end

			// Add entity references
			Field(o => o.FormId, type: typeof(IdGraphType));

			// GraphQL reference to entity AchievementsEntity via reference Form
			Field<AchievementsEntityType, AchievementsEntity>()
				.Name("Form")
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddBatchLoader<Guid, AchievementsEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetFormForAchievementsEntityFormTileEntity",
						async keys =>
						{
							var results = await QueryHelpers.BuildQueryResolver<AchievementsEntity>(
								context,
								x => keys.Contains(x.Id));
							return results.ToDictionary(x => x.Id, x => x);
						});

					return loader.LoadAsync(context.Source.FormId);
				});

			// % protected region % [Add any extra GraphQL references here] off begin
			// % protected region % [Add any extra GraphQL references here] end
		}
	}

	/// <summary>
	/// The GraphQL input type for mutation input
	/// </summary>
	public class AchievementsEntityFormTileEntityInputType : InputObjectGraphType<AchievementsEntityFormTileEntity>
	{
		public AchievementsEntityFormTileEntityInputType()
		{
			Name = "AchievementsEntityFormTileEntityInput";
			Description = "The input object for adding a new AchievementsEntityFormTileEntity";

			// Add entity fields
			Field<IdGraphType>("Id");
			Field<DateTimeGraphType>("Created");
			Field<DateTimeGraphType>("Modified");
			Field<StringGraphType>("Tile").Description = @"The tile that the form is contained in";

			// Add entity references
			Field<IdGraphType>("FormId");

			// Add references to foreign models to allow nested creation
			Field<AchievementsEntityInputType>("Form");

			// % protected region % [Add any extra GraphQL input fields here] off begin
			// % protected region % [Add any extra GraphQL input fields here] end
		}
	}

}