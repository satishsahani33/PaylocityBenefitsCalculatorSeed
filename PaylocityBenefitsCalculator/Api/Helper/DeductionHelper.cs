using Api.Dtos.Employee;
using Api.Helper.Contracts;
using Api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace Api.Helper
{
    public class DeductionHelper
    {
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

        public class EmployeeDeduction : IDependentDeduction
        {
            public decimal GetDeductionAmount(GetEmployeeDto employee)
            {

                decimal amount = 0;
                var keyValuePair = BenefitDeductionRule.GetAllBenefitDeductionTypes()
                                    .Single(x => x.Key == "EmployeeDeductionPerMonth");
                amount = decimal.Add(amount, CalculateDeduction(keyValuePair.Key, keyValuePair.Value));
                return amount;
            }
        }
        public class NonRelationDeduction : IDependentDeduction
        {
            public decimal GetDeductionAmount(GetEmployeeDto employee)
            {
                decimal amount = 0;
                var keyValuePair = BenefitDeductionRule.GetAllBenefitDeductionTypes()
                                    .Single(x => x.Key == "NonRelationDependentDeductionPerMonth");
                foreach (var dependent in employee.Dependents)
                {
                    if (dependent.Relationship == Relationship.None)
                    {
                        amount = decimal.Add(amount, CalculateDeduction(keyValuePair.Key, keyValuePair.Value));
                    }
                }
                return amount;
            }
        }
        public class SpouseDeduction : IDependentDeduction
        {
            public decimal GetDeductionAmount(GetEmployeeDto employee)
            {
                decimal amount = 0;
                var keyValuePair = BenefitDeductionRule.GetAllBenefitDeductionTypes()
                                    .Single(x => x.Key == "SpouseDependentDeductionPerMonth");
                foreach (var dependent in employee.Dependents)
                {
                    if (dependent.Relationship == Relationship.Spouse)
                    {
                        amount = decimal.Add(amount, CalculateDeduction(keyValuePair.Key, keyValuePair.Value));
                    }
                }
                return amount;
            }
        }
        public class DomesticPartnerDeduction : IDependentDeduction
        {
            public decimal GetDeductionAmount(GetEmployeeDto employee)
            {
                decimal amount = 0;
                var keyValuePair = BenefitDeductionRule.GetAllBenefitDeductionTypes()
                                    .Single(x => x.Key == "DomesticPartnerDependentDeductionPerMonth");
                foreach (var dependent in employee.Dependents)
                {
                    if (dependent.Relationship == Relationship.DomesticPartner)
                    {
                        amount = decimal.Add(amount, CalculateDeduction(keyValuePair.Key, keyValuePair.Value));
                    }
                }
                return amount;
            }
        }
        public class ChildDeduction : IDependentDeduction
        {
            public decimal GetDeductionAmount(GetEmployeeDto employee)
            {
                decimal amount = 0;
                var keyValuePair = BenefitDeductionRule.GetAllBenefitDeductionTypes()
                                    .Single(x => x.Key == "ChildDependentDeductionPerMonth");
                foreach (var dependent in employee.Dependents)
                {
                    if (dependent.Relationship == Relationship.Child)
                    {
                        amount = decimal.Add(amount, CalculateDeduction(keyValuePair.Key, keyValuePair.Value));
                    }
                }
                return amount;
            }
        }
        public class AgeBasedDeduction : IDependentDeduction
        {
            public decimal GetDeductionAmount(GetEmployeeDto employee)
            {
                decimal amount = 0;
                var keyValuePair = BenefitDeductionRule.GetAllBenefitDeductionTypes()
                                    .Single(x => x.Key == "AdditionalDeductionBasedOnDependentAgePerMonth");
                foreach (var dependent in employee.Dependents)
                {
                    int ageOfDependent = new DateTime(DateTime.Now.Subtract(dependent.DateOfBirth).Ticks).Year - 1;
                    if (ageOfDependent > 50)
                    {
                        amount = decimal.Add(amount, CalculateDeduction(keyValuePair.Key, keyValuePair.Value));
                    }
                }
                return amount;
            }
        }
        public class SalaryBasedDeduction : IDependentDeduction
        {
            public decimal GetDeductionAmount(GetEmployeeDto employee)
            {
                decimal amount = 0;
                if (employee.Salary > 80000)
                {
                    var keyValuePair = BenefitDeductionRule.GetAllBenefitDeductionTypes()
                                        .Single(x => x.Key == "AdditionalDeductionBasedOnSalaryPerYear");
                    amount = decimal.Add(amount, CalculateDeduction(keyValuePair.Key, decimal.Multiply(employee.Salary, keyValuePair.Value)));
                }
                return amount;
            }
        }
    
        public static decimal GetEmployeeDeduction(GetEmployeeDto employee)
        {
            EmployeeDeduction deductionAmount = new EmployeeDeduction();
            return deductionAmount.GetDeductionAmount(employee);
        }
        public static decimal GetNonRelationDependentDeduction(GetEmployeeDto employee)
        {
            NonRelationDeduction deductionAmount = new NonRelationDeduction();
            return deductionAmount.GetDeductionAmount(employee);
        }
        public static decimal GetSpouseDependentDeduction(GetEmployeeDto employee)
        {
            SpouseDeduction deductionAmount = new SpouseDeduction();
            return deductionAmount.GetDeductionAmount(employee);
        }
        public static decimal GetDomesticPartnerDependentDeduction(GetEmployeeDto employee)
        {
            DomesticPartnerDeduction deductionAmount = new DomesticPartnerDeduction();
            return deductionAmount.GetDeductionAmount(employee);
        }
        public static decimal GetChildDependentDeduction(GetEmployeeDto employee)
        {
            ChildDeduction employeeDeduction = new ChildDeduction();
            return employeeDeduction.GetDeductionAmount(employee);
        }
        public static decimal GetSalaryBasedDeduction(GetEmployeeDto employee)
        {
            SalaryBasedDeduction employeeDeduction = new SalaryBasedDeduction();
            return employeeDeduction.GetDeductionAmount(employee);
        }
        public static decimal GetAgeBasedDependentDeduction(GetEmployeeDto employee)
        {
            AgeBasedDeduction employeeDeduction = new AgeBasedDeduction();
            return employeeDeduction.GetDeductionAmount(employee);
        }
    }
}
