import { Component, OnInit } from '@angular/core';
import { InterestCollectionService } from './interest.collection.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IInterestCollection } from './interest.collection';
import { CommitLookupService } from './commit.lookup.service';
import { ICommitLookup } from './commit.lookup';

@Component({
  selector: 'app-interests-detail',
  templateUrl: './interests-detail.component.html',
  styleUrls: ['./interests-detail.component.css']
})
export class InterestsDetailComponent implements OnInit {

  gitHubUrl = 'https://github.com/stubarrpriest/mpsinterests/commit/';

  interestCollection: IInterestCollection;

  commitLookup: ICommitLookup[];

  errorMessage = '';

  constructor(private route: ActivatedRoute,
    private router: Router,
    private interestCollectionService: InterestCollectionService,
    private commitLookupService: CommitLookupService) { }

  ngOnInit() {
    const param = this.route.snapshot.paramMap.get("identifier");
    if (param) {
      this.getCommitLookup();
      this.getInterestCollection(param);
    }
  }

  getInterestCollection(identifier: string) {
    this.interestCollectionService.getInterest(identifier).subscribe({
      next: interestCollection => this.interestCollection = interestCollection,
      error: err => this.errorMessage = err
    });
  }

  getCommitLookup() {
    this.commitLookupService.getCommitLookups().then(
      x =>{
        this.commitLookup = x;
        console.log(`Commit lookup initialised ${x.length}`);
      }).catch(y => this.commitLookupService.onError(y));
  }

  getDiffUrlFor(publicationSet: string) : string
  {
    console.log(publicationSet);

    if(this.commitLookup)
    {
      let lookupItem = this.commitLookup.find(x => x.publicationSet === publicationSet);
    
      if(lookupItem)
      {
        return this.gitHubUrl + lookupItem.commitId;
      }

    }

    return "";
  }

}
