import { Component, OnInit } from '@angular/core';
import { ApiService } from './services/api.service';
import { Customer } from './models/customer';
import { Product } from './models/product';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  customer: Customer;
  products: Product[];

  constructor(private apiService: ApiService) {

  }

  ngOnInit(): void {
    this.apiService.getDefaultCustomer()
      .subscribe(customer => this.customer = customer);

    this.apiService.getProducts()
      .subscribe(products => {
        console.log(products);
        this.products = products;
      });
  }
}
