import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AlertService, ErrorHandlerService, UserService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'register.component.html',
    styles: [' :host { width: 100%; } ']
})
export class RegisterComponent {
    model: any = {};
    loading = false;

    constructor(
        private router: Router,
        private userService: UserService,
        private alertService: AlertService,
        private errorHandler: ErrorHandlerService) { }

    register() {
        this.loading = true;
        this.userService.register(this.model.email, this.model.password, this.model.confirmPassword, this.model.phoneNumber)
            .subscribe(
            data => {
                this.alertService.success('Registration successful', true);
                this.router.navigate(['/login']);
            },
            error => {
                this.errorHandler.handle(error);
                this.loading = false;
            });
    }
}