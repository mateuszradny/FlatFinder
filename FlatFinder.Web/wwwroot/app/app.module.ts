import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BaseRequestOptions } from '@angular/http';

import { AppComponent } from './app.component';
import { routing } from './app.routing';

import { AlertComponent } from './_directives/index';
import { AuthGuard, AdminGuard } from './_guards/index';
import { AlertService, AuthenticationService, ErrorHandlerService, FlatService, OfferService, UserService, ViewedFlatsService } from './_services/index';
import { FlatListComponent } from './flat-list/index';
import { FlatEditorComponent } from './flat-editor/index';
import { FlatViewerComponent } from './flat-viewer/index';
import { SoldFlatsReportComponent } from './sold-flats-report/index';
import { UserListComponent } from './user-list/index';
import { UserChangePasswordComponent } from './user-change-password/index';
import { OfferListComponent } from './offer-list/index';
import { LoginComponent } from './login/index';
import { RegisterComponent } from './register/index';

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        routing
    ],
    declarations: [
        AppComponent,
        AlertComponent,
        FlatListComponent,
        FlatEditorComponent,
        FlatViewerComponent,
        SoldFlatsReportComponent,
        UserListComponent,
        UserChangePasswordComponent,
        OfferListComponent,
        LoginComponent,
        RegisterComponent,
    ],
    providers: [
        AuthGuard,
        AdminGuard,
        AlertService,
        AuthenticationService,
        BaseRequestOptions,
        ErrorHandlerService,
        FlatService,
        OfferService,
        UserService,
        ViewedFlatsService,
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }