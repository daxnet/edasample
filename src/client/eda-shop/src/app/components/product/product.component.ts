import { Component, OnInit, Input } from '@angular/core';
import { Product } from 'src/app/models/product';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { MessageBoxComponent } from '../shared/message-box/message-box.component';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  @Input() product: Product;

  constructor(private dialog: MatDialog) { }

  ngOnInit() {
  }

  getProductImageUrl() {
    return `${environment.serviceUri}products-service/images/${this.product.pictureFileName}`;
  }

  showMessage() {
    this.dialog.open(MessageBoxComponent, {
      width: '250px',
      data: { title: 'aaa', text: 'bbbb'}
    });
  }
}
