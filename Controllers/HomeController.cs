using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheetAPI.Data;
using TimeSheetAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Time_Sheet_API.Models;

namespace Time_Sheet_API.Controllers

{

    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TimesheetController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Timesheet>>> GetTimesheets()
        {
            return await _context.Timesheets.Include(t => t.Employee).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Timesheet>> GetTimesheet(int id)
        {
            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet == null)
            {
                return NotFound();
            }
            return timesheet;
        }

        [HttpPost]
        public async Task<ActionResult<Timesheet>> PostTimesheet(Timesheet timesheet)
        {
            _context.Timesheets.Add(timesheet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTimesheet), new { id = timesheet.Id }, timesheet);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimesheet(int id, Timesheet timesheet)
        {
            if (id != timesheet.Id)
            {
                return BadRequest();
            }

            _context.Entry(timesheet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Timesheets.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // 🔹 DELETE: api/timesheet/{id} (Delete a timesheet)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimesheet(int id)
        {
            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet == null)
            {
                return NotFound();
            }

            _context.Timesheets.Remove(timesheet);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

[Route("api/[controller]")]
[ApiController]
public class TimesheetController : ControllerBase
{
    private readonly AppDbContext _context;

    public TimesheetController(AppDbContext context)
    {
        _context = context;
    }

    // 1️⃣ Submit Timesheet
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitTimesheet([FromBody] Timesheet timesheet)
    {
        if (timesheet == null)
            return BadRequest("Invalid data.");

        timesheet.Status = "Pending"; // Default status
        _context.Timesheets.Add(timesheet);
        await _context.SaveChangesAsync();

        return Ok("Timesheet submitted successfully.");
    }

    // 2️⃣ Approve or Reject Timesheet
    [HttpPut("update-status/{id}")]
    public async Task<IActionResult> UpdateTimesheetStatus(int id, [FromQuery] string status)
    {
        var timesheet = await _context.Timesheets.FindAsync(id);
        if (timesheet == null)
            return NotFound("Timesheet not found.");

        if (status != "Approved" && status != "Rejected")
            return BadRequest("Invalid status.");

        timesheet.Status = status;
        await _context.SaveChangesAsync();

        return Ok($"Timesheet {status.ToLower()} successfully.");
    }
}
