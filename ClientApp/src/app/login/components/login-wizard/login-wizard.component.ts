import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login-wizard',
  templateUrl: './login-wizard.component.html',
  styleUrls: []
})
export class LoginWizardComponent {
  tzSent = false;
  tz = '';
  constructor(private router: Router, private authService: AuthService) {}

  async submitTz(tz: string) {
    await this.authService.askForOtp(tz);
    this.tz = tz;
    this.tzSent = true;
  }

  async submitCode(code: string) {
    await this.authService.login(this.tz, code);
    this.router.navigate(['/orders/from']);
  }

  sendTzAgain() {
    this.submitTz(this.tz);
  }
}
