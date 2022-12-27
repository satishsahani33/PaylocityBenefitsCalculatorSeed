import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { ApiResponse, PayCheck } from '@app/models';
const baseUrl = environment.apiUrl + '/paychecks'
@Injectable({
  providedIn: 'root'
})
export class PayCheckService {
  constructor(private http: HttpClient) { }

  getAll(pageNumber :string= '1', pageSize:string= '100', orderBy:string= 'Id',sortBy:string= 'asc') {
    return this.http.get<ApiResponse>(`${baseUrl}/${pageNumber}/${pageSize}/${orderBy}/${sortBy}`);
}

  getById(id: string) {
      return this.http.get<ApiResponse>(`${baseUrl}/${id}`);
  }

  getAllPayChecksOfEmployee(id: string) {
    return this.http.get<ApiResponse>(`${baseUrl}/employee/${id}`);
  }
  create(id: string, year:string, month:string) {
    return this.http.post<ApiResponse>(`${baseUrl}/${id}/${year}/${month}`,'');
      //return this.http.post(baseUrl, id);
  }

  update(id: string, params: any) {
      return this.http.put(`${baseUrl}/${id}`, params);
  }

  delete(id: string) {
      return this.http.delete(`${baseUrl}/${id}`);
  }
}
