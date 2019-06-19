import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ItemSearchResult } from 'src/model';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styles: []
})
export class SearchResultsComponent {
  @Input() items: ItemSearchResult[] = [];
  @Output() select = new EventEmitter<ItemSearchResult>();

  openOrderDetails(item: ItemSearchResult) {
    console.log('openOrderDetails', item)
  }
}
