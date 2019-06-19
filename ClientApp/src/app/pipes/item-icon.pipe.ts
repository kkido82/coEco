import { Pipe, PipeTransform, OnInit } from '@angular/core';
import { ItemsService } from '../services/items.service';
import { Item } from 'src/model';

const DEFAULT_ICON = '';

const getIconSrc = (items: Item[], id: number) => {
  const currentItem = items.filter(i => i.itemId === id);
  if (currentItem) {
    return `assets/imgs/${currentItem[0].iconName}.png`;
  }

  return DEFAULT_ICON;
};

@Pipe({
  name: 'itemIcon'
})
export class ItemIconPipe implements PipeTransform {
  constructor(private itemsService: ItemsService) {}

  transform(id: number) {
    return getIconSrc(this.itemsService.items, id);
  }
}
