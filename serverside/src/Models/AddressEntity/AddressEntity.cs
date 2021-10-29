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
	[Table("Address")]
	// % protected region % [Configure entity attributes here] end
	// % protected region % [Modify class declaration here] off begin
	public class AddressEntity : IOwnerAbstractModel 
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

		// % protected region % [Customise Unit here] off begin
		[EntityAttribute]
		public String Unit { get; set; }
		// % protected region % [Customise Unit here] end

		// % protected region % [Customise AddressLine1 here] off begin
		[EntityAttribute]
		public String AddressLine1 { get; set; }
		// % protected region % [Customise AddressLine1 here] end

		// % protected region % [Customise AddressLine2 here] off begin
		[EntityAttribute]
		public String AddressLine2 { get; set; }
		// % protected region % [Customise AddressLine2 here] end

		// % protected region % [Customise Suburb here] off begin
		[EntityAttribute]
		public String Suburb { get; set; }
		// % protected region % [Customise Suburb here] end

		// % protected region % [Customise Postcode here] off begin
		[EntityAttribute]
		public int? Postcode { get; set; }
		// % protected region % [Customise Postcode here] end

		// % protected region % [Customise City here] off begin
		[EntityAttribute]
		public String City { get; set; }
		// % protected region % [Customise City here] end

		// % protected region % [Customise Country here] off begin
		[EntityAttribute]
		public String Country { get; set; }
		// % protected region % [Customise Country here] end

		// % protected region % [Add any further attributes here] off begin
		// % protected region % [Add any further attributes here] end

		public AddressEntity()
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
			new VisitorsAddressEntity(),
			new StaffAddressEntity(),
			// % protected region % [Override ACLs here] end
			// % protected region % [Add any further ACL entries here] off begin
			// % protected region % [Add any further ACL entries here] end
		};

		/// <summary>
		/// Reference to the versions for this form
		/// </summary>
		/// <see cref="Firstapp2257.Models.AddressEntityFormVersion"/>
		[EntityForeignKey("FormVersions", "Form", false, typeof(AddressEntityFormVersion))]
		public ICollection<AddressEntityFormVersion> FormVersions { get; set; }

		/// <summary>
		/// The current published version for the form
		/// </summary>
		/// <see cref="Firstapp2257.Models.AddressEntityFormVersion"/>
		public Guid? PublishedVersionId { get; set; }
		[EntityForeignKey("PublishedVersion", "PublishedForm", false, typeof(AddressEntityFormVersion))]
		public AddressEntityFormVersion PublishedVersion { get; set; }

		// % protected region % [Customise Staffss here] off begin
		/// <summary>
		/// Incoming one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.StaffEntity"/>
		[EntityForeignKey("Staffss", "Address", false, typeof(StaffEntity))]
		public ICollection<StaffEntity> Staffss { get; set; }
		// % protected region % [Customise Staffss here] end

		// % protected region % [Customise Studentss here] off begin
		/// <summary>
		/// Incoming one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.StudentsEntity"/>
		[EntityForeignKey("Studentss", "Address", false, typeof(StudentsEntity))]
		public ICollection<StudentsEntity> Studentss { get; set; }
		// % protected region % [Customise Studentss here] end

		// % protected region % [Customise FormPages here] off begin
		/// <summary>
		/// Incoming one to many reference
		/// </summary>
		/// <see cref="Firstapp2257.Models.AddressEntityFormTileEntity"/>
		[EntityForeignKey("FormPages", "Form", false, typeof(AddressEntityFormTileEntity))]
		public ICollection<AddressEntityFormTileEntity> FormPages { get; set; }
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
			var modelList = models.Cast<AddressEntity>().ToList();
			var ids = modelList.Select(t => t.Id).ToList();

			switch (reference)
			{
				case "Staffss":
					var staffsIds = modelList.SelectMany(x => x.Staffss.Select(m => m.Id)).ToList();
					var oldstaffs = await dbContext.StaffEntity
						.Where(m => m.AddressId.HasValue && ids.Contains(m.AddressId.Value))
						.Where(m => !staffsIds.Contains(m.Id))
						.ToListAsync(cancellation);

					foreach (var staffs in oldstaffs)
					{
						staffs.AddressId = null;
					}

					dbContext.StaffEntity.UpdateRange(oldstaffs);
					return oldstaffs.Count;
				case "Studentss":
					var studentsIds = modelList.SelectMany(x => x.Studentss.Select(m => m.Id)).ToList();
					var oldstudents = await dbContext.StudentsEntity
						.Where(m => m.AddressId.HasValue && ids.Contains(m.AddressId.Value))
						.Where(m => !studentsIds.Contains(m.Id))
						.ToListAsync(cancellation);

					foreach (var students in oldstudents)
					{
						students.AddressId = null;
					}

					dbContext.StudentsEntity.UpdateRange(oldstudents);
					return oldstudents.Count;
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