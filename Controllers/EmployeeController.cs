using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accurate_ERP.Data;
using Accurate_ERP.Models;

namespace Accurate_ERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public EmployeesController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // POST: api/Employees
       [HttpPost]
public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
{
    Console.WriteLine($"Received employee: {employee.Name}, {employee.Email}");

    // Ensure that the employee data is valid (add any necessary validation)
    if (employee == null)
    {
        return BadRequest("Invalid employee data");
    }

    _context.Employees.Add(employee);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
}

        // PUT: api/Employees/5
       [HttpPut("{id}")]
public async Task<IActionResult> PutEmployee(int id, Employee employee)

{
    if (id != employee.Id)
    {
        return BadRequest("Employee ID mismatch");
    }

    _context.Entry(employee).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!EmployeeExists(id))
        {
            return NotFound();
        }
        else
        {
            throw;
        }
    }

    return Ok("Update success");
}

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}