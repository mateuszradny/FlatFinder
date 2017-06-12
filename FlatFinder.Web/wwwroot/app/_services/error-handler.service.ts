import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Router } from '@angular/router';

import { AlertService } from './alert.service';
import { ErrorResponse } from '../_models/error-response';

@Injectable()
export class ErrorHandlerService {
    constructor(
        private alertService: AlertService,
        private router: Router) { }

    public handle(response: Response, keepAfterNavigationChange = false) {
        if (response.status === 401) {
            this.alertService.error("Your session has expired. Log in again.", true);
            this.router.navigate(['/login'], { queryParams: { returnUrl: this.router.routerState.snapshot.url } });
        }

        let errorMessage = this.getErrorMessage(response);
        this.alertService.error(errorMessage, keepAfterNavigationChange);
    }

    private getErrorMessage(response: Response): string {
        let errorResponse: ErrorResponse = response.json();
        if (errorResponse.message)
            return errorResponse.message;

        return errorResponse.errors.map(x => x.message).join(" ");
    }
}