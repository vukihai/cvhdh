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
using Firstapp2257.Utility;

namespace Firstapp2257.Helpers
{
	public static class ExpressionHelper
	{
		/// <summary>
		/// Aggregates a list of expressions using an OR conjunction
		/// </summary>
		/// <param name="expressions"> The expressions to aggregate </param>
		/// <typeparam name="TModel"> Model returned by expressions </typeparam>
		/// <returns> An expression that can be used for the where condition of a linq query </returns>
		public static Expression<Func<TModel, bool>> OrExpressions<TModel>(
			IEnumerable<Expression<Func<TModel, bool>>> expressions)
		{
			Expression<Func<TModel, bool>> baseRule = _ => false;
			var filter = Expression.OrElse(baseRule.Body, baseRule.Body);

			var param = Expression.Parameter(typeof(TModel), "model");

			filter = expressions
				.Select(expr => (Expression<Func<TModel, bool>>) ParameterReplacer.Replace(
					param, 
					expr.Parameters.First(),
					expr))
				.Aggregate(filter, (current, expression) =>
					Expression.OrElse(current, expression.Body));

			return Expression.Lambda<Func<TModel, bool>>(filter, param);
		}

		/// <summary>
		/// Aggregates a list of expressions using an AND conjunction
		/// </summary>
		/// <param name="expressions"> The expressions to aggregate </param>
		/// <typeparam name="TModel"> Model returned by expressions </typeparam>
		/// <returns> An expression that can be used for the where condition of a linq query </returns>
		public static Expression<Func<TModel, bool>> AndExpressions<TModel>(
			IEnumerable<Expression<Func<TModel, bool>>> expressions)
		{
			Expression<Func<TModel, bool>> baseRule = _ => true;
			var filter = Expression.AndAlso(baseRule.Body, baseRule.Body);

			var param = Expression.Parameter(typeof(TModel), "model");

			filter = expressions
				.Select(expr => (Expression<Func<TModel, bool>>) ParameterReplacer.Replace(
					param,
					expr.Parameters.First(),
					expr))
				.Aggregate(filter, (current, expression) =>
					Expression.AndAlso(current, expression.Body));

			return Expression.Lambda<Func<TModel, bool>>(filter, param);
		}

		/// <summary>
		/// Combines many expressions with <see cref="Expression.OrElse(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)"/>
		/// </summary>
		/// <param name="expressions">An array of expressions to combine.</param>
		/// <returns>A new expression that is all the expression arguments combined with a logical OR.</returns>
		/// <exception cref="ArgumentException">If no expressions are passed in.</exception>
		public static Expression OrElse(params Expression[] expressions)
		{
			switch (expressions.Length)
			{
				case 0:
					throw new ArgumentException("Must pass at least 1 expression");
				case 1:
					return expressions.First();
				case >= 2:
				{
					var expression = expressions[0];

					for (var i = 1; i < expressions.Length; i++)
					{
						expression = Expression.OrElse(expression, expressions[i]);
					}

					return expression;
				}
				default:
					throw new ArgumentException("Invalid expression count");
			}
		}
	}
}