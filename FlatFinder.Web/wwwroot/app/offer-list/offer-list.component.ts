import { Component, OnInit, OnDestroy } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { User, PurchaseOffer } from '../_models/index';
import { AlertService, AuthenticationService, OfferService, ErrorHandlerService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'offer-list.component.html',
    styles: [' :host { width: 100%; } ']
})
export class OfferListComponent implements OnInit, OnDestroy {
    constructor(
        private offerService: OfferService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private errorHandlerService: ErrorHandlerService
    ) { }

    private subscriptions: Subscription[] = [];

    public currentUser: User = null;
    public offers: PurchaseOffer[] = [];

    ngOnInit() {
        this.subscriptions.push(this.authenticationService.getCurrentUser().subscribe(user => this.currentUser = user));
        this.subscriptions.push(this.offerService.get().subscribe(users => this.offers = users));
    }

    ngOnDestroy() {
        this.subscriptions.forEach(sub => sub.unsubscribe());
    }

    remove(offerId: number) {
        this.offerService.remove(offerId)
            .subscribe(
            data => {
                this.alertService.success('Offer removed');
                this.offers = this.offers.filter(x => x.offerId !== offerId);
            },
            error => {
                this.errorHandlerService.handle(error);
            });
    }
}