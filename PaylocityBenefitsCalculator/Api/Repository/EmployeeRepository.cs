using Api.DataContext;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.PayCheck;
using Api.Models;
using Api.Repository.Contracts;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Api.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperApplicationContext _applicationContext;

        private ICommonRepository _commonRepository;
        public EmployeeRepository(DapperApplicationContext applicationContext, ICommonRepository commonRepository)
        {
            _applicationContext = applicationContext;
            _commonRepository = commonRepository;
        }
        public async Task<IEnumerable<GetEmployeeDto>> GetAllEmployees(int pageNumber = 1, int pageSize = 100, string orderBy = "", string sortBy="")
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                var query = "SELECT Id, FirstName, LastName, Salary, DateOfBirth FROM employees WITH (NOLOCK) " +
                    "ORDER by Id OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";
                var parameters = new DynamicParameters();
                parameters.Add("PageNumber", pageNumber, DbType.Int64);
                parameters.Add("PageSize", pageSize, DbType.Int64);
                var employees = await connection.QueryAsync<GetEmployeeDto>(query, parameters);
                if (employees != null)
                {
                    var dependentQuery = "SELECT * FROM dependents WITH (NOLOCK)  WHERE EmployeeId = @Id";
                    foreach (var employee in employees.ToList())
                    {
                        var dependents = await connection.QueryAsync<GetDependentDto>(dependentQuery, new { employee.Id });
                        if (dependents != null)
                        {
                            employee.Dependents = dependents.ToList();
                        }
                    }
                    return employees.ToList();
                }
                return null;
            }
        }
        public async Task<GetEmployeeDto> GetEmployee(int id)
        {
            return await _commonRepository.GetEmployee(id);
        }
        public async Task<GetEmployeeDto> AddEmployee(AddEmployeeDto employee)
        {
            GetEmployeeDto result = new GetEmployeeDto();

            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var query = "insert into Employees(FirstName, LastName, Salary,DateOfBirth) " +
                                             "Values(@FirstName, @LastName, @Salary,@DateOfBirth)" +
                                             "SELECT CAST(SCOPE_IDENTITY() as int)";

                    var parameters = new DynamicParameters();
                    parameters.Add("FirstName", employee.FirstName, DbType.String);
                    parameters.Add("LastName", employee.LastName, DbType.String);
                    parameters.Add("Salary", employee.Salary, DbType.Decimal);
                    parameters.Add("DateOfBirth", employee.DateOfBirth, DbType.Date);

                    var newEmployeeId = await connection.QuerySingleAsync<int>(query, parameters, transaction: transaction);

                    result.Id = newEmployeeId;
                    result.FirstName = employee.FirstName;
                    result.LastName = employee.LastName;
                    result.Salary = employee.Salary;
                    result.DateOfBirth = employee.DateOfBirth;
                    if (employee.Dependents is not null && employee.Dependents.Count > 0)
                    {
                        foreach (AddDependentDto addDependentDto in employee.Dependents)
                        {
                            GetDependentDto getDependentDto = new GetDependentDto();
                            var dependentQuery = "insert into Dependents(FirstName, LastName, DateOfBirth, Relationship, EmployeeId) " +
                                                               "Values(@FirstName, @LastName,@DateOfBirth, @Relationship, @EmployeeId)" +
                                                                "SELECT CAST(SCOPE_IDENTITY() as int)"; ;
                            var dependentQueryParameters = new DynamicParameters();
                            dependentQueryParameters.Add("FirstName", addDependentDto.FirstName, DbType.String);
                            dependentQueryParameters.Add("LastName", addDependentDto.LastName, DbType.String);
                            dependentQueryParameters.Add("DateOfBirth", addDependentDto.DateOfBirth, DbType.Date);
                            dependentQueryParameters.Add("Relationship", addDependentDto.Relationship, DbType.String);
                            dependentQueryParameters.Add("EmployeeId", newEmployeeId, DbType.Int64);
                            var newDependentId = await connection.QuerySingleAsync<int>(dependentQuery, dependentQueryParameters, transaction: transaction);

                            getDependentDto.Id = newDependentId;
                            getDependentDto.FirstName = addDependentDto.FirstName;
                            getDependentDto.LastName = addDependentDto.LastName;
                            getDependentDto.DateOfBirth = addDependentDto.DateOfBirth;
                            getDependentDto.Relationship = addDependentDto.Relationship;

                            result.Dependents?.Add(getDependentDto);
                        }
                    }
                    transaction.Commit();
                }
            }
            return result;
        }
        public async Task<GetEmployeeDto> UpdateEmployee(int id, UpdateEmployeeDto employee)
        {
           
            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var query = "Update Employees WITH (ROWLOCK) set FirstName = @FirstName, LastName = @LastName, Salary = @Salary " +
                        " OUTPUT INSERTED.* WHERE Id = @id";

                    var parameters = new DynamicParameters();
                    parameters.Add("Id", id, DbType.Int64);
                    parameters.Add("FirstName", employee.FirstName, DbType.String);
                    parameters.Add("LastName", employee.LastName, DbType.String);
                    parameters.Add("Salary", employee.Salary, DbType.Decimal);

                    var updatedEmployee = await connection.QuerySingleOrDefaultAsync<GetEmployeeDto>(query, parameters,transaction);
                    transaction.Commit();
                    return updatedEmployee;
                }
            }
        }
        public async Task<GetEmployeeDto> DeleteEmployee(int id)
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var deleteQueryPaychecksTable = "DELETE FROM PayChecks WITH (ROWLOCK) WHERE EmployeeId = @id";
                    var deleteQueryDependentsTable = "DELETE FROM Dependents WITH (ROWLOCK) WHERE EmployeeId = @id";
                    var deleteQueryEmployeesTable = "DELETE FROM Employees WITH (ROWLOCK) OUTPUT DELETED.*  WHERE Id = @id";
                    var parameters = new DynamicParameters();
                    parameters.Add("Id", id, DbType.Int64);

                    await connection.ExecuteAsync(deleteQueryPaychecksTable, parameters, transaction);
                    await connection.ExecuteAsync(deleteQueryDependentsTable, parameters, transaction);
                    var deletedEmployee = await connection.QuerySingleOrDefaultAsync<GetEmployeeDto>(deleteQueryEmployeesTable, parameters, transaction);
                    transaction.Commit();
                    return deletedEmployee;
                }
            }
        }

        public bool ValidateEmployeeDetails(AddEmployeeDto employeeDto)
        {
            int spouseCount = (from e in employeeDto.Dependents
                                    where (e.Relationship == Relationship.Spouse || e.Relationship == Relationship.DomesticPartner)
                                    select e).Count();
            return spouseCount <= 1 ? true:false;
        }
        public async Task<IEnumerable<GetEmployeeDto>> AddMockDataEmployee(List<AddEmployeeDto> employees)
        {
            List<GetEmployeeDto> result = new List<GetEmployeeDto>();
            foreach(AddEmployeeDto employee in employees)
            {
                GetEmployeeDto createdEmployee = await AddEmployee(employee);
                result.Add(createdEmployee);
            }
            return result;
        }
        public async Task<bool> DatabaseCleanUp()
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var deleteQueryPaychecksTable = "DELETE * FROM PayChecks WITH (ROWLOCK)";
                    var deleteQueryDependentsTable = "DELETE * FROM Dependents WITH (ROWLOCK)";
                    var deleteQueryEmployeesTable = "DELETE * FROM Employees WITH (ROWLOCK)";
                    var parameters = new DynamicParameters();
                    await connection.ExecuteAsync(deleteQueryPaychecksTable, parameters, transaction);
                    await connection.ExecuteAsync(deleteQueryDependentsTable, parameters, transaction);
                    await connection.ExecuteAsync(deleteQueryEmployeesTable, parameters, transaction);
                    transaction.Commit();
                    return true;
                }
            }
        }
    }
}
