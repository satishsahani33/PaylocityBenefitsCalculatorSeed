using Api.Dtos.Employee;
using Api.Dtos.PayCheck;
using Api.Models;

namespace Api.Helper
{
    public static class PayCheckHelper
    {
        public static GetPayCheckDto CalculatePayCheck(GetEmployeeDto employee, string year = "", string month = "")
        {
            GetPayCheckDto employeePayCheck = new GetPayCheckDto();
            Deduction deduction = GetAllDeduction(employee);
            employeePayCheck.EmployeeId = employee.Id;
            employeePayCheck.EmployeeFirstName = employee.FirstName;
            employeePayCheck.EmployeeLastName = employee.LastName;
            employeePayCheck.YearlySalary = employee.Salary;
            employeePayCheck.EmployeeDeduction = deduction.EmployeeDeduction;
            employeePayCheck.DependentDeduction = deduction.DependentDeduction;
            employeePayCheck.GrossAmount = deduction.GrossSalaryPerPayCheck;
            employeePayCheck.AdditionalDeductionBasedOnSalary = deduction.AdditionalDeductionBasedOnSalary;
            employeePayCheck.AdditionalDeductionBasedOnDependentAge = deduction.AdditionalDeductionBasedOnDependentAge;
            employeePayCheck.NetAmount = deduction.NetSalaryPerPayCheck;
            employeePayCheck.Year = year;
            employeePayCheck.Month = month;

            return employeePayCheck;
        }
        public static Deduction GetAllDeduction(GetEmployeeDto employee)
        {
            Deduction deduction = new Deduction();
            decimal netDeduction = 0.00m;
            deduction.GrossSalaryPerPayCheck = DeductionHelper.CalculateDeduction("salary", employee.Salary);
            
            deduction.EmployeeDeduction = DeductionHelper.GetEmployeeDeduction(employee);
            netDeduction = decimal.Add(netDeduction, deduction.EmployeeDeduction);

            deduction.NonRelationDependentDeduction = DeductionHelper.GetNonRelationDependentDeduction(employee);
            netDeduction = decimal.Add(netDeduction, deduction.NonRelationDependentDeduction);

            deduction.SpouseDependentDeduction = DeductionHelper.GetSpouseDependentDeduction(employee);
            netDeduction = decimal.Add(netDeduction, deduction.SpouseDependentDeduction);

            deduction.DomesticPartnerDependentDeduction = DeductionHelper.GetDomesticPartnerDependentDeduction(employee);
            netDeduction = decimal.Add(netDeduction, deduction.DomesticPartnerDependentDeduction);

            deduction.ChildDependentDeduction = DeductionHelper.GetChildDependentDeduction(employee);
            netDeduction = decimal.Add(netDeduction, deduction.ChildDependentDeduction);

            deduction.AdditionalDeductionBasedOnSalary = DeductionHelper.GetSalaryBasedDeduction(employee);
            netDeduction = decimal.Add(netDeduction, deduction.AdditionalDeductionBasedOnSalary);

            deduction.AdditionalDeductionBasedOnDependentAge = DeductionHelper.GetAgeBasedDependentDeduction(employee);
            netDeduction = decimal.Add(netDeduction, deduction.AdditionalDeductionBasedOnDependentAge);

            decimal dependentDeduction = 0.00m;
            dependentDeduction = decimal.Add(dependentDeduction, deduction.NonRelationDependentDeduction);
            dependentDeduction = decimal.Add(dependentDeduction, deduction.SpouseDependentDeduction);
            dependentDeduction = decimal.Add(dependentDeduction, deduction.DomesticPartnerDependentDeduction);
            dependentDeduction = decimal.Add(dependentDeduction, deduction.ChildDependentDeduction);
            dependentDeduction = decimal.Add(dependentDeduction, deduction.AdditionalDeductionBasedOnDependentAge);
            deduction.DependentDeduction = dependentDeduction;

            deduction.NetDeduction = netDeduction;
            deduction.NetSalaryPerPayCheck = decimal.Subtract(deduction.GrossSalaryPerPayCheck, deduction.NetDeduction);
            
            return deduction;
        }

    }
}
