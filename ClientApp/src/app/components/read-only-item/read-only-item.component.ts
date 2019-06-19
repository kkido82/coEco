import { Component, OnInit, Input } from '@angular/core';
import { ItemsService } from 'src/app/services/items.service';
import { OrderOverview, Item } from 'src/model';

@Component({
  selector: 'app-read-only-item',
  templateUrl: './read-only-item.component.html',
  styles: []
})
export class ReadOnlyItemComponent implements OnInit {
  @Input() data: OrderOverview;
  @Input() isTo = false;
  allItems: Item[];
  unitName = '';
  ngOnInit(): void {
    this.unitName = this.isTo ? this.data.toUnit : this.data.fromUnit;
  }
}
