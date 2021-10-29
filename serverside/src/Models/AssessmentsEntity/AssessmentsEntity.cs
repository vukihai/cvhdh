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
	[Table("Assessments")]
	// % protected region % [Configure entity attributes here] end
	// % protected region % [Modify class declaration here] off begin
	public class AssessmentsEntity : IOwnerAbstractModel 
	// % protected region % [Modify class declaration here] end
	{
		[Key]
		public Guid Id { get; set; }
		public Guid Owner { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		// % protected region % [Customise StartDate here] off begin
		[EntityAttribute]
		public DateTime? StartDate { get; set; }
		// % protected region % [Customise StartDate here] end

		// % protected region % [Customise EndDate here] off begin
		[EntityAttribute]
		public DateTime? EndDate { get; set; }
		// % protected region % [Customise EndDate here] end

		// % protected region % [Customise Summary here] off begin
		[EntityAttribute]
		public String Summary { get; set; }
		// % protected region % [Customise Summary here] end

		// % protected region % [Customise Recommendations here] off begin
		[EntityAttribute]
		public String Recommendations { get; set; }
		// % protected region % [Customise Recommendations here] end

		// % protected region % [Add any further attributes here] off begin
		// % protected region % [Add any further attributes here] end

		public AssessmentsEntity()
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
			new VisitorsAssessmentsEntity(),
			new StaffAssessmentsEntity(),
			// % protected region % [Override ACLs here] end
			// % protected region % [Add any further ACL entries here] off begin
			// % protected region % [Add any further ACL entries here] end
		};

		// % protected region % [Customise AssessmentNotess here] off begin
		/// <summary>
		/// Incoming one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.AssessmentNotesEntity"/>
		[EntityForeignKey("AssessmentNotess", "Assessments", false, typeof(AssessmentNotesEntity))]
		public ICollection<AssessmentNotesEntity> AssessmentNotess { get; set; }
		// % protected region % [Customise AssessmentNotess here] end

		// % protected region % [Customise Students here] off begin
		/// <summary>
		/// Outgoing one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.StudentsEntity"/>
		public Guid? StudentsId { get; set; }
		[EntityForeignKey("Students", "Assessmentss", false, typeof(StudentsEntity))]
		public StudentsEntity Students { get; set; }
		// % protected region % [Customise Students here] end

		// % protected region % [Customise Commentss here] off begin
		/// <summary>
		/// Incoming one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.CommentsEntity"/>
		[EntityForeignKey("Commentss", "Assessments", false, typeof(CommentsEntity))]
		public ICollection<CommentsEntity> Commentss { get; set; }
		// % protected region % [Customise Commentss here] end

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
			var modelList = models.Cast<AssessmentsEntity>().ToList();
			var ids = modelList.Select(t => t.Id).ToList();

			switch (reference)
			{
				case "AssessmentNotess":
					var assessmentNotesIds = modelList.SelectMany(x => x.AssessmentNotess.Select(m => m.Id)).ToList();
					var oldassessmentNotes = await dbContext.AssessmentNotesEntity
						.Where(m => m.AssessmentsId.HasValue && ids.Contains(m.AssessmentsId.Value))
						.Where(m => !assessmentNotesIds.Contains(m.Id))
						.ToListAsync(cancellation);

					foreach (var assessmentNotes in oldassessmentNotes)
					{
						assessmentNotes.AssessmentsId = null;
					}

					dbContext.AssessmentNotesEntity.UpdateRange(oldassessmentNotes);
					return oldassessmentNotes.Count;
				case "Commentss":
					var commentsIds = modelList.SelectMany(x => x.Commentss.Select(m => m.Id)).ToList();
					var oldcomments = await dbContext.CommentsEntity
						.Where(m => m.AssessmentsId.HasValue && ids.Contains(m.AssessmentsId.Value))
						.Where(m => !commentsIds.Contains(m.Id))
						.ToListAsync(cancellation);

					foreach (var comments in oldcomments)
					{
						comments.AssessmentsId = null;
					}

					dbContext.CommentsEntity.UpdateRange(oldcomments);
					return oldcomments.Count;
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