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
        public static decimal CalculateDeduction(string deductionName, decimal deductionValue)
        {
            decimal deductionAmount = deductionValue;
            if (deductionName.Contains("PerMonth"))
            {
                deductionAmount = decimal.Multiply(deductionValue, 12);
            }
            deductionAmount = decimal.Divide(deductionAmount, 26);
            return deductionAmount;
        }
        public static Deduction GetAllDeduction(GetEmployeeDto employee)
        {
            Deduction deduction = new Deduction();
            Dictionary<string, decimal> deductionTypes = BenefitDeductionRule.GetAllBenefitDeductionTypes();
            deduction.GrossSalaryPerPayCheck = CalculateDeduction("salary", employee.Salary);

            decimal netDeduction = 0.00m;
            if (deductionTypes != null && deductionTypes.Count > 0)
            {
                foreach (KeyValuePair<string, decimal> deductionType in deductionTypes)
                {
                    if (deductionType.Key == "EmployeeDeductionPerMonth")
                    {
                        deduction.EmployeeDeduction = CalculateDeduction(deductionType.Key, deductionType.Value);
                        netDeduction = decimal.Add(netDeduction, deduction.EmployeeDeduction);
                    }
                    else if (deductionType.Key == "NonRelationDependentDeductionPerMonth")
                    {
                        deduction.NonRelationDependentDeduction = CalculateDeduction(deductionType.Key, deductionType.Value);
                    }
                    else if (deductionType.Key == "SpouseDependentDeductionPerMonth")
                    {
                        deduction.SpouseDependentDeduction = CalculateDeduction(deductionType.Key, deductionType.Value);
                    }
                    else if (deductionType.Key == "DomesticPartnerDependentDeductionPerMonth")
                    {
                        deduction.DomesticPartnerDependentDeduction = CalculateDeduction(deductionType.Key, deductionType.Value);
                    }
                    else if (deductionType.Key == "ChildDependentDeductionPerMonth")
                    {
                        deduction.ChildDependentDeduction = CalculateDeduction(deductionType.Key, deductionType.Value);
                    }
                    else if (deductionType.Key == "AdditionalDeductionBasedOnSalaryPerYear")
                    {
                        if (employee.Salary > 80000)
                        {
                            deduction.AdditionalDeductionBasedOnSalary = CalculateDeduction(deductionType.Key, decimal.Multiply(employee.Salary, deductionType.Value));
                            netDeduction = decimal.Add(netDeduction, deduction.AdditionalDeductionBasedOnSalary);
                        }
                    }
                    else if (deductionType.Key == "AdditionalDeductionBasedOnDependentAgePerMonth")
                    {
                        deduction.AdditionalDeductionBasedOnDependentAge = CalculateDeduction(deductionType.Key, deductionType.Value);
                    }
                }
            }

            //Count to track the number of dependents
            int noneRelationDependentCount = 0;
            int spouseDependentCount = 0;
            int domesticPartnerDependentCount = 0;
            int childDependentCount = 0;
            int ageCount = 0;
            if (employee.Dependents.Count > 0)
            {
                foreach (var dependent in employee.Dependents)
                {
                    if (dependent.Relationship == Relationship.None) { noneRelationDependentCount++; }
                    if (dependent.Relationship == Relationship.Spouse) { spouseDependentCount++; }
                    if (dependent.Relationship == Relationship.DomesticPartner) { domesticPartnerDependentCount++; }
                    if (dependent.Relationship == Relationship.Child) { childDependentCount++; }

                    int ageOfDependent = new DateTime(DateTime.Now.Subtract(dependent.DateOfBirth).Ticks).Year - 1;
                    if (ageOfDependent > 50)
                    {
                        ageCount++;
                    }
                }
            }
            deduction.NonRelationDependentDeduction = decimal.Multiply(deduction.NonRelationDependentDeduction, noneRelationDependentCount);
            deduction.SpouseDependentDeduction = decimal.Multiply(deduction.SpouseDependentDeduction, spouseDependentCount);
            deduction.DomesticPartnerDependentDeduction = decimal.Multiply(deduction.DomesticPartnerDependentDeduction, domesticPartnerDependentCount);
            deduction.ChildDependentDeduction = decimal.Multiply(deduction.ChildDependentDeduction, childDependentCount);
            
            decimal dependentDeduction = 0.00m;
            dependentDeduction = decimal.Add(dependentDeduction, deduction.NonRelationDependentDeduction);
            dependentDeduction = decimal.Add(dependentDeduction, deduction.SpouseDependentDeduction);
            dependentDeduction = decimal.Add(dependentDeduction, deduction.DomesticPartnerDependentDeduction);
            dependentDeduction = decimal.Add(dependentDeduction, deduction.ChildDependentDeduction);

            deduction.DependentDeduction = dependentDeduction;

            deduction.AdditionalDeductionBasedOnDependentAge = decimal.Multiply(deduction.AdditionalDeductionBasedOnDependentAge, ageCount);

            netDeduction = decimal.Add(netDeduction, deduction.NonRelationDependentDeduction);
            netDeduction = decimal.Add(netDeduction, deduction.SpouseDependentDeduction);
            netDeduction = decimal.Add(netDeduction, deduction.DomesticPartnerDependentDeduction);
            netDeduction = decimal.Add(netDeduction, deduction.ChildDependentDeduction);
            netDeduction = decimal.Add(netDeduction, deduction.AdditionalDeductionBasedOnDependentAge);
            deduction.NetDeduction = netDeduction;

            deduction.NetSalaryPerPayCheck = decimal.Subtract(deduction.GrossSalaryPerPayCheck, deduction.NetDeduction);


            return deduction;
        }

    }
}
