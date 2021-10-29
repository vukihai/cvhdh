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
	// % protected region % [Configure entity attributes here] end
	// % protected region % [Modify class declaration here] off begin
	public class StaffEntity : User, IOwnerAbstractModel 
	// % protected region % [Modify class declaration here] end
	{
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

		// % protected region % [Customise Role here] off begin
		[EntityAttribute]
		public StaffRoles Role { get; set; }
		// % protected region % [Customise Role here] end

		// % protected region % [Customise ContactNumber here] off begin
		[EntityAttribute]
		public String ContactNumber { get; set; }
		// % protected region % [Customise ContactNumber here] end

		// % protected region % [Add any further attributes here] off begin
		// % protected region % [Add any further attributes here] end

		public StaffEntity()
		{
			// % protected region % [Add any constructor logic here] off begin
			// % protected region % [Add any constructor logic here] end
		}

		// % protected region % [Customise ACL attributes here] off begin
		[NotMapped]
		[JsonIgnore]
		// % protected region % [Customise ACL attributes here] end
		public override IEnumerable<IAcl> Acls => new List<IAcl>
		{
			// % protected region % [Override ACLs here] off begin
			new SuperAdministratorsScheme(),
			new VisitorsStaffEntity(),
			new StaffStaffEntity(),
			// % protected region % [Override ACLs here] end
			// % protected region % [Add any further ACL entries here] off begin
			// % protected region % [Add any further ACL entries here] end
		};

		// % protected region % [Customise AssessmentNotess here] off begin
		/// <summary>
		/// Incoming one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.AssessmentNotesEntity"/>
		[EntityForeignKey("AssessmentNotess", "Staff", false, typeof(AssessmentNotesEntity))]
		public ICollection<AssessmentNotesEntity> AssessmentNotess { get; set; }
		// % protected region % [Customise AssessmentNotess here] end

		// % protected region % [Customise Address here] off begin
		/// <summary>
		/// Outgoing one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.AddressEntity"/>
		public Guid? AddressId { get; set; }
		[EntityForeignKey("Address", "Staffss", false, typeof(AddressEntity))]
		public AddressEntity Address { get; set; }
		// % protected region % [Customise Address here] end

		public override async Task BeforeSave(
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

		public override async Task AfterSave(
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

		public async override Task<int> CleanReference<T>(
			string reference,
			IEnumerable<T> models,
			Firstapp2257DBContext dbContext,
			CancellationToken cancellation = default)
			{
			var modelList = models.Cast<StaffEntity>().ToList();
			var ids = modelList.Select(t => t.Id).ToList();

			switch (reference)
			{
				case "AssessmentNotess":
					var assessmentNotesIds = modelList.SelectMany(x => x.AssessmentNotess.Select(m => m.Id)).ToList();
					var oldassessmentNotes = await dbContext.AssessmentNotesEntity
						.Where(m => m.StaffId.HasValue && ids.Contains(m.StaffId.Value))
						.Where(m => !assessmentNotesIds.Contains(m.Id))
						.ToListAsync(cancellation);

					foreach (var assessmentNotes in oldassessmentNotes)
					{
						assessmentNotes.StaffId = null;
					}

					dbContext.AssessmentNotesEntity.UpdateRange(oldassessmentNotes);
					return oldassessmentNotes.Count;
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