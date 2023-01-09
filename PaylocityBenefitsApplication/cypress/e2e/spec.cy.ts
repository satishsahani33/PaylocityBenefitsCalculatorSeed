describe('template spec', () => {
  it('passes', () => {
    cy.visit('http://localhost:4200')
    cy.contains('Home')
    cy.contains('Employee')
    cy.contains('Dependents')
    cy.contains('Pay Checks')
    cy.contains('Load Mock Data')
    cy.contains('Welcome to Paylocity Benefits Calculator')
    cy.contains('For more option click on the nav bar link')

    cy.contains('Employee').click()
    cy.url().should('include', '/employee')
    cy.wait(2000)

    //Load mock data if no employee is available
    cy.get('body').then(($body) => {
      if ($body.text().includes('No employee available')) {
        cy.contains('Fetch Next Block of Employee')
        cy.contains('Fetch Previous Block of Employee')
        cy.contains('Load Mock Data').click()
        cy.contains('Please Confirm')
        //  Verify that the value has been updated
        cy.contains('OK').click()
        cy.contains('Employee').click()
        cy.wait(1000)
      }
    })
     cy.get('tbody tr', {log:false}).each(($tr, index) => {
      if(index != 5)
      {
        return
      }
      // Log beginning of row test
      cy.then(() => Cypress.log({
        displayName: 'Employee With No Dependents',
        message: `TEST Employee With No Dependents`,
      }))
      cy.wait(2000)
      cy.wrap($tr, {log:false}).find('td:nth-child(1)', {log:false}).then($firstCol => {
        const textEnvelope = $firstCol.text();
        if (index == 5) {
          cy.wrap($firstCol, {log:false}).siblings({log:false}).eq(-1, {log:false}).contains('View Only').click()
            cy.get('select#yearDropdown').select('2020')
            cy.get('select#monthDropdown').select('Pay Check 1')
            cy.get('#okPopup').click()
            cy.get('#netAmount').should("have.text","$2,439.27")
            cy.wait(2000)
            cy.contains('Back').click()
            cy.contains('Back').click()

        }
      })
    })

    cy.get('tbody tr', {log:false}).each(($tr, index) => {
      if(index != 4)
      {
        return
      }
      // Log beginning of row test
      cy.then(() => Cypress.log({
        displayName: 'Employee With Dependents',
        message: `TEST ${index}`,
      }))
      cy.wait(2000)
      cy.wrap($tr, {log:false}).find('td:nth-child(1)', {log:false}).then($firstCol => {
        const textEnvelope = $firstCol.text();
        if (index != 4) {
          cy.wrap($firstCol, {log:false}).siblings({log:false}).eq(-1, {log:false}).contains('View Only').click()
            cy.get('select#yearDropdown').select('2020')
            cy.get('select#monthDropdown').select('Pay Check 2')
            cy.get('#okPopup').click()
            cy.get('#netAmount').should("have.text","$2,189.15")
            cy.wait(2000)
            cy.contains('Back').click()
            cy.contains('Back').click()
        }
      })
    })

    cy.get('tbody tr', {log:false}).each(($tr, index) => {
      if(index != 3)
      {
        return
      }
      // Log beginning of row test
      cy.then(() => Cypress.log({
        displayName: 'Employee With Salary Less Than 80K',
        message: `TEST ${index}`,
      }))
      cy.wait(2000)
    
      cy.wrap($tr, {log:false}).find('td:nth-child(1)', {log:false}).then($firstCol => {
        const textEnvelope = $firstCol.text();
        if (index == 3) {
          cy.wrap($firstCol, {log:false}).siblings({log:false}).eq(-1, {log:false}).contains('View Only').click()
            cy.get('select#yearDropdown').select('2020')
            cy.get('select#monthDropdown').select('Pay Check 3')
            cy.get('#okPopup').click()
            cy.get('#netAmount').should("have.text","$923.50")
            cy.wait(2000)
            cy.contains('Back').click()
            cy.contains('Back').click()

        }
      })
    })

    cy.get('tbody tr', {log:false}).each(($tr, index) => {
      if(index != 2)
      {
        return
      }
      // Log beginning of row test
      cy.then(() => Cypress.log({
        displayName: 'Employee With Salary Greater Than 80K',
        message: `TEST ${index}`,
      }))
      cy.wait(2000)
    
      cy.wrap($tr, {log:false}).find('td:nth-child(1)', {log:false}).then($firstCol => {
        const textEnvelope = $firstCol.text();
        if (index == 2) {
          cy.wrap($firstCol, {log:false}).siblings({log:false}).eq(-1, {log:false}).contains('View Only').click()
            cy.get('select#yearDropdown').select('2020')
            cy.get('select#monthDropdown').select('Pay Check 1')
            cy.get('#okPopup').click()
            cy.get('#netAmount').should("have.text","$2,205.11")
            cy.wait(2000)
            cy.contains('Back').click()
            cy.contains('Back').click()

        }
      })
    })

    cy.get('tbody tr', {log:false}).each(($tr, index) => {
      if(index != 1)
      {
        return
      }
      // Log beginning of row test
      cy.then(() => Cypress.log({
        displayName: 'Employee With Salary Less Than BenefitAmount',
        message: `TEST ${index}`,
      }))
      cy.wait(2000)
    
      cy.wrap($tr, {log:false}).find('td:nth-child(1)', {log:false}).then($firstCol => {
        const textEnvelope = $firstCol.text();
        if (index != 1) {
          cy.wrap($firstCol, {log:false}).siblings({log:false}).eq(-1, {log:false}).contains('View Only').click()
            cy.get('select#yearDropdown').select('2020')
            cy.get('select#monthDropdown').select('Pay Check 2')
            cy.get('#okPopup').click()
            cy.get('#netAmount').should("have.text","-$209.86")
            cy.wait(2000)
            cy.contains('Back').click()
            cy.contains('Back').click()

        }
      })
    })

    cy.get('tbody tr', {log:false}).each(($tr, index) => {
      if(index != 0)
      {
        return
      }
      // Log beginning of row test
      cy.then(() => Cypress.log({
        displayName: 'Employee With Dependent AgeG reater Than 50',
        message: `TEST Employee With Dependent AgeG reater Than 50`,
      }))
      cy.wait(2000)
    
      cy.wrap($tr, {log:false}).find('td:nth-child(1)', {log:false}).then($firstCol => {
        const textEnvelope = $firstCol.text();
        if (index == 0) {
          cy.wrap($firstCol, {log:false}).siblings({log:false}).eq(-1, {log:false}).contains('View Only').click()
            cy.get('select#yearDropdown').select('2020')
            cy.get('select#monthDropdown').select('Pay Check 3')
            cy.get('#okPopup').click()
            cy.get('#netAmount').should("have.text","$3,880.80")
            cy.wait(2000)
            cy.contains('Back').click()
            cy.contains('Back').click()
        }
      })
    })
  })
})
