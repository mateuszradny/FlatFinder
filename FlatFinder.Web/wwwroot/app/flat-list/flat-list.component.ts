import { Component, OnInit } from '@angular/core';

import { Flat, FlatSearchQuery, User } from '../_models/index';
import { AlertService, AuthenticationService, FlatService, ViewedFlatsService, ErrorHandlerService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'flat-list.component.html',
    styles: [' :host { width: 100%; } ']
})
export class FlatListComponent {
    constructor(
        private flatService: FlatService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private errorHandlerService: ErrorHandlerService,
        private viewedFlatsService: ViewedFlatsService
    ) { }

    flats: Flat[] = [];
    recentlyViewed: Flat[] = []
    user: User = null;
    query: FlatSearchQuery = new FlatSearchQuery();

    ngOnInit() {
        this.authenticationService.getCurrentUser()
            .subscribe(user => this.user = user);

        this.flatService.getAll().subscribe(flats => this.flats = flats);
        this.recentlyViewed = this.viewedFlatsService.getRecentlyViewedFlats();
    }

    search() {
        this.flatService.search(this.query).subscribe(flats => this.flats = flats);
    }

    clearFilters() {
        this.query = new FlatSearchQuery();
        this.search();
    }
}