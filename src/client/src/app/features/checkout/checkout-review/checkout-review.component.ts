import { CurrencyPipe } from '@angular/common';
import { Component, inject, Input } from '@angular/core';
import { CartService } from '../../../core/services/cart.service';
import { ConfirmationToken } from '@stripe/stripe-js';
import { AdressPipe } from "../../../shared/pipes/address.pipe";
import { PaymentCardPipe } from '../../../shared/pipes/payment-card.pipe';

@Component({
  selector: 'app-checkout-review',
  imports: [
    CurrencyPipe,
    AdressPipe,
    PaymentCardPipe
],
  templateUrl: './checkout-review.component.html',
  styleUrl: './checkout-review.component.scss'
})
export class CheckoutReviewComponent {
  cartService = inject(CartService);
  @Input() confirmationToken?: ConfirmationToken

}
