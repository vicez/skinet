import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { OrdersService } from 'src/app/orders/orders.service';
import { IBasketTotal } from '../../models/basket';

@Component({
  selector: 'app-order-totals',
  templateUrl: './order-totals.component.html',
  styleUrls: ['./order-totals.component.scss']
})
export class OrderTotalsComponent implements OnInit {
  basketTotal$: Observable<IBasketTotal>;
  @Input() isOrder: boolean = false;

  constructor(private basketService: BasketService,
              private orderService: OrdersService) { }

  ngOnInit(): void {
    this.basketTotal$ = this.isOrder ? this.orderService.orderTotal$ : this.basketService.basketTotal$;
  }

}
