import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IProduct } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;
  productId: any;

  constructor(private shopService: ShopService,
              private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.productId = this.route.snapshot.params.id;
    this.getProduct(this.productId);
  }

  getProduct(id: any){
    return this.shopService.getProduct(id).subscribe(res => {
      this.product = res;
    }, error => {
      console.log(error);
    });
  }

}
