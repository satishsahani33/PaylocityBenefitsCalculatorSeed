# What is this
This is a simple application for calculating pay check by applying some deduction rule.

# Feaures
• User is able to add/edit/delete employees, dependents and pay checks
  For Dependents:
  1. An employee may only have 1 spouse or domestic partner (not both)
  2. An employee may have an unlimited number of children

• User is able to calculate and view a paycheck for an employee based on year and pay check month given the following rules:

 Rules for calculation of pay check
   1. 26 paychecks per year with deductions spread as evenly as possible on each paycheck
   2. employees have a base cost of $1,000 per month (for benefits)
   3. each dependent represents an additional $600 cost per month (for benefits)
  employees that make more than $80,000 per year will incur an additional 2% of their yearly salary in benefits costs
   4. dependents that are over 50 years old will incur an additional $200 per month

## Run the application

1. Clone the repo from github [GitHub Link (https://github.com/satishsahani33/PaylocityBenefitsCalculatorSeed)]

2. Navigate to the folder "PaylocityBenefitsCalculatorSeed"

3. There are two three projects inside the folder "PaylocityBenefitsCalculatorSeed"
    a. app folder --> React Application for UI
    b. PaylocityBenefitsApplication folder --> Angular Application for UI
    c. PaylocityBenefitsCalculator folder --> Asp.Net Core Web API for Backend API

I have used angular for UI.

## To Run the angular application

1. PaylocityBenefitsApplication

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 15.0.4 and Node 18.12.21

2. Install the Angular CLI

3. Navigate to PaylocityBenefitsApplication and then run following command through command line

      ** run npm install --> It will install required packages
      
      ** ng serve --> To run the application
4. Development server

5. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.


# To run the Asp .Net Core API
1. Navigate to PaylocityBenefitsCalculator and open solution file in Visual Studio 2022

2. Requirements:

Entity Frameworkcore, Mssql local database, dapper

3. I have used sql local database for this project, so
      Create the "mypaycheckdb" database in ms sql local database.
      Restore Nuget packages from package manager, Right click solution file and select Restore Nuget Packages. It will install all the required packages.

4. I have uses entity framework core and dapper

5. Run the following two commands in package manager console. Make sure "mypaycheckdb" is already created in sql local database otherwise the command will throws error.

     Add-Migration InitalMigration
     Update-Database
6. After successfully executing these commands run the application(press F5), it will open Asp.Net Core Web API

7. Make sure you are using the correct port
    ** Default angular UI runs at: `http://localhost:4200/`
    ** API runs at: `https://localhost:7124/swagger/index.htm`

8. Browse through angular application

9. Click Load Mock Data in UI to load mock data in database


# To run the Unit Test 
I have also create 6 test scenario in ApiTests project.
To run simply 
    ** Open solution file in visual studio 2022
    ** Go to Test Menu -> Run All Test

# To run the E2E testing in Angular
I have used [Cypress](https://docs.cypress.io/) to implement e2e testing.

Install and configure Cypress
    npm install cypress --save-dev

For now, I have configured only the e2e testing.
To run End-to-End testing
  1. Make sure the angular application is running at `http://localhost:4200/`
  2. Open new terminal and navigarte to angular application folder
      ** Execute command
      ** npm run cypress:open
      ** It will open cypress test window
      ** Click on E2E testing and choose any browser and click start E2E testing
      ** It will open test in browser and shows the list of available test
      ** Click spec.cy.ts 
      ** It will execute all test configured in spec.cy.ts file
      ** I have create six scenarios for paycheck validation.
# Note: 
In case if you face any issue while running application, please feel free to email at satishsahani33@gmail.com


