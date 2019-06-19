import { Component, OnInit, Input } from '@angular/core';
import { OrderService } from 'src/app/services/order.service';
import { OrderDetails, OrderOverview } from 'src/model';
import { AuthService } from 'src/app/services/auth.service';
import { merge, Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-lending-to',
  templateUrl: './lending-to.component.html',
  styleUrls: []
})
export class LendingToComponent implements OnInit {
  @Input() items: OrderOverview[] = [];
  order: OrderDetails = null;
  orderDetailsOpened = false;
  userUnitId = 0;
  subs: Subscription[] = [];
  constructor(
    private orderService: OrderService,
    authService: AuthService
  ) {
    this.userUnitId = authService.user.unitId;
  }

  ngOnInit() {
    console.log('LendingToComponent');

    this.orderService.lendingTo().then(res => {
      this.items = res;
    });

    const sub = merge(
      this.orderService.newOrders,
      this.orderService.updatedOrders
    )
    .pipe(filter(o => o.fromUnitId === this.userUnitId))
    .subscribe(this.updateOrder.bind(this));
    this.subs.push(sub);
  }

  async showDetails(order: OrderOverview) {
    this.order = await this.orderService.get(order.id);
    this.orderDetailsOpened = true;
  }

  closeDetails(order: OrderOverview) {
    this.order = null;
    this.orderDetailsOpened = false;
  }

  updateOrder(order:OrderOverview) {
    const cur = this.items.filter(a => a.id == order.id)[0];
    if (cur) {
      Object.assign(cur, order);
    }
  }
}
