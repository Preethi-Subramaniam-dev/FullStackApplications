import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SoftwareListItem } from '../models/software';


@Injectable({
  providedIn: 'root',
})
export class SoftwareService {
    private apiUrl = 'https://localhost:62693/api/Software';
    private http = inject(HttpClient);

    getSoftware(): Observable<SoftwareListItem[]> {
        return this.http.get<SoftwareListItem[]>(this.apiUrl + '/all');
  }
}



