import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ItemSearchResult } from 'src/model';

@Component({
  selector: 'app-search-result-item',
  templateUrl: './search-result-item.component.html',
  styles: []
})
export class SearchResultItemComponent  {
  @Input() data: ItemSearchResult;
  @Output() select = new EventEmitter<ItemSearchResult>();
}
