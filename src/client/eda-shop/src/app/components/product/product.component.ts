import { Component, OnInit, Input } from '@angular/core';
import { Product } from 'src/app/models/product';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  @Input() product: Product;

  constructor() { }

  ngOnInit() {
  }

  getProductImageUrl() {
    return `${environment.serviceUri}products-service/images/${this.product.pictureFileName}`;
  }
}
