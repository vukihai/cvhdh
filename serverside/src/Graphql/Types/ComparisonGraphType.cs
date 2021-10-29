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
using GraphQL.EntityFramework;
using GraphQL.Types;
namespace Firstapp2257.Graphql.Types
{
	/// <summary>
	/// An object that maps where comparisons to GraphQL enum comparisons
	/// </summary>
	public class ComparisonGraphType: EnumerationGraphType
	{
		public ComparisonGraphType()
		{
			AddValue("contains", null, Comparison.Contains);
			AddValue("endsWith", null, Comparison.EndsWith);
			AddValue("equal", null, Comparison.Equal);
			AddValue("greaterThan", null, Comparison.GreaterThan);
			AddValue("greaterThanOrEqual", null, Comparison.GreaterThanOrEqual);
			AddValue("notIn", null, Comparison.NotIn, "Negation Property used with the 'in' comparison should be used in place of this");
			AddValue("in", null, Comparison.In);
			AddValue("lessThan", null, Comparison.LessThan);
			AddValue("lessThanOrEqual", null, Comparison.LessThanOrEqual);
			AddValue("like", null, Comparison.Like);
			AddValue("startsWith", null, Comparison.StartsWith);
		}
	}
}