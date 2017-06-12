import { Component, OnInit } from '@angular/core';

import { SoldFlatsReport } from '../_models/index';
import { FlatService, ErrorHandlerService } from '../_services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'sold-flats-report.component.html',
    styles: [' :host { width: 100%; } '],
    selector: 'sold-flats-report'
})
export class SoldFlatsReportComponent implements OnInit {
    constructor(
        private flatService: FlatService,
        private errorHandler: ErrorHandlerService
    ) { }

    public report: SoldFlatsReport = new SoldFlatsReport();
    public fromDate: Date;
    public toDate: Date;

    ngOnInit() {
        let now = new Date();

        this.fromDate = new Date(now.getFullYear(), now.getMonth(), 1);
        this.toDate = now;

        this.generate();
    }

    generate() {
        this.flatService.generateReport(this.fromDate, this.toDate).subscribe(report => this.report = report);
    }
}