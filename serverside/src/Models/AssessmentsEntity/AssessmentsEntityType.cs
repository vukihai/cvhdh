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
	public class AssessmentsEntityType : ObjectGraphType<AssessmentsEntity>
	{
		public AssessmentsEntityType()
		{

			// Add model fields to type
			Field(o => o.Id, type: typeof(NonNullGraphType<IdGraphType>));
			Field(o => o.Created, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.Modified, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.StartDate, type: typeof(DateTimeGraphType));
			Field(o => o.EndDate, type: typeof(DateTimeGraphType));
			Field(o => o.Summary, type: typeof(StringGraphType));
			Field(o => o.Recommendations, type: typeof(StringGraphType));
			// % protected region % [Add any extra GraphQL fields here] off begin
			// % protected region % [Add any extra GraphQL fields here] end

			// Add entity references
			Field(o => o.StudentsId, type: typeof(IdGraphType));

			// GraphQL reference to entity AssessmentNotesEntity via reference AssessmentNotes
			Field<ListGraphType<NonNullGraphType<AssessmentNotesEntityType>>, IEnumerable<AssessmentNotesEntity>>()
				.Name("AssessmentNotess")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid?, AssessmentNotesEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetAssessmentNotessForAssessmentsEntity",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<AssessmentNotesEntity>(context, new ReadOptions {DisableAudit = true});
							var results = await query
								.Where(x => x.AssessmentsId.HasValue && keys.Contains(x.AssessmentsId))
								.Select(x => x.AssessmentsId.Value)
								.Distinct()
								.SelectMany(x => query
									.Where(y => y.AssessmentsId == x)
									.AddIdCondition(args.Id)
									.AddIdsCondition(args.Ids)
									.AddWhereFilter(args.Where)
									.AddConditionalWhereFilter(args.Conditions)
									.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
									.AddOrderBys(args.OrderBy)
									.AddSkip(args.Skip)
									.AddTake(args.Take))
								.ToListAsync(context.CancellationToken);
							return results.ToLookup(x => x.AssessmentsId, x => x);
						});

					return loader.LoadAsync(context.Source.Id);
				});

			// GraphQL reference to entity StudentsEntity via reference Students
			Field<StudentsEntityType, StudentsEntity>()
				.Name("Students")
				.ResolveAsync(async context =>
				{
					if (!context.Source.StudentsId.HasValue)
					{
						return null;
					}

					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddBatchLoader<Guid?, StudentsEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetStudentsForAssessmentsEntity",
						async keys =>
						{
							var results = await QueryHelpers.BuildQueryResolver<StudentsEntity>(
								context,
								x => keys.Contains(x.Id));
							return results.ToDictionary(x => new Guid?(x.Id), x => x);
						});

					return loader.LoadAsync(context.Source.StudentsId);
				});

			// GraphQL reference to entity CommentsEntity via reference Comments
			Field<ListGraphType<NonNullGraphType<CommentsEntityType>>, IEnumerable<CommentsEntity>>()
				.Name("Commentss")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid?, CommentsEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetCommentssForAssessmentsEntity",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<CommentsEntity>(context, new ReadOptions {DisableAudit = true});
							var results = await query
								.Where(x => x.AssessmentsId.HasValue && keys.Contains(x.AssessmentsId))
								.Select(x => x.AssessmentsId.Value)
								.Distinct()
								.SelectMany(x => query
									.Where(y => y.AssessmentsId == x)
									.AddIdCondition(args.Id)
									.AddIdsCondition(args.Ids)
									.AddWhereFilter(args.Where)
									.AddConditionalWhereFilter(args.Conditions)
									.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
									.AddOrderBys(args.OrderBy)
									.AddSkip(args.Skip)
									.AddTake(args.Take))
								.ToListAsync(context.CancellationToken);
							return results.ToLookup(x => x.AssessmentsId, x => x);
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
	public class AssessmentsEntityInputType : InputObjectGraphType<AssessmentsEntity>
	{
		public AssessmentsEntityInputType()
		{
			Name = "AssessmentsEntityInput";
			Description = "The input object for adding a new AssessmentsEntity";

			// Add entity fields
			Field<IdGraphType>("Id");
			Field<DateTimeGraphType>("Created");
			Field<DateTimeGraphType>("Modified");
			Field<DateTimeGraphType>("StartDate");
			Field<DateTimeGraphType>("EndDate");
			Field<StringGraphType>("Summary");
			Field<StringGraphType>("Recommendations");

			// Add entity references
			Field<IdGraphType>("StudentsId");

			// Add references to foreign models to allow nested creation
			Field<ListGraphType<AssessmentNotesEntityInputType>>("AssessmentNotess");
			Field<StudentsEntityInputType>("Students");
			Field<ListGraphType<CommentsEntityInputType>>("Commentss");

			// % protected region % [Add any extra GraphQL input fields here] off begin
			// % protected region % [Add any extra GraphQL input fields here] end
		}
	}

}