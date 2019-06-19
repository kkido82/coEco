import { Routes } from '@angular/router';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { LoginLayoutComponent } from './login/components/login-layout/login-layout.component';
import { AppLayoutComponent } from './components/app-layout/app-layout.component';
import { LendingFromComponent } from './components/lending-from/lending-from.component';
import { LendingToComponent } from './components/lending-to/lending-to.component';
import { ManageItemsComponent } from './components/manage-items/manage-items.component';
import { AuthGuard } from './services/guards/auth-guard.service';
import { ProblemReportComponent } from './components/problem-report/problem-report.component';
import { LoginWizardComponent } from './login/components/login-wizard/login-wizard.component';

export const appRoutes: Routes = [
  {
    path: '',
    component: AppLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'orders/from', pathMatch: 'full' },
      { path: 'orders/from', component: LendingFromComponent },
      { path: 'orders/to', component: LendingToComponent },
      { path: 'items', component: ManageItemsComponent },
      { path: 'report/:id', component: ProblemReportComponent },
    ]
  },
  {
    path: 'login',
    component: LoginLayoutComponent,
    children: [
      { path: '', component: LoginWizardComponent }
    ]
  },
  { path: '**', component: PageNotFoundComponent }
];
