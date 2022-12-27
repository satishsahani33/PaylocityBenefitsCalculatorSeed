using Api.DataContext;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Repository.Contracts;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Dapper;
namespace Api.Repository
{
    public class CommonRepository: ICommonRepository
    {
        private readonly DapperApplicationContext _applicationContext;

        public CommonRepository(DapperApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public async Task<GetEmployeeDto> GetEmployee(int id)
        {
            var query = "SELECT FirstName, LastName, Salary, DateOfBirth FROM employees  WITH (NOLOCK) WHERE Id = @Id";
            var dependentQuery = "SELECT FirstName, LastName, DateOfBirth, Relationship,EmployeeId FROM dependents  WITH (NOLOCK) WHERE EmployeeId = @Id";
            using (var connection = _applicationContext.CreateConnection())
            {
                var employee = await connection.QuerySingleOrDefaultAsync<GetEmployeeDto>(query, new { id });
                if (employee != null)
                {
                    var dependents = await connection.QueryAsync<GetDependentDto>(dependentQuery, new { id });
                    if (dependents != null)
                    {
                        employee.Dependents = dependents.ToList();
                    }
                }
                return employee;
            }
        }
    }
}
