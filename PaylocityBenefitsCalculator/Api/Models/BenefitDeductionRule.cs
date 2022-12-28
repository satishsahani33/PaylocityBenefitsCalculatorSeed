namespace Api.Models
{
    public class BenefitDeductionRule
    {
        public static Dictionary<string, decimal> GetAllBenefitDeductionTypes()
        {
            var dictionary = new Dictionary<string, decimal>();
            dictionary.Add("EmployeeDeductionPerMonth", 1000);
            dictionary.Add("NonRelationDependentDeductionPerMonth", 600);
            dictionary.Add("SpouseDependentDeductionPerMonth", 600);
            dictionary.Add("DomesticPartnerDependentDeductionPerMonth", 600);
            dictionary.Add("ChildDependentDeductionPerMonth", 600);
            dictionary.Add("AdditionalDeductionBasedOnSalaryPerYear", 0.02m);
            dictionary.Add("AdditionalDeductionBasedOnDependentAgePerMonth", 200);
            return dictionary;
        }
    }
    public class Deduction
    {
        public decimal GrossSalaryPerPayCheck { get; set; }
        public decimal EmployeeDeduction { get; set; }
        public decimal DependentDeduction { get; set; }
        public decimal ChildDependentDeduction { get; set; }
        public decimal SpouseDependentDeduction { get; set; }
        public decimal DomesticPartnerDependentDeduction { get; set; }
        public decimal NonRelationDependentDeduction { get; set; }
        public decimal AllDependentDeduction { get; set; }
        public decimal AdditionalDeductionBasedOnSalary { get; set; }
        public decimal AdditionalDeductionBasedOnDependentAge { get; set; }
        public decimal NetDeduction { get; set; }
        public decimal NetSalaryPerPayCheck { get; set; }
    }
}

