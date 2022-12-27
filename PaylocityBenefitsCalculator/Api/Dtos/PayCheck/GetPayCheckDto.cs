using Api.Dtos.Employee;
using Microsoft.EntityFrameworkCore;

namespace Api.Dtos.PayCheck
{
    public class GetPayCheckDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeFirstName { get; set; }
        public string? EmployeeLastName { get; set; }
        [Precision(18, 2)]
        public decimal YearlySalary { get; set; }
        [Precision(18, 2)]
        public decimal GrossAmount { get; set; }
        [Precision(18, 2)]
        public decimal EmployeeDeduction { get; set; }
        [Precision(18, 2)]
        public decimal DependentDeduction { get; set; }
        [Precision(18, 2)]
        public decimal AdditionalDeductionBasedOnSalary { get; set; }
        [Precision(18, 2)]
        public decimal AdditionalDeductionBasedOnDependentAge { get; set; }
        [Precision(18, 2)]
        public decimal NetAmount { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
    }
}

/*
 
Each Paycheck contains	4000
Deduction For Employee	461.5384615
Deduction For Dependent	830.7692308
Since yearly salary > 80000	80
Since 1 Dependent age > 50	92.30769231
	
Final Amount in each paycheck	2535.384615

 */