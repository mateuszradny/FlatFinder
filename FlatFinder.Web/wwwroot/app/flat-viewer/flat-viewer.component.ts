import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/operator/map';

import { Flat, User } from '../_models/index';
import { AlertService, AuthenticationService, FlatService, OfferService, ErrorHandlerService, ViewedFlatsService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'flat-viewer.component.html',
    styles: [' :host { width: 100%; } ']
})
export class FlatViewerComponent implements OnInit, OnDestroy {
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private flatService: FlatService,
        private offerService: OfferService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private errorHandlerService: ErrorHandlerService,
        private viewedFlatsService: ViewedFlatsService
    ) { }

    private subscriptions: Subscription[] = [];
    private user: User = new User();

    public flat: Flat = new Flat();

    ngOnInit() {
        this.subscriptions.push(this.authenticationService.getCurrentUser().subscribe(user => this.user = user));
        this.subscriptions.push(this.route.params.subscribe(params => {
            if (!params['id']) {
                this.router.navigate(['']);
            } else {
                this.flatService.get(+params['id']).subscribe(
                    flat => {
                        this.flat = flat;
                        this.viewedFlatsService.addToRecentlyViewedFlats(this.flat);
                    },
                    error => {
                        this.errorHandlerService.handle(error, true);
                        this.router.navigate(['']);
                    }
                )
            }
        }));
    }

    ngOnDestroy() {
        this.subscriptions.forEach(sub => sub.unsubscribe());
    }

    addOffer() {
        this.offerService.addOffer(this.flat.id).subscribe(
            data => {
                this.alertService.success("Offer was added");
            },
            error => {
                this.errorHandlerService.handle(error);
            }
        );
    }

    remove() {
        this.flatService.remove(this.flat.id).subscribe(
            data => {
                this.alertService.success("Flat was removed", true);
                this.router.navigate(['']);
            },
            error => {
                this.errorHandlerService.handle(error);
            }
        );
    }

    setAsSold() {
        this.flatService.setAsSold(this.flat.id).subscribe(
            data => this.alertService.success("Flat was set as sold."),
            error => this.errorHandlerService.handle(error)
        );
    }
}