using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Repository.Contracts
{
    public interface ICommonRepository
    {
        public Task<GetEmployeeDto> GetEmployee(int id);
    }
}
