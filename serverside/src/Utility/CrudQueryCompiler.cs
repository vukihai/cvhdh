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
using System.Reflection;
using System.Threading;
using Firstapp2257.Helpers;
using Firstapp2257.Models.Internal;
using GraphQL.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Firstapp2257.Utility
{
	public class CrudQueryCompiler : QueryCompiler
	{
		private static readonly MethodInfo AddIdConditionMethod = typeof(QueryableExtensions).GetMethod("AddIdCondition");
		private static readonly MethodInfo AddIdsConditionMethod = typeof(QueryableExtensions).GetMethod("AddIdsCondition");
		private static readonly MethodInfo AddWhereFilterMethod = typeof(QueryableExtensions).GetMethod("AddWhereFilter");
		private static readonly MethodInfo AddConditionalWhereFilterMethod = typeof(QueryableExtensions).GetMethod("AddConditionalWhereFilter");
		private static readonly MethodInfo AddConditionalHasFilterMethod = typeof(QueryableExtensions).GetMethod("AddConditionalHasFilter");
		private static readonly MethodInfo AddOrderBysMethod = typeof(QueryableExtensions).GetMethod("AddOrderBys");
		private static readonly MethodInfo AddSkipMethod = typeof(QueryableExtensions).GetMethod("AddSkip");
		private static readonly MethodInfo AddTakeMethod = typeof(QueryableExtensions).GetMethod("AddTake");

		private readonly IQueryContextFactory _queryContextFactory;
		private readonly ICompiledQueryCache _compiledQueryCache;
		private readonly ICompiledQueryCacheKeyGenerator _compiledQueryCacheKeyGenerator;
		private readonly IDatabase _database;
		private readonly IDiagnosticsLogger<DbLoggerCategory.Query> _logger;
		private readonly Type _contextType;
		private readonly IEvaluatableExpressionFilter _evaluatableExpressionFilter;
		private readonly IModel _model;

		// % protected region % [Add any class fields here] off begin
		// % protected region % [Add any class fields here] end

		private class CrudVisitor : ExpressionVisitor
		{
			protected override Expression VisitMethodCall(MethodCallExpression node)
			{
				// % protected region % [Override VisitMethodCall here] off begin
				var genericArg = node.Method.GetGenericArguments().FirstOrDefault();
				if (genericArg == null)
				{
					return base.VisitMethodCall(node);
				}

				if (MethodEquals(node, AddIdConditionMethod, genericArg))
				{
					var idValue = Expression.Lambda<Func<Guid?>>(node.Arguments[1]).Compile()();
					if (idValue.HasValue)
					{
						var param = Expression.Parameter(genericArg);
						node = Expression.Call(
							ReflectionCache.WhereMethod.MakeGenericMethod(genericArg),
							node.Arguments.First(),
							Expression.Lambda(
								Expression.Equal(Expression.Constant(idValue), Expression.PropertyOrField(param, "Id")),
								param));
					}
					else
					{
						return Visit(node.Arguments.First());
					}
				}

				if (MethodEquals(node, AddIdsConditionMethod, genericArg))
				{
					var idsValue = Expression.Lambda<Func<List<Guid>>>(node.Arguments[1]).Compile()();
					if (idsValue != null && idsValue.Any())
					{
						var param = Expression.Parameter(genericArg);
						node = Expression.Call(
							ReflectionCache.WhereMethod.MakeGenericMethod(genericArg),
							node.Arguments.First(),
							Expression.Lambda(
								Expression.Call(
									ReflectionCache.ContainsMethod.MakeGenericMethod(typeof(Guid)),
									Expression.Constant(idsValue),
									Expression.PropertyOrField(param, "Id")),
								param));
					}
					else
					{
						return Visit(node.Arguments.First());
					}
				}

				if (MethodEquals(node, AddWhereFilterMethod, genericArg))
				{
					var wheresValue = Expression.Lambda<Func<List<WhereExpression>>>(node.Arguments[1]).Compile()();
					if (wheresValue != null && wheresValue.Any())
					{
						Expression body = Expression.Constant(true);
						var param = Expression.Parameter(genericArg);
						foreach (var whereExpression in wheresValue)
						{
							dynamic condition = ReflectionCache.BuildWhereExpressionMethod
								.MakeGenericMethod(genericArg)
								.Invoke(null, new object[] {whereExpression, param});
							body = Expression.And(body, condition.Body);
						}
						node = Expression.Call(
							ReflectionCache.WhereMethod.MakeGenericMethod(genericArg),
							node.Arguments.First(),
							Expression.Lambda(body, param));
					}
					else
					{
						return Visit(node.Arguments.First());
					}
				}

				if (MethodEquals(node, AddConditionalWhereFilterMethod, genericArg))
				{
					var wheresValue = Expression.Lambda<Func<List<List<WhereExpression>>>>(node.Arguments[1]).Compile()();
					if (wheresValue != null && wheresValue.Any())
					{
						var condition = (Expression) ReflectionCache.BuildConditionalWhereMethod
							.MakeGenericMethod(genericArg)
							.Invoke(null, new object[] { wheresValue });
						node = Expression.Call(
							ReflectionCache.WhereMethod.MakeGenericMethod(genericArg),
							node.Arguments.First(),
							condition);
					}
					else
					{
						return Visit(node.Arguments.First());
					}
				}

				if (MethodEquals(node, AddConditionalHasFilterMethod, genericArg))
				{
					var hasValues = Expression.Lambda<Func<List<List<HasCondition>>>>(node.Arguments[1]).Compile()();
					var serviceProviderValue = Expression.Lambda<Func<IServiceProvider>>(node.Arguments[2]).Compile()();
					if (hasValues != null && hasValues.Any())
					{
						var condition = (Expression) ReflectionCache.BuildConditionalHasMethod
							.MakeGenericMethod(genericArg)
							.Invoke(null, new object[] {hasValues, serviceProviderValue});
						node = Expression.Call(
							ReflectionCache.WhereMethod.MakeGenericMethod(genericArg),
							node.Arguments.First(),
							condition);
					}
					else
					{
						return Visit(node.Arguments.First());
					}
				}

				if (MethodEquals(node, AddOrderBysMethod, genericArg))
				{
					var orderByValues = Expression.Lambda<Func<List<OrderBy>>>(node.Arguments[1]).Compile()();
					if (orderByValues != null && orderByValues.Any())
					{
						for (var i = 0; i < orderByValues.Count; i++)
						{
							var orderBy = orderByValues[i];

							var param = Expression.Parameter(genericArg);
							var field = ReflectionCache.GetPropertyRecursive(param, orderBy.Path.Split('.'));
							var func = Expression.Lambda(Expression.Convert(field, typeof(object)), param);

							if (orderBy.Descending != null && orderBy.Descending == true)
							{
								node = i == 0
									? Expression.Call(
										ReflectionCache.OrderByDescendingMethod.MakeGenericMethod(genericArg, typeof(object)),
										node.Arguments.First(),
										func)
									: Expression.Call(
										ReflectionCache.ThenByDescendingMethod.MakeGenericMethod(genericArg, typeof(object)),
										node.Arguments.First(),
										func);
							}
							else
							{
								node = i == 0
									? Expression.Call(
										ReflectionCache.OrderByMethod.MakeGenericMethod(genericArg, typeof(object)),
										node.Arguments.First(),
										func)
									: Expression.Call(
										ReflectionCache.ThenByMethod.MakeGenericMethod(genericArg, typeof(object)),
										node.Arguments.First(),
										func);
							}
						}
					}
					else
					{
						return Visit(node.Arguments.First());
					}
				}

				if (MethodEquals(node, AddSkipMethod, genericArg))
				{
					var skipValue = Expression.Lambda<Func<int?>>(node.Arguments[1]).Compile()();
					if (skipValue.HasValue)
					{
						node = Expression.Call(
							ReflectionCache.SkipMethod.MakeGenericMethod(genericArg),
							node.Arguments.First(),
							Expression.Constant(skipValue));
					}
					else
					{
						return Visit(node.Arguments.First());
					}
				}

				if (MethodEquals(node, AddTakeMethod, genericArg))
				{
					var takeValue = Expression.Lambda<Func<int?>>(node.Arguments[1]).Compile()();
					if (takeValue.HasValue)
					{
						node = Expression.Call(
							ReflectionCache.TakeMethod.MakeGenericMethod(genericArg),
							node.Arguments.First(),
							Expression.Constant(takeValue));
					}
					else
					{
						return Visit(node.Arguments.First());
					}
				}
				return base.VisitMethodCall(node);
				// % protected region % [Override VisitMethodCall here] end
			}

			// % protected region % [Override VisitMethodCall here] off begin
			private static bool MethodEquals(MethodCallExpression methodCall, MethodInfo methodInfo, Type genericType)
			{
				try
				{
					return methodCall.Method.Name == methodInfo.Name
						&& methodCall.Method == methodInfo.MakeGenericMethod(genericType);
				}
				catch
				{
					return false;
				}
			}
			// % protected region % [Override VisitMethodCall here] end

			// % protected region % [Add any further visitor methods here] off begin
			// % protected region % [Add any further visitor methods here] end
		}

		public CrudQueryCompiler(
			// % protected region % [Add any further dependancy injection here] off begin
			// % protected region % [Add any further dependancy injection here] end
			IQueryContextFactory queryContextFactory,
			ICompiledQueryCache compiledQueryCache,
			ICompiledQueryCacheKeyGenerator compiledQueryCacheKeyGenerator,
			IDatabase database,
			IDiagnosticsLogger<DbLoggerCategory.Query> logger,
			ICurrentDbContext currentContext,
			IEvaluatableExpressionFilter evaluatableExpressionFilter,
			IModel model)
			: base(queryContextFactory, compiledQueryCache, compiledQueryCacheKeyGenerator, database, logger, currentContext, evaluatableExpressionFilter, model)
		{
			_queryContextFactory = queryContextFactory;
			_compiledQueryCache = compiledQueryCache;
			_compiledQueryCacheKeyGenerator = compiledQueryCacheKeyGenerator;
			_database = database;
			_logger = logger;
			_contextType = currentContext.Context.GetType();
			_evaluatableExpressionFilter = evaluatableExpressionFilter;
			_model = model;
			// % protected region % [Add any constructor logic here] off begin
			// % protected region % [Add any constructor logic here] end
		}

		public override TResult Execute<TResult>(Expression query)
		{
			// % protected region % [Override VisitMethodCall here] off begin
			var visitor = new CrudVisitor();
			return base.Execute<TResult>(visitor.Visit(query));
			// % protected region % [Override VisitMethodCall here] end
		}

		public override TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken = default)
		{
			// % protected region % [Override VisitMethodCall here] off begin
			var visitor = new CrudVisitor();
			return base.ExecuteAsync<TResult>(visitor.Visit(query), cancellationToken);
			// % protected region % [Override VisitMethodCall here] end
		}

		// % protected region % [Add any further methods here] off begin
		// % protected region % [Add any further methods here] end
	}
}