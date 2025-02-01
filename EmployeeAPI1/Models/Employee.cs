using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI1.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Employee last name is required.")]
        [StringLength(64, ErrorMessage = "Employee last name cannot exceed 64 characters.")]
        public string Employee_LastName { get; set; }

        [Required(ErrorMessage = "Employee first name is required.")]
        [StringLength(64, ErrorMessage = "Employee first name cannot exceed 64 characters.")]
        public string Employee_FirstName { get; set; }

        [Required(ErrorMessage = "Employee date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime Employee_DateofBirth { get; set; }

        [Required(ErrorMessage = "Employee date of joining is required.")]
        [DataType(DataType.Date)]
        public DateTime Employee_DateofJoining { get; set; }

        [Required(ErrorMessage = "Employee department is required.")]
        [StringLength(64, ErrorMessage = "Employee department cannot exceed 64 characters.")]
        public string Employee_Department { get; set; }

        [Required(ErrorMessage = "Employee salary is required.")]
        [Range(0, 9999999.99, ErrorMessage = "Employee salary must be between 0 and 9999999.99.")]
        public decimal Employee_Salary { get; set; }

        [Required(ErrorMessage = "Logged user ID is required.")]
        public int LoggedUserID { get; set; }
    }
}