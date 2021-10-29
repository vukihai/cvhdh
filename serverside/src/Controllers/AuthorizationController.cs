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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Firstapp2257.Exceptions;
using Firstapp2257.Helpers;
using Firstapp2257.Models;
using Firstapp2257.Models.Internal.Identity;
using Firstapp2257.Services.Interfaces;
using Firstapp2257.Services.TwoFactor;
using Firstapp2257.Utility;

// % protected region % [Customise Authorization Library imports here] off begin
using AspNet.Security.OpenIdConnect.Primitives;
using OpenIddict.Mvc.Internal;
// % protected region % [Customise Authorization Library imports here] end

// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257.Controllers
{
	/// <summary>
	/// Controller for authenticating existing users
	/// </summary>
	[Route("/api/authorization")]
	[ApiController]
	public class AuthorizationController : Controller
	{
		// % protected region % [Customise AuthorizationController fields here] off begin
		private readonly IUserService _userService;
		private readonly ISignInService _signInService;
		private readonly SignInManager<User> _signInManager;
		private readonly IXsrfService _xsrfService;
		private readonly UserManager<User> _userManager;
		private readonly ITwoFactorMethodEventFactory _twoFactorMethodEventFactory;
		private readonly IOptions<AntiforgeryOptions> _antiforgeryOptions;
		private readonly ILogger<AuthorizationController> _logger;
		// % protected region % [Customise AuthorizationController fields here] end

		// % protected region % [Add any additional fields here] off begin
		// % protected region % [Add any additional fields here] end

		public AuthorizationController(
			// % protected region % [Add any constructor arguments here] off begin
			// % protected region % [Add any constructor arguments here] end
			IUserService userService,
			ISignInService signInService,
			SignInManager<User> signInManager,
			IXsrfService xsrfService,
			UserManager<User> userManager,
			ITwoFactorMethodEventFactory twoFactorMethodEventFactory,
			IOptions<AntiforgeryOptions> antiforgeryOptions,
			ILogger<AuthorizationController> logger)
		{
			// % protected region % [Add any constructor initial logic here] off begin
			// % protected region % [Add any constructor initial logic here] end

			// % protected region % [Customise AuthorizationController middle logic here] off begin
			_userService = userService;
			_signInService = signInService;
			_signInManager = signInManager;
			_xsrfService = xsrfService;
			_userManager = userManager;
			_twoFactorMethodEventFactory = twoFactorMethodEventFactory;
			_antiforgeryOptions = antiforgeryOptions;
			_logger = logger;
			// % protected region % [Customise AuthorizationController middle logic here] end

			// % protected region % [Add any constructor end logic here] off begin
			// % protected region % [Add any constructor end logic here] end
		}

		// % protected region % [Customise Exchange method implementation here] off begin
		/// <summary>
		/// Grants a token to authenticate a user for a session. Tokens should be used by clients that don't support
		/// cookies such as mobile apps and api consumers.
		/// </summary>
		/// <param name="request">
		/// An x-www-form-urlencoded body with keys for grant_type, username and password.
		/// The only current supported grant type is "password"
		/// </param>
		/// <returns>A sign in result for with an access_token field for authentication</returns>
		[HttpPost("connect/token")]
		[Produces("application/json")]
		public async Task<ActionResult<OpenIdConnectResponse>> Exchange([ModelBinder(typeof(OpenIddictMvcBinder))] OpenIdConnectRequest request)
		{
			await using var timeBuffer = new TimeBufferedSection(100);
			try
			{
				var ticket = await _signInService.Exchange(request);
				return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
			}
			catch (InvalidUserPasswordException e)
			{
				return Unauthorized(new OpenIdConnectResponse
				{
					Error = OpenIdConnectConstants.Errors.InvalidGrant,
					ErrorDescription = e.Message
				});
			}
			catch (InvalidGrantTypeException e)
			{
				return Unauthorized(new OpenIdConnectResponse
				{
					Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
					ErrorDescription = e.Message
				});
			}
			catch
			{
				return Unauthorized();
			}
		}
		// % protected region % [Customise Exchange method implementation here] end

		// % protected region % [Configure authorization login logic here] off begin
		/// <summary>
		/// Logs into the site providing an auth cookie and a xsrf token
		/// </summary>
		/// <param name="details">The details required to login</param>
		/// <returns>
		/// 200 OK on success, or 401 on failure. If the request is successful it returns a XSRF token, an antiforgery
		/// token and a login token as cookies.
		/// </returns>
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody]LoginDetails details)
		{
			await using var timeBuffer = new TimeBufferedSection(100);
			var user = await _userManager.FindByNameAsync(details.Username);
			var result = SignInResult.Failed;
			if (user != null)
			{
				result = await _signInManager.PasswordSignInAsync(
					user,
					details.Password,
					details.RememberMe,
					false);
			}

			if (result.RequiresTwoFactor)
			{
				var method = await _twoFactorMethodEventFactory.PickTwoFactorMethodAsync(user);

				if (method == null)
				{
					return Unauthorized("This account is not allowed to log in");
				}

				var events = _twoFactorMethodEventFactory.GetTwoFactorMethodEvents(method);
				var token = await _userManager.GenerateUserTokenAsync(
					user,
					method,
					TwoFactorConstants.TokenPurpose);
				var loginData = await events.OnLogin(user, token);

				return Ok(new TwoFactorLoginResponse
				{
					Method = method,
					Data = loginData,
				});
			}

			if (result.IsLockedOut)
			{
				return Unauthorized("This account has been locked");
			}

			if (result.IsNotAllowed)
			{
				return Unauthorized("This account is not allowed to log in");
			}

			if (!result.Succeeded)
			{
				return Unauthorized("Invalid username or password");
			}

			_xsrfService.AddXsrfToken(HttpContext, HttpContext.GetStoredPrincipal());
			return Ok(await _userService.GetUser(user));
		}
		// % protected region % [Configure authorization login logic here] end

		/// <summary>
		/// Removes the authentication cookie that can be used to authenticate against the site
		/// </summary>
		/// <param name="redirect">A redirect to send to send after the finish of the request</param>
		/// <param name="clean">If set to true then all cookies are purged, otherwise only login related ones.</param>
		/// <returns>
		/// Either a 200 OK or a 302 Found if a redirect url is present
		/// </returns>
		[HttpGet("logout")]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout([FromQuery]string redirect, [FromQuery]bool clean = false)
		{
			// % protected region % [Configure logout login logic here] off begin
			// Sign out of the session
			await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

			// Sign out of the two factor auth scheme if a user is in the two factor sign in stage
			await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);

			// Delete antiforgery token
			Response.Cookies.Delete(
				_antiforgeryOptions.Value.Cookie.Name!,
				_antiforgeryOptions.Value.Cookie.Build(HttpContext));

			// Delete XSRF token
			_xsrfService.RemoveXsrfToken(HttpContext);

			if (clean)
			{
				// Delete any cookies
				foreach (var cookiesKey in HttpContext.Request.Cookies.Keys)
				{
					HttpContext.Response.Cookies.Delete(cookiesKey);
					HttpContext.Response.Cookies.Delete(cookiesKey, new CookieOptions
					{
						Secure = true,
					});
				}
			}

			// If given a redirect url go to that page, otherwise return Ok
			if (string.IsNullOrWhiteSpace(redirect))
			{
				return Ok();
			}

			return Redirect(redirect);
			// % protected region % [Configure logout login logic here] end
		}

		// % protected region % [Customise ValidateTwoFactorToken method here] off begin
		/// <summary>
		/// Validates a two factor token for a user. If the token is successfully validated then the user is signed into
		/// the application.
		/// </summary>
		/// <param name="model">Arguments for the validate request.</param>
		/// <param name="cancellationToken">A cancellation token for the operation.</param>
		/// <returns>200 OK on success.</returns>
		[HttpPost("validate-2fa")]
		public async Task<IActionResult> ValidateTwoFactorToken(
			[FromBody]TwoFactorDetailsModel model,
			CancellationToken cancellationToken = default)
		{
			var result = await _signInManager.TwoFactorSignInAsync(
				model.Method,
				model.Token,
				model.RememberMe,
				model.RememberTwoFactor);

			if (!result.Succeeded)
			{
				return Unauthorized();
			}

			return Ok();
		}
		// % protected region % [Customise ValidateTwoFactorToken method here] end

		private void AddErrors(IEnumerable<IdentityError> errors)
		{
			// % protected region % [Override AddErrors here] off begin
			foreach (var error in errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			// % protected region % [Override AddErrors here] end
		}

		// % protected region % [Add any authorization controller methods here] off begin
		// % protected region % [Add any authorization controller methods here] end
	}

	/// <summary>
	/// The details needed to login using cookie auth
	/// </summary>
	public class LoginDetails
	{
		// % protected region % [Customise login details] off begin
		/// <summary>
		/// The username of the user that is logging in
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// The password of the user that is logging in
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Should the user be sent a persistent token instead of a session token
		/// </summary>
		public bool RememberMe { get; set; }
		// % protected region % [Customise login details] end
	}

	public class TwoFactorDetailsModel
	{
		/// <summary>
		/// The two factor token to validate.
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		/// The two factor method to validate against.
		/// </summary>
		public string Method { get; set; }

		/// <summary>
		/// Is the remember me flag set for login.
		/// </summary>
		public bool RememberMe { get; set; } = false;

		/// <summary>
		/// Is the remember me flag set for the two factor token.
		/// </summary>
		public bool RememberTwoFactor { get; set; } = false;

		// % protected region % [Add any additional TwoFactorDetailsModel fields here] off begin
		// % protected region % [Add any additional TwoFactorDetailsModel fields here] end
	}

	// % protected region % [Add any additional methods here] off begin
	// % protected region % [Add any additional methods here] end
}
