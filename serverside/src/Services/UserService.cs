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
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Firstapp2257.Configuration;
using Firstapp2257.Exceptions;
using Firstapp2257.Models;
using Firstapp2257.Models.Internal.Emails;
using Firstapp2257.Models.Internal.Identity;
using Firstapp2257.Services.Interfaces;
using Firstapp2257.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RazorLight;

// % protected region % [Customise Authorization Library imports here] off begin
using AspNet.Security.OpenIdConnect.Primitives;
// % protected region % [Customise Authorization Library imports here] end

// % protected region % [Add any extra user service imports here] off begin
// % protected region % [Add any extra user service imports here] end

namespace Firstapp2257.Services
{
	/// <summary>
	/// Model for creating new users
	/// </summary>
	public class RegisterModel
	{

		// % protected region % [Customise RegisterModel fields] off begin
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		public ICollection<string> Groups { get; set; }
		// % protected region % [Customise RegisterModel fields] end

		// % protected region % [Add extra properties to the register model here] off begin
		// % protected region % [Add extra properties to the register model here] end
	}

	public class RegisterResult
	{
		public User User { get; set; }

		public IdentityResult Result { get; set; }

		// % protected region % [Add extra properties to the register result model here] off begin
		// % protected region % [Add extra properties to the register result model here] end
	}

	// % protected region % [Customise UpdateUserModel here] off begin
	public class UserUpdateModel : RegisterModel
	{
		public Guid Id { get; set; }

		public new string Password { get; set; }
	}
	// % protected region % [Customise UpdateUserModel here] end

	/// <summary>
	/// Service for handling user operations
	/// </summary>
	public class UserService : IUserService
	{
		private const string ResetEmailResourceKey = "Firstapp2257.Assets.Emails.ResetPassword";
		private const string RegisterEmailResourceKey = "Firstapp2257.Assets.Emails.RegisterEmail";

		private readonly IOptions<IdentityOptions> _identityOptions;
		private readonly IUserClaimsPrincipalFactory<User> _claimsPrincipalFactory;
		private readonly IServiceProvider _serviceProvider;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Group> _roleManager;
		private readonly IRazorLightEngine _razorLightEngine;
		private readonly IBackgroundJobService _backgroundJobService;
		private readonly ServerSettings _serverSettings;
		// % protected region % [Add any extra readonly fields here] off begin
		// % protected region % [Add any extra readonly fields here] end

		public UserService(
			// % protected region % [Add any extra params here] off begin
			// % protected region % [Add any extra params here] end
			IOptions<IdentityOptions> identityOptions,
			IUserClaimsPrincipalFactory<User> claimsPrincipalFactory,
			IServiceProvider serviceProvider,
			UserManager<User> userManager,
			RoleManager<Group> roleManager,
			IRazorLightEngine razorLightEngine,
			IBackgroundJobService backgroundJobService,
			IOptions<ServerSettings> serverSettings)
		{
			// % protected region % [Add initialisations here] off begin
			// % protected region % [Add initialisations here] end
			_identityOptions = identityOptions;
			_claimsPrincipalFactory = claimsPrincipalFactory;
			_serviceProvider = serviceProvider;
			_userManager = userManager;
			_roleManager = roleManager;
			_razorLightEngine = razorLightEngine;
			_backgroundJobService = backgroundJobService;
			_serverSettings = serverSettings.Value;
		}

		public async Task<List<UserResult>> GetUsers() {
			// % protected region % [Change get users here] off begin
			return await _userManager.Users.Select(user => new UserResult(user, null)).ToListAsync();
			// % protected region % [Change get users here] end
		}

		public async Task<UserResult> GetUser(ClaimsPrincipal principal)
		{
			// % protected region % [Change get user claims principal overload here] off begin
			try
			{
				var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == principal.Identity.Name);
				return await GetUser(user);
			}
			catch
			{
				throw new InvalidIdException();
			}
			// % protected region % [Change get user claims principal overload here] end
		}

		public async Task<UserResult> GetUser(User user)
		{
			// % protected region % [Change get user method here] off begin
			var roleNames = (await _userManager.GetRolesAsync(user)).ToList();
			var roles = await _roleManager.Roles.Where(role => roleNames.Contains(role.Name)).ToListAsync();
			return new UserResult(user, roles);
			// % protected region % [Change get user method here] end
		}

		public async Task<User> GetUserFromClaim(ClaimsPrincipal principal)
		{
			// % protected region % [Change get user from claim method here] off begin
			return await _userManager
				.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.UserName == principal.Identity.Name);
			// % protected region % [Change get user from claim method here] end
		}

		public async Task<RegisterResult> RegisterUser(
			RegisterModel model,
			IEnumerable<string> groups,
			bool sendRegisterEmail = false)
		{
			// % protected region % [Change register user method here] off begin
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user != null)
			{
				throw new DuplicateUserException();
			}

			user = new User
			{
				UserName = model.Email,
				Email = model.Email,
			};

			return await RegisterUser(user, model.Password, groups, sendRegisterEmail);
			// % protected region % [Change register user method here] end
		}

		public async Task<RegisterResult> RegisterUser(
			User user,
			string password,
			IEnumerable<string> groups,
			bool sendRegisterEmail = false)
		{
			// % protected region % [Change register user here] off begin
			// A user should own their own entity
			if(user.Id == default(Guid))
			{
				user.Id = Guid.NewGuid();
			}
			if (string.IsNullOrWhiteSpace(user.UserName))
			{
				user.UserName = user.Email;
			}

			user.Owner = user.Id;
			user.EmailConfirmed = !sendRegisterEmail;

			var result = await _userManager.CreateAsync(user, password);

			if(!result.Succeeded)
			{
				return new RegisterResult { Result = result, User = user };
			}

			var newUser = await _userManager.Users.FirstAsync(u => u.UserName == user.UserName);

			if (sendRegisterEmail)
			{
				var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

				var serverUrl = _serverSettings.ServerUrl;
				var uriBuilder = new UriBuilder($"{serverUrl}/confirm-email");

				var query = HttpUtility.ParseQueryString("");
				query["token"] = token;
				query["username"] = newUser.UserName;
				uriBuilder.Query = query.ToString()!;

				var email = await _razorLightEngine.CompileRenderAsync(
					RegisterEmailResourceKey,
					new RegisterEmailModel
					{
						UserName = user.UserName,
						ConfirmEmailUrl = uriBuilder.Uri.ToString(),
						ServiceProvider = _serviceProvider,
					});

				_backgroundJobService.StartBackgroundJob<IEmailService>(emailService => emailService.SendEmail(new EmailEntity
				{
					To = new[] {newUser.Email},
					Body = email,
					Subject = "Confirm Account",
				}, default));
			}

			if (groups != null)
			{
				await _userManager.AddToRolesAsync(newUser, groups);
			}

			return new RegisterResult { Result = result, User = newUser};
			// % protected region % [Change register user here] end
		}

		public async Task<IdentityResult> ConfirmEmail(string email, string token)
		{
			// % protected region % [Change confirm email method here] off begin
			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (user == null)
			{
				return IdentityResult.Failed(new IdentityErrorDescriber().InvalidToken());
			}
			user.LockoutEnd = null;
			return await _userManager.ConfirmEmailAsync(user, token);
			// % protected region % [Change confirm email method here] end
		}


		// % protected region % [Customise UpdateUser here.] off begin
		public async Task<IdentityResult> UpdateUser(UserUpdateModel model)
		{
			var user = await _userManager.FindByNameAsync(model.Email);

			if (user == null) {
				throw new UserNotFoundException();
			}

			user.UserName = model.Email;
			user.Email = model.Email;

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded && model.Password != null) {
				var token = await _userManager.GeneratePasswordResetTokenAsync(user);
				result = await _userManager.ResetPasswordAsync(user, token, model.Password);
			}

			if (model.Groups != null)
			{
				var currentGroups = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRolesAsync(user, currentGroups);
				await _userManager.AddToRolesAsync(user, model.Groups);
			}

			return result;
		}
		// % protected region % [Customise UpdateUser here.] end

		// % protected region % [Customise SendPasswordResetEmail method implementation here] off begin
		public async Task<bool> SendPasswordResetEmail(User user)
		{
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);

			var serverUrl = _serverSettings.ServerUrl;
			var uriBuilder = new UriBuilder($"{serverUrl}/reset-password");

			var query = HttpUtility.ParseQueryString("");
			query["username"] = user.UserName;
			query["token"] = token;

			uriBuilder.Query = query.ToString()!;

			var email = await _razorLightEngine.CompileRenderAsync(
				ResetEmailResourceKey,
				new ResetPasswordEmailModel
				{
					UserName = user.UserName,
					PasswordResetUrl = uriBuilder.Uri.ToString(),
					ServiceProvider = _serviceProvider,
				});

			_backgroundJobService.StartBackgroundJob<IEmailService>(emailService => emailService.SendEmail(new EmailEntity
			{
				To = new [] {user.Email},
				Body = email,
				Subject = "Reset Password",
			}, default));

			return true;
		}
		// % protected region % [Customise SendPasswordResetEmail method implementation here] end

		public async Task<bool> DeleteUser(Guid id)
		{
			// % protected region % [Change delete user method here] off begin
			var user = await _userManager.FindByIdAsync(id.ToString());

			if (user == null)
			{
				return false;
			}

			var result = await _userManager.DeleteAsync(user);
			return result.Succeeded;
			// % protected region % [Change delete user method here] end
		}

		public async Task<ClaimsPrincipal> CreateUserPrincipal(
			User user,
			string authenticationScheme = StaticIdentityConstants.ApplicationScheme)
		{
			// % protected region % [Adjust the claims if required] off begin
			var principal = await _claimsPrincipalFactory.CreateAsync(user);
			return new ClaimsPrincipal(new ClaimsIdentity(
				principal.Identity,
				principal.Claims,
				authenticationScheme,
				_identityOptions.Value.ClaimsIdentity.UserNameClaimType,
				_identityOptions.Value.ClaimsIdentity.RoleClaimType));
			// % protected region % [Adjust the claims if required] end
		}

		// % protected region % [Add any extra user service methods here] off begin
		// % protected region % [Add any extra user service methods here] end
	}
}
