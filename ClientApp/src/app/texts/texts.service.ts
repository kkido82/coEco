import { Injectable } from '@angular/core';
import { Observable, of, Subject } from 'rxjs';

const handleResponse = (res: Response) => {
  if (res.status !== 200) {
    throw res;
  }
  return res.json();
};

const langUrl = (lang: string) => `assets/texts/${lang}.json`;

const apiGet = (url: string) => fetch(url).then(handleResponse);

const getLang = (lang: string) => apiGet(langUrl(lang));

@Injectable()
export class TextsService {
  curLang = '';
  langs = {};
  _changes = new Subject<string>();

  async load(lang = 'il', forceReload = false) {
    if (!this.langs[lang] || forceReload) {
      const texts = await getLang(lang);
      this.langs[lang] = texts;
    }
    this.curLang = lang;
    this._changes.next(lang);
  }

  get(key: string) {
    return this.langs[this.curLang][key];
  }

  get changes(): Observable<string> {
    return this._changes.asObservable();
  }
}
