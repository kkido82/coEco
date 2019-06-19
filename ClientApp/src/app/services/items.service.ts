import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Item, ItemSearchResult, OrderDetails } from 'src/model';
import { apiPost, apiGet } from '../helpers/api';

const ITEMS_URL = '/api/items';
const SEARCH_URL = '/api/items/search';

const getItemIds = (items: Item[], term: string) => {
  return items.filter(i => i.itemName.includes(term)).map(i => i.itemId);
};

const search = (ids = []) => apiPost<ItemSearchResult[]>(SEARCH_URL, { ids });

@Injectable({
  providedIn: 'root'
})
export class ItemsService {
  constructor() {}

  _items: Item[] = [];

  async load() {
    this._items = await apiGet<Item[]>(ITEMS_URL);
  }

  getItems() {
    return Promise.resolve(this._items);
  }

  get items() {
    return this._items;
  }

  searchItem(term: string) {
    const itemIds = getItemIds(this._items, term);
    if (itemIds.length === 0) {
      return Promise.resolve([]);
    }
    return search(itemIds);
  }
}
