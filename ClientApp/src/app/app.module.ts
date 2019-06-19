import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { ComponentsModule } from './components/components.module';
import { appRoutes } from './routes';
import { LoginModule } from './login/login.module';
import { TextsModule } from './texts/texts.module';
import { InitApp } from './init-app';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    ComponentsModule,
    LoginModule,
    TextsModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: false, useHash: true } // <-- debugging purposes only
    )
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: (initApp: InitApp) =>
        function() {
          return initApp.init();
        },
      deps: [InitApp],
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
