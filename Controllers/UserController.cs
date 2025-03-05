using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Cortracker360_Accurate_API.Controllers
{
    // NOTE: Authentication and authorization are intentionally disabled for local development.
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/Users
        // Returns a list of all users along with their assigned roles.
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesList = await Task.WhenAll(users.Select(async user => new
            {
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            }));
            return Ok(userRolesList);
        }

        // DELETE: api/Users?email=user@example.com
        // Deletes a user by email.
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound($"User with email {email} not found.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return StatusCode(500, "Error deleting user.");

            return Ok("User deleted successfully.");
        }
    }
}
