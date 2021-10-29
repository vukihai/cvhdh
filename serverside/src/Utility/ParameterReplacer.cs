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
using System.Linq.Expressions;

namespace Firstapp2257.Utility
{
	/// <summary>
	/// Specialised expression visitor for replacing instances of a parameter expression with a new parameter.
	/// </summary>
	public class ParameterReplacer : ExpressionVisitor
	{
		private readonly Expression _parameterToUse;
		private readonly ParameterExpression _parameterToReplace;

		/// <summary>
		/// Constructs a <see cref="ParameterReplacer"/> that will replace all instances of a
		/// <see cref="ParameterExpression"/> with the value passed into <paramref name="parameterToUse"/>
		/// </summary>
		/// <param name="parameterToUse">
		/// The <see cref="ParameterExpression"/> that will be used to replace all parameters in the visited
		/// expression with.
		/// </param>
		public ParameterReplacer(Expression parameterToUse)
		{
			_parameterToUse = parameterToUse;
		}

		/// <summary>
		/// Constructs a <see cref="ParameterReplacer"/> that will replace any instance of
		/// <paramref name="parameterToReplace"/> with <paramref name="parameterToUse"/>.
		/// </summary>
		/// <param name="parameterToUse">
		/// The <see cref="ParameterExpression"/> that will be used to replace <paramref name="parameterToUse"/>
		/// with.
		/// </param>
		/// <param name="parameterToReplace">
		/// The <see cref="ParameterExpression"/> that will be replaced by <paramref name="parameterToUse"/>.
		/// </param>
		public ParameterReplacer(Expression parameterToUse, ParameterExpression parameterToReplace)
		{
			_parameterToUse = parameterToUse;
			_parameterToReplace = parameterToReplace;
		}

		/// <summary>
		/// Performs the parameter replacing operation.
		/// </summary>
		/// <param name="node">The expression to replace the parameters on.</param>
		/// <returns>A new expression with the replaced parameters.</returns>
		protected override Expression VisitParameter(ParameterExpression node)
		{
			if (_parameterToReplace != null)
			{
				return node == _parameterToReplace ? _parameterToUse : node;
			}

			return _parameterToUse;
		}

		/// <summary>
		/// Replaces all instances of a of <see cref="ParameterExpression"/> in the provided expression with
		/// <paramref name="parameter"/>.
		/// </summary>
		/// <param name="parameter">The parameter used replace the instances in the expression.</param>
		/// <param name="expression">The expression to replace the parameters in.</param>
		/// <returns>A new expression with the parameters replaced.</returns>
		public static Expression Replace(Expression parameter, Expression expression)
		{
			return new ParameterReplacer(parameter).Visit(expression);
		}

		/// <summary>
		/// Replaces all instances of <paramref name="parameterToReplace"/> with <paramref name="parameterToUse"/>
		/// in the provided expression.
		/// </summary>
		/// <param name="parameterToUse">The parameter used to replace instances in the expression.</param>
		/// <param name="parameterToReplace">The parameter to replace in the expression.</param>
		/// <param name="expression">The expression to replace the parameters in.</param>
		/// <returns>A new expression with the parameters replaced.</returns>
		public static Expression Replace(
			Expression parameterToUse,
			ParameterExpression parameterToReplace,
			Expression expression)
		{
			return new ParameterReplacer(parameterToUse, parameterToReplace).Visit(expression);
		}
	}
}