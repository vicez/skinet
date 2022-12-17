import { IAddress } from "./address"
import { IDeliveryMethod } from "./deliveryMetthod"

export interface IOrderToCreate {
    basketId: string
    deliveryMethodId: number
    shipToAddress: IAddress
  }

  export interface IOrder {
    id: number
    buyerEmail: string
    orderDate: string
    shipToAddress: IAddress
    deliveryMethod: IDeliveryMethod
    orderItems: IOrderItem[]
    subtotal: number
    orderStatus: number
    paymentIntentId: string
  }

  
  export interface IOrderItem {
    id: number
    productItemOrdered: ProductItemOrdered
    quantity: number
    price: number
  }
  
  export interface ProductItemOrdered {
    productItemId: number
    productName: string
    pictureUrl: string
  }
  
  