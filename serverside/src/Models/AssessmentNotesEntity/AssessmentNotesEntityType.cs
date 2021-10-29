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
	public class AssessmentNotesEntityType : ObjectGraphType<AssessmentNotesEntity>
	{
		public AssessmentNotesEntityType()
		{

			// Add model fields to type
			Field(o => o.Id, type: typeof(NonNullGraphType<IdGraphType>));
			Field(o => o.Created, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.Modified, type: typeof(NonNullGraphType<DateTimeGraphType>));
			Field(o => o.SessionNotes, type: typeof(StringGraphType));
			Field(o => o.SessionDate, type: typeof(DateTimeGraphType));
			// % protected region % [Add any extra GraphQL fields here] off begin
			// % protected region % [Add any extra GraphQL fields here] end

			// Add entity references
			Field(o => o.AssessmentsId, type: typeof(IdGraphType));
			Field(o => o.StaffId, type: typeof(IdGraphType));

			// GraphQL reference to entity AssessmentsEntity via reference Assessments
			Field<AssessmentsEntityType, AssessmentsEntity>()
				.Name("Assessments")
				.ResolveAsync(async context =>
				{
					if (!context.Source.AssessmentsId.HasValue)
					{
						return null;
					}

					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddBatchLoader<Guid?, AssessmentsEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetAssessmentsForAssessmentNotesEntity",
						async keys =>
						{
							var results = await QueryHelpers.BuildQueryResolver<AssessmentsEntity>(
								context,
								x => keys.Contains(x.Id));
							return results.ToDictionary(x => new Guid?(x.Id), x => x);
						});

					return loader.LoadAsync(context.Source.AssessmentsId);
				});

			// GraphQL reference to entity StaffEntity via reference Staff
			Field<StaffEntityType, StaffEntity>()
				.Name("Staff")
				.ResolveAsync(async context =>
				{
					if (!context.Source.StaffId.HasValue)
					{
						return null;
					}

					var graphQlContext = (Firstapp2257GraphQlContext) context.UserContext;
					var accessor = graphQlContext.ServiceProvider.GetRequiredService<IDataLoaderContextAccessor>();

					var loader = accessor.Context.GetOrAddBatchLoader<Guid?, StaffEntity>(
						string.Join("-", context.ResponsePath.Where(x => x is string)) + "GetStaffForAssessmentNotesEntity",
						async keys =>
						{
							var results = await QueryHelpers.BuildQueryResolver<StaffEntity>(
								context,
								x => keys.Contains(x.Id));
							return results.ToDictionary(x => new Guid?(x.Id), x => x);
						});

					return loader.LoadAsync(context.Source.StaffId);
				});

			// % protected region % [Add any extra GraphQL references here] off begin
			// % protected region % [Add any extra GraphQL references here] end
		}
	}

	/// <summary>
	/// The GraphQL input type for mutation input
	/// </summary>
	public class AssessmentNotesEntityInputType : InputObjectGraphType<AssessmentNotesEntity>
	{
		public AssessmentNotesEntityInputType()
		{
			Name = "AssessmentNotesEntityInput";
			Description = "The input object for adding a new AssessmentNotesEntity";

			// Add entity fields
			Field<IdGraphType>("Id");
			Field<DateTimeGraphType>("Created");
			Field<DateTimeGraphType>("Modified");
			Field<StringGraphType>("SessionNotes");
			Field<DateTimeGraphType>("SessionDate");

			// Add entity references
			Field<IdGraphType>("AssessmentsId");
			Field<IdGraphType>("StaffId");

			// Add references to foreign models to allow nested creation
			Field<AssessmentsEntityInputType>("Assessments");
			Field<StaffEntityInputType>("Staff");

			// % protected region % [Add any extra GraphQL input fields here] off begin
			// % protected region % [Add any extra GraphQL input fields here] end
		}
	}

}