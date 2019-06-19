import { Component, OnInit, ViewChild } from '@angular/core';
import { UnitsService } from 'src/app/services/units.service';
import { UnitItem, UserPermissions, UnitItemBase, Item } from 'src/model';
import { AuthService } from 'src/app/services/auth.service';
import { ItemsService } from 'src/app/services/items.service';
import { SearchBarComponent } from '../search-bar/search-bar.component';

interface UnitItemVM extends UnitItem {
  isNew: boolean;
}

const mapToUnitItem = (item: Item): UnitItem => ({
  description: '',
  itemId: item.itemId,
  qty: 0,
  itemName: item.itemName
});

const filterNew = (currentItems: UnitItem[], allItems: Item[]) => {
  const existingIds = currentItems.map(a => a.itemId);
  return allItems.filter(i => !existingIds.includes(i.itemId));
};

const mapToViewModels = (currentItems: UnitItem[], allItems: Item[]) => {
  const current = currentItems.map(
    item => <UnitItemVM>{ ...item, isNew: false }
  );
  const news = filterNew(currentItems, allItems).map(
    item =>
      <UnitItemVM>{
        description: '',
        isNew: true,
        itemId: item.itemId,
        itemName: item.itemName,
        qty: 0
      }
  );

  return current.concat(news);
};

@Component({
  selector: 'app-manage-items',
  templateUrl: './manage-items.component.html',
  styleUrls: []
})
export class ManageItemsComponent implements OnInit {
  @ViewChild('searchBar') searchBar: SearchBarComponent;
  allItems: UnitItemVM[] = [];
  itemsToShow: UnitItemVM[] = [];
  permissions: UserPermissions;
  term = '';
  isAdd = false;

  constructor(
    private unitService: UnitsService,
    private itemsService: ItemsService,
    private authService: AuthService
  ) {}

  async ngOnInit() {
    const user = await this.authService.getUser();
    this.permissions = user.permissions;
    const unitItems = await this.unitService.items();
    this.allItems = mapToViewModels(unitItems, this.itemsService.items);
    this.filter();
  }

  searchItems(term) {
    // implement later - search in all items by id only items that's not in the unit items
    this.term = term;
    this.isAdd = !!this.term;
    this.filter();
  }

  filter() {
    this.itemsToShow = this.allItems.filter(vm => {
      if (this.isAdd) {
        return vm.isNew && vm.itemName.includes(this.term);
      }
      return !vm.isNew;
    });
  }

  submitItem(unitItem: UnitItemBase) {
    console.log('subimtii 4234');
  }

  async onItemChange(update: UnitItemBase) {
    if (!this.isAdd) {
      console.log('update item', update);
      await this.unitService.manageItem(update);
      const vm = this.allItems.filter(a => a.itemId === update.itemId)[0];
      Object.assign(vm, update);
    } else {
      const vm = this.allItems.filter(a => a.itemId === update.itemId)[0];
      Object.assign(vm, update);
    }
  }

  async onAdd(item: UnitItemBase) {
    if (!this.isAdd) {
      return;
    }
    //item.isNew = false;
    console.log('add item', item);
    await this.unitService.manageItem(item);
    const vm = this.allItems.filter(a => a.itemId === item.itemId)[0];
    vm.isNew = false;
    this.isAdd = false;
    this.searchBar.clearInput();
    this.filter();
  }
}
