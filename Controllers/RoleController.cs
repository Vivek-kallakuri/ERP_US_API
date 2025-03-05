using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;

namespace Cortracker360_Accurate_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RoleController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToList();
            return Ok(roles);
        }

        // Endpoint to update a user's role.
        [HttpPost("update")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateRoleModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.NewRole))
                return BadRequest("Email and NewRole are required.");

            // Find the user by email.
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("User not found.");

            // Get current roles of the user.
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove all current roles.
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return StatusCode(500, "Error removing user roles.");

            // Add the new role.
            var addResult = await _userManager.AddToRoleAsync(user, model.NewRole);
            if (!addResult.Succeeded)
                return StatusCode(500, "Error adding new role.");

            return Ok("User role updated successfully.");
        }
    }

    public class UpdateRoleModel
    {
        public string Email { get; set; }
        public string NewRole { get; set; }
    }
}
