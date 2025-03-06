using System.ComponentModel.DataAnnotations;

namespace Accurate_ERP.Models    
{
    public class Employee
    {
        //created a class connecting to a database here
        [Key]
        public int Id { get; set; }  // Primary Key
        public string? Name { get; set; }  // Employee Name
        public string? Email { get; set; }  // Employee Email
        public string? Phone { get; set; }  // Contact Number
        public string? Department { get; set; }  // Department Name
        public decimal Salary { get; set; }  // Employee Salary
    }
}