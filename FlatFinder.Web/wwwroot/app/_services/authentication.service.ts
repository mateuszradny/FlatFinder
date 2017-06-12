import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response } from '@angular/http';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'

import { User } from '../_models/user'

@Injectable()
export class AuthenticationService {
    constructor(private http: Http) { }

    private currentUser = new BehaviorSubject<User>(this.getUserFromLocalStorageOrNull());

    getAuthHeaders() {
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        if (currentUser && currentUser.token) {
            let headers = new Headers({ 'Authorization': 'Bearer ' + currentUser.token });
            return new RequestOptions({ headers: headers });
        }

        return null;
    }

    getCurrentUser() {
        return this.currentUser.asObservable();
    }

    login(email: string, password: string) {
        return this.http.post('/api/account/token', { email: email, password: password })
            .map((response: Response) => {
                let user = response.json();
                if (user && user.token) {
                    localStorage.setItem('currentUser', JSON.stringify(user));
                    this.currentUser.next(this.getUserFromLocalStorageOrNull());
                }
            });
    }

    logout() {
        localStorage.removeItem('currentUser');
        this.currentUser.next(null);
    }

    private getUserFromLocalStorageOrNull(): User {
        let token = localStorage.getItem('currentUser');
        if (token) {
            let values = this.parseToken(token);
            return {
                id: +values['sub'],
                email: values['email'],
                phoneNumber: values['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone'],
                roles: values['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || []
            };
        }

        return null;
    }

    private parseToken(token: string): any {
        let base64 = token.split('.')[1].replace('-', '+').replace('_', '/');
        return JSON.parse(window.atob(base64));
    }
}