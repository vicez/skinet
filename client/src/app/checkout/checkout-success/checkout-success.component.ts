import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IOrder } from 'src/app/shared/models/order';
import { Tracing } from 'trace_events';

@Component({
  selector: 'app-checkout-success',
  templateUrl: './checkout-success.component.html',
  styleUrls: ['./checkout-success.component.scss']
})
export class CheckoutSuccessComponent implements OnInit { 
  order: IOrder;
  orderPath: string;
  btnText: string;

  constructor(private router: Router) {
    const navigattion = this.router.getCurrentNavigation();
    const state = navigattion?.extras?.state;
    if(state){
      this.order = state as IOrder;
      this.btnText = "View your order";
      this.orderPath = `/orders/${this.order.id}`;
    }
    else{
      this.btnText = "View your orders";
      this.orderPath = `/orders`;
    }
   }

  ngOnInit(): void {
  }

}
