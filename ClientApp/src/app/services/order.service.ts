import { apiGet, apiPost } from '../helpers/api';
import {
  OrderDetails,
  CreateOrder,
  UpdateOrderStatus,
  OrderOverview,
  ReportProblemRequest
} from 'src/model';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

const BASE_URL = '/api/orders/';
const LENDING_TO_URL = BASE_URL + 'lending-to/';
const LENDING_FROM_URL = BASE_URL + 'lending-from/';
const NEW_ORDER_URL = BASE_URL + 'new/';
const UPDATE_STATUS_URL = BASE_URL + 'status/';
const REPORT_PROBLEM_URL = BASE_URL + 'problems/';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private subNewOrders = new Subject<OrderOverview>();
  private subUpdatedOrders = new Subject<OrderOverview>();

  get newOrders() {
    return this.subNewOrders.asObservable();
  }

  get updatedOrders() {
    return this.subUpdatedOrders.asObservable();
  }

  get(id: any) {
    return apiGet<OrderDetails>(BASE_URL + id);
  }

  lendingFrom() {
    return apiGet<OrderOverview[]>(LENDING_FROM_URL);
  }

  lendingTo() {
    return apiGet<OrderOverview[]>(LENDING_TO_URL);
  }

  getNew(unitItemId: number) {
    return apiGet<OrderDetails>(NEW_ORDER_URL + unitItemId);
  }

  async createOrder(cmd: CreateOrder): Promise<OrderOverview> {
    const order = await apiPost<OrderOverview>(BASE_URL, cmd);
    this.subNewOrders.next(order);
    return order;
  }

  async updateStatus(cmd: UpdateOrderStatus) {
    const order = await apiPost<OrderOverview>(UPDATE_STATUS_URL, cmd);
    this.subUpdatedOrders.next(order);
    return order;
  }

  reportProblem(req: ReportProblemRequest) {
    return apiPost<any>(REPORT_PROBLEM_URL, req);
  }
}
