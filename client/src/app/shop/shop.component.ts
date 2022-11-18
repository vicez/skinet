import { ThisReceiver } from '@angular/compiler';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IProductType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild("search", {static: true}) searchElRef: ElementRef;
  products: IProduct[];
  brands: IBrand[];
  productTypes: IProductType[];
  totalCount: number;
  shopParams: ShopParams = new ShopParams();
  sortOptions = [
    {name: "Alphabetical", value: "name"},
    {name: "Price: Low to High", value: "priceAsc"},
    {name: "Price: High to Low", value: "priceDesc"},
  ];

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getProductBrands();
    this.getProductTypes();
  }

  getProducts(){
    return this.shopService.getProducts(this.shopParams).subscribe(res => {
      this.products = res.data;
      this.shopParams.pageNumber = res.pageIndex;
      this.shopParams.pageSize = res.pageSize;
      this.totalCount = res.count;
    }, error => {
      console.log(error);
    });
  }
  getProductBrands(){
    return this.shopService.getProductBrands().subscribe(res => {
      this.brands = [{id: 0, name: "All"}, ...res];
    }, error => {
      console.log(error);
    });
  }
  getProductTypes(){
    return this.shopService.getProductTypes().subscribe(res => {
      this.productTypes = [{id: 0, name: "All"}, ...res];
    }, error => {
      console.log(error);
    });
  }

  onBrandIdSelected(brandId: number){
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
  onTypeIdSelected(typeId: number){
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
  onSortSelected(sort: string){
    this.shopParams.sort = sort;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
  onPageChanged(event: any){
    if(this.shopParams.pageNumber != event){
      this.shopParams.pageNumber = event;
      this.getProducts();
    }
  }
  onSearch(){
    this.shopParams.search = this.searchElRef.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
  onReset(){
    this.searchElRef.nativeElement.value = "";
    this.shopParams = new ShopParams();
    this.getProducts();
  }

}
