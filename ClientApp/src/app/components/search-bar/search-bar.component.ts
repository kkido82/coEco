import { TextsService } from 'src/app/texts/texts.service';
import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  Output,
  EventEmitter
} from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styles: []
})
export class SearchBarComponent implements OnInit {
  @ViewChild('input') searchInput: ElementRef<HTMLInputElement>;
  placeholder;
  @Output() submitSearch = new EventEmitter();
  form: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    private textsService: TextsService
  ) {}

  ngOnInit() {
    this.form = this.formBuilder.group({
      inputValue: ['']
    });

    this.placeholder = this.textsService.get('search_here');
  }

  clearInput() {
    this.form.get('inputValue').setValue('');
    this.submitSearch.emit('');
  }

  submit(value: string) {
    this.submitSearch.emit(value);
  }

  reset() {
    this.searchInput.nativeElement.value = '';
  }
}
