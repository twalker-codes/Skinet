import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';

@Pipe({
  name: 'address',
  standalone: true
})
export class AdressPipe implements PipeTransform {

  transform(value?: ConfirmationToken['shipping'], ...args: unknown[]): unknown {
    if(value?.address && value.name){
      const {line1, line2, city, state, country, postal_code} = value.address;
      return `${value.name}, ${line1}${line2 ? ', ' + line2 : ''}, ${city}, 
        ${state}, ${country}, ${postal_code}`;
    } else {
      return 'Unknown address'; 
    }
  }
}
