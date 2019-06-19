import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

const routesLink = {
  '/orders/from': 1,
  '/orders/to': 2,
  '/items': 3
};

@Component({
  selector: 'app-main-navbar',
  templateUrl: './main-navbar.component.html',
  styles: []
})
export class MainNavbarComponent implements OnInit {
  activeLink = 1;
  constructor(private router: Router) {}

  ngOnInit() {
    this.updateFromUrl(this.router.url);
    this.router.events.subscribe(evt => {
      if (evt instanceof NavigationEnd) {
        this.updateFromUrl(evt.urlAfterRedirects);
      }
    });
  }

  setActiveLink(num) {
    this.activeLink = num;
  }

  updateFromUrl(url: string) {
    const link = routesLink[url];
    if (link) {
      this.setActiveLink(link);
    }
  }
}
