import { Injectable } from '@angular/core';
import { ICommitLookup } from './commit.lookup';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class CommitLookupService{

    private commitLookupUrl = 'api/repo/publicationsetcommits.json';

    constructor(private http: HttpClient){

    }

    async getCommitLookups(): Promise<ICommitLookup[]> {
        return await this.http.get<ICommitLookup[]>(this.commitLookupUrl).toPromise();
    }
    
    onError(errorInstance: HttpErrorResponse){
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