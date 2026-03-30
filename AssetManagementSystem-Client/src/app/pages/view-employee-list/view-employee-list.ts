import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeService } from '../../services/employee';
import { Employee as EmployeeModel } from '../../models/employee';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';


@Component({
  selector: 'view-employee-list',
  standalone: true,
  imports: [CommonModule, MatTableModule],
  templateUrl: './view-employee-list.html',
  styleUrl: './view-employee-list.css',
})
export class ViewEmployeeList implements OnInit {
  private readonly employeeService = inject(EmployeeService);
  dataSource = new MatTableDataSource<EmployeeModel>([]);

  ngOnInit(): void {
    this.employeeService.getEmployees().subscribe(employees => {
      this.dataSource.data = employees;
    });
  }
}
