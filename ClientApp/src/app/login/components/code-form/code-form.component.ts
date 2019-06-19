import { Placeholder } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TextsService } from 'src/app/texts/texts.service';

@Component({
  selector: 'app-code-form',
  templateUrl: './code-form.component.html',
  styleUrls: []
})
export class CodeFormComponent {
  form: FormGroup;
  formSubmitted: boolean;
  showSentAgainMsg = false;
  sentAgainMsg: any = '';
  errors = {
    required: '',
    minlength: '',
    maxlength: '',
    pattern: ''
  };
  placeholder;
  @Output() sendTzAgain = new EventEmitter();
  @Output() submitCode = new EventEmitter<string>();

  constructor(
    private formBuilder: FormBuilder,
    private textService: TextsService
  ) {}

  // tslint:disable-next-line:use-life-cycle-interface
  ngOnInit() {
    this.form = this.formBuilder.group({
      code: [
        '111111',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(6),
          Validators.pattern('^[0-9]*$')
        ]
      ]
    });
    const errors = this.errors;
    errors.required = this.textService.get('required_error');
    errors.minlength = this.textService.get('minlength_error');
    errors.maxlength = this.textService.get('maxlength_error');
    errors.pattern = this.textService.get('pattern_error');
    this.placeholder = this.textService.get('type_code');
  }

  getErrorMessage() {
    const ctr = this.form.controls.code;
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

  submit() {
    this.formSubmitted = true;
    if (!this.form.valid) {
      return;
    } else {
      this.submitCode.emit(this.form.value.code);
    }
  }
}
