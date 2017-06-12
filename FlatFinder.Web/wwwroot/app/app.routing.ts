import { Routes, RouterModule } from '@angular/router';

import { FlatEditorComponent } from './flat-editor/index';
import { FlatViewerComponent } from './flat-viewer/index';
import { FlatListComponent } from './flat-list/index';
import { UserListComponent } from './user-list/index';
import { UserChangePasswordComponent } from './user-change-password/index';
import { OfferListComponent } from './offer-list/index';
import { LoginComponent } from './login/index';
import { RegisterComponent } from './register/index';
import { AuthGuard, AdminGuard } from './_guards/index';

const appRoutes: Routes = [
    { path: '', component: FlatListComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'flat-editor/:id', component: FlatEditorComponent, canActivate: [AuthGuard] },
    { path: 'flat-viewer/:id', component: FlatViewerComponent },
    { path: 'user-list', component: UserListComponent, canActivate: [AdminGuard] },
    { path: 'user-change-password/:id', component: UserChangePasswordComponent, canActivate: [AdminGuard] },
    { path: 'offer-list', component: OfferListComponent, canActivate: [AuthGuard] },

    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);