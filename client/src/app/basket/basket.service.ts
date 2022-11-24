import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { timeEnd } from 'console';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotal } from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseURl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotal>(null);
  basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }

  getBasket(id: string){
    return this.http.get(this.baseURl + "Basket?id=" + id). pipe(
      map((basket: IBasket) => {
        this.basketSource.next(basket);
        this.calculateTotals();
      })
    )
  }

  setBasket(basket: IBasket){
    return this.http.post(this.baseURl + "Basket", basket).subscribe((response: IBasket) => {
      this.basketSource.next(response);
      this.calculateTotals();
    }, error => {
      console.log(error);
    });
  }

  deleteBasket(id: string){
    return this.http.delete(this.baseURl + "Basket?id=" + id). subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem("basket_id");
    }, error => {
      console.log(error);
    });
  }

  getCurrentBasketValue(): IBasket{
    return this.basketSource.value;
  }

  addItemToBasket(product: IProduct, quantity = 1){
    const itemToAdd: IBasketItem = this.mapProductToBasketItem(product,  quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items  = this.updateORAddItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  updateORAddItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(item => item.id == itemToAdd.id);
    if(index === -1){
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    else{
      items[index].quantity += quantity;
    }
    return items;
  }

  incrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if(basket.items[foundItemIndex].quantity > 1){
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    }
    else{
      this.removeItemFromBasket(item);
    }
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if(basket.items.some(x => x.id === item.id)){
      basket.items = basket.items.filter(x => x.id !== item.id);
      if(basket.items.length > 0){
        this.setBasket(basket);
      }
      else{
        this.deleteBasket(basket.id);
      }
    }
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = shipping + subtotal;
    this.basketTotalSource.next({shipping, subtotal, total});
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem("basket_id", basket.id);
    return basket;
  }

  private mapProductToBasketItem(product: IProduct, quantity: number): IBasketItem {
    return {
      id: product.id,
      productName: product.name,
      price: product.price,
      pictureUrl: product.pictureUrl,
      quantity,
      brand: product.productBrand,
      type: product.productType
    }
  }


}
