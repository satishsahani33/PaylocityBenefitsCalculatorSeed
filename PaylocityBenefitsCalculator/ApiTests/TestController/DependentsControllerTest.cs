using Api.Controllers;
using Api.DataContext;
using Api.Repository;
using Api.Repository.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.TestController
{
    public class DependentsControllerTest
    {
        private readonly EmployeeDbContext _employeeDbContext;
        private readonly DapperApplicationContext _applicationContext;
        private readonly IConfiguration _configuration;

        private readonly IDependentRepository _dependentRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly DependentsController _dependentsController;
        string sqlConnection = "server=(localdb)\\MSSQLLocalDB; database=benefitpaycheckdb; Integrated Security=true; Encrypt=false";

        public DependentsControllerTest()
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
            _dependentRepository = new DependentRepository(_applicationContext, _commonRepository);

            _dependentsController = new DependentsController(_dependentRepository);
        }
        [Fact]
        public async void GetAllDependents()
        {
            var dependents = await _dependentRepository.GetAllDependents(1, 100);
        }
    }
}
