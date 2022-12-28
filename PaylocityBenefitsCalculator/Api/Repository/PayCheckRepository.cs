using Api.DataContext;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.PayCheck;
using Api.Helper;
using Api.Models;
using Api.Repository.Contracts;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace Api.Repository
{
    public class PayCheckRepository : IPayCheckRepository
    {
        private readonly DapperApplicationContext _applicationContext;

        private ICommonRepository _commonRepository;
        public PayCheckRepository(DapperApplicationContext applicationContext, ICommonRepository commonRepository)
        {
            _applicationContext = applicationContext;
            _commonRepository = commonRepository;
        }
        public async Task<IEnumerable<GetPayCheckDto>> GetAllPayChecks(int pageNumber = 1, int pageSize = 100, string orderBy = "", string sortBy = "")
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {


                    var query = "SELECT Id, EmployeeId, EmployeeFirstName, EmployeeLastName, YearlySalary, GrossAmount," +
                        "EmployeeDeduction, DependentDeduction, AdditionalDeductionBasedOnSalary, " +
                        "AdditionalDeductionBasedOnDependentAge, NetAmount,[Year],[Month] " +
                        "FROM paychecks WITH (NOLOCK)" +
                        "ORDER BY EmployeeId, [YEAR], [MONTH] OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";
                    var parameters = new DynamicParameters();
                    parameters.Add("PageNumber", pageNumber, DbType.Int64);
                    parameters.Add("PageSize", pageSize, DbType.Int64);

                    var payChecks = await connection.QueryAsync<GetPayCheckDto>(query, parameters, transaction);
                    transaction.Commit();
                    return payChecks;
                }
            }
        }
        public async Task<GetPayCheckDto> GetPayCheck(int id)
        {
            var query = "SELECT Id, EmployeeId, EmployeeFirstName, EmployeeLastName, YearlySalary, GrossAmount, " +
                "EmployeeDeduction, DependentDeduction, AdditionalDeductionBasedOnSalary, " +
                "AdditionalDeductionBasedOnDependentAge, NetAmount,[Year],[Month] FROM paychecks WITH (NOLOCK) WHERE Id = @id";
            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var employeePayCheck = await connection.QuerySingleOrDefaultAsync<GetPayCheckDto>(query, new { id },transaction);
                    transaction.Commit();
                    return employeePayCheck;
                }
            }
        }
        public async Task<GetPayCheckDto> AddPayCheck(int employeeId, string year, string month)
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    IEnumerable<GetPayCheckDto> checkIfPayCheckExist = await GetEmployeePayChecks(employeeId, year, month);
                    if (checkIfPayCheckExist != null && checkIfPayCheckExist.ToList().Count > 0)
                    {
                        throw new Exception("Pay Check Already Calculate for EmployeeId = " + employeeId + " Year = " + year + " Month = " + month);
                        //Question to TPO: What will happen if user try to calculate pay check which was already calculated for specific year and month
                        //var deleteQueryPaychecksTable = "DELETE FROM PayChecks WITH (ROWLOCK) WHERE EmployeeId = @id";
                        //var deleteQueryPaychecksParamter = new DynamicParameters();
                        //deleteQueryPaychecksParamter.Add("Id", employeeId, DbType.Int64);
                        //await connection.ExecuteAsync(deleteQueryPaychecksTable, deleteQueryPaychecksParamter, transaction);
                    }
                    GetEmployeeDto employee = await _commonRepository.GetEmployee(employeeId);
                    if (employee == null)
                    {
                        return null;
                    }
                    GetPayCheckDto employeePayCheck = CalculatePayCheck(employee, year, month);
                    if (employeePayCheck.NetAmount < 0)
                    {
                        throw new Exception("Error while calculating pay check!!! Total deduction is greaer than employee salary for employee id = " + employeeId);
                    }

                    var query = "insert into paychecks(EmployeeId, EmployeeFirstName, EmployeeLastName, YearlySalary," +
                                                    "GrossAmount,EmployeeDeduction,DependentDeduction,AdditionalDeductionBasedOnSalary," +
                                                    "AdditionalDeductionBasedOnDependentAge,NetAmount, Year, Month)" +
                                             "values(@EmployeeId, @EmployeeFirstName, @EmployeeLastName, @YearlySalary," +
                                                    "@GrossAmount,@EmployeeDeduction,@DependentDeduction,@AdditionalDeductionBasedOnSalary," +
                                                    "@AdditionalDeductionBasedOnDependentAge,@NetAmount, @Year, @Month)" +
                                                    "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var parameters = new DynamicParameters();
                    parameters.Add("EmployeeId", employeeId, DbType.Int64);
                    parameters.Add("EmployeeFirstName", employee.FirstName, DbType.String);
                    parameters.Add("EmployeeLastName", employee.LastName, DbType.String);
                    parameters.Add("YearlySalary", employee.Salary, DbType.Decimal);
                    parameters.Add("GrossAmount", employeePayCheck.GrossAmount, DbType.Decimal);
                    parameters.Add("EmployeeDeduction", employeePayCheck.EmployeeDeduction, DbType.Decimal);
                    parameters.Add("DependentDeduction", employeePayCheck.DependentDeduction, DbType.Decimal);
                    parameters.Add("AdditionalDeductionBasedOnSalary", employeePayCheck.AdditionalDeductionBasedOnSalary, DbType.Decimal);
                    parameters.Add("AdditionalDeductionBasedOnDependentAge", employeePayCheck.AdditionalDeductionBasedOnDependentAge, DbType.Decimal);
                    parameters.Add("NetAmount", employeePayCheck.NetAmount, DbType.Decimal);
                    parameters.Add("Year", employeePayCheck.Year, DbType.String);
                    parameters.Add("Month", employeePayCheck.Month, DbType.String);

                    var newPayCheckId = await connection.QuerySingleAsync<int>(query, parameters, transaction);
                    transaction.Commit();

                    employeePayCheck.Id = newPayCheckId;
                    return employeePayCheck;
                }
            }
        }

        public async Task<GetPayCheckDto> UpdatePayCheck(int payCheckId, int employeeId, string year, string month)
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    IEnumerable<GetPayCheckDto> checkIfPayCheckExist = await GetEmployeePayChecks(employeeId, year, month);
                    if (checkIfPayCheckExist != null && checkIfPayCheckExist.ToList().Count > 0)
                    {
                        //In case of update, delete the previous caluclated pay check and re-calculate
                        var deleteQueryPaychecksTable = "DELETE FROM PayChecks WITH (ROWLOCK) WHERE EmployeeId = @id";
                        var deleteQueryPaychecksParamter = new DynamicParameters();
                        deleteQueryPaychecksParamter.Add("Id", employeeId, DbType.Int64);
                        await connection.ExecuteAsync(deleteQueryPaychecksTable, deleteQueryPaychecksParamter, transaction);
                    }
                    GetEmployeeDto employee = await _commonRepository.GetEmployee(employeeId);
                    if (employee == null)
                    {
                        return null;
                    }
                    GetPayCheckDto employeePayCheck = CalculatePayCheck(employee, year, month);
                    if (employeePayCheck.NetAmount < 0)
                    {
                        throw new Exception("Error while calculating pay check!!! Total deduction is greaer than employee salary for employee id = " + employeeId);
                    }

                    var query = "insert into paychecks(EmployeeId, EmployeeFirstName, EmployeeLastName, YearlySalary," +
                                                    "GrossAmount,EmployeeDeduction,DependentDeduction,AdditionalDeductionBasedOnSalary," +
                                                    "AdditionalDeductionBasedOnDependentAge,NetAmount, Year, Month)" +
                                             "values(@EmployeeId, @EmployeeFirstName, @EmployeeLastName, @YearlySalary," +
                                                    "@GrossAmount,@EmployeeDeduction,@DependentDeduction,@AdditionalDeductionBasedOnSalary," +
                                                    "@AdditionalDeductionBasedOnDependentAge,@NetAmount, @Year, @Month)" +
                                                    "SELECT CAST(SCOPE_IDENTITY() as int)";
                    var parameters = new DynamicParameters();
                    parameters.Add("EmployeeId", employeeId, DbType.Int64);
                    parameters.Add("EmployeeFirstName", employee.FirstName, DbType.String);
                    parameters.Add("EmployeeLastName", employee.LastName, DbType.String);
                    parameters.Add("YearlySalary", employee.Salary, DbType.Decimal);
                    parameters.Add("GrossAmount", employeePayCheck.GrossAmount, DbType.Decimal);
                    parameters.Add("EmployeeDeduction", employeePayCheck.EmployeeDeduction, DbType.Decimal);
                    parameters.Add("DependentDeduction", employeePayCheck.DependentDeduction, DbType.Decimal);
                    parameters.Add("AdditionalDeductionBasedOnSalary", employeePayCheck.AdditionalDeductionBasedOnSalary, DbType.Decimal);
                    parameters.Add("AdditionalDeductionBasedOnDependentAge", employeePayCheck.AdditionalDeductionBasedOnDependentAge, DbType.Decimal);
                    parameters.Add("NetAmount", employeePayCheck.NetAmount, DbType.Decimal);
                    parameters.Add("Year", employeePayCheck.Year, DbType.String);
                    parameters.Add("Month", employeePayCheck.Month, DbType.String);

                    var newPayCheckId = await connection.QuerySingleAsync<int>(query, parameters, transaction);
                    transaction.Commit();

                    employeePayCheck.Id = newPayCheckId;
                    return employeePayCheck;
                }
            }
        }
        public async Task<GetPayCheckDto> DeletePayCheck(int id)
        {
            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var query = "DELETE from PayChecks WITH (ROWLOCK) OUTPUT DELETED.* WHERE Id = @Id";
                    var dependent = await connection.QuerySingleOrDefaultAsync<GetPayCheckDto>(query, new { id }, transaction);
                    transaction.Commit();
                    return dependent;
                }
            }
        }

        public async Task<IEnumerable<GetPayCheckDto>> GetEmployeePayChecks(int id, string year = "", string month = "")
        {
            string subQuery = "";

            using (var connection = _applicationContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var parameters = new DynamicParameters();
                    if (year != "" && month != "")
                    {
                        subQuery = @" AND [YEAR] = @Year and [Month] = @Month";
                        parameters.Add("Year", year, DbType.String);
                        parameters.Add("Month", month, DbType.String);
                    }
                    parameters.Add("Id", id, DbType.Int64);

                    var query = "SELECT Id, EmployeeId, EmployeeFirstName, EmployeeLastName, YearlySalary, GrossAmount, " +
                        "EmployeeDeduction, DependentDeduction, AdditionalDeductionBasedOnSalary, " +
                        "AdditionalDeductionBasedOnDependentAge, NetAmount," +
                        "[Year],[Month] " +
                        "FROM paychecks WITH (NOLOCK) WHERE EmployeeId = @id" + subQuery + " ORDER BY EmployeeId, [YEAR], [MONTH]";

                    var payChecks = await connection.QueryAsync<GetPayCheckDto>(query, parameters,transaction);
                    transaction.Commit();
                    return payChecks;
                }
            }
        }
        public GetPayCheckDto CalculatePayCheck(GetEmployeeDto employee, string year = "", string month = "")
        {
            GetPayCheckDto employeePayCheck = PayCheckHelper.CalculatePayCheck(employee, year, month);
            return employeePayCheck;
        }
        public async Task<GetPayCheckDto> ViewPayCheck(int employeeId, string year = "", string month = "")
        {
            GetPayCheckDto payCheck = new GetPayCheckDto();
            GetEmployeeDto employee = await _commonRepository.GetEmployee(employeeId);
            payCheck = CalculatePayCheck(employee, year, month);
            return payCheck;
        }
    }
}
