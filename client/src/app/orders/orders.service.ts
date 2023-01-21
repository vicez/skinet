import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IBasket, IBasketItem, IBasketTotal } from '../shared/models/basket';
import { IOrder, IOrderItem } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseURl = environment.apiUrl;
  private orderSource = new BehaviorSubject<IBasket>(null);
  order$ = this.orderSource.asObservable();
  private orderTotalSource = new BehaviorSubject<IBasketTotal>(null);
  orderTotal$ = this.orderTotalSource.asObservable();

  constructor(private http: HttpClient) { }

  getOrders(){
    return this.http.get(this.baseURl + "Orders").pipe(map((order: IOrder[]) => {
      return order;
    }));
  }

  getSingleOrder(id){
    return this.http.get(this.baseURl + "Orders/" + id).pipe(map((order: IOrder) => {
      let total: IBasketTotal = {
        shipping: order.shippingPrice,
        subtotal: order.subtotal,
        total: order.total
      }
      let basketItem: IBasketItem[] = this.convertToBasketItem(order.orderItems);
      let basket: IBasket = {
        id: "",
        items: basketItem
      }
      this.orderTotalSource.next(total);
      this.orderSource.next(basket);
      return order;
    }));
  }

  convertToBasketItem(orderItem: IOrderItem[]): IBasketItem[]{
    let basketItem: IBasketItem[] = [];
    orderItem.forEach(each => {
      basketItem.push({
        id: each.productId,
        productName: each.productName,
        price: each.price,
        quantity: each.quantity,
        pictureUrl: each.pictureUrl,
        brand: "",
        type: ""
      });
    });
    return basketItem
  }


}

