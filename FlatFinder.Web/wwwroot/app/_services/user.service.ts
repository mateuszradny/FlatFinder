import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { User } from '../_models/index';
import { AuthenticationService } from '../_services/index';

@Injectable()
export class UserService {
    constructor(private http: Http, private authenticationService: AuthenticationService) { }

    changePassword(userId: number, password: string, confirmPassword: string) {
        return this.http.post('/api/user/change-password/' + userId, { password: password, confirmPassword: confirmPassword }, this.authenticationService.getAuthHeaders());
    }

    get(): Observable<User[]> {
        return this.http.get('/api/user', this.authenticationService.getAuthHeaders())
            .map(response => response.json());
    }

    register(email: string, password: string, confirmPassword: string, phoneNumber: string) {
        return this.http.post('/api/account/register', { email: email, password: password, confirmPassword: confirmPassword, phoneNumber: phoneNumber });
    }

    remove(userId: number) {
        return this.http.delete('/api/user/' + userId, this.authenticationService.getAuthHeaders());
    }
}