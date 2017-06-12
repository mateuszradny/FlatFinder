import { Injectable } from '@angular/core';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'
import 'rxjs/add/observable/zip';

import { AuthenticationService, FlatService } from '../_services/index';
import { Flat, User } from '../_models/index'

@Injectable()
export class ViewedFlatsService {
    constructor(private flatService: FlatService) {
    }

    addToRecentlyViewedFlats(flat: Flat) {
        let flats = this.getRecentlyViewedFlats();
        let index = flats.map(x => x.id).indexOf(flat.id);
        if (index === -1) {
            flats.push(flat);

            if (flats.length > 3)
                flats = flats.slice(Math.max(flats.length - 3, 1));

            localStorage.setItem('recentlyViewedFlat', JSON.stringify(flats));
        } else {
            flats[index] = flat;
        }
    }

    getRecentlyViewedFlats(): Flat[] {
        let idsAsJson = localStorage.getItem('recentlyViewedFlat');
        if (idsAsJson)
            return JSON.parse(idsAsJson);

        return [];
    }
}