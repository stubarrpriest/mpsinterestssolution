import { Injectable } from '@angular/core';
import { IInterestSummary } from './interest.summary';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import {catchError, tap, map} from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class InterestSummaryService{

    private productUrl = 'api/interests/summary.json';

    constructor(private http: HttpClient){

    }

    getInterests(): Observable<IInterestSummary[]> {
        return this.http.get<IInterestSummary[]>(this.productUrl).pipe(
            catchError(this.onError)
        );
    }
    
    private onError(errorInstance: HttpErrorResponse){
        let message = '';
        
        if(errorInstance.error instanceof ErrorEvent){
            message = 'Error: ${errorInstance.error.Message}';
        } else{
            message = 'Server error: ${err.status}, ${err.message}';
        }

        console.error(message);

        return throwError(message);
    }
}