import { Employee } from "./employee";
import { Relationship } from "./relationship";
export class Dependent{
    id?: string;
    firstName?: string;
    lastName?: string;
    dateOfBirth?: Date;
    relationship?: Relationship;
    employeeId?: string;
    employee?: Employee
}