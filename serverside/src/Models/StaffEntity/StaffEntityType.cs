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
using Firstapp2257.Enums;
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
	public class StaffEntityType : ObjectGraphType<StaffEntity>
	{
		public StaffEntityType()
		{

			// Add model fields to type
			Field(o => o.Id, type: typeof(NonNullGraphType<IdGraphType>));
			Field(o => o.Created, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.Modified, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.Email, type: typeof(StringGraphType));
			Field(o => o.FirstName, type: typeof(StringGraphType));
			Field(o => o.LastName, type: typeof(StringGraphType));
			Field(o => o.Role, type: typeof(EnumerationGraphType<StaffRoles>));
			Field(o => o.ContactNumber, type: typeof(StringGraphType));
			// % protected region % [Add any extra GraphQL fields here] off begin
			// % protected region % [Add any extra GraphQL fields here] end

			// Add entity references
			Field(o => o.AddressId, type: typeof(IdGraphType));

			// GraphQL reference to entity AssessmentNotesEntity via reference AssessmentNotes
			Field<ListGraphType<NonNullGraphType<AssessmentNotesEntityType>>, IEnumerable<AssessmentNotesEntity>>()
				.Name("AssessmentNotess")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid?, AssessmentNotesEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetAssessmentNotessForStaffEntity",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<AssessmentNotesEntity>(context, new ReadOptions {DisableAudit = true});
							var results = await query
								.Where(x => x.StaffId.HasValue && keys.Contains(x.StaffId))
								.Select(x => x.StaffId.Value)
								.Distinct()
								.SelectMany(x => query
									.Where(y => y.StaffId == x)
									.AddIdCondition(args.Id)
									.AddIdsCondition(args.Ids)
									.AddWhereFilter(args.Where)
									.AddConditionalWhereFilter(args.Conditions)
									.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
									.AddOrderBys(args.OrderBy)
									.AddSkip(args.Skip)
									.AddTake(args.Take))
								.ToListAsync(context.CancellationToken);
							return results.ToLookup(x => x.StaffId, x => x);
						});

					return loader.LoadAsync(context.Source.Id);
				});

			// GraphQL reference to entity AddressEntity via reference Address
			Field<AddressEntityType, AddressEntity>()
				.Name("Address")
				.ResolveAsync(async context =>
				{
					if (!context.Source.AddressId.HasValue)
					{
						return null;
					}

					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddBatchLoader<Guid?, AddressEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetAddressForStaffEntity",
						async keys =>
						{
							var results = await QueryHelpers.BuildQueryResolver<AddressEntity>(
								context,
								x => keys.Contains(x.Id));
							return results.ToDictionary(x => new Guid?(x.Id), x => x);
						});

					return loader.LoadAsync(context.Source.AddressId);
				});

			// % protected region % [Add any extra GraphQL references here] off begin
			// % protected region % [Add any extra GraphQL references here] end
		}
	}

	/// <summary>
	/// The GraphQL input type for mutation input
	/// </summary>
	public class StaffEntityInputType : InputObjectGraphType<StaffEntity>
	{
		public StaffEntityInputType()
		{
			Name = "StaffEntityInput";
			Description = "The input object for adding a new StaffEntity";

			// Add entity fields
			Field<IdGraphType>("Id");
			Field<DateTimeGraphType>("Created");
			Field<DateTimeGraphType>("Modified");
			Field<StringGraphType>("FirstName");
			Field<StringGraphType>("LastName");
			Field<EnumerationGraphType<StaffRoles>>("Role");
			Field<StringGraphType>("ContactNumber");

			// Add entity references
			Field<IdGraphType>("AddressId");

			// Add references to foreign models to allow nested creation
			Field<ListGraphType<AssessmentNotesEntityInputType>>("AssessmentNotess");
			Field<AddressEntityInputType>("Address");

			// % protected region % [Add any extra GraphQL input fields here] off begin
			// % protected region % [Add any extra GraphQL input fields here] end
		}
	}

	/// <summary>
	/// The GraphQL input type for creating a user entity
	/// </summary>
	public class StaffEntityCreateInputType : InputObjectGraphType<StaffEntity>
	{
		public StaffEntityCreateInputType()
		{
			Name = "StaffEntityCreateInput";
			Description = "The input object for creating a new StaffEntity";

			// Add entity fields
			Field<IdGraphType>("Id");
			Field<DateTimeGraphType>("Created");
			Field<DateTimeGraphType>("Modified");

			// Add fields specific to a user entity
			Field<StringGraphType>("Email");
			Field<StringGraphType>("Password");

			Field<StringGraphType>("FirstName");
			Field<StringGraphType>("LastName");
			Field<EnumerationGraphType<StaffRoles>>("Role");
			Field<StringGraphType>("ContactNumber");

			// Add entity references
			Field<IdGraphType>("AddressId");


			// Add references to foreign models to allow nested creation
			Field<ListGraphType<AssessmentNotesEntityInputType>>("AssessmentNotess");
			Field<AddressEntityInputType>("Address");

			// % protected region % [Add any extra GraphQL create input fields here] off begin
			// % protected region % [Add any extra GraphQL create input fields here] end
		}
	}
}