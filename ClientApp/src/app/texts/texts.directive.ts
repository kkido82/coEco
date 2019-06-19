import { Directive, ElementRef, Input, OnInit } from '@angular/core';
import { TextsService } from './texts.service';

@Directive({
  selector: '[appText]'
})
export class TextDirective implements OnInit {
  constructor(private service: TextsService, private el: ElementRef) {}
  @Input('appText') value = '';

  ngOnInit(): void {
    this.setText();
    this.service.changes.subscribe(() => this.setText());
  }

  setText() {
    this.el.nativeElement.innerHTML = this.service.get(this.value) || this.value;
  }
}
