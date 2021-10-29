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
	public class AddressEntityType : ObjectGraphType<AddressEntity>
	{
		public AddressEntityType()
		{

			// Add model fields to type
			Field(o => o.Id, type: typeof(NonNullGraphType<IdGraphType>));
			Field(o => o.Created, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.Modified, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.Unit, type: typeof(StringGraphType));
			Field(o => o.AddressLine1, type: typeof(StringGraphType));
			Field(o => o.AddressLine2, type: typeof(StringGraphType));
			Field(o => o.Suburb, type: typeof(StringGraphType));
			Field(o => o.Postcode, type: typeof(IntGraphType));
			Field(o => o.City, type: typeof(StringGraphType));
			Field(o => o.Country, type: typeof(StringGraphType));
			Field(o => o.Name, type: typeof(StringGraphType));
			Field(o => o.PublishedVersionId, type: typeof(IdGraphType));
			// % protected region % [Add any extra GraphQL fields here] off begin
			// % protected region % [Add any extra GraphQL fields here] end

			// Add entity references
			Field<ListGraphType<AddressEntityFormVersionType>, IEnumerable<AddressEntityFormVersion>>()
				.Name("FormVersions")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();
					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid, AddressEntityFormVersion>(
						"GetFormVersionsForAddressEntity",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<AddressEntityFormVersion>(context, new ReadOptions {DisableAudit = true});
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
			Field<AddressEntityFormVersionType, AddressEntityFormVersion>()
				.Name("PublishedVersion")
				.ResolveAsync(async context =>
				{
					if (!context.Source.PublishedVersionId.HasValue)
					{
						return null;
					}
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();
					var loader = accessor.Context.GetOrAddBatchLoader<Guid?, AddressEntityFormVersion>(
						"GetSpacePoliceOfficerForIncidentSubmissionEntity",
						async keys =>
						{
							var results = await QueryHelpers.BuildQueryResolver<AddressEntityFormVersion>(
								context,
								x => keys.Contains(x.Id));
							return results.ToDictionary(x => new Guid?(x.Id), x => x);
						});
					return loader.LoadAsync(context.Source.PublishedVersionId);
				});

			// GraphQL reference to entity StaffEntity via reference Staffs
			Field<ListGraphType<NonNullGraphType<StaffEntityType>>, IEnumerable<StaffEntity>>()
				.Name("Staffss")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid?, StaffEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetStaffssForAddressEntity",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<StaffEntity>(context, new ReadOptions {DisableAudit = true});
							var results = await query
								.Where(x => x.AddressId.HasValue && keys.Contains(x.AddressId))
								.Select(x => x.AddressId.Value)
								.Distinct()
								.SelectMany(x => query
									.Where(y => y.AddressId == x)
									.AddIdCondition(args.Id)
									.AddIdsCondition(args.Ids)
									.AddWhereFilter(args.Where)
									.AddConditionalWhereFilter(args.Conditions)
									.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
									.AddOrderBys(args.OrderBy)
									.AddSkip(args.Skip)
									.AddTake(args.Take))
								.ToListAsync(context.CancellationToken);
							return results.ToLookup(x => x.AddressId, x => x);
						});

					return loader.LoadAsync(context.Source.Id);
				});

			// GraphQL reference to entity StudentsEntity via reference Students
			Field<ListGraphType<NonNullGraphType<StudentsEntityType>>, IEnumerable<StudentsEntity>>()
				.Name("Studentss")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid?, StudentsEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetStudentssForAddressEntity",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<StudentsEntity>(context, new ReadOptions {DisableAudit = true});
							var results = await query
								.Where(x => x.AddressId.HasValue && keys.Contains(x.AddressId))
								.Select(x => x.AddressId.Value)
								.Distinct()
								.SelectMany(x => query
									.Where(y => y.AddressId == x)
									.AddIdCondition(args.Id)
									.AddIdsCondition(args.Ids)
									.AddWhereFilter(args.Where)
									.AddConditionalWhereFilter(args.Conditions)
									.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
									.AddOrderBys(args.OrderBy)
									.AddSkip(args.Skip)
									.AddTake(args.Take))
								.ToListAsync(context.CancellationToken);
							return results.ToLookup(x => x.AddressId, x => x);
						});

					return loader.LoadAsync(context.Source.Id);
				});

			// GraphQL reference to entity AddressEntityFormTileEntity via reference FormPage
			Field<ListGraphType<NonNullGraphType<AddressEntityFormTileEntityType>>, IEnumerable<AddressEntityFormTileEntity>>()
				.Name("FormPages")
				.AddCommonArguments()
				.ResolveAsync(async context =>
				{
					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid, AddressEntityFormTileEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetFormPagesForAddressEntity",
						async keys =>
						{
							var args = new CommonArguments(context);
							var query = QueryHelpers.CreateResolveFunction<AddressEntityFormTileEntity>(context, new ReadOptions {DisableAudit = true});
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
	public class AddressEntityInputType : InputObjectGraphType<AddressEntity>
	{
		public AddressEntityInputType()
		{
			Name = "AddressEntityInput";
			Description = "The input object for adding a new AddressEntity";

			// Add entity fields
			Field<IdGraphType>("Id");
			Field<DateTimeGraphType>("Created");
			Field<DateTimeGraphType>("Modified");
			Field<StringGraphType>("Unit");
			Field<StringGraphType>("AddressLine1");
			Field<StringGraphType>("AddressLine2");
			Field<StringGraphType>("Suburb");
			Field<IntGraphType>("Postcode");
			Field<StringGraphType>("City");
			Field<StringGraphType>("Country");
			Field<StringGraphType>("Name");
			Field<IdGraphType>("PublishedVersionId").Description = "The current published version for the form";
			Field<ListGraphType<AddressEntityFormVersionInputType>>("FormVersions").Description = "The versions for this form";

			// Add entity references

			// Add references to foreign models to allow nested creation
			Field<ListGraphType<StaffEntityInputType>>("Staffss");
			Field<ListGraphType<StudentsEntityInputType>>("Studentss");
			Field<ListGraphType<AddressEntityFormTileEntityInputType>>("FormPages");

			// % protected region % [Add any extra GraphQL input fields here] off begin
			// % protected region % [Add any extra GraphQL input fields here] end
		}
	}

}