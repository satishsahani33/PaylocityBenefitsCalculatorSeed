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
    public class DependentsController : ControllerBase
    {
        private readonly IDependentRepository _dependentRepository;
        public DependentsController(IDependentRepository dependentRepository)
        {
            _dependentRepository = dependentRepository;
        }
        
        /// <summary>
        /// Fetch list of all dependents of all employees
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        /// <exception cref="DependentCustomException"></exception>
        [SwaggerOperation(Summary = "Get all dependents")]
        [HttpGet("{pageNumber}/{pageSize}/{orderBy}/{sortBy}")]
        public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll(int pageNumber = 1, int pageSize = 100, string orderBy = "Id", string sortBy = "asc")
        {
            var result = new ApiResponse<List<GetDependentDto>>();
            try
            {
                var dependents = await _dependentRepository.GetAllDependents(pageNumber, pageSize, orderBy, sortBy);
                if (dependents != null)
                {
                    result.Data = dependents.ToList(); 
                    result.Success = true;
                    result.Message = "All depndents fetched successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "No dependents available!!!";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DependentCustomException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Fetch information of dependent
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="DependentCustomException"></exception>
        [SwaggerOperation(Summary = "Get dependent by id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
        {
            var result = new ApiResponse<GetDependentDto>();
            try
            {
                var dependent = await _dependentRepository.GetDependent(id);
                result.Data = dependent;
                result.Success = true;
                
                if (dependent != null)
                {
                    result.Message = "Found Dependent With Id = " + id.ToString();
                }
                else
                {
                    result.Success = false;
                    result.Message = "Dependent With Id = " + id.ToString() + " Not Found";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DependentCustomException(ex.Message + " With Dependent Id = " + id.ToString(), ex);
            }
        }
        /// <summary>
        /// Fetch list of all dependents of an employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="DependentCustomException"></exception>
        [SwaggerOperation(Summary = "Get all dependents of an employee")]
        [HttpGet]
        [Route("Employee/{id}")]
        public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> EmployeeDependent(int id)
        {
            var result = new ApiResponse<List<GetDependentDto>>();
            try
            {
                var dependents = await _dependentRepository.GetEmployeeDependents(id);
                result.Success = true;
                if (dependents != null)
                {
                    result.Data = dependents.ToList();
                    result.Message = "Fetched list of dependents of an employee.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "No dependents available for employee id = " + id.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DependentCustomException(ex.Message + " With Dependent Id = " + id.ToString(), ex);
            }
        }
        /// <summary>
        /// Create a new dependent of an employee
        /// </summary>
        /// <param name="newDependent"></param>
        /// <returns></returns>
        /// <exception cref="DependentCustomException"></exception>
        [SwaggerOperation(Summary = "Add dependent")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<List<AddDependentWithEmployeeIdDto>>>> AddDependent(AddDependentWithEmployeeIdDto newDependent)
        {
            try
            {
                var result = new ApiResponse<List<AddDependentWithEmployeeIdDto>>();

                if (!(newDependent.Relationship == Relationship.Spouse || newDependent.Relationship == Relationship.DomesticPartner)
                    || await _dependentRepository.ValidateDependentRelationship(newDependent.Relationship, newDependent.EmployeeId))
                {
                    var dependents = await _dependentRepository.AddDependent(newDependent);
                    if (dependents != null)
                    {
                        result.Data = dependents.ToList();
                        result.Success = true;
                        result.Message = "Dependent added successfully";
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "An employee can have only either spouse or domestic partner";
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "An employee can have only either spouse or domestic partner";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DependentCustomException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Update a dependent
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedDependent"></param>
        /// <returns></returns>
        /// <exception cref="DependentCustomException"></exception>
        [SwaggerOperation(Summary = "Update dependent")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GetDependentDto>>> UpdateDependent(int id, UpdateDependentDto updatedDependent)
        {
            try
            {
                var result = new ApiResponse<GetDependentDto>();
                if (await _dependentRepository.ValidateDependentRelationship(updatedDependent.Relationship, -1, id))
                {
                    var dependent = await _dependentRepository.UpdateDependent(id, updatedDependent);
                    result.Success = true;
                    if (dependent != null)
                    {
                        result.Data = dependent;
                        result.Message = "Dependent updated successfully";
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "No dependents available for Id = " + id.ToString();
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "An employee can have only either spouse or domestic partner";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DependentCustomException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Delete a dependent
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="DependentCustomException"></exception>
        [SwaggerOperation(Summary = "Delete dependent")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<GetDependentDto>>> DeleteDependent(int id)
        {
            try
            {
                var result = new ApiResponse<GetDependentDto>();
                var dependent = await _dependentRepository.DeleteDependent(id);
                if (dependent != null)
                {
                    result.Data = dependent;
                    result.Success= true;
                    result.Message = "Dependent with Id = " + id + " is deleted successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Dependent with Id = " + id + " is not available.";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DependentCustomException(ex.Message, ex);
            }
        }
    }
}
