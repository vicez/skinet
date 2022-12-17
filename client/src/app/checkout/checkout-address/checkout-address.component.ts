import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMetthod';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  

  constructor(private accountService: AccountService, private toasterService: ToastrService) { }

  ngOnInit(): void {
  }

  saveUserAddress(){
    this.accountService.updateUserAddress(this.checkoutForm.get("addressForm").value).subscribe(() => {
      this.toasterService.success("Address Saved");
    }, error => {
      this.toasterService.error(error.message);
      console.log(error);
    })
  }

}
