using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            // var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindEmailFromClaimsPrincipal(User);
            // var user = await _userManager.FindByEmailAsync(email);

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTO>> GetAddress()
        {
            // var email = User.FindFirstValue(ClaimTypes.Email);
            // var user = await _userManager.FindByEmailAsync(email);
            var user = await _userManager.FindUsersByClaimsPrincipalWithAddressAsync(User);


            return _mapper.Map<Address, AddressDTO>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO address)
        {
            var user = await _userManager.FindUsersByClaimsPrincipalWithAddressAsync(User);
            user.Address = _mapper.Map<AddressDTO, Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if(result.Succeeded) return Ok(_mapper.Map<Address, AddressDTO>(user.Address));

            return BadRequest("Problem updating the user");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if(CheckEmailExistsAsync(registerDTO.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []
                {"Email address is in use"}});
            }
            var user = new AppUser
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                UserName = registerDTO.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }
    }
}