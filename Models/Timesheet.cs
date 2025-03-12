using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimeSheetAPI.Models
{
    public class Timesheet
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime Date { get; set; }
        public int HoursWorked { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
    }
}
