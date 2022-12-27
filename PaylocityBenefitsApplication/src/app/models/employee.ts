import { Dependent } from "./dependent";
export class Employee{
    id?: string;
    firstName?: string;
    lastName?: string;
    salary?: string;
    dateOfBirth?: Date;
    dependents?: Dependent[];
}