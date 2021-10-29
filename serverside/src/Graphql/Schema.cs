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
using System.Threading.Tasks;
using Firstapp2257.Graphql.Fields;
using Firstapp2257.Graphql.Helpers;
using Firstapp2257.Graphql.Types;
using Firstapp2257.Models;
using Firstapp2257.Models.RegistrationModels;
using Firstapp2257.Services;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Firstapp2257.Graphql
{
	/// <summary>
	/// The GraphQL schema class for fetching and mutating data
	/// </summary>
	public class Firstapp2257Schema : Schema
	{
		public Firstapp2257Schema(IServiceProvider resolver) : base(resolver)
		{
			Query = resolver.GetRequiredService<Firstapp2257Query>();
			Mutation = resolver.GetRequiredService<Firstapp2257Mutation>();
			// % protected region % [Add any extra schema constructor options here] off begin
			// % protected region % [Add any extra schema constructor options here] end
		}

		// % protected region % [Add any schema methods here] off begin
		// % protected region % [Add any schema methods here] end
	}

	/// <summary>
	/// The query class for the GraphQL schema
	/// </summary>
	public class Firstapp2257Query : ObjectGraphType
	{
		private const string WhereDesc = "A list of where conditions that are joined with an AND";
		private const string ConditionalWhereDesc = "A list of lists of where conditions. The conditions inside the " +
													"innermost lists are joined with and OR and the results of those " +
													"lists are joined with an AND";

		public Firstapp2257Query()
		{
			// Add query types for each entity
			// % protected region % [Override query fields here] off begin
			AddModelQueryField<AchievementsEntityType, AchievementsEntity>("AchievementsEntity");
			AddModelQueryField<AchievementsEntityFormVersionType, AchievementsEntityFormVersion>("AchievementsEntityFormVersion");
			AddModelQueryField<AddressEntityType, AddressEntity>("AddressEntity");
			AddModelQueryField<AddressEntityFormVersionType, AddressEntityFormVersion>("AddressEntityFormVersion");
			AddModelQueryField<StudentsEntityType, StudentsEntity>("StudentsEntity");
			AddModelQueryField<StudentsEntityFormVersionType, StudentsEntityFormVersion>("StudentsEntityFormVersion");
			AddModelQueryField<AssessmentNotesEntityType, AssessmentNotesEntity>("AssessmentNotesEntity");
			AddModelQueryField<AssessmentsEntityType, AssessmentsEntity>("AssessmentsEntity");
			AddModelQueryField<CommentsEntityType, CommentsEntity>("CommentsEntity");
			AddModelQueryField<StaffEntityType, StaffEntity>("StaffEntity");
			AddModelQueryField<AchievementsSubmissionEntityType, AchievementsSubmissionEntity>("AchievementsSubmissionEntity");
			AddModelQueryField<AddressSubmissionEntityType, AddressSubmissionEntity>("AddressSubmissionEntity");
			AddModelQueryField<StudentsSubmissionEntityType, StudentsSubmissionEntity>("StudentsSubmissionEntity");
			AddModelQueryField<AchievementsEntityFormTileEntityType, AchievementsEntityFormTileEntity>("AchievementsEntityFormTileEntity");
			AddModelQueryField<AddressEntityFormTileEntityType, AddressEntityFormTileEntity>("AddressEntityFormTileEntity");
			AddModelQueryField<StudentsEntityFormTileEntityType, StudentsEntityFormTileEntity>("StudentsEntityFormTileEntity");

			// Add query types for each many to many reference
			// % protected region % [Override query fields here] end

			// % protected region % [Add any extra query config here] off begin
			// % protected region % [Add any extra query config here] end
		}

		/// <summary>
		/// Adds single, multiple and connection queries to query
		/// </summary>
		/// <typeparam name="TModelType">The GraphQL type for returning data</typeparam>
		/// <typeparam name="TModel">The EF model type for querying the DB</typeparam>
		/// <param name="name">The name of the entity</param>
		public void AddModelQueryField<TModelType, TModel>(string name)
			where TModelType : ObjectGraphType<TModel>
			where TModel : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Add any extra logic before adding entity query fields here] off begin
			// % protected region % [Add any extra logic before adding entity query fields here] end

			// % protected region % [Override single query here] off begin
			Field<TModelType, TModel>()
				.Name(name)
				.Argument<IdGraphType>("id")
				.Argument<ListGraphType<IdGraphType>>("ids")
				.Argument<ListGraphType<WhereType>>("where")
				.Argument<ListGraphType<ListGraphType<WhereType>>>("conditions")
				.Description($"Query for fetching a single {name}")
				.ResolveAsync(context => QueryHelpers.BuildSingleQueryResolver<TModel>(
					context,
					options: new ReadOptions { DisableAudit = false }));
			// % protected region % [Override single query here] end

			// % protected region % [Override multiple query here] off begin
			Field<NonNullGraphType<ListGraphType<NonNullGraphType<TModelType>>>, IEnumerable<TModel>>()
				.Name($"{name}s")
				.AddCommonArguments()
				.Description($"Query for fetching multiple {name}s")
				.ResolveAsync(context => QueryHelpers.BuildQueryResolver<TModel>(
					context,
					options: new ReadOptions { DisableAudit = false }));
			// % protected region % [Override multiple query here] end

			// % protected region % [Override count query here] off begin
			Field<NumberObjectType, NumberObject>()
				.Name($"count{name}s")
				.Argument<IdGraphType>("id")
				.Argument<ListGraphType<IdGraphType>>("ids")
				.Argument<ListGraphType<WhereType>>("where")
				.Argument<ListGraphType<ListGraphType<WhereType>>>("conditions")
				.Argument<ListGraphType<ListGraphType<HasConditionType>>>("has")
				.Description("Counts the number of models according to a given set of conditions")
				.ResolveAsync(context => QueryHelpers.BuildCountQueryResolver<TModel>(
					context,
					new ReadOptions { DisableAudit = false }));
			// % protected region % [Override count query here] end

			// % protected region % [Override conditional query here] off begin
			Field<NonNullGraphType<ListGraphType<NonNullGraphType<TModelType>>>, IEnumerable<TModel>>()
				.Name($"{name}sConditional")
				.AddCommonArguments()
				.Description(ConditionalWhereDesc)
				.ResolveAsync(context => QueryHelpers.BuildQueryResolver<TModel>(
					context,
					options: new ReadOptions { DisableAudit = false }));
			// % protected region % [Override conditional query here] end

			// % protected region % [Override count conditional query here] off begin
			Field<NumberObjectType, NumberObject>()
				.Name($"count{name}sConditional")
				.Argument<IdGraphType>("id")
				.Argument<ListGraphType<IdGraphType>>("ids")
				.Argument<ListGraphType<WhereType>>("where")
				.Argument<ListGraphType<ListGraphType<WhereType>>>("conditions")
				.Argument<ListGraphType<ListGraphType<HasConditionType>>>("has")
				.Description("Counts the number of models according to a given set of conditions")
				.ResolveAsync(context => QueryHelpers.BuildCountQueryResolver<TModel>(
					context,
					new ReadOptions { DisableAudit = false }));
			// % protected region % [Override count conditional query here] end

			// % protected region % [Add any extra per entity fields here] off begin
			// % protected region % [Add any extra per entity fields here] end
		}

		// % protected region % [Add any extra query methods here] off begin
		// % protected region % [Add any extra query methods here] end
	}

	/// <summary>
	/// The mutation class for the GraphQL schema
	/// </summary>
	public class Firstapp2257Mutation : ObjectGraphType<object>
	{
		private const string ConditionalWhereDesc = "A list of lists of where conditions. The conditions inside the " +
											"innermost lists are joined with and OR and the results of those " +
											"lists are joined with an AND";

		public Firstapp2257Mutation()
		{
			Name = "Mutation";

			// Add input types for each entity
			// % protected region % [Override mutation fields here] off begin
			AddMutationField<AchievementsEntityInputType, AchievementsEntityInputType, AchievementsEntityType, AchievementsEntity>("AchievementsEntity");
			AddMutationField<AchievementsEntityFormVersionInputType, AchievementsEntityFormVersionInputType, AchievementsEntityFormVersionType, AchievementsEntityFormVersion>(
				"AchievementsEntityFormVersion",
				deleteMutation: context => Task.FromResult((object)new Guid[]{}));
			AddMutationField<AddressEntityInputType, AddressEntityInputType, AddressEntityType, AddressEntity>("AddressEntity");
			AddMutationField<AddressEntityFormVersionInputType, AddressEntityFormVersionInputType, AddressEntityFormVersionType, AddressEntityFormVersion>(
				"AddressEntityFormVersion",
				deleteMutation: context => Task.FromResult((object)new Guid[]{}));
			AddMutationField<StudentsEntityInputType, StudentsEntityInputType, StudentsEntityType, StudentsEntity>("StudentsEntity");
			AddMutationField<StudentsEntityFormVersionInputType, StudentsEntityFormVersionInputType, StudentsEntityFormVersionType, StudentsEntityFormVersion>(
				"StudentsEntityFormVersion",
				deleteMutation: context => Task.FromResult((object)new Guid[]{}));
			AddMutationField<AssessmentNotesEntityInputType, AssessmentNotesEntityInputType, AssessmentNotesEntityType, AssessmentNotesEntity>("AssessmentNotesEntity");
			AddMutationField<AssessmentsEntityInputType, AssessmentsEntityInputType, AssessmentsEntityType, AssessmentsEntity>("AssessmentsEntity");
			AddMutationField<CommentsEntityInputType, CommentsEntityInputType, CommentsEntityType, CommentsEntity>("CommentsEntity");
			AddMutationField<StaffEntityCreateInputType, StaffEntityInputType, StaffEntityType, StaffEntity>(
				"StaffEntity",
				CreateMutation.CreateUserCreateMutation<StaffEntity, StaffEntityRegistrationModel, StaffEntityGraphQlRegistrationModel>("StaffEntity"));
			AddMutationField<AchievementsSubmissionEntityInputType, AchievementsSubmissionEntityInputType, AchievementsSubmissionEntityType, AchievementsSubmissionEntity>("AchievementsSubmissionEntity");
			AddMutationField<AddressSubmissionEntityInputType, AddressSubmissionEntityInputType, AddressSubmissionEntityType, AddressSubmissionEntity>("AddressSubmissionEntity");
			AddMutationField<StudentsSubmissionEntityInputType, StudentsSubmissionEntityInputType, StudentsSubmissionEntityType, StudentsSubmissionEntity>("StudentsSubmissionEntity");
			AddMutationField<AchievementsEntityFormTileEntityInputType, AchievementsEntityFormTileEntityInputType, AchievementsEntityFormTileEntityType, AchievementsEntityFormTileEntity>("AchievementsEntityFormTileEntity");
			AddMutationField<AddressEntityFormTileEntityInputType, AddressEntityFormTileEntityInputType, AddressEntityFormTileEntityType, AddressEntityFormTileEntity>("AddressEntityFormTileEntity");
			AddMutationField<StudentsEntityFormTileEntityInputType, StudentsEntityFormTileEntityInputType, StudentsEntityFormTileEntityType, StudentsEntityFormTileEntity>("StudentsEntityFormTileEntity");

			// Add input types for each many to many reference
			// % protected region % [Override mutation fields here] end

			// % protected region % [Add any extra mutation queries here] off begin
			// % protected region % [Add any extra mutation queries here] end
		}

		/// <summary>
		/// Adds the required mutation fields to the GraphQL schema for create, update and delete
		/// </summary>
		/// <typeparam name="TModelCreateInputType">The GraphQL input type used for the create functions</typeparam>
		/// <typeparam name="TModelUpdateInputType">The GraphQL Input Type used for the update functions</typeparam>
		/// <typeparam name="TModelType">The GraphQL model type used for returning data</typeparam>
		/// <typeparam name="TModel">The EF model type for saving to the DB</typeparam>
		/// <param name="name">The name of the entity</param>
		/// <param name="createMutation">An override for the create mutation</param>
		/// <param name="updateMutation">An override for the update mutation</param>
		/// <param name="deleteMutation">An override for the delete mutation</param>
		/// <param name="conditionalUpdateMutation">An override for the conditional update mutation</param>
		/// <param name="conditionalDeleteMutation">An override for the conditional delete mutation</param>
		public void AddMutationField<TModelCreateInputType, TModelUpdateInputType, TModelType, TModel>(
			string name,
			Func<IResolveFieldContext<object>, Task<object>> createMutation = null,
			Func<IResolveFieldContext<object>, Task<object>> updateMutation = null,
			Func<IResolveFieldContext<object>, Task<object>> deleteMutation = null,
			Func<IResolveFieldContext<object>, Task<object>> conditionalUpdateMutation = null,
			Func<IResolveFieldContext<object>, Task<object>> conditionalDeleteMutation = null)
			where TModelCreateInputType : InputObjectGraphType<TModel>
			where TModelUpdateInputType : InputObjectGraphType<TModel>
			where TModelType : ObjectGraphType<TModel>
			where TModel : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Add any extra logic before adding entity mutation fields here] off begin
			// % protected region % [Add any extra logic before adding entity mutation fields here] end

			// % protected region % [Override create mutation here] off begin
			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<TModelType>>>>(
				$"create{name}",
				arguments: new QueryArguments(
					new QueryArgument<ListGraphType<TModelCreateInputType>> { Name = name + "s" },
					new QueryArgument<ListGraphType<StringGraphType>> { Name = "MergeReferences" }
				),
				resolve: createMutation ?? CreateMutation.CreateCreateMutation<TModel>(name)
			);
			// % protected region % [Override create mutation here] end

			// % protected region % [Override update mutation here] off begin
			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<TModelType>>>>(
				$"update{name}",
				arguments: new QueryArguments(
					new QueryArgument<ListGraphType<TModelUpdateInputType>> { Name = name + "s" },
					new QueryArgument<ListGraphType<StringGraphType>> { Name = "MergeReferences" }
				),
				resolve: updateMutation ?? UpdateMutation.CreateUpdateMutation<TModel>(name)
			);
			// % protected region % [Override update mutation here] end

			// % protected region % [Override delete mutation here] off begin
			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<IdObjectType>>>>(
				$"delete{name}",
				arguments: new QueryArguments(
					new QueryArgument<ListGraphType<IdGraphType>> { Name = $"{name}Ids" }
				),
				resolve: deleteMutation ?? DeleteMutation.CreateDeleteMutation<TModel>(name)
			);
			// % protected region % [Override delete mutation here] end

			// % protected region % [Override update conditional mutation here] off begin
			FieldAsync<BooleanObjectType>(
				$"update{name}sConditional",
				arguments: new QueryArguments(
					new QueryArgument<IdGraphType> { Name = "id" },
					new QueryArgument<ListGraphType<IdGraphType>> { Name = "ids" },
					new QueryArgument<ListGraphType<ListGraphType<WhereType>>>
					{
						Name = "conditions",
						Description = ConditionalWhereDesc
					},
					new QueryArgument<TModelUpdateInputType> { Name = "valuesToUpdate" },
					new QueryArgument<ListGraphType<StringGraphType>> { Name = "fieldsToUpdate" }
				),
				resolve: conditionalUpdateMutation ?? UpdateMutation.CreateConditionalUpdateMutation<TModel>(name)
			);
			// % protected region % [Override update conditional mutation here] end

			// % protected region % [Override delete conditional mutation here] off begin
			FieldAsync<BooleanObjectType>(
				$"delete{name}sConditional",
				arguments: new QueryArguments(
					new QueryArgument<IdGraphType> { Name = "id" },
					new QueryArgument<ListGraphType<IdGraphType>> { Name = "ids" },
					new QueryArgument<ListGraphType<ListGraphType<WhereType>>>
					{
						Name = "conditions",
						Description = ConditionalWhereDesc
					}
				),
				resolve: conditionalDeleteMutation ?? DeleteMutation.CreateConditionalDeleteMutation<TModel>(name)
			);
			// % protected region % [Override delete conditional mutation here] end

			// % protected region % [Add any extra per entity mutations here] off begin
			// % protected region % [Add any extra per entity mutations here] end
		}

		// % protected region % [Add any extra mutation methods here] off begin
		// % protected region % [Add any extra mutation methods here] end
	}
}