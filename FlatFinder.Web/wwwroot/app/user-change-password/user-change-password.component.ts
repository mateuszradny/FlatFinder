import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { AlertService, AuthenticationService, ErrorHandlerService, UserService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'user-change-password.component.html',
    styles: [' :host { width: 100%; } ']
})
export class UserChangePasswordComponent implements OnInit, OnDestroy {
    model: any = {};
    loading = false;

    private subscriptions: Subscription[] = [];
    private userId: number;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private userService: UserService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private errorHandler: ErrorHandlerService) { }

    ngOnInit() {
        this.subscriptions.push(this.authenticationService.getCurrentUser().subscribe(user => {
            if (user == null)
                this.router.navigate(['']);
        }));

        this.subscriptions.push(this.route.params.subscribe(params => {
            if (!params['id']) {
                this.router.navigate(['/user-list']);
            } else {
                this.userId = params['id'];
            }
        }));
    }

    ngOnDestroy() {
        this.subscriptions.forEach(sub => sub.unsubscribe());
    }

    changePassword() {
        this.loading = true;
        this.userService.changePassword(this.userId, this.model.password, this.model.confirmPassword)
            .subscribe(
            data => {
                this.alertService.success('Password changed', true);
                this.router.navigate(['/user-list']);
            },
            error => {
                this.errorHandler.handle(error);
                this.loading = false;
            });
    }
}