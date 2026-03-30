import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface EmployeeModel {
  name: string;
  email: string;
  department: string;
  phone: string;
}

@Component({
  selector: 'employee-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './employee.html',
  styleUrl: './employee.css',
})
export class Employee {

}
