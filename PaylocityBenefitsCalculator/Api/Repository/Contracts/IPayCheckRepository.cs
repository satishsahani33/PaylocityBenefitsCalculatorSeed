using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.PayCheck;
using Api.Models;
using System;

namespace Api.Repository.Contracts
{
    public interface IPayCheckRepository
    {
        public Task<IEnumerable<GetPayCheckDto>> GetAllPayChecks(int pageNumber = 1, int pageSize = 100, string orderBy="EmployeeId", string sortBy="asc");
        public Task<GetPayCheckDto> GetPayCheck(int id);
        public Task<GetPayCheckDto> AddPayCheck(int employeeId,string year, string month);
        public Task<GetPayCheckDto> UpdatePayCheck(int id, int employeeId, string year, string month);
        public Task<GetPayCheckDto> DeletePayCheck(int id);
        public Task<IEnumerable<GetPayCheckDto>> GetEmployeePayChecks(int id, string year="", string month = "" );

        public GetPayCheckDto CalculatePayCheck(GetEmployeeDto employee, string year="", string month="");
        public Task<GetPayCheckDto> ViewPayCheck(int employeeId, string year = "", string month = "");
    }
}
