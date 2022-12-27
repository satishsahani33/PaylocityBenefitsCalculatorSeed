import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse, Dependent } from '@app/models';
import { environment } from '@environments/environment';

const baseUrl = `${environment.apiUrl}/dependents`;
@Injectable({
  providedIn: 'root'
})
export class DependentService {
  constructor(private http: HttpClient) { }

  getAll(pageNumber :string= '1', pageSize:string= '100', orderBy:string= 'Id',sortBy:string= 'asc') {
    return this.http.get<ApiResponse>(`${baseUrl}/${pageNumber}/${pageSize}/${orderBy}/${sortBy}`);
}
  getAllDependentsOfEmployee(id: string) {
    return this.http.get<ApiResponse>(`${baseUrl}/employee/${id}`);
  }
  getById(id: string) {
      return this.http.get<ApiResponse>(`${baseUrl}/${id}`);
  }

  create(params: any) {
      return this.http.post(baseUrl, params);
  }

  update(id: string, params: any) {
      return this.http.put(`${baseUrl}/${id}`, params);
  }

  delete(id: string) {
      return this.http.delete(`${baseUrl}/${id}`);
  }
}

