import { Component, OnInit, OnDestroy } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { User } from '../_models/index';
import { AlertService, AuthenticationService, UserService, ErrorHandlerService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'user-list.component.html',
    styles: [' :host { width: 100%; } ']
})
export class UserListComponent implements OnInit, OnDestroy {
    constructor(
        private userService: UserService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private errorHandlerService: ErrorHandlerService
    ) { }

    private subscriptions: Subscription[] = [];

    public currentUser: User = null;
    public users: User[] = [];

    ngOnInit() {
        this.subscriptions.push(this.authenticationService.getCurrentUser().subscribe(user => this.currentUser = user));
        this.subscriptions.push(this.userService.get().subscribe(users => this.users = users));
    }

    ngOnDestroy() {
        this.subscriptions.forEach(sub => sub.unsubscribe());
    }

    remove(userId: number) {
        this.userService.remove(userId)
            .subscribe(
            data => {
                this.alertService.success('Event removed');
                this.users = this.users.filter(x => x.id !== userId);
            },
            error => {
                this.errorHandlerService.handle(error);
            });
    }
}