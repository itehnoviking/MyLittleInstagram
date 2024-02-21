using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using MyLittleInstagram.Core.DTOs;
using MyLittleInstagram.Core.Interfaces.Services;
using MyLittleInstagram.Models.Requests;
using MyLittleInstagram.Models.Responses;
using Serilog;

namespace MyLittleInstagram.Controllers;

public class AccountController : ControllerBase
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;


        public AccountController(ITokenService tokenService, IMapper mapper, IAccountService accountService)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _accountService = accountService;
        }

        [HttpPost("auth")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(AuthenticateResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest? request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    return BadRequest(new ResponseMessage { Message = "Request is null or invalid" });
                }

                var dto = _mapper.Map<LoginDto>(request);
                var response = await _tokenService.GetTokenAsync(dto, GetIpAddress());

                if (response == null)
                {
                    return BadRequest(new ResponseMessage { Message = "Username or password is incorrect" });
                }

                SetTokenCookie(response.RefreshToken);

                return Ok(_mapper.Map<AuthenticateResponse>(response));
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, $"Failed to get orders: {ex.Message}");
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(AuthenticateResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refresh-token"];

                var response = await _tokenService.RefreshTokenAsync(refreshToken, GetIpAddress());

                if (response == null)
                {
                    return BadRequest(new ResponseMessage { Message = "Invalid token" });
                }

                SetTokenCookie(response.RefreshToken);

                return Ok(_mapper.Map<AuthenticateResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPost("revoke-token")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RevokeToken(RevokeTokenRequest request)
        {
            try
            {
                var token = request.Token ?? Request.Cookies["refresh-token"];

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new ResponseMessage() { Message = "Token is required" });
                }

                await _tokenService.RevokeTokenAsync(token, GetIpAddress());

                return Ok(new ResponseMessage { Message = "Token is revoked" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPost("register"), AllowAnonymous]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RegisterResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    return BadRequest(new ResponseMessage { Message = "Request is invalid or null!" });
                }

                var registerDto = _mapper.Map<RegisterDto>(request);

                var response = await _accountServiceCqs.CreateUserAsync(registerDto, request.Role);


                if (response == true)
                {
                    return Ok(new ResponseMessage { Message = "Registration completed!" });
                }

                else
                {
                    return BadRequest(new ResponseMessage { Message = "Logic app is wrong work" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPut("change-password"), Authorize]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid || request == null)
                {
                    return BadRequest(new ResponseMessage { Message = "Request is invalid or null!" });
                }

                var response =
                    await _accountServiceCqs.ChangePasswordAsync(request.Email, request.OldPassword,
                        request.NewPassword);

                return Ok(new ResponseMessage { Message = "Password successfully changed!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-forwarded-For"))
            {
                return Request.Headers["X-forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }
        }

        private void SetTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(2)
            };

            Response.Cookies.Append("refresh-token", refreshToken, cookieOptions);
        }

    }
}