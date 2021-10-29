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
using System.Net;
using APITests.EntityObjects.Models;
using APITests.Factories;
using APITests.Setup;
using APITests.Utils;
using EntityObject.Enums;
using RestSharp;
using Xunit;
using Xunit.Abstractions;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace APITests.Tests.BotWritten
{
	public class ValidatorApiTests : IClassFixture<StartupTestFixture>
	{
		private readonly StartupTestFixture _configure;
		private readonly ITestOutputHelper _output;

		public ValidatorApiTests(StartupTestFixture configure, ITestOutputHelper output)
		{
			_configure = configure;
			_output = output;
		}

		// create multiple entities by passing in the factory and number you wish to create
		public static TheoryData<EntityFactory> EntitiesFactoryData()
		{
			return new TheoryData<EntityFactory>
			{
				new EntityFactory("StudentsEntity"),
				new EntityFactory("StaffEntity"),
			};
		}
		// % protected region % [Add custom CreateInvalidEntity traits here] off begin
		// % protected region % [Add custom CreateInvalidEntity traits here] end
		// % protected region % [Customize CreateInvalidEntity here] off begin
		[Theory]
		[Trait("Category", "BotWritten")]
		[Trait("Category", "Integration")]
		[MemberData(nameof(EntitiesFactoryData))]
		public void CreateInvalidEntity(EntityFactory entityFactory)
		{
			var entityObject = entityFactory.Construct(1)[0];

			//setup the rest client
			var client = new RestClient
			{
				BaseUrl = new Uri($"{_configure.BaseUrl}/api/graphql")
			};

			// get references to other entities
			foreach (var reference in entityObject.References)
			{
				var referenceIdName =  reference.Type == ReferenceType.MANY
					? reference.OppositeName + "s"
					: reference.OppositeName + "Id" ;

				//if we have a self reference, set it to null and continue
				if (reference.EntityName == entityObject.EntityName)
				{
					// set all entitites in the entity list to null
					entityObject.ReferenceIdDictionary[referenceIdName] = null;
					continue;
				}

				var createdEntity = new CreateApiTests(_configure, _output).CreateEntities(new EntityFactory(reference.EntityName),1)[0];

				//add the created entity to our dictionary for the child entity for all entities in the list
				entityObject.ReferenceIdDictionary[referenceIdName] = createdEntity.Id;
			}

			var invalidEntities = entityObject.GetInvalidMutatedJsons().ToList();
			CheckInvalidJsonsForInvalidResponse(entityObject, invalidEntities, client);
		}
		// % protected region % [Customize CreateInvalidEntity here] end

		// % protected region % [Customize CheckInvalidJsonsForInvalidResponse method here] off begin
		private void CheckInvalidJsonsForInvalidResponse(BaseEntity entityObject, List<(string expectedError,
			RestSharp.JsonObject jsonObject)> invalidEntities, RestClient client)
		{
			// Looping through to test one by one because invalid enum error is thrown by
			// GraphQl Deserializing and it only returns the first error it comes with in one request
			invalidEntities.ForEach(invalidEntity =>
			{
				var invalidJsons = new List<RestSharp.JsonObject>() { invalidEntity.jsonObject };

				//setup the request
				var request = RequestHelpers.BasicPostRequest();
				var loginToken = new LoginToken(_configure.BaseUrl, _configure.SuperUsername, _configure.SuperPassword);
				var authorizationToken = $"{loginToken.TokenType} {loginToken.AccessToken}";
				request.AddHeader("Authorization", authorizationToken);
				var query = QueryBuilder.InvalidEntityQueryBuilder(entityObject, invalidJsons);
				request.AddParameter("text/json", query, ParameterType.RequestBody);

				// execute the request
				var response = client.Execute(request);

				// check the response contains the expected error for the failed validator
				Assert.Contains(invalidEntity.expectedError, response.Content);

				// check that a bad request status code was returned in the response
				Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
			});
		}
		// % protected region % [Customize CheckInvalidJsonsForInvalidResponse method here] end
	}
}