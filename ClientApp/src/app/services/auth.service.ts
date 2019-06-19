import { Injectable } from '@angular/core';
import { User } from 'src/model';
import { apiPost, apiGet } from '../helpers/api';
import { Subject, Observable } from 'rxjs';

const BaseUrl = '/api/account/';
const SendTzUrl = BaseUrl + 'sendtz';
const LoginUrl = BaseUrl + 'login';
const UserUser = BaseUrl + 'user';

const sendTz = (tz: string) => apiPost(SendTzUrl, { tz });

const login = (tz: string, code: string) => apiPost(LoginUrl, { tz, code });

const getUser = () => apiGet<User>(UserUser);
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  _logins = new Subject<User>();
  user: User = null;
  async loadUser() {
    try {
      const user = await getUser();
      this._logins.next(user);
      this.user = user;
    } catch (a) {
      return 0;
    }
  }
  getUser() {
    return getUser();
  }

  isAuthenticated() {
    return this.user != null;
  }

  askForOtp(tz: string) {
    return sendTz(tz);
  }

  async login(tz: string, code: string) {
    await login(tz, code);
    this.user = await getUser();
    console.log(this.user);
  }

  logout() {
    // just for dev
    this.user = null;
    return new Promise((resolve, reject) => {
      resolve();
    });
  }

  get logins(): Observable<User> {
    return this._logins.asObservable();
  }
}
