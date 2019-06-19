import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { OrderService } from 'src/app/services/order.service';
import { OrderOverview, OrderDetails } from 'src/model';

@Component({
  selector: 'app-previous-requests',
  templateUrl: './previous-requests.component.html',
  styles: []
})
export class PreviousRequestsComponent {
  @Input() requests: OrderOverview[] = [];
  @Output() select = new EventEmitter<OrderOverview>();
  orderDetailsOpened = false;
  order: OrderDetails = null;
  constructor(private orderService: OrderService) {}

  // async showDetails(order: OrderOverview) {
  //   this.order = await this.orderService.get(order.id);
  //   this.orderDetailsOpened = true;
  // }

  // closeDetails(order: OrderOverview) {
  //   if (order) {
  //     const cur = this.requests.filter(a => a.id == order.id)[0];
  //     if (cur) {
  //       Object.assign(cur, order);
  //     }
  //   }
  //   this.order = null;
  //   this.orderDetailsOpened = false;
  // }
}
