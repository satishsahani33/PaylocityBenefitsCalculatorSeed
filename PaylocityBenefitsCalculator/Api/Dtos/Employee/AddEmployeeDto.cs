using Api.Dtos.Dependent;

namespace Api.Dtos.Employee
{

    public class AddEmployeeDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal Salary { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<AddDependentDto>? Dependents { get; set; }
    }
}
