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
using System.Threading.Tasks;
using Firstapp2257.Models;
using ServersideTests.Helpers;
using Xunit;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// to prevent warnings of using the model type in theory data.
#pragma warning disable xUnit1026
#pragma warning disable S2699

namespace ServersideTests.Tests.Integration.BotWritten.GroupSecurityTests.Read
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Integration")]
	[Trait("Category", "Security")]
	public class UnauthenticatedReadTests : BaseReadTest
	{

		public UnauthenticatedReadTests()
		{
			// % protected region % [Add constructor logic here] off begin
			// % protected region % [Add constructor logic here] end
		}

		public static TheoryData<IAbstractModel, bool, string> UnauthenticatedReadSecurityData 
		{
			get
			{
				var data = new TheoryData<IAbstractModel, bool, string>
				{
					// % protected region % [Configure entity theory data for Unauthenticated here] off begin
					{new AchievementsEntity(), true, null},
					{new AddressEntity(), true, null},
					{new StudentsEntity(), true, null},
					{new AssessmentNotesEntity(), true, null},
					{new AssessmentsEntity(), true, null},
					{new CommentsEntity(), true, null},
					{new AchievementsSubmissionEntity(), true, null},
					{new AddressSubmissionEntity(), true, null},
					{new StudentsSubmissionEntity(), true, null},
					{new AchievementsEntityFormTileEntity(), true, null},
					{new AddressEntityFormTileEntity(), true, null},
					{new StudentsEntityFormTileEntity(), true, null},
					// % protected region % [Configure entity theory data for Unauthenticated here] end
				};
				// % protected region % [Add any extra theory data here] off begin
				// % protected region % [Add any extra theory data here] end
				return data;
			}
		}

		// % protected region % [Overwrite read security test attributes here] off begin
		[Theory]
		[MemberData(nameof(UnauthenticatedReadSecurityData))]
		// % protected region % [Overwrite read security test attributes here] end
		public async Task UnauthenticatedReadSecurityTests<T>(T entity, bool canRead, string groupName)
			where T : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Overwrite read security test here] off begin
			await ReadTest(entity, canRead, groupName);
			// % protected region % [Overwrite read security test here] end
		}
	}
}