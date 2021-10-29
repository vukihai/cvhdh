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
using System.Threading.Tasks;
using Firstapp2257.Exceptions;
using Firstapp2257.Models;
using Firstapp2257.Models.RegistrationModels;
using Firstapp2257.Services;
using Firstapp2257.Services.Interfaces;
using Firstapp2257.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Controllers
{
	[ApiController]
	[Authorize]
	[Route("/api/register")]
	public class RegisterController : Controller
	{
		private readonly IUserService _userService;
		private readonly ILogger<RegisterController> _logger;

		public RegisterController(
			IUserService userService,
			ILogger<RegisterController> logger)
		{
			_userService = userService;
			_logger = logger;
		}

		// % protected region % [The default register function for Staff Entity] off begin
		/// <summary>
		/// Registers a new Staff Entity user
		/// </summary>
		/// <param name="registrationModel">The fields to set on the user</param>
		/// <returns>A user result on success or a list of errors on failure</returns>
		[HttpPost]
		[Route("staff-entity")]
		[Authorize(Roles = "Visitors,Staff,Super Administrators")]
		public async Task<IActionResult> RegisterStaffEntity([FromBody] StaffEntityRegistrationModel registrationModel)
		{
			var model = registrationModel.ToModel();
			return await Register(model, registrationModel.Password, registrationModel.Groups);
		}
		// % protected region % [The default register function for Staff Entity] end


		// % protected region % [Customise confirm email here] off begin
		[HttpPost]
		[Route("confirm-email")]
		[AllowAnonymous]
		public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel model)
		{
			await using var timeBuffer = new TimeBufferedSection(100);
			var result = await _userService.ConfirmEmail(model.Email, model.Token);

			if (!result.Succeeded)
			{
				_logger.LogInformation("User confirm email validation failed for {Email}", model.Email);
				_logger.LogInformation(JsonConvert.SerializeObject(result));
				return Unauthorized();
			}

			return Ok();
		}
		// % protected region % [Customise confirm email here] end

		// % protected region % [The default register function] off begin
		private async Task<IActionResult> Register(User model, string password, IEnumerable<string> groups)
		{
			try
			{
				var result = await _userService.RegisterUser(model, password, groups, true);

				if (result.Result.Succeeded == false)
				{
					_logger.LogInformation("Failed to create user {Email}", model.Email);
					_logger.LogInformation(JsonConvert.SerializeObject(result.Result));
					return BadRequest(new ApiErrorResponse(result.Result.Errors.Select(e => e.Description)));
				}

				var userResponse = await _userService.GetUser(result.User);
				return Ok(userResponse);
			}
			catch (DuplicateUserException e)
			{
				_logger.LogInformation("Attempted to create duplicate user. Email: {Email}", model.Email);
				// In the case of a duplicate user return a 409 Conflict response code
				return StatusCode(StatusCodes.Status409Conflict, new ApiErrorResponse(e.Message));
			}
		}
		// % protected region % [The default register function] end

		// % protected region % [Add any extra registration methods here] off begin
		// % protected region % [Add any extra registration methods here] end
	}

	// % protected region % [Add any extra content here] off begin
	// % protected region % [Add any extra content here] end
}