import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { AuthenticationService } from '../_services/index';
import { Flat, FlatSearchQuery, SoldFlatsReport } from '../_models/index';

@Injectable()
export class FlatService {
    constructor(private http: Http, private authenticationService: AuthenticationService) { }

    add(flat: Flat) {
        return this.http.post('/api/flat', flat, this.authenticationService.getAuthHeaders());
    }

    generateReport(from: Date, to: Date): Observable<SoldFlatsReport> {
        return this.http.post('/api/flat/generate-report', { from: from, to: to }, this.authenticationService.getAuthHeaders())
            .map(response => response.json());
    }

    getAll(): Observable<Flat[]> {
        return this.http.get('/api/flat', this.authenticationService.getAuthHeaders())
            .map(response => response.json());
    }

    get(id: number): Observable<Flat> {
        return this.http.get('/api/flat/' + id, this.authenticationService.getAuthHeaders())
            .map(response => response.json());
    }

    search(query: FlatSearchQuery): Observable<Flat[]> {
        return this.http.post('/api/flat/search', query, this.authenticationService.getAuthHeaders())
            .map(response => response.json());
    }

    update(flat: Flat) {
        return this.http.put('/api/flat/' + flat.id, flat, this.authenticationService.getAuthHeaders());
    }

    remove(id: number) {
        return this.http.delete('/api/flat/' + id, this.authenticationService.getAuthHeaders());
    }

    setAsSold(id: number) {
        return this.http.post('/api/flat/set-as-sold/' + id, {}, this.authenticationService.getAuthHeaders());
    }
}