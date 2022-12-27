using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class PayCheck
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
