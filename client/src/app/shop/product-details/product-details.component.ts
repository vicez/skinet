import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;
  productId: any;
  quantity = 1;

  constructor(private shopService: ShopService,
              private route: ActivatedRoute,
              private basketService: BasketService,
              private bcService: BreadcrumbService) { 
      this.bcService.set('@productDetails', ' ');
      }

  ngOnInit(): void {
    this.productId = this.route.snapshot.params.id;
    this.loadProduct(this.productId);
  }

  addItemToBasket(){
    this.basketService.addItemToBasket(this.product, this.quantity)
  }

  incrementItemQuantity(){
    this.quantity++;
  }
  
  decrementItemQuantity(){
    if(this.quantity > 1){
      this.quantity--;
    }
  }

  loadProduct(id: any){
    return this.shopService.getProduct(id).subscribe(res => {
      this.product = res;
      this.bcService.set('@productDetails', this.product.name);
    }, error => {
      console.log(error);
    });
  }

}
