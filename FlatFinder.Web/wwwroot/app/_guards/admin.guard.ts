import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { AuthenticationService } from '../_services/index';

@Injectable()
export class AdminGuard implements CanActivate {
    constructor(private router: Router, private authenticationService: AuthenticationService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.authenticationService.getCurrentUser().map(user => {
            if (user == null) {
                this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
                return false;
            }

            return user.roles && user.roles.indexOf("Admin") !== -1;
        });
    }
}