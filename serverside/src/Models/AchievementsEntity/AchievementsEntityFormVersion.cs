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
	/// <summary>
	/// The form versions for the Achievements Entity form behaviour
	/// </summary>
	// % protected region % [Configure entity attributes here] off begin
	[Table("AchievementsEntityFormVersion")]
	// % protected region % [Configure entity attributes here] end
	// % protected region % [Modify class declaration here] off begin
	public class AchievementsEntityFormVersion : IOwnerAbstractModel 
	// % protected region % [Modify class declaration here] end
	{
		[Key]
		public Guid Id { get; set; }
		public Guid Owner { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		/// <summary>
		/// The version number of this form version
		/// </summary>
		// % protected region % [Customise Version here] off begin
		[EntityAttribute]
		public int? Version { get; set; }
		// % protected region % [Customise Version here] end

		/// <summary>
		/// The form data for this version
		/// </summary>
		[Column(TypeName = "text")]
		// % protected region % [Customise FormData here] off begin
		[EntityAttribute]
		public String FormData { get; set; }
		// % protected region % [Customise FormData here] end

		[NotMapped]
		public bool PublishVersion { get; set; } = false;

		// % protected region % [Add any further attributes here] off begin
		// % protected region % [Add any further attributes here] end

		public AchievementsEntityFormVersion()
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
			new VisitorsAchievementsEntity(),
			new StaffAchievementsEntity(),
			// % protected region % [Override ACLs here] end
			// % protected region % [Add any further ACL entries here] off begin
			// % protected region % [Add any further ACL entries here] end
		};

		/// <summary>
		/// Reference to the form entity for this version entity
		/// </summary>
		/// <see cref="Firstapp2257.Models.AchievementsEntity"/>
		public Guid FormId { get; set; }
		[EntityForeignKey("Form", "FormVersions", false, typeof(AchievementsEntity))]
		public AchievementsEntity Form { get; set; }

		/// <summary>
		/// The current form that this version is published against if it is published
		/// </summary>
		/// <see cref="Firstapp2257.Models.AchievementsEntity"/>
		[EntityForeignKey("PublishedForm", "PublishedVersion", false, typeof(AchievementsEntity))]
		public AchievementsEntity? PublishedForm { get; set; }

		/// <summary>
		/// Reference to the submissions for this form version
		/// </summary>
		/// <see cref="Firstapp2257.Models.AchievementsSubmissionEntity"/>
		[EntityForeignKey("FormSubmissions", "FormVersion", false, typeof(AchievementsSubmissionEntity))]
		public ICollection<AchievementsSubmissionEntity> FormSubmissions { get; set; }

		public async Task BeforeSave(
			EntityState operation,
			Firstapp2257DBContext dbContext,
			IServiceProvider serviceProvider,
			CancellationToken cancellationToken = default)
		{
			// % protected region % [Add any initial before save logic here] off begin
			// % protected region % [Add any initial before save logic here] end

			if (operation == EntityState.Added)
			{
				var lastVersion = dbContext
					.AchievementsEntityFormVersion
					.AsNoTracking()
					.OrderByDescending(m => m.Version)
					.FirstOrDefault(m => m.FormId == FormId);
				Version = lastVersion != null ? lastVersion.Version + 1 : 1;
			}

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

			if (PublishVersion)
			{
				var formModel = dbContext.AchievementsEntity.FirstOrDefault(m => m.Id == FormId);
				if (formModel != null)
				{
					formModel.PublishedVersionId = Id;
					dbContext.SaveChanges();
				}
			}
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
			var modelList = models.Cast<AchievementsEntityFormVersion>().ToList();
			var ids = modelList.Select(t => t.Id).ToList();

			switch (reference)
			{
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