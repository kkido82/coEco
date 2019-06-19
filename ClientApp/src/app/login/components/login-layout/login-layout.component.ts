import { Component, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-login-layout',
  templateUrl: './login-layout.component.html',
  styleUrls: []
})
export class LoginLayoutComponent  {
  texts = {
    firstStep: {
      mainTitle: 'מה תרצו לשתף היום',
      placeholder: 'הקלידו כאן מספר ת.ז.'
    },
    secondStep: {
      mainTitle: 'תודה!',
      subTitle: 'שלחנו לך קוד בן 6 ספרות להשלמת הזיהוי'
    },
    contact: {
      title: 'רשומים לאפליקציה ולא מצליחים להיכנס? צרו קשר',
      phones: ['0505555555', '0503333333'],
      extraInfo: 'כניסה למערכת תתאפשר רק למשתמשי מערכת שיתוף משאבים'
    }
  };

  constructor(private renderer: Renderer2) {}

  ngOnInit() {
    this.renderer.addClass(document.body, 'inLogin');
  }

  // tslint:disable-next-line:use-life-cycle-interface
  ngOnDestroy() {
    this.renderer.removeClass(document.body, 'inLogin');
  }
}
