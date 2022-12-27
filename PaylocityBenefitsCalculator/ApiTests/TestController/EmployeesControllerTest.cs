using Api.Controllers;
using Api.DataContext;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repository;
using Api.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.TestController
{
    public class EmployeesControllerTest
    {
        private readonly EmployeeDbContext _employeeDbContext;
        private readonly DapperApplicationContext _applicationContext;
        private readonly IConfiguration _configuration;

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDependentRepository _dependentRepository;
        private readonly IPayCheckRepository _payCheckRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly EmployeesController _employeesController;
        string sqlConnection = "server=(localdb)\\MSSQLLocalDB; database=benefitpaycheckdb; Integrated Security=true; Encrypt=false";
        public EmployeesControllerTest() 
        {

            var myConfiguration = new Dictionary<string, string>
                        {
                            { "ConnectionStrings:SqlDbConnection", sqlConnection}
                        };
    
            // Connect to database from this test project
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _employeeDbContext = new EmployeeDbContext();
            _applicationContext = new DapperApplicationContext(configuration);
            _commonRepository = new CommonRepository(_applicationContext);
            _employeeRepository = new EmployeeRepository(_applicationContext,_commonRepository);
            
            _employeesController = new EmployeesController(_employeeRepository);

        }
        [Fact]
        public async void GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees(1, 100);
        }
    }
}
