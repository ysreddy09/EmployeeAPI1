using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using EmployeeAPI1.Models;

namespace EmployeeAPI1.Data
{
    public class EmployeeRepository
    {
        private readonly IConfiguration _configuration;

        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        public async Task<dynamic> UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                using (var conn = Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@return", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@errorID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@errorMessage", dbType: DbType.String, size: 2048, direction: ParameterDirection.Output);
                    parameters.Add("@EmployeeID", employee.EmployeeID);
                    parameters.Add("@Employee_LastName", employee.Employee_LastName);
                    parameters.Add("@Employee_FirstName", employee.Employee_FirstName);
                    parameters.Add("@Employee_DateofBirth", employee.Employee_DateofBirth);
                    parameters.Add("@Employee_DateofJoining", employee.Employee_DateofJoining);
                    parameters.Add("@Employee_Department", employee.Employee_Department);
                    parameters.Add("@Employee_Salary", employee.Employee_Salary);
                    parameters.Add("@loggedUserID", employee.LoggedUserID);

                    // Execute the stored procedure
                    await conn.ExecuteAsync("USP_Employee_Info_Update", parameters, commandType: CommandType.StoredProcedure);

                    // Get the output parameters
                    var returnCode = parameters.Get<int>("@return");
                    var errorID = parameters.Get<int>("@errorID");
                    var errorMessage = parameters.Get<string>("@errorMessage");
                    var updatedEmployeeID = parameters.Get<int>("@EmployeeID");
                    var updatedLastName = parameters.Get<string>("@Employee_LastName");
                    var updatedFirstName = parameters.Get<string>("@Employee_FirstName");
                    var updatedDateOfBirth = parameters.Get<DateTime>("@Employee_DateofBirth");
                    var updatedDateOfJoining = parameters.Get<DateTime>("@Employee_DateofJoining");
                    var updatedDepartment = parameters.Get<string>("@Employee_Department");
                    var updatedSalary = parameters.Get<decimal>("@Employee_Salary");

                    // Return all parameters, including success or failure information
                    return new
                    {
                        Return = returnCode,
                        ErrorID = errorID,
                        ErrorMessage = errorMessage,
                        Message = returnCode == 1 ? "Employee updated successfully." : "Employee update failed.",
                        Employee = new
                        {
                            EmployeeID = updatedEmployeeID,
                            Employee_LastName = updatedLastName,
                            Employee_FirstName = updatedFirstName,
                            Employee_DateofBirth = updatedDateOfBirth,
                            Employee_DateofJoining = updatedDateOfJoining,
                            Employee_Department = updatedDepartment,
                            Employee_Salary = updatedSalary
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    Return = -1,
                    ErrorID = -1,
                    ErrorMessage = $"An error occurred while updating the employee: {ex.Message}",
                    Message = "An unexpected error occurred."
                };
            }
        }


        public async Task<dynamic> CreateEmployeeAsync(Employee employee)
        {
            try
            {
                using (var conn = Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@return", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@errorID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@errorMessage", dbType: DbType.String, size: 2048, direction: ParameterDirection.Output);
                    parameters.Add("@EmployeeID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@Employee_LastName", employee.Employee_LastName);
                    parameters.Add("@Employee_FirstName", employee.Employee_FirstName);
                    parameters.Add("@Employee_DateofBirth", employee.Employee_DateofBirth);
                    parameters.Add("@Employee_DateofJoining", employee.Employee_DateofJoining);
                    parameters.Add("@Employee_Department", employee.Employee_Department);
                    parameters.Add("@Employee_Salary", employee.Employee_Salary);
                    parameters.Add("@loggedUserID", employee.LoggedUserID);

                    // Execute the stored procedure
                    await conn.ExecuteAsync("USP_Employee_Info_Insert", parameters, commandType: CommandType.StoredProcedure);

                    // Get the output parameters
                    var returnCode = parameters.Get<int>("@return");
                    var errorID = parameters.Get<int>("@errorID");
                    var errorMessage = parameters.Get<string>("@errorMessage");
                    var employeeID = parameters.Get<int>("@EmployeeID");
                    var lastName = parameters.Get<string>("@Employee_LastName");
                    var firstName = parameters.Get<string>("@Employee_FirstName");
                    var dateOfBirth = parameters.Get<DateTime>("@Employee_DateofBirth");
                    var dateOfJoining = parameters.Get<DateTime>("@Employee_DateofJoining");
                    var department = parameters.Get<string>("@Employee_Department");
                    var salary = parameters.Get<decimal>("@Employee_Salary");

                    // Return all parameters, including success or failure information
                    return new
                    {
                        Return = returnCode,
                        ErrorID = errorID,
                        ErrorMessage = errorMessage,
                        Message = returnCode == 1 ? "Employee created successfully." : "Employee creation failed.",
                        Employee = new
                        {
                            EmployeeID = employeeID,
                            Employee_LastName = lastName,
                            Employee_FirstName = firstName,
                            Employee_DateofBirth = dateOfBirth,
                            Employee_DateofJoining = dateOfJoining,
                            Employee_Department = department,
                            Employee_Salary = salary
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    Return = -1,
                    ErrorID = -1,
                    ErrorMessage = $"An error occurred while creating the employee: {ex.Message}",
                    Message = "An unexpected error occurred."
                };
            }
        }

        public async Task<dynamic> GetEmployeesAsync(int? employeeID, string? employeeName, int loggedUserID)
        {
            try
            {
                using (var conn = Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@return", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@errorID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@errorMessage", dbType: DbType.String, size: 2048, direction: ParameterDirection.Output);
                    parameters.Add("@EmployeeID", employeeID);
                    parameters.Add("@EmployeeName", employeeName);
                    parameters.Add("@loggedUserID", loggedUserID);

                    var employees = await conn.QueryAsync<Employee>("USP_Employee_Info_List", parameters, commandType: CommandType.StoredProcedure);

                    return new
                    {
                        Return = parameters.Get<int>("@return"),
                        ErrorID = parameters.Get<int>("@errorID"),
                        ErrorMessage = parameters.Get<string>("@errorMessage"),
                        Employees = employees
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    Return = -1,
                    ErrorID = -1,
                    ErrorMessage = $"An error occurred while retrieving employees: {ex.Message}"
                };
            }
        }

        public async Task<dynamic> DeleteEmployeeAsync(int employeeID, int loggedUserID)
        {
            try
            {
                using (var conn = Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@return", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@errorID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@errorMessage", dbType: DbType.String, size: 2048, direction: ParameterDirection.Output);
                    parameters.Add("@EmployeeID", employeeID);
                    parameters.Add("@loggedUserID", loggedUserID);

                    await conn.ExecuteAsync("USP_Employee_Info_Delete", parameters, commandType: CommandType.StoredProcedure);

                    // Get the output parameters
                    var returnCode = parameters.Get<int>("@return");
                    var errorID = parameters.Get<int>("@errorID");
                    var errorMessage = parameters.Get<string>("@errorMessage");
                    var employeeIDOutput = parameters.Get<int>("@EmployeeID");
                    var loggedUserIDOutput = parameters.Get<int>("@loggedUserID");

                    // Debug the values
                    Console.WriteLine($"Return Code: {returnCode}, Error ID: {errorID}, Error Message: {errorMessage}");

                    // Return all parameters including success or failure information
                    return new
                    {
                        Return = returnCode,
                        ErrorID = errorID,
                        ErrorMessage = errorMessage,
                        EmployeeID = employeeIDOutput,
                        LoggedUserID = loggedUserIDOutput,
                        //Message = returnCode == 1 ? "Employee deleted successfully." : "Employee deletion failed."
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    Return = -1,
                    ErrorID = -1,
                    ErrorMessage = $"An error occurred while deleting the employee: {ex.Message}",
                    Message = "An unexpected error occurred."
                };
            }
        }


    }
}