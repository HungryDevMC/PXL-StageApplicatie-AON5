import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppComponent} from './app.component';
import {MainHeaderComponent} from './main/header.component';
import {CompanyCreateExerciseComponent} from './company/create-exercise/create-exercise.component';
import {CompanyHomeComponent} from './company/home/company-home.component';
import {CompanyAppComponent} from './company/company-app.component';
import {RouterModule} from '@angular/router';
import {CoordinatorHomeComponent} from './coordinator/dashboard/coordinator-home.component';
import {CoordinatorAppComponent} from './coordinator/coordinator-app.component';
import {LoginComponent} from './main/login/login.component';
import {NotfoundComponent} from './main/notfound/notfound.component';
import {CoordinatorProposalsComponent} from './coordinator/proposals/coordinator-voorstellen.component';
import {StudentAppComponent} from './student/student-app.component';
import {StudentHomeComponent} from './student/home/student-home.component';
import {RegisterComponent} from './main/register/register.component';
import {RegisterCompanyComponent} from './main/register/company/register-company.component';
import {RegisterPxlComponent} from './main/register/pxl/register-pxl.component';
import {RegisterTypeComponent} from './main/register/type/register-type.component';
import {ProposalListComponent} from './main/proposal-list/proposal-list.component';
import {ProposalBasicComponent} from './main/proposal/variations/basic/proposal-basic.component';
import {FormsModule} from '@angular/forms';
import {TeacherAppComponent} from './teacher/teacher-app.component';
import {ProposalAppComponent} from './main/proposal/proposal-app.component';
import {ProposalTeacherComponent} from './main/proposal/variations/teacher/proposal-teacher.component';
import {TeacherReviewComponent} from './teacher/review/teacher-review.component';
import {ProposalCoordinatorComponent} from "./main/proposal/variations/coordinator/proposal-coordinator.component";
import {ProposalNewCoordinatorComponent} from "./main/proposal/variations/coordinator/new/proposal-new-coordinator.component";
import {ProposalTeachersReviewedCoordinatorComponent} from "./main/proposal/variations/coordinator/teachers_reviewed/proposal-teachers-reviewed-coordinator.component";
import {CompanyUpdateExerciseComponent} from "./company/update-exercise/company-update-exercise.component";
import {ProposalCompanyUpdateComponent} from "./main/proposal/variations/company/update/proposal-company-update.component";
import {ProposalCompanyComponent} from "./main/proposal/variations/company/proposal-company.component";
import {ProposalCompanyBasicComponent} from "./main/proposal/variations/company/basic/proposal-company-basic.component";
import {CoordinatorAllProposalsComponent} from "./coordinator/all_proposals/coordinator-voorstellen.component";
import {EmailVerificationComponent} from "./main/email_verification/email-verification.component";
import {ProposalCoordinatorBasicComponent} from "./main/proposal/variations/coordinator/basic/proposal-coordinator-basic.component";
import {CoordinatorUserViewComponent} from "./coordinator/users/coordinator-userview.component";
import { ChangePasswordComponent } from './main/change-password/change-password.component';
import { UnauthorizedPageComponent } from './main/unauthorized-page/unauthorized-page.component';

@NgModule({
  declarations: [
    AppComponent,
    MainHeaderComponent,
    CompanyCreateExerciseComponent,
    CompanyHomeComponent,
    CompanyAppComponent,
    CoordinatorHomeComponent,
    CoordinatorAppComponent,
    LoginComponent,
    NotfoundComponent,
    CoordinatorProposalsComponent,
    StudentAppComponent,
    StudentHomeComponent,
    RegisterComponent,
    RegisterCompanyComponent,
    RegisterPxlComponent,
    RegisterTypeComponent,
    ProposalListComponent,
    ProposalBasicComponent,
    TeacherAppComponent,
    ProposalAppComponent,
    ProposalTeacherComponent,
    TeacherReviewComponent,
    ProposalCoordinatorComponent,
    ProposalTeachersReviewedCoordinatorComponent,
    ProposalNewCoordinatorComponent,
    CompanyUpdateExerciseComponent,
    ProposalCompanyUpdateComponent,
    ProposalCompanyComponent,
    ProposalCompanyBasicComponent,
    CoordinatorAllProposalsComponent,
    EmailVerificationComponent,
    ProposalCoordinatorBasicComponent,
    ChangePasswordComponent,
    UnauthorizedPageComponent,
    CoordinatorUserViewComponent,
    ChangePasswordComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forChild([
      {
        path: 'proposal',
        component: ProposalAppComponent,
        children: [
          {
            path: 'student/:id',
            component: ProposalBasicComponent
          },
          {
            path: 'teacher/:id',
            component: ProposalTeacherComponent
          },
          {
            path: 'coordinator/:id',
            component: ProposalCoordinatorComponent
          },
          {
            path: 'company/:id',
            component: ProposalCompanyComponent
          },
          {
            path: 'company/basic/:id',
            component: ProposalCompanyBasicComponent
          },
          {
            path: 'company/update/:id',
            component: ProposalCompanyUpdateComponent
          },
          {
            path: 'coordinator/new/:id',
            component: ProposalNewCoordinatorComponent
          },
          {
            path: 'coordinator/teachers_reviewed/:id',
            component: ProposalTeachersReviewedCoordinatorComponent
          },
          {
            path: 'coordinator/basic/:id',
            component: ProposalCoordinatorBasicComponent
          }
        ]
      }
    ]),
    RouterModule.forChild([
      {
        path: 'teacher',
        component: TeacherAppComponent,
        children: [
          {
            path: 'proposals_review',
            component: TeacherReviewComponent,
          },
          {
            path: '',
            redirectTo: 'proposals_review',
            pathMatch: 'full'
          }
        ]
      }
    ]),
    RouterModule.forChild([
      {
        path: 'register',
        component: RegisterComponent,
        children: [
          {
            path: 'company',
            component: RegisterCompanyComponent
          },
          {
            path: 'pxl',
            component: RegisterPxlComponent
          },
          {
            path: 'type',
            component: RegisterTypeComponent
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'type'
          }
        ]
      }
    ]),
    RouterModule.forChild([
      {
        path: 'company',
        component: CompanyAppComponent,
        children: [
          {
            path: 'home',
            component: CompanyHomeComponent
          },
          {
            path: 'create',
            component: CompanyCreateExerciseComponent
          },
          {
            path: 'update/:id',
            component: CompanyUpdateExerciseComponent
          },
          {
            path: '',
            redirectTo: 'home',
            pathMatch: 'full'
          }
        ]
      }
    ]),
    RouterModule.forChild([
      {
        path: 'coordinator',
        component: CoordinatorAppComponent,
        children: [
          {
            path: 'home',
            component: CoordinatorHomeComponent
          },
          {
            path: 'proposals',
            component: CoordinatorProposalsComponent
          },
          {
            path: 'all_proposals',
            component: CoordinatorAllProposalsComponent
          },
          {
            path: 'users',
            component: CoordinatorUserViewComponent
          },
          {
            path: '',
            redirectTo: 'home',
            pathMatch: 'full'
          }
        ]
      }
    ]),
    RouterModule.forChild([
      {
        path: 'student',
        component: StudentAppComponent,
        children: [
          {
            path: 'home',
            component: StudentHomeComponent
          },
          {
            path: '',
            redirectTo: 'home',
            pathMatch: 'full'
          }
        ]
      }
    ]),
    RouterModule.forRoot([
      {path: 'company', component: CompanyAppComponent},
      {path: 'coordinator', component: CoordinatorAppComponent},
      {path: 'student', component: StudentAppComponent},
      {path: 'login', component: LoginComponent},
      {path: 'register', component: RegisterComponent},
      {path: 'teacher', component: TeacherAppComponent},
      {path: 'unauthorized-page', component: UnauthorizedPageComponent},
      {path: 'change-password', component: ChangePasswordComponent},
      {path: 'proposal/:id', component: ProposalBasicComponent},
      {path: 'verify', component: EmailVerificationComponent},
      {path: '', redirectTo: 'login', pathMatch: 'full'},
      {path: '404', component: NotfoundComponent},
      {path: '**', redirectTo: '404'}
    ], {useHash: true}),
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
