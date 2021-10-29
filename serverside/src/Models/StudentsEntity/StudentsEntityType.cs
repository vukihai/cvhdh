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
	public class StudentsEntityType : ObjectGraphType<StudentsEntity>
	{
		public StudentsEntityType()
		{

			// Add model fields to type
			Field(o => o.Id, type: typeof(NonNullGraphType<IdGraphType>));
			Field(o => o.Created, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.Modified, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.FirstName, type: typeof(StringGraphType));
			Field(o => o.LastName, type: typeof(StringGraphType));
			Field(o => o.ContactNumber, type: typeof(StringGraphType));
			Field(o => o.Email, type: typeof(StringGraphType));
			Field(o => o.EnrolmentStart, type: typeof(DateTimeGraphType));
			Field(o => o.EnrolmentEnd, type: typeof(DateTimeGraphType));
			Field(o => o.Name, type: typeof(StringGraphType));
			Field(o => o.PublishedVersionId, type: typeof(IdGraphType));
			// % protected region % [Add any extra GraphQL fields here] off begin
			// % protected region % [Add any extra GraphQL fields here] end

			// Add entity references
			Field<ListGraphType<StudentsEntityFormVersionType>, IEnumerable<StudentsEntityFormVersion>>()
				.Name("FormVersions")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();
					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid, StudentsEntityFormVersion>(
						"GetFormVersionsForStudentsEntity",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<StudentsEntityFormVersion>(context, new ReadOptions {DisableAudit = true});
							var results = await query
								.Where(x => keys.Contains(x.FormId))
								.Select(x => x.FormId)
								.Distinct()
								.SelectMany(x => query
									.Where(y => y.FormId == x)
									.AddIdCondition(args.Id)
									.AddIdsCondition(args.Ids)
									.AddWhereFilter(args.Where)
									.AddConditionalWhereFilter(args.Conditions)
									.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
									.AddOrderBys(args.OrderBy)
									.AddSkip(args.Skip)
									.AddTake(args.Take))
								.ToListAsync(context.CancellationToken);
							return results.ToLookup(x => x.FormId, x => x);
						});
					return loader.LoadAsync(context.Source.Id);
				});
			Field<StudentsEntityFormVersionType, StudentsEntityFormVersion>()
				.Name("PublishedVersion")
				.ResolveAsync(async context =>
				{
					if (!context.Source.PublishedVersionId.HasValue)
					{
						return null;
					}
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();
					var loader = accessor.Context.GetOrAddBatchLoader<Guid?, StudentsEntityFormVersion>(
						"GetSpacePoliceOfficerForIncidentSubmissionEntity",
						async keys =>
						{
							var results = await QueryHelpers.BuildQueryResolver<StudentsEntityFormVersion>(
								context,
								x => keys.Contains(x.Id));
							return results.ToDictionary(x => new Guid?(x.Id), x => x);
						});
					return loader.LoadAsync(context.Source.PublishedVersionId);
				});
			Field(o => o.AddressId, type: typeof(IdGraphType));

			// GraphQL reference to entity AchievementsEntity via reference Achievements
			Field<ListGraphType<NonNullGraphType<AchievementsEntityType>>, IEnumerable<AchievementsEntity>>()
				.Name("Achievementss")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid?, AchievementsEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetAchievementssForStudentsEntity",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<AchievementsEntity>(context, new ReadOptions {DisableAudit = true});
							var results = await query
								.Where(x => x.StudentsId.HasValue && keys.Contains(x.StudentsId))
								.Select(x => x.StudentsId.Value)
								.Distinct()
								.SelectMany(x => query
									.Where(y => y.StudentsId == x)
									.AddIdCondition(args.Id)
									.AddIdsCondition(args.Ids)
									.AddWhereFilter(args.Where)
									.AddConditionalWhereFilter(args.Conditions)
									.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
									.AddOrderBys(args.OrderBy)
									.AddSkip(args.Skip)
									.AddTake(args.Take))
								.ToListAsync(context.CancellationToken);
							return results.ToLookup(x => x.StudentsId, x => x);
						});

					return loader.LoadAsync(context.Source.Id);
				});

			// GraphQL reference to entity AssessmentsEntity via reference Assessments
			Field<ListGraphType<NonNullGraphType<AssessmentsEntityType>>, IEnumerable<AssessmentsEntity>>()
				.Name("Assessmentss")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid?, AssessmentsEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetAssessmentssForStudentsEntity",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<AssessmentsEntity>(context, new ReadOptions {DisableAudit = true});
							var results = await query
								.Where(x => x.StudentsId.HasValue && keys.Contains(x.StudentsId))
								.Select(x => x.StudentsId.Value)
								.Distinct()
								.SelectMany(x => query
									.Where(y => y.StudentsId == x)
									.AddIdCondition(args.Id)
									.AddIdsCondition(args.Ids)
									.AddWhereFilter(args.Where)
									.AddConditionalWhereFilter(args.Conditions)
									.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
									.AddOrderBys(args.OrderBy)
									.AddSkip(args.Skip)
									.AddTake(args.Take))
								.ToListAsync(context.CancellationToken);
							return results.ToLookup(x => x.StudentsId, x => x);
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
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetAddressForStudentsEntity",
						async keys =>
						{
							var results = await QueryHelpers.BuildQueryResolver<AddressEntity>(
								context,
								x => keys.Contains(x.Id));
							return results.ToDictionary(x => new Guid?(x.Id), x => x);
						});

					return loader.LoadAsync(context.Source.AddressId);
				});

			// GraphQL reference to entity StudentsEntityFormTileEntity via reference FormPage
			Field<ListGraphType<NonNullGraphType<StudentsEntityFormTileEntityType>>, IEnumerable<StudentsEntityFormTileEntity>>()
				.Name("FormPages")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid, StudentsEntityFormTileEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetFormPagesForStudentsEntity",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<StudentsEntityFormTileEntity>(context, new ReadOptions {DisableAudit = true});
							var results = await query
								.Where(x => keys.Contains(x.FormId))
								.Select(x => x.FormId)
								.Distinct()
								.SelectMany(x => query
									.Where(y => y.FormId == x)
									.AddIdCondition(args.Id)
									.AddIdsCondition(args.Ids)
									.AddWhereFilter(args.Where)
									.AddConditionalWhereFilter(args.Conditions)
									.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
									.AddOrderBys(args.OrderBy)
									.AddSkip(args.Skip)
									.AddTake(args.Take))
								.ToListAsync(context.CancellationToken);
							return results.ToLookup(x => x.FormId, x => x);
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
	public class StudentsEntityInputType : InputObjectGraphType<StudentsEntity>
	{
		public StudentsEntityInputType()
		{
			Name = "StudentsEntityInput";
			Description = "The input object for adding a new StudentsEntity";

			// Add entity fields
			Field<IdGraphType>("Id");
			Field<DateTimeGraphType>("Created");
			Field<DateTimeGraphType>("Modified");
			Field<StringGraphType>("FirstName");
			Field<StringGraphType>("LastName");
			Field<StringGraphType>("ContactNumber");
			Field<StringGraphType>("Email");
			Field<DateTimeGraphType>("EnrolmentStart");
			Field<DateTimeGraphType>("EnrolmentEnd");
			Field<StringGraphType>("Name");
			Field<IdGraphType>("PublishedVersionId").Description = "The current published version for the form";
			Field<ListGraphType<StudentsEntityFormVersionInputType>>("FormVersions").Description = "The versions for this form";

			// Add entity references
			Field<IdGraphType>("AddressId");

			// Add references to foreign models to allow nested creation
			Field<ListGraphType<AchievementsEntityInputType>>("Achievementss");
			Field<ListGraphType<AssessmentsEntityInputType>>("Assessmentss");
			Field<AddressEntityInputType>("Address");
			Field<ListGraphType<StudentsEntityFormTileEntityInputType>>("FormPages");

			// % protected region % [Add any extra GraphQL input fields here] off begin
			// % protected region % [Add any extra GraphQL input fields here] end
		}
	}

}