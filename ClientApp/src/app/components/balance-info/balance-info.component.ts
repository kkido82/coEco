import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { UnitsService } from 'src/app/services/units.service';
import { Balance } from 'src/model';

@Component({
  selector: 'app-balance-info',
  templateUrl: './balance-info.component.html',
  styles: []
})
export class BalanceInfoComponent implements OnInit {
  @Output() close = new EventEmitter();
  @Output() setBadgeCount = new EventEmitter;
  data: Balance;
  constructor(private unitsService: UnitsService) {}

  async ngOnInit() {

    this.data = await this.unitsService.balance();
    this.setBadgeCount.emit(this.data.currentBalance);

  }

  closeBalanceDetails() {
    this.close.emit();
  }
}
