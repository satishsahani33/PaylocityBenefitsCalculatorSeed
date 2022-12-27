using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Helper
{
    public static class EmployeeMockData
    {
        public static List<AddEmployeeDto> EmployeeList()
        {
            var employees = new List<AddEmployeeDto>
            {
                new()
                {
                    //Id = 1,
                    FirstName = "LeBron",
                    LastName = "James",
                    Salary = 75420.99m,
                    DateOfBirth = new DateTime(1984, 12, 30)
                },
                new()
                {
                    //Id = 2,
                    FirstName = "Ja",
                    LastName = "Morant",
                    Salary = 92365.22m,
                    DateOfBirth = new DateTime(1999, 8, 10),
                    Dependents = new List<AddDependentDto>
                    {
                        new()
                        {
                            //Id = 1,
                            FirstName = "Spouse",
                            LastName = "Morant",
                            Relationship = Relationship.Spouse,
                            DateOfBirth = new DateTime(1998, 3, 3)
                        },
                        new()
                        {
                            //Id = 2,
                            FirstName = "Child1",
                            LastName = "Morant",
                            Relationship = Relationship.Child,
                            DateOfBirth = new DateTime(2020, 6, 23)
                        },
                        new()
                        {
                            //Id = 3,
                            FirstName = "Child2",
                            LastName = "Morant",
                            Relationship = Relationship.Child,
                            DateOfBirth = new DateTime(2021, 5, 18)
                        }
                    }
                },
                new()
                {
                    //Id = 3,
                    FirstName = "Michael",
                    LastName = "Jordan",
                    Salary = 43211.12m,
                    DateOfBirth = new DateTime(1963, 2, 17),
                    Dependents = new List<AddDependentDto>
                    {
                        new()
                        {
                            //Id = 4,
                            FirstName = "DP",
                            LastName = "Jordan",
                            Relationship = Relationship.DomesticPartner,
                            DateOfBirth = new DateTime(1974, 1, 2)
                        }
                    }
                },
                new()
                {
                    //Id = 4,
                    FirstName = "Michael",
                    LastName = "Holding",
                    Salary = 80543.72m,
                    DateOfBirth = new DateTime(1990, 2, 17),
                    Dependents = new List<AddDependentDto>
                    {
                        new()
                        {
                            //Id = 5,
                            FirstName = "Mitchel",
                            LastName = "Holding",
                            Relationship = Relationship.DomesticPartner,
                            DateOfBirth = new DateTime(1950, 1, 2)
                        }
                    }
                },
                new()
                {
                    //Id = 5,
                    FirstName = "Jason",
                    LastName = "Snider",
                    Salary = 30543.72m,
                    DateOfBirth = new DateTime(1985, 12, 12),
                    Dependents = new List<AddDependentDto>
                    {
                        new()
                        {
                            //Id = 6,
                            FirstName = "Emily",
                            LastName = "Brown",
                            Relationship = Relationship.DomesticPartner,
                            DateOfBirth = new DateTime(1999, 1, 2)
                        },
                        new()
                        {
                            //Id = 7,
                            FirstName = "David",
                            LastName = "Smith",
                            Relationship = Relationship.Child,
                            DateOfBirth = new DateTime(2012, 1, 2)
                        },
                        new()
                        {
                            //Id = 8,
                            FirstName = "Ricky",
                            LastName = "Wagner",
                            Relationship = Relationship.None,
                            DateOfBirth = new DateTime(1962, 1, 2)
                        }
                    }
                },
                new ()
                {
                    //Id = 6,
                    FirstName = "Cory",
                    LastName = "Abraham",
                    Salary = 125000.72m,
                    DateOfBirth = new DateTime(1999, 11, 10),
                    Dependents = new List<AddDependentDto>
                    {
                        new ()
                        {
                            //Id = 9,
                            FirstName = "Raphel",
                            LastName = "Abraham",
                            Relationship = Relationship.DomesticPartner,
                            DateOfBirth = new DateTime(1970,10, 12)
                        }
                    }
                }
            };
            return employees;
        }
    }
}
