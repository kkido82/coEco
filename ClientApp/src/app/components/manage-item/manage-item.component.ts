import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { fade } from 'src/assets/animations';
import { Item, UnitItemBase, UnitItem } from 'src/model';

@Component({
  selector: 'app-manage-item',
  templateUrl: './manage-item.component.html',
  styles: [],
  animations: [fade]
})
export class ManageItemComponent implements OnInit {
  @Input() item: UnitItem;
  @Input() permissions;
  @Output() itemChange = new EventEmitter<UnitItemBase>();
  form: FormGroup;
  showTextBox = false;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.form = this.formBuilder.group({
      quantity: [this.item.qty, [Validators.required, Validators.min(1)]],
      description: [this.item.description]
    });
  }

  openTextBox() {
    this.showTextBox = true;
  }

  closeTextBox() {
    this.showTextBox = false;
  }

  submitItem() {
    if (!this.form.valid) {
      return;
    }
    const form = this.form.value;

    const unitItem: UnitItemBase = {
      itemId: this.item.itemId,
      qty: form.quantity,
      description: form.description
    };
    this.itemChange.emit(unitItem);
  }
}
