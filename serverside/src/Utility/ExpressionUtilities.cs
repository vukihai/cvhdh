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
using Firstapp2257.Models.Internal;
using GraphQL.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Firstapp2257.Utility
{
	/// <summary>
	/// Utilities for building expressions.
	/// </summary>
	public static class ExpressionUtilities
	{
		private const string ConditionalExpressionParamName = "conditionBuilderExpressionParam";

		/// <summary>
		/// Expression builder for a given condition.
		/// </summary>
		/// <param name="condition">The condition to build an expression for.</param>
		/// <param name="param">The parameter for the returned expression.</param>
		/// <typeparam name="TModel">The type of the entity to build the condition for.</typeparam>
		/// <typeparam name="TCondition">The type of condition to build an expression for.</typeparam>
		/// <returns>An expression that represents the provided expression.</returns>
		public delegate Expression<Func<TModel, bool>> ExpressionBuilderDelegate<TModel, in TCondition>(
			TCondition condition,
			ParameterExpression param);

		/// <summary>
		/// Builds a condition expression given an enumerable or enumerable conditions and a function that can convert
		/// a condition into an expression that can be embedded in a where statement.
		/// </summary>
		/// <param name="wheres">The conditions to compose.</param>
		/// <param name="expressionBuilder">The function to construct the expression for each condition.</param>
		/// <typeparam name="TModel">The type of the entity that the condition will be composed on.</typeparam>
		/// <typeparam name="TCondition">The type of the condition.</typeparam>
		/// <returns>A new expression that can be embedded in the body of another expression.</returns>
		/// A condition can take any form, however it must be representative of a an expression that can be placed
		/// in the .Where of an EF Linq expression. It is recommended that a condition object is just a POCO, to allow
		/// for the easiest possible parsing of the condition.
		///
		/// These conditions are nested in 2 levels of arrays for joining them together with different conditional logic.
		/// An example of this is as follows.
		/// <code>
		/// [
		/// 	[{A}, {B}],
		/// 	[{C}],
		/// 	[{D}, {E}, {F}],
		/// ]
		/// </code>
		/// Where the {A} .. {F} are condition objects.
		///
		/// This function will be executed such that the inner conditions linked by OR and the outer arrays are linked
		/// by AND. This will make the example have the logical flow of
		///
		/// <code>
		/// ({A} OR {B}) AND ({C}) AND ({D} OR {E} OR {F})
		/// </code>
		public static Expression<Func<TModel, bool>> BuildConditionalExpression<TModel, TCondition>(
			IEnumerable<IEnumerable<TCondition>> wheres,
			ExpressionBuilderDelegate<TModel, TCondition> expressionBuilder)
		{
			// % protected region % [Change BuildConditionalExpression here] off begin
			Expression<Func<TModel, bool>> falseExpression = _ => false;
			Expression<Func<TModel, bool>> trueExpression = _ => true;

			var result = Expression.OrElse(trueExpression.Body, trueExpression.Body);
			var param = Expression.Parameter(typeof(TModel), ConditionalExpressionParamName);

			foreach (var where in wheres)
			{
				var combinedPredicate = Expression.OrElse(falseExpression.Body, falseExpression.Body);
				foreach (var expression in where)
				{
					var predicate = expressionBuilder(expression, param);
					combinedPredicate = Expression.OrElse(combinedPredicate, predicate.Body);
				}

				result = Expression.AndAlso(result, combinedPredicate);
			}

			return Expression.Lambda<Func<TModel, bool>>(result, param);
			// % protected region % [Change BuildConditionalExpression here] end
		}

		/// <summary>
		/// Builds an expression to represent the provided where condition.
		/// </summary>
		/// <param name="where">The where condition to deserialise into an expression.</param>
		/// <param name="param">The parameter for the returned expression.</param>
		/// <typeparam name="T">The type of the entity to build the where expression for.</typeparam>
		/// <returns>A new expression that represents the provided where condition.</returns>
		public static Expression<Func<T, bool>> BuildWhereExpression<T>(
			WhereExpression where,
			ParameterExpression param)
		{
			// % protected region % [Change BuildWhereExpression here] off begin
			param ??= Expression.Parameter(typeof(T), "models");

			Expression<Func<T, bool>> predicate;
			if (where.Comparison == Comparison.Like &&
				ReflectionCache.ILikeMethod != null &&
				(where.Case == StringComparison.CurrentCultureIgnoreCase ||
				where.Case == StringComparison.OrdinalIgnoreCase ||
				where.Case == StringComparison.InvariantCultureIgnoreCase))
			{
				// var propertyParam = Expression.Parameter(typeof(T));
				var field = ReflectionCache.GetPropertyRecursive(param, where.Path.Split('.'));

				var likeExpression = Expression.Call(
					ReflectionCache.ILikeMethod,
					Expression.Constant(EF.Functions),
					Expression.Convert(field, typeof(string)),
					Expression.Constant(where.Value?.FirstOrDefault()));

				predicate = where.Negate
					? Expression.Lambda<Func<T, bool>>(Expression.Not(likeExpression), param)
					: Expression.Lambda<Func<T, bool>>(likeExpression, param);
			}
			else
			{
				predicate = ExpressionBuilder<T>.BuildPredicate(new[] {where});
			}

			return ParameterReplacer.Replace(param, predicate) as Expression<Func<T, bool>>;
			// % protected region % [Change BuildWhereExpression here] end
		}

		/// <summary>
		/// Builds an expression to represent the provided has condition.
		/// </summary>
		/// <param name="hasCondition">The has condition to deserialise into an expression.</param>
		/// <param name="param">The parameter for the returned expression.</param>
		/// <param name="serviceProvider">
		/// A service provider to derive the security rules for the has expression from.
		/// If no service provided is given then the nested query will not have security applied.</param>
		/// <typeparam name="T">The type of the entity to build the where expression for.</typeparam>
		/// <returns>A new expression that represents the provided where condition.</returns>
		/// <exception cref="InvalidOperationException">If the provided has expression name is invalid.</exception>
		public static Expression<Func<T, bool>> BuildHasExpression<T>(
			HasCondition hasCondition,
			ParameterExpression param,
			IServiceProvider serviceProvider = null)
		{
			// % protected region % [Change BuildHasExpression here] off begin
			// If no path is provided then do not do any checks
			if (string.IsNullOrWhiteSpace(hasCondition.Path))
			{
				return Expression.Lambda<Func<T, bool>>(Expression.Constant(true), param);
			}

			param ??= Expression.Parameter(typeof(T), "models");

			// Get the field to apply the condition to
			var collectionReferences = ReflectionCache.GetCollectionReferences(typeof(T)).ToList();
			var referenceProperty = collectionReferences
				.FirstOrDefault(x => string.Equals(x.Name, hasCondition.Path, StringComparison.InvariantCultureIgnoreCase));
			if (referenceProperty == default)
			{
				throw new InvalidOperationException(
					$"{hasCondition.Path} is not a valid reference. " +
					$"Valid references for a has condition are {string.Join(", ", collectionReferences.Select(x => $"'{x.Name}'"))}");
			}

			// Get the type of the field
			var referenceType = referenceProperty.PropertyType.GetGenericArguments().First();

			// Construct a conditional where condition based off of the values in the has expression
			var hasConditionExpression = ReflectionCache.BuildConditionalWhereMethod.MakeGenericMethod(referenceType)
				.Invoke(null, new object[] {hasCondition.Conditions});
			var hasConditionBody = (Expression) hasConditionExpression!.GetType()
				.GetProperty("Body")!.GetValue(hasConditionExpression);

			// Construct the parameters for the functions
			// The inner condition is used for the x.Any(y =>...)
			var anyParam = Expression.Parameter(referenceType, "innerCondition");

			// Get the reference to apply the any condition to
			var reference = Expression.PropertyOrField(param, hasCondition.Path);

			// Get security rules for this subexpression
			Expression securityBody;
			if (serviceProvider != null)
			{
				var security = ReflectionCache
					.CreateReadSecurityFilterMethod
					.MakeGenericMethod(referenceType)
					.Invoke(null, new object[] {serviceProvider});
				securityBody = (Expression) security!.GetType().GetProperty("Body")!.GetValue(security);
			}
			else
			{
				securityBody = Expression.Constant(true);
			}

			// Join the security rules with the regular conditions
			var combinedExpression = Expression.And(hasConditionBody!, securityBody!);

			// Replace all parameter expressions in the any expression body with the one declared before
			var anyExpression = ParameterReplacer.Replace(anyParam, Expression.Lambda(combinedExpression, anyParam));

			// Call the any method with the created expression
			Expression result = Expression.Call(
				ReflectionCache.EnumerableAnyMethod.MakeGenericMethod(referenceType),
				reference,
				anyExpression);

			if (hasCondition.Negate)
			{
				result = Expression.Not(result);
			}

			return Expression.Lambda<Func<T, bool>>(result, param);
			// % protected region % [Change BuildHasExpression here] end
		}

		/// <summary>
		/// Builds a conditional where expression based off of the provided where conditions.
		/// </summary>
		/// <param name="wheres">An array of arrays of where conditions to represent the conditional structure.</param>
		/// <typeparam name="T">The type of the entity to build the expression for.</typeparam>
		/// <returns>An expression representing the provided where condition.</returns>
		public static Expression<Func<T, bool>> BuildConditionalWhere<T>(
			IEnumerable<IEnumerable<WhereExpression>> wheres)
		{
			// % protected region % [Change BuildConditionalWhere here] off begin
			if (wheres is null)
			{
				return _ => true;
			}

			return BuildConditionalExpression(wheres, BuildWhereExpression<T>);
			// % protected region % [Change BuildConditionalWhere here] end
		}

		/// <summary>
		/// Builds a conditional has expression based off of the provided has conditions.
		/// </summary>
		/// <param name="hasConditions">An array of arrays of has conditions to represent the conditional structure.</param>
		/// <param name="serviceProvider">
		/// A service provider to derive security rules from. If this argument is null than security will not be applied
		/// to the nested query.
		/// </param>
		/// <typeparam name="T">The typeof the entity to build the expression for.</typeparam>
		/// <returns>An expression representing the provided has condition.</returns>
		public static Expression<Func<T, bool>> BuildConditionalHas<T>(
			IEnumerable<IEnumerable<HasCondition>> hasConditions,
			IServiceProvider serviceProvider = null)
		{
			// % protected region % [Change BuildConditionalHas here] off begin
			if (hasConditions is null)
			{
				return _ => true;
			}

			return BuildConditionalExpression(
				hasConditions,
				(condition, param) => BuildHasExpression<T>(condition, param, serviceProvider));
			// % protected region % [Change BuildConditionalHas here] end
		}
	}
}
