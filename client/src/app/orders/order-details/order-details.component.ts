import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  orderId: any;
  order: IOrder;

  constructor(private bcService: BreadcrumbService,
              private route: ActivatedRoute,
              private orderService: OrdersService) {
    this.bcService.set('@orderDetails', ' ');
  }

  ngOnInit(): void {
    this.orderId= this.route.snapshot.params["id"];
    this.getSingleOrder();
  }

  getSingleOrder(){
    this.orderService.getSingleOrder(this.orderId).subscribe((order: IOrder) => {
      this.order = order;
      this.bcService.set('@orderDetails', `Order #${this.orderId} - ${order.orderStatus}`);
    });
  }

}
