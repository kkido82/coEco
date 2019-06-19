import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginLayoutComponent } from './components/login-layout/login-layout.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginWizardComponent } from './components/login-wizard/login-wizard.component';
import { CodeFormComponent } from './components/code-form/code-form.component';
import { TextsModule } from '../texts/texts.module';

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, TextsModule],
  declarations: [
    LoginLayoutComponent,
    LoginFormComponent,
    CodeFormComponent,
    LoginWizardComponent
  ],
  entryComponents: [LoginWizardComponent]
})
export class LoginModule {}
