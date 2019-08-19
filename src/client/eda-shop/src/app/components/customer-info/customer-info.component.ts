import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { Customer } from 'src/app/models/customer';

@Component({
  selector: 'app-customer-info',
  templateUrl: './customer-info.component.html',
  styleUrls: ['./customer-info.component.css']
})
export class CustomerInfoComponent implements OnInit {

  customer: Customer;

  constructor(private api: ApiService) { }

  ngOnInit() {
    this.readCustomer();
  }

  readCustomer() {
    this.api.getDefaultCustomer().subscribe(customer => {
      this.customer = customer;
    });
  }

}
