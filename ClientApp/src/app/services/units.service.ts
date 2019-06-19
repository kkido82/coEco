import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Balance, UnitItem, UnitItemBase } from 'src/model';
import { apiGet, apiPost } from 'src/app/helpers/api';

const BASE_URL = '/api/';
const BALANCE_URL = '/api/units/balance';

@Injectable({
  providedIn: 'root'
})
export class UnitsService {
  constructor(private http: HttpClient) {}
  balance() {
    return apiGet<Balance>(BALANCE_URL);
  }

  items() {
    return apiGet<UnitItem[]>(BASE_URL + 'units/items');
  }

  manageItem(model: UnitItemBase) {
    return apiPost<any>(BASE_URL + 'units/items', model);
  }
}
