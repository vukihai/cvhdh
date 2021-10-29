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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firstapp2257.Exceptions;
using Firstapp2257.Helpers;
using Firstapp2257.Models;
using Firstapp2257.Models.Internal;
using Firstapp2257.Models.Internal.Identity;
using Firstapp2257.Services;
using Firstapp2257.Services.Interfaces;
using Firstapp2257.Services.TwoFactor;
using Firstapp2257.Utility;
using GraphQL.EntityFramework;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Controllers
{
	/// <summary>
	/// Controller for managing users
	/// </summary>
	[Route("/api/account")]
	[Authorize]
	[ApiController]
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly IUserService _userService;
		private readonly RoleManager<Group> _roleManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IIdentityService _identityService;
		private readonly ITwoFactorMethodEventFactory _twoFactorMethodEventFactory;
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<AccountController> _logger;

		// % protected region % [Add extra account controller fields] off begin
		// % protected region % [Add extra account controller fields] end

		public class UsernameModel
		{
			// % protected region % [Override UsernameModel fields here] off begin
			/// <summary>
			/// The username to reset the password of
			/// </summary>
			[Required]
			public string Username { get; set; }
			// % protected region % [Override UsernameModel fields here] end
		}

		public class ResetPasswordModel
		{
			// % protected region % [Override ResetPasswordModel fields here] off begin
			/// <summary>
			/// The username to reset the password for
			/// </summary>
			[Required]
			public string Username { get; set; }

			/// <summary>
			/// The password to reset to
			/// </summary>
			[Required]
			public string Password { get; set; }

			/// <summary>
			/// The password reset token
			/// </summary>
			[Required]
			public string Token { get; set; }
			// % protected region % [Override ResetPasswordModel fields here] end
		}

		public class AllUserRequestModel
		{
			// % protected region % [Override AllUserRequestModel fields here] off begin
			/// <summary>
			/// The conditions for sorting user entities
			/// </summary>
			public List<OrderBy> SortConditions { get; set; }
			/// <summary>
			/// The pagination options
			/// </summary>
			public PaginationOptions PaginationOptions { get; set; }
			/// <summary>
			/// The search conditions
			/// </summary>
			public IEnumerable<IEnumerable<WhereExpression>> SearchConditions { get; set; }
			// % protected region % [Override AllUserRequestModel fields here] end
		}

		public class UserListModel
		{
			// % protected region % [Override UserListModel fields here] off begin
			/// <summary>
			/// The total number of users
			/// </summary>
			[Required]
			public int countUsers { get; set; }
			/// <summary>
			/// A paginated list of users
			/// </summary>
			[Required]
			public IEnumerable<UserDto> Users { get; set; }
			// % protected region % [Override UserListModel fields here] end
		}

		public class ConfigureTwoFactorModel
		{
			/// <summary>
			/// The type of 2 factor method to be configured.
			/// </summary>
			[Required]
			public string Method { get; set; }

			/// <summary>
			/// The username for the user to configure the 2 factor method for.
			/// </summary>
			[Required]
			public string UserName { get; set; }
		}

		public class DisableTwoFactorModel
		{
			/// <summary>
			/// The username of the user to disable 2fa for.
			/// </summary>
			[Required]
			public string UserName { get; set; }
		}

		public AccountController(
			// % protected region % [Add extra account controller arguments] off begin
			// % protected region % [Add extra account controller arguments] end
			UserManager<User> userManager,
			IUserService userService,
			RoleManager<Group> roleManager,
			SignInManager<User> signInManager,
			IIdentityService identityService,
			ITwoFactorMethodEventFactory twoFactorMethodEventFactory,
			IServiceProvider serviceProvider,
			ILogger<AccountController> logger)
		{
			// % protected region % [Add extra account controller constructor logic] off begin
			// % protected region % [Add extra account controller constructor logic] end
			_userManager = userManager;
			_userService = userService;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_identityService = identityService;
			_twoFactorMethodEventFactory = twoFactorMethodEventFactory;
			_serviceProvider = serviceProvider;
			_logger = logger;
		}

		/// <summary>
		/// Gets the logged in user
		/// </summary>
		/// <returns>The current logged in user</returns>
		[HttpGet]
		[HttpPost]
		[Produces("application/json")]
		[Route("me")]
		[Authorize]
		public async Task<UserResult> Get()
		{
			// % protected region % [Override Get here] off begin
			var user = await _userService.GetUser(User);
			return user;
			// % protected region % [Override Get here] end
		}

		/// <summary>
		/// Gets all the user groups in the system
		/// </summary>
		/// <returns>A list of user groups</returns>
		/// <response code="200">On a successful response</response>
		/// <response code="401">On failing to authenticate</response>
		[HttpGet]
		[Authorize]
		[Route("groups")]
		[ProducesResponseType(200)]
		[ProducesResponseType(401)]
		public async Task<IEnumerable<string>> GetRoles()
		{
			// % protected region % [Override GetRoles here] off begin
			return await _roleManager.Roles.Select(group => group.Name).ToListAsync();
			// % protected region % [Override GetRoles here] end
		}

		/// <summary>
		/// Get a paginated list of all the users in the system
		/// </summary>
		/// <returns> A list of user list model </returns>
		/// <response code="200"> On a successful response </response>
		/// <response code="401"> On failing to authenticate </response>
		[HttpPost]
		[Authorize]
		[Route("users")]
		[ProducesResponseType(200)]
		[ProducesResponseType(401)]
		public async Task<UserListModel> GetUsers([FromBody] AllUserRequestModel options)
		{
			// % protected region % [Override GetUsers here] off begin
			await _identityService.RetrieveUserAsync();

			var userQuery = _userManager.Users
				.Where(UsersFilter.AllUsersFilter(
					_identityService.User,
					_identityService.Groups,
					DATABASE_OPERATION.READ,
					_serviceProvider))
				.AddConditionalWhereFilter(options.SearchConditions)
				.AddOrderBys(options.SortConditions);

			var users = await userQuery
				.AddPagination(new Pagination(options.PaginationOptions))
				.ToListAsync();

			var dtos = new List<UserDto>();
			foreach (var user in users)
			{
				var dto = new UserDto(user);

				// If a user has 2fa enabled then get their 2fa provider.
				if (dto.TwoFactorEnabled)
				{
					dto.TwoFactorMethod = await _twoFactorMethodEventFactory.PickTwoFactorMethodAsync(user);
				}

				dtos.Add(dto);
			}

			return new UserListModel
			{
				countUsers = await userQuery.CountAsync(),
				Users = dtos,
			};
			// % protected region % [Override GetUsers here] end
		}

		/// <summary>
		/// Deactivates a user
		/// </summary>
		/// <returns> 200OK on success </returns>
		/// <response code="200"> On a successful response </response>
		/// <response code="401"> On failing to authenticate </response>
		[HttpPost]
		[Authorize]
		[Route("deactivate")]
		[ProducesResponseType(200)]
		[ProducesResponseType(401)]
		public async Task<IActionResult> DeactivateUser([FromBody] UsernameModel deactivateUser)
		{
			// % protected region % [Override DeactivateUser here] off begin
			await _identityService.RetrieveUserAsync();

			var user = _userManager.Users
				.Where(UsersFilter.AllUsersFilter(
					_identityService.User,
					_identityService.Groups,
					DATABASE_OPERATION.UPDATE,
					_serviceProvider))
				.FirstOrDefault(u => u.UserName == deactivateUser.Username);

			if (user == null)
			{
				return BadRequest("The user does not exist or you do not have permission to deactivate the user");
			}

			// Lock them out until the end of time and update the security stamp to invalidate all current logins.
			user.LockoutEnd = DateTimeOffset.MaxValue;
			await _userManager.UpdateSecurityStampAsync(user);

			return Ok();
			// % protected region % [Override DeactivateUser here] end
		}

		/// <summary>
		/// Activates a user
		/// </summary>
		/// <returns> 200OK on success </returns>
		/// <response code="200"> On a successful response </response>
		/// <response code="401"> On failing to authenticate </response>
		[HttpPost]
		[Authorize]
		[Route("activate")]
		[ProducesResponseType(200)]
		[ProducesResponseType(401)]
		public async Task<IActionResult> ActivateUser([FromBody] UsernameModel userModel)
		{
			// % protected region % [Override ActivateUser here] off begin
			await _identityService.RetrieveUserAsync();

			var user = _userManager
				.Users
				.Where(UsersFilter.AllUsersFilter(
					_identityService.User,
					_identityService.Groups,
					DATABASE_OPERATION.UPDATE,
					_serviceProvider))
				.FirstOrDefault(u => u.UserName == userModel.Username);

			if (user == null)
			{
				return BadRequest("The user does not exist or you do not have permission to activate the user");
			}

			user.EmailConfirmed = true;
			await _userManager.SetLockoutEndDateAsync(user, null);

			return Ok();
			// % protected region % [Override ActivateUser here] end
		}

		// % protected region % [adjust the reset password endpoint] off begin
		/// <summary>
		/// Sends a reset password email to a specified user
		/// </summary>
		/// <param name="userModel">The user details</param>
		/// <returns>Returns 200 OK</returns>
		[HttpPost("reset-password-request")]
		[AllowAnonymous]
		[ProducesResponseType(200)]
		public async Task<IActionResult> ResetPasswordRequest([FromBody]UsernameModel userModel)
		{
			await using var timeBuffer = new TimeBufferedSection(100);
			try
			{
				var user = await _userManager.Users.FirstAsync(u => u.UserName == userModel.Username);
				await _userService.SendPasswordResetEmail(user);
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
			}
			return Ok();
		}
		// % protected region % [adjust the reset password endpoint] end

		// % protected region % [Customize reset password endpoint here] off begin
		/// <summary>
		/// Resets a users password for the forgot password workflow
		/// </summary>
		/// <param name="details">The username, password and reset password token for the user</param>
		/// <returns>200 OK on success and 401 on failure</returns>
		[HttpPost("reset-password")]
		[AllowAnonymous]
		[ProducesResponseType(200)]
		[ProducesResponseType(401)]
		public async Task<IActionResult> ResetPassword(ResetPasswordModel details)
		{
			await using var timeBuffer = new TimeBufferedSection(100);
			try
			{
				var user = _userManager.Users.FirstOrDefault(u => u.UserName == details.Username);

				if (!await _userManager.VerifyUserTokenAsync(
					user,
					_userManager.Options.Tokens.PasswordResetTokenProvider,
					"ResetPassword",
					details.Token))
				{
					throw new UnauthorizedAccessException($"Invalid password reset token for {details.Username}");
				}

				var result = await _userManager.ResetPasswordAsync(user, details.Token, details.Password);
				if (!result.Succeeded)
				{
					throw new IdentityOperationException(result);
				}

				return Ok();
			}
			catch (IdentityOperationException e)
			{
				_logger.LogError(e.ToString());
				return BadRequest(new ApiErrorResponse(e.IdentityResult.Errors.Select(ie => ie.Description)));
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				return Unauthorized(new ApiErrorResponse("Could not update user"));
			}
		}
		// % protected region % [Customize reset password endpoint here] end

		// % protected region % [Customize get two factor methods endpoint here] off begin
		/// <summary>
		/// Gets the 2 factor methods that are available to be configured.
		/// </summary>
		/// <param name="userName">The username of the user to get the valid 2 factor methods for.</param>
		/// <param name="cancellationToken">Cancellation token for the operation.</param>
		/// <returns>list of 2 factor methods available to be configured.</returns>
		[HttpGet("valid-2fa")]
		[Authorize]
		[ProducesResponseType(typeof(IEnumerable<string>), 200)]
		public async Task<IActionResult> GetTwoFactorMethods(
			[FromQuery]string userName,
			CancellationToken cancellationToken = default)
		{
			await _identityService.RetrieveUserAsync();
			var user = await _userManager
				.Users
				.Where(UsersFilter.AllUsersFilter(
					_identityService.User,
					_identityService.Groups,
					DATABASE_OPERATION.READ,
					_serviceProvider))
				.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);

			if (user == null)
			{
				return Forbid();
			}

			return Ok(await _twoFactorMethodEventFactory.GetConfigurableTwoFactorMethods(user));
		}
		// % protected region % [Customize get two factor methods endpoint here] end

		// % protected region % [Customize configure two factor endpoint here] off begin
		/// <summary>
		/// Configures a 2 factor authentication method. Note that if the user that that is specified to
		/// configure two factor for in this request is logged in then they will be logged out within a minute.
		/// If the current logged in user is the user being configured then they are logged out immediately and are
		/// sent a two factor challenge.
		/// </summary>
		/// <param name="model">Parameters for the configuration.</param>
		/// <param name="cancellationToken">Cancellation token for the operation.</param>
		/// <returns>Information for the ui on how to finish setup of the method.</returns>
		[HttpPost("configure-2fa")]
		[Authorize]
		public async Task<IActionResult> ConfigureTwoFactorAuthentication(
			[FromBody]ConfigureTwoFactorModel model,
			CancellationToken cancellationToken = default)
		{
			await _identityService.RetrieveUserAsync();
			var user = await _userManager
				.Users
				.Where(UsersFilter.AllUsersFilter(
					_identityService.User,
					_identityService.Groups,
					DATABASE_OPERATION.UPDATE,
					_serviceProvider))
				.FirstOrDefaultAsync(u => u.UserName == model.UserName, cancellationToken);

			if (user == null)
			{
				return Forbid();
			}

			var methodEvents = _twoFactorMethodEventFactory.GetTwoFactorMethodEvents(model.Method);

			try
			{
				var result = await methodEvents.OnConfiguring(user);
				if (_identityService.User.UserName != model.UserName)
				{
					return Ok(result);
				}

				// If the current logged in user is the user that had the two factor authentication method changed then
				// their security stamp will be changed and they will be logged out. Sign them in and if they need two
				// factor then send them a token.
				await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
				var signInResult = await _signInManager.SignInOrTwoFactorAsync(user, false);
				if (!signInResult.RequiresTwoFactor)
				{
					return Ok(result);
				}

				var token = await _userManager.GenerateUserTokenAsync(
					user,
					model.Method,
					TwoFactorConstants.TokenPurpose);
				await methodEvents.OnLogin(user, token);
				return Ok(result);
			}
			catch (Exception e)
			{
				await methodEvents.OnRemoveMethod(user);
				_logger.LogError("Error while activating authenticator app: {Exception}", e);
				return Problem("Could not activate authenticator app");
			}
		}
		// % protected region % [Customize configure two factor endpoint here] end

		// % protected region % [Customize disable two factor endpoint here] off begin
		/// <summary>
		/// Removes two factor authentication from a user.
		/// </summary>
		/// <param name="model">Parameters for the operation.</param>
		/// <param name="cancellationToken">Cancellation token for the operation.</param>
		/// <returns>200 Ok on success. Other response codes otherwise.</returns>
		[HttpPost("disable-2fa")]
		[Authorize]
		public async Task<IActionResult> DisableTwoFactorAuthentication(
			[FromBody]DisableTwoFactorModel model,
			CancellationToken cancellationToken = default)
		{
			await _identityService.RetrieveUserAsync();
			var user = await _userManager
				.Users
				.Where(UsersFilter.AllUsersFilter(
					_identityService.User,
					_identityService.Groups,
					DATABASE_OPERATION.UPDATE,
					_serviceProvider))
				.FirstOrDefaultAsync(u => u.UserName == model.UserName, cancellationToken);

			if (user == null)
			{
				return Forbid();
			}

			if (!await _userManager.GetTwoFactorEnabledAsync(user))
			{
				return BadRequest("This user does not have two factor authentication enabled");
			}

			var method = await _twoFactorMethodEventFactory.PickTwoFactorMethodAsync(user);

			if (method is not null)
			{
				var methodEvents = _twoFactorMethodEventFactory.GetTwoFactorMethodEvents(method);
				await methodEvents.OnRemoveMethod(user);
			}
			else
			{
				// If a user has two factor enabled but no method, then they are in a bad state. In this case
				// we just need to clean up as best as we can.
				user.PreferredTwoFactorMethod = null;
				await _userManager.SetTwoFactorEnabledAsync(user, false);
			}

			// If the user has changed their own 2 factor method then their security stamp will be changed.
			// Therefore they need to be logged in with a new cookie. Since 2fa has just been removed use SignInAsync
			// instead of SignInOrTwoFactorAsync.
			if (_identityService.User.UserName == model.UserName)
			{
				await _signInManager.SignInAsync(user, false);
			}

			return Ok();
		}
		// % protected region % [Customize disable two factor endpoint here] end

		private void AddErrors(IEnumerable<IdentityError> errors)
		{
			// % protected region % [Override AddErrors here] off begin
			foreach (var error in errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			// % protected region % [Override AddErrors here] end
		}

		// % protected region % [Add any account controller methods here] off begin
		// % protected region % [Add any account controller methods here] end
	}
}

