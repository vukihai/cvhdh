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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firstapp2257.Enums;
using Firstapp2257.Security;
using Firstapp2257.Security.Acl;
using Firstapp2257.Validators;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Z.EntityFramework.Plus;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Firstapp2257.Models {
	// % protected region % [Configure entity attributes here] off begin
	[Table("Students")]
	// % protected region % [Configure entity attributes here] end
	// % protected region % [Modify class declaration here] off begin
	public class StudentsEntity : IOwnerAbstractModel 
	// % protected region % [Modify class declaration here] end
	{
		[Key]
		public Guid Id { get; set; }
		public Guid Owner { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		[Required]
		[EntityAttribute]
		public string Name { get; set; }

		[Required]
		// % protected region % [Customise FirstName here] off begin
		[EntityAttribute]
		public String FirstName { get; set; }
		// % protected region % [Customise FirstName here] end

		[Required]
		// % protected region % [Customise LastName here] off begin
		[EntityAttribute]
		public String LastName { get; set; }
		// % protected region % [Customise LastName here] end

		// % protected region % [Customise ContactNumber here] off begin
		[EntityAttribute]
		public String ContactNumber { get; set; }
		// % protected region % [Customise ContactNumber here] end

		// % protected region % [Customise Email here] off begin
		[EntityAttribute]
		public String Email { get; set; }
		// % protected region % [Customise Email here] end

		// % protected region % [Customise EnrolmentStart here] off begin
		[EntityAttribute]
		public DateTime? EnrolmentStart { get; set; }
		// % protected region % [Customise EnrolmentStart here] end

		// % protected region % [Customise EnrolmentEnd here] off begin
		[EntityAttribute]
		public DateTime? EnrolmentEnd { get; set; }
		// % protected region % [Customise EnrolmentEnd here] end

		// % protected region % [Add any further attributes here] off begin
		// % protected region % [Add any further attributes here] end

		public StudentsEntity()
		{
			// % protected region % [Add any constructor logic here] off begin
			// % protected region % [Add any constructor logic here] end
		}

		// % protected region % [Customise ACL attributes here] off begin
		[NotMapped]
		[JsonIgnore]
		// % protected region % [Customise ACL attributes here] end
		public IEnumerable<IAcl> Acls => new List<IAcl>
		{
			// % protected region % [Override ACLs here] off begin
			new SuperAdministratorsScheme(),
			new VisitorsStudentsEntity(),
			new StaffStudentsEntity(),
			// % protected region % [Override ACLs here] end
			// % protected region % [Add any further ACL entries here] off begin
			// % protected region % [Add any further ACL entries here] end
		};

		/// <summary>
		/// Reference to the versions for this form
		/// </summary>
		/// <see cref="Firstapp2257.Models.StudentsEntityFormVersion"/>
		[EntityForeignKey("FormVersions", "Form", false, typeof(StudentsEntityFormVersion))]
		public ICollection<StudentsEntityFormVersion> FormVersions { get; set; }

		/// <summary>
		/// The current published version for the form
		/// </summary>
		/// <see cref="Firstapp2257.Models.StudentsEntityFormVersion"/>
		public Guid? PublishedVersionId { get; set; }
		[EntityForeignKey("PublishedVersion", "PublishedForm", false, typeof(StudentsEntityFormVersion))]
		public StudentsEntityFormVersion PublishedVersion { get; set; }

		// % protected region % [Customise Achievementss here] off begin
		/// <summary>
		/// Incoming one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.AchievementsEntity"/>
		[EntityForeignKey("Achievementss", "Students", false, typeof(AchievementsEntity))]
		public ICollection<AchievementsEntity> Achievementss { get; set; }
		// % protected region % [Customise Achievementss here] end

		// % protected region % [Customise Assessmentss here] off begin
		/// <summary>
		/// Incoming one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.AssessmentsEntity"/>
		[EntityForeignKey("Assessmentss", "Students", false, typeof(AssessmentsEntity))]
		public ICollection<AssessmentsEntity> Assessmentss { get; set; }
		// % protected region % [Customise Assessmentss here] end

		// % protected region % [Customise Address here] off begin
		/// <summary>
		/// Outgoing one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.AddressEntity"/>
		public Guid? AddressId { get; set; }
		[EntityForeignKey("Address", "Studentss", false, typeof(AddressEntity))]
		public AddressEntity Address { get; set; }
		// % protected region % [Customise Address here] end

		// % protected region % [Customise FormPages here] off begin
		/// <summary>
		/// Incoming one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.StudentsEntityFormTileEntity"/>
		[EntityForeignKey("FormPages", "Form", false, typeof(StudentsEntityFormTileEntity))]
		public ICollection<StudentsEntityFormTileEntity> FormPages { get; set; }
		// % protected region % [Customise FormPages here] end

		public async Task BeforeSave(
			EntityState operation,
			Firstapp2257DBContext dbContext,
			IServiceProvider serviceProvider,
			CancellationToken cancellationToken = default)
		{
			// % protected region % [Add any initial before save logic here] off begin
			// % protected region % [Add any initial before save logic here] end

			// % protected region % [Add any before save logic here] off begin
			// % protected region % [Add any before save logic here] end
		}

		public async Task AfterSave(
			EntityState operation,
			Firstapp2257DBContext dbContext,
			IServiceProvider serviceProvider,
			ICollection<ChangeState> changes,
			CancellationToken cancellationToken = default)
		{
			// % protected region % [Add any initial after save logic here] off begin
			// % protected region % [Add any initial after save logic here] end

			// % protected region % [Add any after save logic here] off begin
			// % protected region % [Add any after save logic here] end
		}

		public async Task<int> CleanReference<T>(
			string reference,
			IEnumerable<T> models,
			Firstapp2257DBContext dbContext,
			CancellationToken cancellation = default)
			where T : IOwnerAbstractModel
			{
			var modelList = models.Cast<StudentsEntity>().ToList();
			var ids = modelList.Select(t => t.Id).ToList();

			switch (reference)
			{
				case "Achievementss":
					var achievementsIds = modelList.SelectMany(x => x.Achievementss.Select(m => m.Id)).ToList();
					var oldachievements = await dbContext.AchievementsEntity
						.Where(m => m.StudentsId.HasValue && ids.Contains(m.StudentsId.Value))
						.Where(m => !achievementsIds.Contains(m.Id))
						.ToListAsync(cancellation);

					foreach (var achievements in oldachievements)
					{
						achievements.StudentsId = null;
					}

					dbContext.AchievementsEntity.UpdateRange(oldachievements);
					return oldachievements.Count;
				case "Assessmentss":
					var assessmentsIds = modelList.SelectMany(x => x.Assessmentss.Select(m => m.Id)).ToList();
					var oldassessments = await dbContext.AssessmentsEntity
						.Where(m => m.StudentsId.HasValue && ids.Contains(m.StudentsId.Value))
						.Where(m => !assessmentsIds.Contains(m.Id))
						.ToListAsync(cancellation);

					foreach (var assessments in oldassessments)
					{
						assessments.StudentsId = null;
					}

					dbContext.AssessmentsEntity.UpdateRange(oldassessments);
					return oldassessments.Count;
				// % protected region % [Add any extra clean reference logic here] off begin
				// % protected region % [Add any extra clean reference logic here] end
				default:
					return 0;
			}
		}

		// % protected region % [Add any further references here] off begin
		// % protected region % [Add any further references here] end
	}
}