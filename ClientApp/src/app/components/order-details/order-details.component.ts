import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import {
  OrderDetails,
  OrderStatusId,
  OrderOverview
} from 'src/model';
import { OrderService } from 'src/app/services/order.service';

interface ActionButton {
  visible: boolean;
  className?: string;
  action?: any;
  appText?: string;
}

const getActionName = (action = OrderStatusId.New) => {
  switch (action) {
    case OrderStatusId.New:
      return 'create';
    case OrderStatusId.Approved:
      return 'approve';
    case OrderStatusId.Confirmed:
      return 'confirm';
    case OrderStatusId.Active:
      return 'activate';
    case OrderStatusId.Completed:
      return 'complete';
    case OrderStatusId.CanceledByLendingUnit:
    case OrderStatusId.CanceledByRequestingUnit:
      return 'cancel';
    default:
      return '';
  }
};

const isCancelAction = (action: number) => action < 0;

const toActionButtons = (action = 0): ActionButton => {
  const actionName = getActionName(action);

  return {
    visible: true,
    appText: 'order-btn-' + actionName,
    className: isCancelAction(action) ? 'btn--light' : 'btn--dark',
    action: action
  };
};

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styles: []
})
export class OrderDetailsComponent implements OnInit {
  form: FormGroup;
  btns: ActionButton[] = [];
  isNewOrder = false;
  showDetails = true;
  @Input() details: OrderDetails;
  @Output() close = new EventEmitter();

  constructor(
    private formBuilder: FormBuilder,
    private orderService: OrderService
  ) {}

  ngOnInit() {
    this.form = this.formBuilder.group({
      remarks: [{ value: '', disabled: this.details.id !== 0 }]
    });
    if (this.details.id === 0) {
      this.isNewOrder = true;
    } else {
      this.isNewOrder = false;
    }
    this.btns = this.details.actions.map(toActionButtons);
    // console.log(this.details);
  }

  async send(action: number) {
    let res: OrderOverview = null;
    if (action === OrderStatusId.New) {
      res = await this.orderService.createOrder({
        itemId: this.details.itemId,
        remarks: this.form.value.remarks,
        fromUnitId: this.details.fromUnitId
      });
    } else {
      res = await this.orderService.updateStatus({
        id: this.details.id,
        orderStatusId: action
      });
    }

    this.closeDetails(res);
  }

  closeDetails(res: OrderOverview = null) {
    this.close.emit(res);
  }

  orderName(id) {
    if (!this.isNewOrder) {
      return `הזמנה מס' ${id}`;
    } else {
      return `הזמנה חדשה`;
    }
  }

  hideProblemReport() {
    this.showDetails = true;
  }

  showProblemReport() {
    this.showDetails = false;
  }

  closeRating() {

  }
}
