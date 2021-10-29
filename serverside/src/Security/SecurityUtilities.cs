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
using System.Collections.Generic;
using Firstapp2257.Security.Acl;

// % protected region % [Add any additional imports here] off begin
// % protected region % [Add any additional imports here] end

namespace Firstapp2257.Security
{
	public static class SecurityUtilities
	{
		public static IEnumerable<IAcl> GetAllAcls()
		{
			return new List<IAcl>
			{
				new SuperAdministratorsScheme(),
				new VisitorsAddressEntity(),
				new VisitorsStaffEntity(),
				new VisitorsStudentsEntity(),
				new VisitorsAssessmentNotesEntity(),
				new VisitorsAssessmentsEntity(),
				new VisitorsAchievementsEntity(),
				new VisitorsCommentsEntity(),
				new VisitorsListStudentPage(),
				new VisitorsFormEditStudentPage(),
				new VisitorsEditArchivementPage(),
				new VisitorsMainMenuPage(),
				new StaffAddressEntity(),
				new StaffStaffEntity(),
				new StaffStudentsEntity(),
				new StaffAssessmentNotesEntity(),
				new StaffAssessmentsEntity(),
				new StaffAchievementsEntity(),
				new StaffCommentsEntity(),
				new StaffListStudentPage(),
				new StaffFormEditStudentPage(),
				new StaffEditArchivementPage(),
				new StaffMainMenuPage(),
				new VisitorsAchievementsSubmission(),
				new StaffAchievementsSubmission(),
				new VisitorsAddressSubmission(),
				new StaffAddressSubmission(),
				new VisitorsStudentsSubmission(),
				new StaffStudentsSubmission(),
				// % protected region % [Add any additional ACLs to the return list here] off begin
				// % protected region % [Add any additional ACLs to the return list here] end
			};
		}
	}
}