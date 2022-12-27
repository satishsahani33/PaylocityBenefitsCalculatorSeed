using Api.CustomException;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repository;
using Api.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Fetch list of all employees
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [SwaggerOperation(Summary = "Get all employees")]
        [HttpGet("{pageNumber}/{pageSize}/{orderBy}/{sortBy}")]
        public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll(int pageNumber=1, int pageSize=100, string orderBy = "Id", string sortBy="asc")
        {
            try
            {
                //task: use a more realistic production approach

                var result = new ApiResponse<List<GetEmployeeDto>>();
                var employees = await _employeeRepository.GetAllEmployees(pageNumber, pageSize, orderBy,sortBy);
                if(employees != null)
                {
                    result.Data = employees.ToList();
                    result.Success = true;
                    result.Message = "Fetches all employees";
                }
                else
                {
                    result.Success = false;
                    result.Message = "No employee available!!!";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new EmployeeCustomException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Fetch information of an employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="EmployeeCustomException"></exception>
        [SwaggerOperation(Summary = "Get employee by id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
        {
            try
            {
                var result = new ApiResponse<GetEmployeeDto>();
                var employee = await _employeeRepository.GetEmployee(id);
                if(employee!= null)
                {
                    result.Data = employee;
                    result.Success = true;
                    result.Message = "Fetches employee information.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Something went wrong while fetching employee with id = "  + id.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new EmployeeCustomException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <param name="newEmployee"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [SwaggerOperation(Summary = "Add employee")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> PostAddEmployee([FromBody] AddEmployeeDto newEmployee)
        {
            try
            {
                ApiResponse<GetEmployeeDto> result = new ApiResponse<GetEmployeeDto>();
                if (_employeeRepository.ValidateEmployeeDetails(newEmployee))
                {
                    var employee = await _employeeRepository.AddEmployee(newEmployee);
                    if(employee!= null)
                    {
                        result.Data = employee;
                        result.Success = true;
                        result.Message = "Employee added succesfully.";
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "An employee can only have either spouse or domestic partner";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new EmployeeCustomException(ex.Message,ex);
            }
        }

        /// <summary>
        /// Updated an employee
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedEmployee"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [SwaggerOperation(Summary = "Update employee")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> UpdateEmployee(int id, UpdateEmployeeDto updatedEmployee)
        {
            try
            {
                var employee = await _employeeRepository.UpdateEmployee(id, updatedEmployee);
                var result = new ApiResponse<GetEmployeeDto>();
                if(employee != null)
                {
                    result.Data = employee;
                    result.Success = true;
                    result.Message = "Employee is updated";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Employee with Id = " + id.ToString() + " can not be updated";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new EmployeeCustomException(ex.Message,ex);
            }
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [SwaggerOperation(Summary = "Delete employee")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> DeleteEmployee(int id)
        {
            try
            {
                var deletedEmployee = await _employeeRepository.DeleteEmployee(id);
                var result = new ApiResponse<GetEmployeeDto>();
                if(deletedEmployee!= null)
                {
                    result.Data = deletedEmployee;
                    result.Success = true; 
                    result.Message = "Employee with Id = " + id + " is deleted successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Employee with Id = " + id.ToString() + " can not be deleted";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new EmployeeCustomException(ex.Message, ex);
            }
        }
    }
}
