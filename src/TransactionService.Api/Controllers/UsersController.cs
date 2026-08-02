using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.DTOs.Users;
using TransactionService.Application.Managers.Interfaces;

namespace TransactionService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
        {
            var users = await _userManager.GetAllAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetById(string id)
        {
            var user = await _userManager.GetByIdAsync(id);

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request)
        {
            var user = await _userManager.CreateAsync(request);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> Update(
        string id,
        UpdateUserRequest request)
        {
            var user = await _userManager.UpdateAsync(id, request);

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userManager.DeleteAsync(id);

            return NoContent();
        }
    }
}
