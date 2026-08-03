using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.DTOs.Users;
using TransactionService.Application.Managers.Interfaces;

namespace TransactionService.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for managing users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>The user list.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
        {
            var users = await _userManager.GetAllAsync();

            return Ok(users);
        }

        /// <summary>
        /// Retrieves a user by identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The requested user.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetById(string id)
        {
            var user = await _userManager.GetByIdAsync(id);

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="request">User information.</param>
        /// <returns>The created user.</returns>
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request)
        {
            var user = await _userManager.CreateAsync(request);

            return Ok(user);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="request">The updated user information.</param>
        /// <returns>The updated user.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> Update(
        string id,
        UpdateUserRequest request)
        {
            var user = await _userManager.UpdateAsync(id, request);

            return Ok(user);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userManager.DeleteAsync(id);

            return NoContent();
        }
    }
}
