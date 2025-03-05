using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;

namespace Cortracker360_Accurate_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Manager")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: api/Role
        // Returns all available roles.
        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles
                .Select(r => new { r.Id, r.Name })
                .ToList();
            return Ok(roles);
        }

        // POST: api/Role/update
        // Updates a user's role by removing all current roles and assigning a new one.
        [HttpPost("update")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateRoleModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.NewRole))
                return BadRequest("Email and NewRole are required.");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("User not found.");

            // Remove all current roles.
            var currentRoles = await _userManager.GetRolesAsync(user);
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
