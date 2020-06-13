import { Injectable } from '@angular/core';
import { IInterestCollection } from './interest.collection';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import {catchError, tap, map} from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class InterestCollectionService{

    private interestUrl = 'api/interests/';

    constructor(private http: HttpClient){

    }

    getInterest(identifier: string): Observable<IInterestCollection> {
        let url = this.interestUrl + identifier + '.json';
        return this.http.get<IInterestCollection>(url).pipe(
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