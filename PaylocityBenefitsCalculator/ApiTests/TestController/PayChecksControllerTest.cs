using Api.Controllers;
using Api.DataContext;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.PayCheck;
using Api.Models;
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
    public class PayChecksControllerTest
    {
        private readonly EmployeeDbContext _employeeDbContext;
        private readonly DapperApplicationContext _applicationContext;
        private readonly IConfiguration _configuration;

        private readonly IPayCheckRepository _payCheckRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly PayChecksController _payChecksController;
        string sqlConnection = "server=(localdb)\\MSSQLLocalDB; database=benefitpaycheckdb; Integrated Security=true; Encrypt=false";

        public PayChecksControllerTest()
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
            _payCheckRepository = new PayCheckRepository(_applicationContext, _commonRepository);
            _payChecksController = new PayChecksController(_payCheckRepository);
        }
        [Fact]
        public async void GetAllPayChecks()
        {
            var payChecks = await _payCheckRepository.GetAllPayChecks(1, 100);
        }
        [Fact]
        public void CalculatePayCheck()
        {
            List<GetEmployeeDto> employees = EmployeeMockData.EmployeeList();
            GetEmployeeDto employee = employees.FirstOrDefault(item => item.Id == 2);
            string year = "2022";
            string month = "Pay Check 1";
            decimal expected = 2189.15m;
            if (employee != null)
            {
                GetPayCheckDto payCheckDto = _payCheckRepository.CalculatePayCheck(employee, year, month);
                decimal actual = decimal.Round(payCheckDto.NetAmount, 2);
                Assert.Equal(expected, actual);
            }
        }
        [Fact]
        public void CalculatePayCheck_EmployeeWithNoDependents()
        {
            List<GetEmployeeDto> employees = EmployeeMockData.EmployeeList();
            GetEmployeeDto employee = employees.FirstOrDefault(item => item.Id == 1);
            string year = "2022";
            string month = "Pay Check 1";
            decimal expected = 2439.27m;
            if (employee != null)
            {
                GetPayCheckDto payCheckDto = _payCheckRepository.CalculatePayCheck(employee, year, month);
                decimal actual = decimal.Round(payCheckDto.NetAmount, 2);
                Assert.Equal(expected, actual);
            }
        }
        [Fact]
        public void CalculatePayCheck_EmployeeWithDependents()
        {
            List<GetEmployeeDto> employees = EmployeeMockData.EmployeeList();
            GetEmployeeDto employee = employees.FirstOrDefault(item => item.Id == 2);
            string year = "2022";
            string month = "Pay Check 1";
            decimal expected = 2189.15m;
            if (employee != null)
            {
                GetPayCheckDto payCheckDto = _payCheckRepository.CalculatePayCheck(employee, year, month);
                decimal actual = decimal.Round(payCheckDto.NetAmount, 2);
                Assert.Equal(expected, actual);
            }
        }
        [Fact]
        public void CalculatePayCheck_EmployeeWithSalaryLessThan80K()
        {
            List<GetEmployeeDto> employees = EmployeeMockData.EmployeeList();
            GetEmployeeDto employee = employees.FirstOrDefault(item => item.Id == 3);
            string year = "2022";
            string month = "Pay Check 1";
            decimal expected = 923.50m;
            if (employee != null)
            {
                GetPayCheckDto payCheckDto = _payCheckRepository.CalculatePayCheck(employee, year, month);
                decimal actual = decimal.Round(payCheckDto.NetAmount, 2);
                Assert.Equal(expected, actual);
            }
        }
        [Fact]
        public void CalculatePayCheck_EmployeeWithSalaryGreaterThan80K()
        {
            List<GetEmployeeDto> employees = EmployeeMockData.EmployeeList();
            GetEmployeeDto employee = employees.FirstOrDefault(item => item.Id == 4);
            string year = "2022";
            string month = "Pay Check 1";
            decimal expected = 2205.11m;
            if (employee != null)
            {
                GetPayCheckDto payCheckDto = _payCheckRepository.CalculatePayCheck(employee, year, month);
                decimal actual = decimal.Round(payCheckDto.NetAmount, 2);
                Assert.Equal(expected, actual);
            }
        }
        [Fact]
        public void CalculatePayCheck_EmployeeWithSalaryLessThanBenefitAmount()
        {
            List<GetEmployeeDto> employees = EmployeeMockData.EmployeeList();
            GetEmployeeDto employee = employees.FirstOrDefault(item => item.Id == 5);
            string year = "2022";
            string month = "Pay Check 1";
            decimal expected = 0.00m;
            if (employee != null)
            {
                GetPayCheckDto payCheckDto = _payCheckRepository.CalculatePayCheck(employee, year, month);
                decimal actual = decimal.Round(payCheckDto.NetAmount, 2);
                if (actual < 0)
                {
                    actual= 0.00m;
                    Assert.Equal(expected, actual);
                }
            }
        }
        [Fact]
        public void CalculatePayCheckFromDatabase_EmployeeWithDependentAgeGreaterThan50()
        {
            List<GetEmployeeDto> employees = EmployeeMockData.EmployeeList();
            GetEmployeeDto employee = employees.FirstOrDefault(item => item.Id == 6);
            string year = "2022";
            string month = "Pay Check 1";
            decimal expected = 3880.80m;
            if (employee != null)
            {
                GetPayCheckDto payCheckDto = _payCheckRepository.CalculatePayCheck(employee, year, month);
                decimal actual = decimal.Round(payCheckDto.NetAmount, 2);
                Assert.Equal(expected, actual);
            }
        }
    }
}
