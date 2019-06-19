import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { ItemsService } from 'src/app/services/items.service';
import { OrderService } from 'src/app/services/order.service';
import { ItemSearchResult, OrderOverview, OrderDetails } from 'src/model';
import { SearchBarComponent } from '../search-bar/search-bar.component';
import { merge, Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-lending-from',
  templateUrl: './lending-from.component.html',
  styleUrls: []
})
export class LendingFromComponent implements OnInit, OnDestroy {
  showSearchResults = false;
  prevRequests: OrderOverview[] = [];
  searchResults: ItemSearchResult[] = [];
  orderDetailsOpened = false;
  order: OrderDetails = null;
  subs: Subscription[] = [];
  userUnitId = 0;
  @ViewChild('searchBar') searchBar: SearchBarComponent;

  constructor(
    private ordersService: OrderService,
    private itemsService: ItemsService,
    private authService: AuthService
  ) {
    this.userUnitId = this.authService.user.unitId;
  }

  ngOnInit() {
    this.ordersService.lendingFrom().then(res => {
      this.prevRequests = res;
    });
    const sub = merge(
      this.ordersService.newOrders,
      this.ordersService.updatedOrders
    )
    .pipe(filter(o => o.toUnitId === this.userUnitId))
    .subscribe(this.addOrUpdateOrder.bind(this));
    this.subs.push(sub);
  }

  ngOnDestroy() {
    try {
      for (const sub of this.subs) {
        sub.unsubscribe();
      }
    } catch (error) {
      console.error(error);
    }
  }

  listText() {
    return this.showSearchResults ? 'תוצאות חיפוש' : 'בקשות קודמות';
  }

  searchItems(term: string) {
    if (!term) {
      this.showSearchResults = false;
      return;
    }
    this.itemsService.searchItem(term).then(res => {
      this.searchResults = res;
      this.showSearchResults = true;
    });
  }

  async updateOrder(orderOverview: OrderOverview) {
    this.order = await this.ordersService.get(orderOverview.id);
    this.orderDetailsOpened = true;
  }

  async openNewOrder(item: ItemSearchResult) {
    this.order = await this.ordersService.getNew(item.itemUnitId);
    this.orderDetailsOpened = true;
  }

  closeDetails() {
    this.order = null;
    this.orderDetailsOpened = false;
  }

  addOrUpdateOrder(order: OrderOverview) {
    const cur = this.prevRequests.filter(a => a.id == order.id)[0];
    if (cur) {
      Object.assign(cur, order);
    } else {
      this.prevRequests = [order].concat(this.prevRequests);
      this.searchBar.reset();
      this.showSearchResults = false;
    }
  }

  selectItem(item: ItemSearchResult) {
    // this.showSearchResults = false;
    // this.searchBar.reset();
    console.log('selected item', item);
  }
}
