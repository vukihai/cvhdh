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
	public class StudentsEntityFormVersionType : ObjectGraphType<StudentsEntityFormVersion>
	{
		public StudentsEntityFormVersionType()
		{
			Description = @"The form versions for the Students Entity form behaviour";

			// Add model fields to type
			Field(o => o.Id, type: typeof(NonNullGraphType<IdGraphType>));
			Field(o => o.Created, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.Modified, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.Version, type: typeof(IntGraphType)).Description(@"The version number of this form version");
			Field(o => o.FormData, type: typeof(StringGraphType)).Description(@"The form data for this version");
			Field(o => o.FormId, type: typeof(IdGraphType));
			// % protected region % [Add any extra GraphQL fields here] off begin
			// % protected region % [Add any extra GraphQL fields here] end

			// Add entity references
			Field<StudentsEntityType, StudentsEntity>()
				.Name("PublishedForm")
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();
					var loader = accessor.Context.GetOrAddBatchLoader<Guid?, StudentsEntity>(
						"GetPublishedFormForStudentsEntityFromVersion",
						async keys =>
						{
							var results = await QueryHelpers.BuildQueryResolver<StudentsEntity>(
								context,
								x => x.PublishedVersionId.HasValue && keys.Contains(x.PublishedVersionId));
							return results.ToDictionary(x => x.PublishedVersionId, x => x);
						});
					return loader.LoadAsync(context.Source.Id);
				});
			Field<StudentsEntityType, StudentsEntity>()
				.Name("Form")
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();
					var loader = accessor.Context.GetOrAddBatchLoader<Guid, StudentsEntity>(
						"GetFormForStudentsEntityFormVersion",
						async keys =>
						{
							var results = await QueryHelpers.BuildQueryResolver<StudentsEntity>(
								context,
								x => keys.Contains(x.Id));
							return results.ToDictionary(x => x.Id, x => x);
						});
					return loader.LoadAsync(context.Source.FormId);
				});
			Field<ListGraphType<StudentsSubmissionEntityType>, IEnumerable<StudentsSubmissionEntity>>()
				.Name("FormSubmissions")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();
					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid, StudentsSubmissionEntity>(
						"GetFormSubmissionsForStudentsEntityFormVersion",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<StudentsSubmissionEntity>(context, new ReadOptions {DisableAudit = true});
							var results = await query
								.Where(x => keys.Contains(x.FormVersionId))
								.Select(x => x.FormVersionId)
								.Distinct()
								.SelectMany(x => query
									.Where(y => y.FormVersionId == x)
									.AddIdCondition(args.Id)
									.AddIdsCondition(args.Ids)
									.AddWhereFilter(args.Where)
									.AddConditionalWhereFilter(args.Conditions)
									.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
									.AddOrderBys(args.OrderBy)
									.AddSkip(args.Skip)
									.AddTake(args.Take))
								.ToListAsync(context.CancellationToken);
							return results.ToLookup(x => x.FormVersionId, x => x);
						});
					return loader.LoadAsync(context.Source.Id);
				});

			// % protected region % [Add any extra GraphQL references here] off begin
			// % protected region % [Add any extra GraphQL references here] end
		}
	}

	/// <summary>
	/// The GraphQL input type for mutation input
	/// </summary>
	public class StudentsEntityFormVersionInputType : InputObjectGraphType<StudentsEntityFormVersion>
	{
		public StudentsEntityFormVersionInputType()
		{
			Name = "StudentsEntityFormVersionInput";
			Description = "The input object for adding a new StudentsEntityFormVersion";

			// Add entity fields
			Field<IdGraphType>("Id");
			Field<DateTimeGraphType>("Created");
			Field<DateTimeGraphType>("Modified");
			Field<IntGraphType>("Version").Description = @"The version number of this form version";
			Field<StringGraphType>("FormData").Description = @"The form data for this version";
			Field<BooleanGraphType>("PublishVersion").Description = @"Should this version be published";
			Field<IdGraphType>("FormId").Description = @"The form id for this version";
			Field<ListGraphType<StudentsSubmissionEntityInputType>>("FormSubmissions").Description = @"The submissions for this form version";

			// Add entity references

			// Add references to foreign models to allow nested creation

			// % protected region % [Add any extra GraphQL input fields here] off begin
			// % protected region % [Add any extra GraphQL input fields here] end
		}
	}

}