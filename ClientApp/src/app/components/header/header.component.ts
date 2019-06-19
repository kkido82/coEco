import { AuthService } from './../../services/auth.service';
import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styles: []
})
export class HeaderComponent implements OnInit {
  badgesCount = {
    messages: '',
    fixes: ''
  };
  showBalance = false;
  showMessages = false;
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {}

  disconnect() {
    this.authService.logout().then(_ => {
      this.router.navigate(['login']);
    });
  }

  showBalanceInfo() {
    this.showBalance = !this.showBalance;
  }

  toggleMessages() {
    this.showMessages = !this.showMessages;
  }

  setBalanceCount(count) {
    this.badgesCount.fixes = count;
  }

  setMsgCount(count) {
    this.badgesCount.messages = count;

  }
}
