import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Location } from '@angular/common';
import { OrderDetails, ReportProblemRequest } from 'src/model';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-problem-report',
  templateUrl: './problem-report.component.html',
  styles: []
})
export class ProblemReportComponent implements OnInit {
  @Input() order: OrderDetails;
  @Output() close = new EventEmitter();

  form: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    private orderService: OrderService
  ) {}

  ngOnInit() {
    this.form = this.formBuilder.group({
      description: ['', Validators.required]
    });
  }

  async submit() {
    console.log('submit')
    if (this.form.invalid) { return; }
    const description = this.form.value.description;
    const req: ReportProblemRequest = {
      orderId: this.order.id,
      description: description
    };
    await this.orderService.reportProblem(req);
    this.close.emit();
  }
}
