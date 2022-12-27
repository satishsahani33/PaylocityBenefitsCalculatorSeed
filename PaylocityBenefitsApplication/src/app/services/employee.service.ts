import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environments/environment';
import { Employee,ApiResponse } from '@app/models';

const baseUrl = `${environment.apiUrl}/employees`;
@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  constructor(private http: HttpClient) { }

  getAll(pageNumber :string= '1', pageSize:string= '100', orderBy:string= 'Id',sortBy:string= 'asc') {
      return this.http.get<ApiResponse>(`${baseUrl}/${pageNumber}/${pageSize}/${orderBy}/${sortBy}`);
  }

  getById(id: string) {
      return this.http.get<ApiResponse>(`${baseUrl}/${id}`);
  }

  create(params: Employee) {
      return this.http.post<ApiResponse>(baseUrl, params);
  }

  update(id: string, params: any) {
      return this.http.put<ApiResponse>(`${baseUrl}/${id}`, params);
  }

  delete(id: string) {
      return this.http.delete<ApiResponse>(`${baseUrl}/${id}`);
  }
}
