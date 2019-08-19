import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from '../models/customer';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  getDefaultCustomer(): Observable<Customer> {
    return this.http.get<Customer>(`${environment.serviceUri}customer-service/${environment.customerId}`);
  }
}
