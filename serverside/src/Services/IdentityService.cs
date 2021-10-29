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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Firstapp2257.Helpers;
using Firstapp2257.Models;
using Firstapp2257.Services.Interfaces;
using Firstapp2257.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Services
{
	public class IdentityService : IIdentityService
	{
		public bool Fetched { get; set; } = false;

		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<User> _userManager;

		/// <inheritdoc />
		public User User { get; set; }

		/// <inheritdoc />
		public IList<string> Groups { get; set; }

		/// <inheritdoc />
		public bool TwoFactorAuthenticated { get; set; }

		// % protected region % [Add any extra class variables here] off begin
		// % protected region % [Add any extra class variables here] end

		public IdentityService(
			// % protected region % [Add any extra constructor arguments here] off begin
			// % protected region % [Add any extra constructor arguments here] end
			IHttpContextAccessor httpContextAccessor,
			UserManager<User> userManager)
		{
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
			// % protected region % [Add any extra constructor logic here] off begin
			// % protected region % [Add any extra constructor logic here] end
		}

		/// <inheritdoc />
		public async Task RetrieveUserAsync()
		{
			// % protected region % [Change RetrieveUserAsync here] off begin
			if (Fetched != true)
			{
				if (_httpContextAccessor.HttpContext?.User.Identity is not ClaimsIdentity identity)
				{
					Fetched = true;
					User = null;
					Groups = null;
					return;
				}

				User = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
				Groups = User == null
					? new List<string>()
					: identity.Claims
						.Where(x => x.Type == identity.RoleClaimType)
						.Select(x => x.Value)
						.ToList();
				Groups.AddRange(SecurityUtilities.GetAllAcls()
					.Where(x => x.IsVisitorAcl && x.Group != null)
					.Select(x => x.Group)
					.ToHashSet());
				TwoFactorAuthenticated = await _httpContextAccessor.HttpContext.IsTwoFactorAuthenticated();
				Fetched = true;
			}
			// % protected region % [Change RetrieveUserAsync here] end
		}

		// % protected region % [Add any further methods here] off begin
		// % protected region % [Add any further methods here] end
	}
}