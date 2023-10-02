using adoptera_hund.api.Models;
using adoptera_hund.api.Services;
using adoptera_hund.api.ViewModels;
using adoptera_hund.api.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace adoptera_hund.api.Controllers;
    [ApiController]
    [Route("api/v1/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly TokenService _tokenService;
        public AccountController(UserManager<UserModel> userManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager; 
        }
        

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }
            return Ok(new UserViewModel{Email = user.Email, Token = await _tokenService.CreateToken(user)});
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(await _userManager.FindByNameAsync(model.UserName) != null)
            {
                return BadRequest("Username is taken");
            }
            if(await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return BadRequest("Email is taken");
            }
            var user = new UserModel
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            
            var result = await _userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            await _userManager.AddToRoleAsync(user, "User");
            return StatusCode(201);
        }
    }
