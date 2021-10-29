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
using System.Linq;
using System.Net;
using APITests.EntityObjects;
using APITests.EntityObjects.Models;
using APITests.Factories;
using APITests.Setup;
using APITests.TheoryData.BotWritten;
using APITests.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using RestSharp;
using Xunit;
using Xunit.Abstractions;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace APITests.Tests.BotWritten
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Integration")]
	public class UserTests : IClassFixture<StartupTestFixture>
	{
		// % protected region % [Customise UserTests fields here] off begin
		private readonly StartupTestFixture _configure;
		private readonly ITestOutputHelper _output;
		// % protected region % [Customise UserTests fields here] end

		private const string UnregisteredAccountError = "This account is not yet activated";
		private const string SuperUsername = "super@example.com";
		private const string SuperPassword = "password";
		private const string InvalidPasswordError = "The username/password couple is invalid.";
		private const string MissingUsernamePasswordError = "The mandatory 'username' and/or 'password' parameters are missing.";

		public static TheoryData<string, string, string> GetExpectedInvalidLoginResponses()
		{
			return new TheoryData<string, string, string>
			{
				{ "invalid-user@example.com", "password",InvalidPasswordError},
				{ "", "password", MissingUsernamePasswordError},
				{ "invalid-user@example.com", "", MissingUsernamePasswordError},
				{ "", "",MissingUsernamePasswordError},
			};
		}

		public static TheoryData<string, string, string> ValidTokenResponse()
		{
			return new TheoryData<string, string, string>
			{
				{SuperUsername, SuperPassword, "Bearer"}
			};
		}

		public UserTests(StartupTestFixture configure, ITestOutputHelper output)
		{
			_configure = configure;
			_output = output;
		}

		// % protected region % [Add custom RegistrationInvalidPasswordTests traits here] off begin
		// % protected region % [Add custom RegistrationInvalidPasswordTests traits here] end
		// % protected region % [Customize RegistrationInvalidPasswordTests here] off begin
		[Theory]
		[ClassData(typeof(PasswordInvalidTheoryData))]
		public void RegistrationInvalidPasswordTests(UserEntityFactory userEntityFactory, string password, string expectedException)
		{
			var userEntity = userEntityFactory.Construct();
			userEntity.Password = password;

			try
			{
				new Registration(userEntity, _output);
			}
			catch (AggregateException e)
			{
				var exceptionInList = e.InnerExceptions.Select(x => x.Message);
				Assert.Contains(expectedException, exceptionInList);
				return;
			}

			throw new Exception("User creation succeeded when it was expected to fail");
		}
		// % protected region % [Customize RegistrationInvalidPasswordTests here] end

		// % protected region % [Add custom RegistrationUserInvalidTests traits here] off begin
		// % protected region % [Add custom RegistrationUserInvalidTests traits here] end
		// % protected region % [Customize RegistrationUserInvalidTests here] off begin
		[Theory]
		[ClassData(typeof(UsernameInvalidTheoryData))]
		public void RegistrationUserInvalidTests(UserEntityFactory userEntityFactory, string username, string expectedException)
		{
			var userEntity = userEntityFactory.Construct();
			userEntity.EmailAddress = username;

			try
			{
				new Registration(userEntity, _output);
			}
			catch (AggregateException e)
			{
				var exceptionInList = e.InnerExceptions.Select(x => x.Message);
				Assert.Contains(expectedException, exceptionInList);
				return;
			}

			throw new Exception("User creation succeeded when it was expected to fail");
		}
		// % protected region % [Customize RegistrationUserInvalidTests here] end

		// % protected region % [Add custom ValidLoginUserTests traits here] off begin
		// % protected region % [Add custom ValidLoginUserTests traits here] end
		// % protected region % [Customize ValidLoginUserTests here] off begin
		[Theory]
		[MemberData(nameof(ValidTokenResponse))]
		public void ValidLoginUserTests(string username, string password, string expectedResponse)
		{
			//try to get an access token, if we get one then we're all sweet
			var loginTokenObject = new LoginToken(_configure.BaseUrl, username, password);
				var accessToken = loginTokenObject.AccessToken;
				var tokenType = loginTokenObject.TokenType;
				Assert.True(accessToken != null && tokenType == expectedResponse);
		}
		// % protected region % [Customize ValidLoginUserTests here] end

		// % protected region % [Add custom InvalidLoginUserTests traits here] off begin
		// % protected region % [Add custom InvalidLoginUserTests traits here] end
		// % protected region % [Customise InvalidLoginUserTests here] off begin
		[Theory]
		[MemberData(nameof(GetExpectedInvalidLoginResponses))]
		public void InvalidLoginUserTests(string username, string password, string expectedResponse)
		{
			Action act = () => new LoginToken(_configure.BaseUrl, username, password);
			act.Should().Throw<Exception>().WithMessage(expectedResponse);
		}
		// % protected region % [Customise InvalidLoginUserTests here] end

		// % protected region % [Add any additional tests here] off begin
		// % protected region % [Add any additional tests here] end
	}
}