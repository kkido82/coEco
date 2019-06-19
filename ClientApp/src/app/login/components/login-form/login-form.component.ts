import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TextsService } from 'src/app/texts/texts.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: []
})
export class LoginFormComponent implements OnInit {
  form: FormGroup;
  formSubmitted: boolean;
  errors = {
    required: '',
    minlength: '',
    maxlength: '',
    pattern: ''
  };
  placeholder;
  @Output() submitTz = new EventEmitter<string>();

  constructor(
    private formBuilder: FormBuilder,
    private textService: TextsService
  ) {}

  ngOnInit() {
    this.form = this.formBuilder.group({
      tz: [
        '777777777',
        [
          Validators.required,
          Validators.minLength(9),
          Validators.maxLength(9),
          Validators.pattern('^[0-9]*$')
        ]
      ]
    });
    const errors = this.errors;
    errors.required = this.textService.get('required_error');
    errors.minlength = this.textService.get('minlength_error');
    errors.maxlength = this.textService.get('maxlength_error');
    errors.pattern = this.textService.get('pattern_error');
    this.placeholder = this.textService.get('type_tz');
  }

  getErrorMessage() {
    const ctr = this.form.controls.tz;
    const errors = this.errors;
    if (ctr.hasError('required')) {
      return errors.required;
    } else if (ctr.hasError('minlength')) {
      return errors.minlength;
    } else if (ctr.hasError('maxlength')) {
      return errors.maxlength;
    } else if (ctr.hasError('pattern')) {
      return errors.pattern;
    }
  }

  doSubmit() {
    this.formSubmitted = true;
    if (!this.form.valid) {
      return;
    } else {
      this.submitTz.emit(this.form.value.tz);
    }
  }
}
