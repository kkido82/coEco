import { Provider, NgModule, APP_INITIALIZER } from '@angular/core';
import { TextDirective } from './texts.directive';
import { TextsService } from './texts.service';

const defaultLang = 'il';

const init = (service: TextsService) => () => service.load(defaultLang);

const InitTexts: Provider = {
  provide: APP_INITIALIZER,
  useFactory: init,
  deps: [TextsService],
  multi: true
};

@NgModule({
  declarations: [TextDirective],
  providers: [TextsService, InitTexts],
  exports: [TextDirective]
})
export class TextsModule {}
