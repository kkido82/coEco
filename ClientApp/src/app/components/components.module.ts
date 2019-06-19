import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MomentModule } from 'ngx-moment';

import { HeaderComponent } from './header/header.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AppLayoutComponent } from './app-layout/app-layout.component';
import { LendingFromComponent } from './lending-from/lending-from.component';
import { LendingToComponent } from './lending-to/lending-to.component';
import { ManageItemsComponent } from './manage-items/manage-items.component';
import { MainNavbarComponent } from './main-navbar/main-navbar.component';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ReadOnlyItemComponent } from './read-only-item/read-only-item.component';
import { PreviousRequestsComponent } from './previous-requests/previous-requests.component';
import { SearchResultsComponent } from './search-results/search-results.component';
import { SearchResultItemComponent } from './search-result-item/search-result-item.component';
import { TimesPipe } from '../pipes/loop-element.pipe';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { StatusNamePipe } from '../pipes/status-text.pipe';
import { BalanceInfoComponent } from './balance-info/balance-info.component';
import { MessagesComponent } from './messages/messages.component';
import { ProblemReportComponent } from './problem-report/problem-report.component';
import { RateItemComponent } from './rate-item/rate-item.component';
import { StarRatingComponent } from './star-rating/star-rating.component';
import { TextsModule } from '../texts/texts.module';
import { LoaderComponent } from './loader/loader.component';
import { ItemIconPipe } from '../pipes/item-icon.pipe';
import { StatusCircleComponent } from './status-circle/status-circle.component';
import { ManageItemComponent } from './manage-item/manage-item.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  imports: [
    CommonModule,
    MomentModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    TextsModule,
    BrowserAnimationsModule
  ],
  declarations: [
    HeaderComponent,
    PageNotFoundComponent,
    AppLayoutComponent,
    LendingFromComponent,
    LendingToComponent,
    ManageItemsComponent,
    MainNavbarComponent,
    SearchBarComponent,
    ReadOnlyItemComponent,
    PreviousRequestsComponent,
    SearchResultsComponent,
    SearchResultItemComponent,
    TimesPipe,
    ItemIconPipe,
    StatusNamePipe,
    OrderDetailsComponent,
    BalanceInfoComponent,
    MessagesComponent,
    ProblemReportComponent,
    RateItemComponent,
    StarRatingComponent,
    LoaderComponent,
    StatusCircleComponent,
    ManageItemComponent
  ],
  exports: [HeaderComponent, BalanceInfoComponent, LoaderComponent],
  entryComponents: [PageNotFoundComponent]
})
export class ComponentsModule {}
