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
            //In Each PayCheck
            decimal totalDeduction = 0.00m;
            decimal perPayCheckMultiplicationConstant = Decimal.Divide(12, 26);

            decimal grossAmount = Decimal.Divide(employee.Salary, 26);

            decimal deductionForEmployee = Decimal.Multiply(BenefitDeductionRule.EmployeeDeductionPerMonth, perPayCheckMultiplicationConstant);
            totalDeduction = Decimal.Add(totalDeduction, deductionForEmployee);

            decimal deductionForDependent = Decimal.Multiply(employee.Dependents.Count, Decimal.Multiply(BenefitDeductionRule.DependentDeductionPerMonth, perPayCheckMultiplicationConstant));
            totalDeduction = Decimal.Add(totalDeduction, deductionForDependent);

            decimal deductionBasedOnSalary = employee.Salary > 80000 ? (Decimal.Multiply(employee.Salary, Decimal.Divide(BenefitDeductionRule.AdditionalDeductionBasedOnSalaryPerYear, 26))) : 0;
            totalDeduction = Decimal.Add(totalDeduction, deductionBasedOnSalary);

            int noOfDependent50YearsOld = 0;
            foreach (var dependent in employee.Dependents)
            {
                int ageOfDependent = new DateTime(DateTime.Now.Subtract(dependent.DateOfBirth).Ticks).Year - 1;
                if (ageOfDependent > 50)
                {
                    noOfDependent50YearsOld++;
                }
            }

            decimal deductionBasedOnDependentAge = Decimal.Multiply(noOfDependent50YearsOld, Decimal.Multiply(BenefitDeductionRule.AdditionalDeductionBasedOnDependentAgePerMonth, perPayCheckMultiplicationConstant));
            totalDeduction = Decimal.Add(totalDeduction, deductionBasedOnDependentAge);

            decimal netAmount = Decimal.Subtract(grossAmount, totalDeduction);

            employeePayCheck.EmployeeId = employee.Id;
            employeePayCheck.EmployeeFirstName = employee.FirstName;
            employeePayCheck.EmployeeLastName = employee.LastName;
            employeePayCheck.YearlySalary = employee.Salary;
            employeePayCheck.EmployeeDeduction = deductionForEmployee;
            employeePayCheck.DependentDeduction = deductionForDependent;
            employeePayCheck.GrossAmount = grossAmount;
            employeePayCheck.AdditionalDeductionBasedOnSalary = deductionBasedOnSalary;
            employeePayCheck.AdditionalDeductionBasedOnDependentAge = deductionBasedOnDependentAge;
            employeePayCheck.NetAmount = netAmount;
            employeePayCheck.Year = year;
            employeePayCheck.Month = month;

            return employeePayCheck;
        }

    }
}
