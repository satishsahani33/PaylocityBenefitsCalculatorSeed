using Api.Dtos.Dependent;
using Api.Dtos.Employee;

namespace Api.Repository.Contracts
{
    public interface IEmployeeRepository
    {
        public Task<IEnumerable<GetEmployeeDto>> GetAllEmployees(int pageNumber = 1, int pageSize = 100, string orderBy = "Id", string sortBy="asc");
        public Task<GetEmployeeDto> GetEmployee(int id);
        public Task<GetEmployeeDto> AddEmployee(AddEmployeeDto employee);
        public Task<GetEmployeeDto> UpdateEmployee(int id, UpdateEmployeeDto employee);
        public Task<GetEmployeeDto> DeleteEmployee(int id);
        public bool ValidateEmployeeDetails(AddEmployeeDto employeeDto);
        
        public Task<IEnumerable<GetEmployeeDto>> AddMockDataEmployee(List<AddEmployeeDto>  employee);

    }
}
