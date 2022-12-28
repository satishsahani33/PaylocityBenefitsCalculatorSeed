using Api.DataContext;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repository.Contracts;
using Dapper;
using System.Data;
using System.Reflection.Metadata;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Api.Repository
{
    public class DependentRepository : IDependentRepository
    {
        private readonly DapperApplicationContext _applicationContext;

        private ICommonRepository _commonRepository;
        public DependentRepository(DapperApplicationContext applicationContext, ICommonRepository commonRepository)
        {
            _applicationContext = applicationContext;
            _commonRepository = commonRepository;
        }
        public async Task<IEnumerable<GetDependentDto>> GetAllDependents(int pageNumber = 1, int pageSize = 100, string orderBy = "Id", string sortBy = "asc")
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                var query = "SELECT Id, FirstName, LastName, DateOfBirth, Relationship, EmployeeId FROM dependents WITH (NOLOCK) " +
                        "ORDER by EmployeeId, FirstName OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";
                var parameters = new DynamicParameters();
                parameters.Add("PageNumber", pageNumber, DbType.Int64);
                parameters.Add("PageSize", pageSize, DbType.Int64);

                var dependents = await connection.QueryAsync<GetDependentDto>(query, parameters);
                return dependents;
            }
        }
        public async Task<GetDependentDto> GetDependent(int id)
        {
            var query = "SELECT Id, FirstName, LastName, DateOfBirth, Relationship FROM dependents  WITH (NOLOCK)" +
                "Where Id = @Id";
            using (var connection = _applicationContext.CreateConnection())
            {
                var dependent = await connection.QuerySingleOrDefaultAsync<GetDependentDto>(query, new { id });
                return dependent;
            }
        }
        public async Task<IEnumerable<AddDependentWithEmployeeIdDto>> AddDependent(AddDependentWithEmployeeIdDto newDependent)
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    GetDependentDto getDependentDto = new GetDependentDto();
                    var query = "insert into Dependents(FirstName, LastName, DateOfBirth, Relationship, EmployeeId) OUTPUT INSERTED.* " +
                                                       "Values(@FirstName, @LastName,@DateOfBirth, @Relationship, @EmployeeId)";
                    var parameters = new DynamicParameters();
                    parameters.Add("FirstName", newDependent.FirstName, DbType.String);
                    parameters.Add("LastName", newDependent.LastName, DbType.String);
                    parameters.Add("DateOfBirth", newDependent.DateOfBirth, DbType.Date);
                    parameters.Add("Relationship", newDependent.Relationship, DbType.String);
                    parameters.Add("EmployeeId", newDependent.EmployeeId, DbType.Int64);

                    var dependent = await connection.QueryAsync<AddDependentWithEmployeeIdDto>(query, parameters, transaction);

                    transaction.Commit();
                    return dependent;
                }
            }

        }
        public async Task<GetDependentDto> UpdateDependent(int id, UpdateDependentDto updatedDependent)
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var query = "Update Dependents  WITH (ROWLOCK) set FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth " +
                            ", Relationship=@Relationship OUTPUT INSERTED.* WHERE Id = @Id ";

                    var parameters = new DynamicParameters();
                    parameters.Add("Id", id, DbType.Int64);
                    parameters.Add("FirstName", updatedDependent.FirstName, DbType.String);
                    parameters.Add("LastName", updatedDependent.LastName, DbType.String);
                    parameters.Add("DateOfBirth", updatedDependent.DateOfBirth, DbType.Date);
                    parameters.Add("Relationship", updatedDependent.Relationship, DbType.String);

                    var updatedEmployeeInfo = await connection.QuerySingleOrDefaultAsync<GetDependentDto>(query, parameters, transaction);
                    if (updatedEmployeeInfo != null)
                    {
                        var dependentInfo = await GetDependentInfo(id);
                        updatedEmployeeInfo.Id = dependentInfo.EmployeeId;
                    }
                    transaction.Commit();
                    return updatedEmployeeInfo;
                }
            }
        }

        public async Task<IEnumerable<GetDependentDto>> GetEmployeeDependents(int id)
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                var query = "select Id, FirstName, LastName, DateOfBirth, Relationship from Dependents WITH (NOLOCK) where EmployeeId = @id";
                var dependents = await connection.QueryAsync<GetDependentDto>(query, new { id });
                return dependents;
            }
        }
        public async Task<GetDependentDto> DeleteDependent(int id)      
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var query = "DELETE from Dependents WITH (ROWLOCK) OUTPUT DELETED.* WHERE Id = @Id";
                    var dependent = await connection.QuerySingleOrDefaultAsync<GetDependentDto>(query, new { id }, transaction);
                    transaction.Commit();
                    return dependent;
                }
            }
        }
        public async Task<Dependent> GetDependentInfo(int id)
        {
            var query = "SELECT Id, FirstName, LastName, DateOfBirth, Relationship, EmployeeId FROM dependents  WITH (NOLOCK)" +
                "Where Id = @Id";
            using (var connection = _applicationContext.CreateConnection())
            {
                var dependent = await connection.QuerySingleOrDefaultAsync<Dependent>(query, new { id });
                return dependent;
            }
        }
        public async Task<bool> ValidateDependentRelationship(Relationship relationship, int employeeId, int dependentId = -1)
        {
            //Here, instead of checking for hard coded value for relationship
            //we must create relationship table and use that table value
            bool result = true;

            var query = "";
            var parameters = new DynamicParameters();
            using (var connection = _applicationContext.CreateConnection())
            {
                //Case of edit dependent
                if (dependentId != -1)
                {
                    query = "select Id, Relationship from Dependents WITH (NOLOCK) " +
                        "where EmployeeId = (select EmployeeId from Dependents where Id = @DependentId)" +
                        " AND (Relationship = @RelationshipSpouse OR Relationship = @RelationshipDomesticPartner)";
                    parameters.Add("DependentId", dependentId, DbType.Int64);
                    parameters.Add("RelationshipSpouse", Relationship.Spouse, DbType.Int64);
                    parameters.Add("RelationshipDomesticPartner", Relationship.DomesticPartner, DbType.Int64);

                    var dependents = await connection.QueryAsync<GetDependentDto>(query, parameters);

                    //Check count of dependents with either spouse or domestic partner
                    if (dependents != null && dependents.ToList().Count > 0)
                    {
                        int isSpouseOrPartnerCount = dependents.Where(item => item.Id == dependentId).ToList().Count;
                        //If employee have spouse/domestic parnter
                        //Allow convertion from spouse to domestic partner or from domestic partner to spouse
                        if (isSpouseOrPartnerCount > 0)
                        {
                            return true;
                        }
                        // If employee already have spouse/domestic parnter, do not allow to to add spouse/domestic partner
                        else if (relationship == Relationship.Spouse || relationship == Relationship.DomesticPartner)
                        {
                            return false;
                        }
                        //An employee can have unlimited number of child
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    query = "select Id, FirstName, LastName, DateOfBirth, Relationship from Dependents where " +
                    "Relationship = @RelationshipSpouse OR Relationship = @RelationshipDomesticPartner and EmployeeId = @employeeId";

                    parameters.Add("EmployeeId", employeeId, DbType.Int64);
                    parameters.Add("RelationshipSpouse", Relationship.Spouse, DbType.Int64);
                    parameters.Add("RelationshipDomesticPartner", Relationship.DomesticPartner, DbType.Int64);

                    var dependents = await connection.QueryAsync<GetDependentDto>(query, parameters);
                    if (dependents != null && dependents.ToList().Count > 0)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
    }
}

