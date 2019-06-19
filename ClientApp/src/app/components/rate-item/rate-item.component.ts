import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-rate-item',
  templateUrl: './rate-item.component.html',
  styles: []
})
export class RateItemComponent implements OnInit {
  startIndex1 = '0';
  startIndex2 = '4';
  @Input() order = {
    itemId: 2,
    itemName: 'מיטה צרפתית',
    fromUnit: 'גדוד 789',
    statusId: 1,
    statusName: 'אושרה - ממתין להעברה',
    orderDate: '30.12.19'
  };
  @Output() close = new EventEmitter();

  show = true;

  constructor() {}

  ngOnInit() {}

  correspondenceRate(rating) {
    console.log('in parent', rating);
  }

  satisfactionRate(rating) {
    // console.log('genral rate');
    console.log('in parent', rating);

  }

  doClose() {
    this.close.emit();
  }
}
