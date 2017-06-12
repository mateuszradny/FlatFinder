import { Component, OnInit, OnDestroy } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { User } from './_models/index';
import { AuthenticationService, OfferService } from './_services/index';

@Component({
    moduleId: module.id,
    selector: 'app',
    templateUrl: 'app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {
    constructor(private authenticationService: AuthenticationService, private offerService: OfferService) {
    }

    private subscriptions: Subscription[] = [];
    public user: User = null;
    public newOfferCount: number = 0;

    ngOnInit() {
        this.subscriptions.push(this.authenticationService.getCurrentUser().subscribe(user => {
            this.user = user

            if (this.user)
                this.offerService.getNewOfferCount().subscribe(count => this.newOfferCount = count);
        }));
    }

    ngOnDestroy() {
        this.subscriptions.forEach(sub => sub.unsubscribe());
    }
}