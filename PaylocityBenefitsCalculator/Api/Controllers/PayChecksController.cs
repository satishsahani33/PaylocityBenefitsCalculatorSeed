using Api.CustomException;
using Api.Dtos.Employee;
using Api.Dtos.PayCheck;
using Api.Models;
using Api.Repository;
using Api.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PayChecksController : ControllerBase
    {
        private readonly IPayCheckRepository _payCheckRepository;
        public PayChecksController(IPayCheckRepository payCheckRepository)
        {
            _payCheckRepository = payCheckRepository;
        }
        /// <summary>
        /// Get all paychecks of all employee
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        /// <exception cref="PayCheckCustomException"></exception>
        [SwaggerOperation(Summary = "Get all paycheck")]
        [HttpGet("{pageNumber}/{pageSize}/{orderBy}/{sortBy}")]
        public async Task<ActionResult<ApiResponse<List<GetPayCheckDto>>>> GetAllPayCheck(int pageNumber = 1, int pageSize = 100, string orderBy = "Id", string sortBy = "asc")
        {
            try
            {
                //task: use a more realistic production approach
                var result = new ApiResponse<List<GetPayCheckDto>>();
                var payChecks = await _payCheckRepository.GetAllPayChecks(pageNumber, pageSize, orderBy, sortBy);
                if (payChecks != null)
                {
                    result.Data = payChecks.ToList();
                    result.Success = true;
                    result.Message = "All pay checks fetched successfully";
                }
                else
                {
                    result.Success = false;
                    result.Message = "No Pay Checks available";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new PayCheckCustomException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Get all pay checks of an employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="PayCheckCustomException"></exception>
        [SwaggerOperation(Summary = "Get all pay checks of an employee")]
        [HttpGet]
        [Route("Employee/{id}")]
        public async Task<ActionResult<ApiResponse<List<GetPayCheckDto>>>> EmployeePayChecks(int id)
        {
            try
            {
                var result = new ApiResponse<List<GetPayCheckDto>>();
                var employees = await _payCheckRepository.GetEmployeePayChecks(id);
                if (employees != null)
                {
                    result.Data = employees.ToList();
                    result.Success = true;
                    result.Message = "Fethces all Pay Checks of employee with id = " + id.ToString();
                }
                else
                {
                    result.Success = false;
                    result.Message = "No Pay Checks available for employee with id = " + id.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new PayCheckCustomException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Fetch details of a pay check
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [SwaggerOperation(Summary = "Get paycheck by id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetPayCheckDto>>> GetPayCheck(int id)
        {
            try
            {
                var result = new ApiResponse<GetPayCheckDto>();
                var employee = await _payCheckRepository.GetPayCheck(id);
                if (employee != null)
                {
                    result.Data = employee;
                    result.Success = true;
                    result.Message = "Pay check details fetched successfully";
                }
                else
                {
                    result.Success = false;
                    result.Message = "No Pay Checks available for id = " + id.ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new PayCheckCustomException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Calculate pay check of an employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [SwaggerOperation(Summary = "Calculate paycheck by employee id")]
        [HttpPost("{employeeId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<GetPayCheckDto>>> AddPayCheck(int employeeId, string year, string month)
        {
            try
            {
                var employee = await _payCheckRepository.AddPayCheck(employeeId, year, month);
                var result = new ApiResponse<GetPayCheckDto>();
                if (employee != null)
                {
                    result.Data = employee;
                    result.Success = true;
                    result.Message = "Pay check added successfully"; ;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Pay Check can not be caluclated for employee id = " + employeeId.ToString() +
                        " Year = " + year + " and Month = " + month;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new PayCheckCustomException(ex.Message, ex);
            }

        }
        /// <summary>
        /// View Pay check of employee before calculating pay check
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        /// <exception cref="PayCheckCustomException"></exception>
        [SwaggerOperation(Summary = "Calculate paycheck by employee id")]
        [HttpGet]
        [Route("Employee/{employeeId}/{year}/{month}")]
        public async Task<ActionResult<ApiResponse<GetPayCheckDto>>> ViewPayCheck(int employeeId, string year, string month)
        {
            try
            {
                var employeePaycheck = await _payCheckRepository.ViewPayCheck(employeeId, year, month);
                var result = new ApiResponse<GetPayCheckDto>();
                if (employeePaycheck != null)
                {
                    result.Data = employeePaycheck;
                    result.Success = true;
                    if(employeePaycheck.NetAmount < 0)
                    {
                        result.Success = false;
                        result.Message = "Employee salary is less than the net pay check amount.\nPay check will not be calculated for this employee."; ;
                    }
                    else
                    {
                        result.Message = "Pay check fetched successfully"; ;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "Pay Check can not be fetched for employee id = " + employeeId.ToString() +
                        " Year = " + year + " and Month = " + month;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new PayCheckCustomException(ex.Message, ex);
            }

        }
        /// <summary>
        /// Update pay check of an employee
        /// </summary>
        /// <param name="payCheckId"></param>
        /// <param name="employeeId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [SwaggerOperation(Summary = "Update paycheck")]
        [HttpPut("{payCheckId}/{employeeId}")]
        public async Task<ActionResult<ApiResponse<GetPayCheckDto>>> UpdatePayCheck(int payCheckId, int employeeId, string year, string month)
        {
            try
            {
                var employee = await _payCheckRepository.UpdatePayCheck(payCheckId, employeeId, year, month);
                var result = new ApiResponse<GetPayCheckDto>();
                if (employee != null)
                {
                    result.Data = employee;
                    result.Success = true;
                    result.Message = "Pay check updated successfully";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Pay Check can not be updated for employee id = " + employeeId.ToString() +
                        " Year = " + year + " and Month = " + month;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new PayCheckCustomException(ex.Message, ex);
            }
        }

        [SwaggerOperation(Summary = "Delete paycheck")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<GetPayCheckDto>>> DeletePayCheck(int id)
        {
            try
            {
                var deletedPayCheck = await _payCheckRepository.DeletePayCheck(id);
                var result = new ApiResponse<GetPayCheckDto>();
                if (deletedPayCheck != null)
                {
                    result.Data = deletedPayCheck;
                    result.Success = true;
                    result.Message = "Pay Check with Id = " + id + " is deleted successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Pay Check with Id = " + id + " is not available.";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new PayCheckCustomException(ex.Message, ex);
            }
        }
    }
}
