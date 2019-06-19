import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-star-rating',
  templateUrl: './star-rating.component.html',
  styles: []
})
export class StarRatingComponent implements OnInit {
  @Input() startIndex;
  @Output() rateChanged = new EventEmitter();
  form: FormGroup;
  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.form = this.formBuilder.group({
      rating: ''
    });
  }

  rate() {
    setTimeout(() => {
      const currentRate = this.form.value.rating;
      this.rateChanged.emit(currentRate);
    });
  }
}
