import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMetthod';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss']
})
export class CheckoutDeliveryComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  deliveryMethods: IDeliveryMethod[];


  constructor(private checkoutService: CheckoutService,
              private basketService: BasketService) { }

  ngOnInit(): void {
    this.checkoutService.getDeliveryMethods().subscribe((res: IDeliveryMethod[]) => {
      this.deliveryMethods = res;
    }, error => {
      console.log(error);
    });
  }

  setDeliveryMethod(dm: IDeliveryMethod){
    this.basketService.setShippingPrice(dm);
  }
}
