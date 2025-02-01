using Microsoft.AspNetCore.Mvc;
using EmployeeAPI1.Data;
using EmployeeAPI1.Models;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository _repository;

        public EmployeeController(IConfiguration configuration)
        {
            _repository = new EmployeeRepository(configuration);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
        {
            try
            {
                var result = await _repository.UpdateEmployeeAsync(employee);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = $"An error occurred in the controller: {ex.Message}" });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                var result = await _repository.CreateEmployeeAsync(employee);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = $"An error occurred in the controller: {ex.Message}" });
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetEmployees([FromQuery] int? employeeID, [FromQuery] string? employeeName, [FromQuery] int loggedUserID)
        {
            try
            {
                var result = await _repository.GetEmployeesAsync(employeeID, employeeName, loggedUserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = $"An error occurred in the controller: {ex.Message}" });
            }
        }

        [HttpDelete("delete/{employeeID}")]
        public async Task<IActionResult> DeleteEmployee(int employeeID, [FromQuery] int loggedUserID)
        {
            try
            {
                var result = await _repository.DeleteEmployeeAsync(employeeID, loggedUserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = $"An error occurred in the controller: {ex.Message}" });
            }
        }
    }
}