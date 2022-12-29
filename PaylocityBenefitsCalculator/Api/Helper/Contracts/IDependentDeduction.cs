using Api.Dtos.Employee;
using Api.Models;

namespace Api.Helper.Contracts
{
    public interface IDependentDeduction
    {
        public decimal GetDeductionAmount(GetEmployeeDto employee);
    }
}
