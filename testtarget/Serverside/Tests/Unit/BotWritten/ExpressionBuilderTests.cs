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
using Firstapp2257.Utility;
using FluentAssertions;
using GraphQL.EntityFramework;
using Xunit;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace ServersideTests.Tests.Unit.BotWritten
{
	public class TestData
	{
		// % protected region % [Override TestData class here] off begin
		public string Name { get; set; }

		[EntityForeignKey("OtherData", "", false, null)]
		public List<TestData> OtherData { get; set; }
		// % protected region % [Override TestData class here] end
	}

	public class TestCondition
	{
		// % protected region % [Override TestCondition class here] off begin
		public string NameEquals { get; set; }
		// % protected region % [Override TestCondition class here] end
	}

	public class ExpressionBuilderTests
	{
		public static TheoryData<HasCondition, string[]> SingleConditionTheoryData()
		{
			// % protected region % [Override SingleConditionTheoryData here] off begin
			return new()
			{
				{new HasCondition {Path = "OtherData"}, new[] {"Has Child", "Has 2 Children" }},
				{new HasCondition(), new[] {"Has Child", "Has 2 Children", "Has no children"}},
				{new HasCondition {Path = "OtherData", Negate = true}, new[] {"Has no children" }},
				{new HasCondition {Path = "OtherData", Conditions = new List<List<WhereExpression>>
				{
					new()
					{
						new WhereExpression
						{
							Path = "Name",
							Comparison = Comparison.Equal,
							Value = new []{"Child"}
						}
					}
				}}, new[] {"Has Child"}},
			};
			// % protected region % [Override SingleConditionTheoryData here] end
		}

		// % protected region % [Add custom TestSingleCondition traits here] off begin
		// % protected region % [Add custom TestSingleCondition traits here] end
		[Theory]
		[MemberData(nameof(SingleConditionTheoryData))]
		public void TestSingleCondition(HasCondition condition, string[] expectedNames)
		{
			// % protected region % [Override TestSingleCondition here] off begin
			var param = Expression.Parameter(typeof(TestData));
			var expression = ExpressionUtilities.BuildHasExpression<TestData>(condition, param);

			var results = GetData().Where(expression.Compile()).ToList();

			results.Should().HaveCount(expectedNames.Length);
			results.Select(x => x.Name).Should().BeEquivalentTo(expectedNames);
			// % protected region % [Override TestSingleCondition here] end
		}

		// % protected region % [Add custom TestInvalidHasConditionReference traits here] off begin
		// % protected region % [Add custom TestInvalidHasConditionReference traits here] end
		[Fact]
		public void TestInvalidHasConditionReference()
		{
			// % protected region % [Override TestInvalidHasConditionReference here] off begin
			var param = Expression.Parameter(typeof(TestData));

			Assert.Throws<InvalidOperationException>(
				() => ExpressionUtilities.BuildHasExpression<TestData>(new HasCondition
				{
					Path = "FailField",
				}, param));
			// % protected region % [Override TestInvalidHasConditionReference here] end
		}

		public static TheoryData<List<List<TestCondition>>, string[]> TestConditionalExpressionBuilderTheoryData()
		{
			// % protected region % [Override TestConditionalExpressionBuilderTheoryData here] off begin
			return new()
			{
				{
					new List<List<TestCondition>>
					{
						new()
						{
							new TestCondition {NameEquals = "Has Child"}
						}
					},
					new[] {"Has Child"}
				},

				{
					new List<List<TestCondition>>
					{
						new()
						{
							new TestCondition {NameEquals = "Has Child"},
							new TestCondition {NameEquals = "Has no children"}
						}
					},
					new[] {"Has Child", "Has no children"}
				},

				{
					new List<List<TestCondition>>
					{
						new()
						{
							new TestCondition {NameEquals = "Has Child"},
						},
						new()
						{
							new TestCondition {NameEquals = "Has no children"}
						}
					},
					Array.Empty<string>()
				},
			};
			// % protected region % [Override TestConditionalExpressionBuilderTheoryData here] end
		}

		// % protected region % [Add custom TestConditionalExpressionBuilder traits here] off begin
		// % protected region % [Add custom TestConditionalExpressionBuilder traits here] end
		[Theory]
		[MemberData(nameof(TestConditionalExpressionBuilderTheoryData))]
		public void TestConditionalExpressionBuilder(List<List<TestCondition>> condition, string[] expectedResults)
		{
			// % protected region % [Override TestConditionalExpressionBuilder here] off begin
			var expression = ExpressionUtilities.BuildConditionalExpression(condition, (testCondition, param) =>
			{
				Expression<Func<TestData, bool>> conditionExpression = x => x.Name == testCondition.NameEquals;
				return ParameterReplacer.Replace(param, conditionExpression) as Expression<Func<TestData, bool>>;
			});

			var results = GetData().Where(expression.Compile()).ToList();

			results.Should().HaveCount(expectedResults.Length);
			results.Select(x => x.Name).Should().BeEquivalentTo(expectedResults);
			// % protected region % [Override TestConditionalExpressionBuilder here] end
		}

		private static IEnumerable<TestData> GetData()
		{
			// % protected region % [Override GetData here] off begin
			return new List<TestData>
			{
				new()
				{
					Name = "Has Child",
					OtherData = new List<TestData> { new() {Name = "Child"} }
				},
				new()
				{
					Name = "Has 2 Children",
					OtherData = new List<TestData>
					{
						new() {Name = "First Child"},
						new() {Name = "Second Child"},
					}
				},
				new() { Name = "Has no children", OtherData = new List<TestData>() }
			};
			// % protected region % [Override GetData here] end
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}