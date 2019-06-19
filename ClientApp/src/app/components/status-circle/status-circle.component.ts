import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-status-circle',
  template: `
    <span
      class="coloredCircle"
      [ngClass]="{
        'coloredCircle--pendind': status === 1 || status === 2,
        'coloredCircle--approved': status === 3,
        'coloredCircle--active': status === 4,
        'coloredCircle--finished': status === 5,
        'coloredCircle--canceled': status === 6 || status === 7
      }"
    >
    </span>
  `
})
export class StatusCircleComponent {
  // @Input() status = -100; //// asaf - not sure why -100?
  @Input() status ;
}
