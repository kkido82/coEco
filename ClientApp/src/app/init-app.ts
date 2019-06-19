import { Injectable } from '@angular/core';
import { AuthService } from './services/auth.service';
import { ItemsService } from './services/items.service';

@Injectable({
  providedIn: 'root'
})
export class InitApp {
  constructor(private auth: AuthService, private items: ItemsService) {}

  async init() {
    await this.auth.logins.subscribe(() => {
      this.items.load();
    });
    try {
      await this.auth.loadUser();

    } catch (error) {

    }
  }
}
