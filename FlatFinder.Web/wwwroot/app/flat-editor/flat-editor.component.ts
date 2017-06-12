import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/operator/map';

import { Flat, User } from '../_models/index';
import { AlertService, AuthenticationService, FlatService, ErrorHandlerService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'flat-editor.component.html',
    styles: [' :host { width: 100%; } ']
})
export class FlatEditorComponent implements OnInit, OnDestroy {
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private flatService: FlatService,
        private alertService: AlertService,
        private authenticationService: AuthenticationService,
        private errorHandlerService: ErrorHandlerService
    ) { }

    public isCreateForm: boolean = false;
    private subscriptions: Subscription[] = [];

    flat: Flat = new Flat();

    ngOnInit() {
        this.subscriptions.push(this.authenticationService.getCurrentUser().subscribe(user => {
            if (user == null)
                this.router.navigate(['']);
        }));

        this.subscriptions.push(this.route.params.subscribe(params => {
            if (!params['id']) {
                this.isCreateForm = true;
            } else {
                this.flatService.get(+params['id']).subscribe(
                    flat => this.flat = flat,
                    error => {
                        this.errorHandlerService.handle(error);
                    }
                )
            }
        }));
    }

    ngOnDestroy() {
        this.subscriptions.forEach(sub => sub.unsubscribe());
    }

    save() {
        let observable: any = this.isCreateForm ? this.flatService.add(this.flat) : this.flatService.update(this.flat);
        observable.subscribe(
            () => {
                this.alertService.success("Flat saved", true);
                this.router.navigate(['']);
            },
            (error: any) => {
                this.errorHandlerService.handle(error);
            });
    }
}