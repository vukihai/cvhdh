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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firstapp2257.Graphql.Types;
using Firstapp2257.Helpers;
using Firstapp2257.Models;
using Firstapp2257.Services;
using GraphQL;
using GraphQL.Builders;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Firstapp2257.Graphql.Helpers
{
	public static class QueryHelpers
	{
		/// <summary>
		/// Creates a resolve function that returns a list a queryable of models.
		/// This respects the security settings and properly applies the auditing context
		/// </summary>
		/// <typeparam name="TModel">The type of the model to create the function for</typeparam>
		/// <returns>A function that takes a graphql context and returns a queryable of models</returns>
		public static IQueryable<TModel> CreateResolveFunction<TModel>(
			IResolveFieldContext<object> context,
			ReadOptions options = null)
			where TModel : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Override CreateResolveFunction here] off begin
			var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
			var crudService = graphQlContext.CrudService;
			var auditFields = AuditReadData.FromGraphqlContext(context);
			options ??= new ReadOptions();
			options.DbContext = graphQlContext.DbContext;
			return crudService.Get<TModel>(auditFields: auditFields, options: options)
				.AsNoTracking()
				.AsSplitQuery();
			// % protected region % [Override CreateResolveFunction here] end
		}

		/// <summary>
		/// Adds common graphql arguments for entity fields.
		/// </summary>
		/// <param name="builder">Field builder to add arguments to.</param>
		/// <typeparam name="TGraphType">The graph type for the entity.</typeparam>
		/// <typeparam name="T">The entity type.</typeparam>
		/// <returns>The field builder with common arguments added.</returns>
		public static FieldBuilder<TGraphType, T> AddCommonArguments<TGraphType, T>(
			this FieldBuilder<TGraphType, T> builder)
		{
			// % protected region % [Override AddCommonArguments here] off begin
			return builder
				.Argument<IdGraphType>("id")
				.Argument<ListGraphType<IdGraphType>>("ids")
				.Argument<ListGraphType<WhereType>>("where")
				.Argument<ListGraphType<ListGraphType<WhereType>>>("conditions")
				.Argument<ListGraphType<ListGraphType<HasConditionType>>>("has")
				.Argument<IntGraphType>("skip")
				.Argument<IntGraphType>("take")
				.Argument<ListGraphType<OrderGraph>>("orderBy");
			// % protected region % [Override AddCommonArguments here] end
		}

		/// <summary>
		/// Creates a query resolver for getting a list of entities with conditions based on the provided graphql
		/// resolve context.
		/// </summary>
		/// <param name="context">The graphql resolve context to get the arguments from.</param>
		/// <param name="filter">A filter expression for query.</param>
		/// <param name="options">Read options to be sent to the crud service.</param>
		/// <typeparam name="T">The type of model entity to build a query for.</typeparam>
		/// <returns>An enumerable of entities that match the provided conditions.</returns>
		public static async Task<IEnumerable<T>> BuildQueryResolver<T>(
			IResolveFieldContext<object> context,
			Expression<Func<T, bool>> filter = null,
			ReadOptions options = null)
			where T : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Override BuildQueryResolver here] off begin
			var args = new CommonArguments(context);

			var query = CreateResolveFunction<T>(context, options ?? new ReadOptions {DisableAudit = true});

			if (filter != null)
			{
				query = query.Where(filter);
			}

			query = query
				.AddIdCondition(args.Id)
				.AddIdsCondition(args.Ids)
				.AddWhereFilter(args.Where)
				.AddConditionalWhereFilter(args.Conditions)
				.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
				.AddOrderBys(args.OrderBy)
				.AddSkip(args.Skip)
				.AddTake(args.Take);

			return await query.ToListAsync(context.CancellationToken);
			// % protected region % [Override BuildQueryResolver here] end
		}

		/// <summary>
		/// Creates a query resolver for getting a single entity with conditions based on the provided graphql
		/// resolve context.
		/// </summary>
		/// <param name="context">The graphql resolve context to get the arguments from.</param>
		/// <param name="filter">A filter expression for query.</param>
		/// <param name="options">Read options to be sent to the crud service.</param>
		/// <typeparam name="T">The type of model entity to build a query for.</typeparam>
		/// <returns>A single entity that matches the provided conditions or otherwise null.</returns>
		public static async Task<T> BuildSingleQueryResolver<T>(
			IResolveFieldContext<object> context,
			Expression<Func<T, bool>> filter = null,
			ReadOptions options = null)
			where T : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Override BuildSingleQueryResolver here] off begin
			var args = new CommonArguments(context);

			var query = CreateResolveFunction<T>(context, options ?? new ReadOptions {DisableAudit = true});

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query
				.AddIdCondition(args.Id)
				.AddIdsCondition(args.Ids)
				.AddWhereFilter(args.Where)
				.AddConditionalWhereFilter(args.Conditions)
				.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider)
				.FirstOrDefaultAsync(context.CancellationToken);
			// % protected region % [Override BuildSingleQueryResolver here] end
		}

		/// <summary>
		/// Creates a query resolver for getting a count of entities with conditions based on the provided graphql
		/// resolve context.
		/// </summary>
		/// <param name="context">The graphql resolve context to get the arguments from.</param>
		/// <param name="options">Read options to be sent to the crud service.</param>
		/// <typeparam name="T">The type of model entity to build a query for.</typeparam>
		/// <returns>A number object that contains the number of entities that satisfy the provided conditions.</returns>
		public static async Task<NumberObject> BuildCountQueryResolver<T>(
			IResolveFieldContext<object> context,
			ReadOptions options = null)
			where T : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Override BuildCountQueryResolver here] off begin
			var args = new CommonArguments(context);

			var query = CreateResolveFunction<T>(context, options ?? new ReadOptions {DisableAudit = true})
				.AddIdCondition(args.Id)
				.AddIdsCondition(args.Ids)
				.AddWhereFilter(args.Where)
				.AddConditionalWhereFilter(args.Conditions)
				.AddConditionalHasFilter(args.Has, ((Firstapp2257GraphQlContext) context.UserContext).ServiceProvider);

			return new NumberObject { Number = await query.CountAsync() };
			// % protected region % [Override BuildCountQueryResolver here] end
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}