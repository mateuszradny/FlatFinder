import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/interval';
import "rxjs/add/operator/mergeMap";
import 'rxjs/add/operator/map';

import { AuthenticationService } from '../_services/index';
import { PurchaseOffer } from '../_models/index';

@Injectable()
export class OfferService {
    constructor(private http: Http, private authenticationService: AuthenticationService) { }

    addOffer(flatId: number) {
        return this.http.post('/api/offer/' + flatId, {}, this.authenticationService.getAuthHeaders())
            .map(response => response.json());
    }

    get(): Observable<PurchaseOffer[]> {
        return this.http.get('/api/offer/', this.authenticationService.getAuthHeaders())
            .map(response => response.json());
    }

    getNewOfferCount(): Observable<number> {
        return Observable.interval(1000)
            .flatMap(() => {
                return this.http.get('/api/offer/new-offer-count', this.authenticationService.getAuthHeaders())
                    .map(response => response.json().count);
            });
    }

    remove(id: number) {
        return this.http.delete('/api/offer/' + id, this.authenticationService.getAuthHeaders());
    }
}