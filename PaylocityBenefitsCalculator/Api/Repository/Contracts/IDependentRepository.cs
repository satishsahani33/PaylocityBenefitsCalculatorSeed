using Api.Dtos.Dependent;
using Api.Models;

namespace Api.Repository.Contracts
{
    public interface IDependentRepository
    {
        public Task<IEnumerable<GetDependentDto>> GetAllDependents(int pageNumber = 1, int pageSize = 100, string orderBy= "EmployeeId", string sortBy="asc");
        public Task<GetDependentDto> GetDependent(int id);
        public Task<IEnumerable<AddDependentWithEmployeeIdDto>> AddDependent(AddDependentWithEmployeeIdDto newDependent);
        public Task<GetDependentDto> UpdateDependent(int id, UpdateDependentDto updatedDependent);
        public Task<GetDependentDto> DeleteDependent(int id);
        public Task<IEnumerable<GetDependentDto>> GetEmployeeDependents(int id);
        public Task<bool> ValidateDependentRelationship(Relationship relationship, int employeeId, int dependentId = -1);
    }
}
