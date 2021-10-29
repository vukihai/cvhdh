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
using Firstapp2257.Models;
using Firstapp2257.Models.Internal;
using Firstapp2257.Services;
using Firstapp2257.Services.Interfaces;
using Firstapp2257.Utility;
using GraphQL.EntityFramework;
using Microsoft.AspNetCore.Identity;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Firstapp2257.Helpers
{
	public static class QueryableExtensions
	{
		/// <summary>
		/// Adds security filters to a queryable
		/// </summary>
		/// <param name="queryable">The queryable to filter</param>
		/// <param name="identityService">
		/// The identity service containing the user data to apply the filtering for
		/// </param>
		/// <param name="userManager">The user manager</param>
		/// <param name="dbContext">The database context to apply the reads with</param>
		/// <param name="serviceProvider">Service provider to pass to the ACLs</param>
		/// <typeparam name="T">The type of the model to apply the conditional over</typeparam>
		/// <returns>The queryable with security filtering applied</returns>
		public static IQueryable<T> AddReadSecurityFiltering<T>(
			this IQueryable<T> queryable,
			IIdentityService identityService,
			UserManager<User> userManager,
			Firstapp2257DBContext dbContext,
			IServiceProvider serviceProvider)
			where T : IOwnerAbstractModel, new()
		{
			// % protected region % [Change AddReadSecurityFiltering here] off begin
			return queryable.Where(SecurityService.CreateReadSecurityFilter<T>(identityService, userManager, dbContext, serviceProvider));
			// % protected region % [Change AddReadSecurityFiltering here] end
		}

		public static IQueryable<T> AddUpdateSecurityFiltering<T>(
			this IQueryable<T> queryable,
			IIdentityService identityService,
			UserManager<User> userManager,
			Firstapp2257DBContext dbContext,
			IServiceProvider serviceProvider)
			where T : IOwnerAbstractModel, new()
		{
			// % protected region % [Change AddUpdateSecurityFiltering here] off begin
			return queryable.Where(SecurityService.CreateUpdateSecurityFilter<T>(identityService, userManager, dbContext, serviceProvider));
			// % protected region % [Change AddUpdateSecurityFiltering here] end
		}

		public static IQueryable<T> AddDeleteSecurityFiltering<T>(
			this IQueryable<T> queryable,
			IIdentityService identityService,
			UserManager<User> userManager,
			Firstapp2257DBContext dbContext,
			IServiceProvider serviceProvider)
			where T : IOwnerAbstractModel, new()
		{
			// % protected region % [Change AddDeleteSecurityFiltering here] off begin
			return queryable.Where(SecurityService.CreateDeleteSecurityFilter<T>(identityService, userManager, dbContext, serviceProvider));
			// % protected region % [Change AddDeleteSecurityFiltering here] end
		}

		public static IQueryable<T> AddPagination<T>(this IQueryable<T> queryable, Pagination pagination)
		{
			// % protected region % [Change AddPagination here] off begin
			if (pagination != null && pagination.PageSize.HasValue && pagination.SkipAmount.HasValue)
			{
				return queryable
					.Skip(pagination.SkipAmount.Value)
					.Take(pagination.PageSize.Value);
			}

			return queryable;
			// % protected region % [Change AddPagination here] end
		}

		/// <summary>
		/// Adds a conditional where statement that handles both AND and OR conditions to the queryable object
		/// </summary>
		/// <param name="models">The queryable to apply the filter to</param>
		/// <param name="wheres">The filters to apply</param>
		/// <typeparam name="T">The type of model to queryable is abstracted over</typeparam>
		/// <returns></returns>
		/// <remarks>
		/// A where condition takes the following form:
		/// <code>{path: "name", comparison: contains, value: "abc"}</code>
		///
		/// This will roughly serialize to the following code:
		/// <code>models.Where(model => model.Name.Contains("abc"))</code>
		///
		///	This argument takes a list of lists of where conditions, giving it the following form
		/// <code>
		/// [
		///		[{A}, {B}],
		/// 	[{C}],
		/// 	[{D}, {E}, {F}],
		/// ]
		/// </code>
		/// Where the {A} .. {F} are where conditions shortened for brevity
		///
		/// This function will be executed such that the inner conditions linked by OR and the outer arrays are linked
		/// by AND. This will make the example have the logical flow of
		///
		/// <code>
		/// ({A} OR {B}) AND ({C}) AND ({D} OR {E} OR {F})
		/// </code>
		///
		/// </remarks>
		public static IQueryable<T> AddConditionalWhereFilter<T>(
			this IQueryable<T> models,
			IEnumerable<IEnumerable<WhereExpression>> wheres)
		{
			// % protected region % [Change AddConditionalWhereFilter here] off begin
			return wheres == null
				? models
				: models.Where(ExpressionUtilities.BuildConditionalWhere<T>(wheres));
			// % protected region % [Change AddConditionalWhereFilter here] end
		}

		/// <summary>
		/// Creates a where statement where all the conditions joined by an AND
		/// </summary>
		/// <param name="models">A queryable to apply the filter over</param>
		/// <param name="wheres">A list of filters to apply</param>
		/// <typeparam name="T">The type of the model to apply the conditional over</typeparam>
		/// <returns>A new queryable that has the conditions applied</returns>
		public static IQueryable<T> AddWhereFilter<T>(this IQueryable<T> models, IEnumerable<WhereExpression> wheres)
		{
			// % protected region % [Change AddWhereFilter here] off begin
			if (wheres == null)
			{
				return models;
			}

			foreach (var where in wheres)
			{
				var predicate = ExpressionUtilities.BuildWhereExpression<T>(where, null);
				models = models.Where(predicate);
			}

			return models;
			// % protected region % [Change AddWhereFilter here] end
		}

		/// <summary>
		/// Creates where condition on a query that checks if the entity has references that satisfy some set of
		/// conditions.
		/// </summary>
		/// <param name="models">The queryable to apply the filter over.</param>
		/// <param name="conditions">The has conditions to apply.</param>
		/// <param name="serviceProvider">A service provider for providing security rules</param>
		/// <typeparam name="T">The type of the model to apply the conditional over</typeparam>
		/// <returns>A new queryable that has the conditions applied</returns>
		public static IQueryable<T> AddConditionalHasFilter<T>(
			this IQueryable<T> models,
			IEnumerable<IEnumerable<HasCondition>> conditions,
			IServiceProvider serviceProvider)
		{
			// % protected region % [Change AddConditionalHasFilter here] off begin
			return conditions == null
				? models
				: models.Where(ExpressionUtilities.BuildConditionalHas<T>(conditions, serviceProvider));
			// % protected region % [Change AddConditionalHasFilter here] end
		}

		/// <summary>
		/// Orders a set of models by a list of order by conditions. The order by conditions are applied first to last.
		/// </summary>
		/// <param name="models">The queryable to order</param>
		/// <param name="orderBys">A list of order by objects to apply</param>
		/// <typeparam name="T">The type of the model to apply the conditional over</typeparam>
		/// <returns>A new queryable that is ordered by the given conditions</returns>
		public static IQueryable<T> AddOrderBys<T>(this IQueryable<T> models, List<OrderBy> orderBys)
		{
			// % protected region % [Change AddOrderBys here] off begin
			if (orderBys == null)
			{
				return models;
			}

			IOrderedQueryable<T> orderedQueryable = null;

			for (var i = 0; i < orderBys.Count; i++)
			{
				var orderBy = orderBys[i];

				var param = Expression.Parameter(typeof(T));
				var field = ReflectionCache.GetPropertyRecursive(param, orderBy.Path.Split('.'));
				var func = Expression.Lambda<Func<T, object>>(Expression.Convert(field, typeof(object)), param);

				if (orderBy.Descending != null && orderBy.Descending == true)
				{
					orderedQueryable = i == 0
						? models.OrderByDescending(func)
						: orderedQueryable.ThenByDescending(func);
				}
				else
				{
					orderedQueryable = i == 0
						? models.OrderBy(func)
						: orderedQueryable.ThenBy(func);
				}
			}

			return orderedQueryable ?? models;
			// % protected region % [Change AddOrderBys here] end
		}

		public static IQueryable<T> AddIdCondition<T>(this IQueryable<T> models, Guid? id)
			where T : IAbstractModel
		{
			// % protected region % [Change AddIdCondition here] off begin
			if (!id.HasValue)
			{
				return models;
			}

			return models.Where(x => x.Id == id.Value);
			// % protected region % [Change AddIdCondition here] end
		}

		public static IQueryable<T> AddIdsCondition<T>(this IQueryable<T> models, IEnumerable<Guid> ids)
			where T : IAbstractModel
		{
			// % protected region % [Change AddIdsCondition here] off begin
			if (ids == null)
			{
				return models;
			}

			return models.Where(x => ids.Contains(x.Id));
			// % protected region % [Change AddIdsCondition here] end
		}

		public static IQueryable<T> AddSkip<T>(this IQueryable<T> models, int? value)
		{
			// % protected region % [Change AddSkip here] off begin
			if (!value.HasValue)
			{
				return models;
			}

			return models.Skip(value.Value);
			// % protected region % [Change AddSkip here] end
		}

		public static IQueryable<T> AddTake<T>(this IQueryable<T> models, int? value)
		{
			// % protected region % [Change AddTake here] off begin
			if (!value.HasValue)
			{
				return models;
			}

			return models.Take(value.Value);
			// % protected region % [Change AddTake here] end
		}

		// % protected region % [Add any further methods here] off begin
		// % protected region % [Add any further methods here] end
	}
}
