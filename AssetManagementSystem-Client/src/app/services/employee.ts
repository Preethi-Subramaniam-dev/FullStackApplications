import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from '../models/employee';


@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
    private apiUrl = 'https://localhost:62693/api/Employee';
    private http = inject(HttpClient);

    getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl + '/all');
  }
}



