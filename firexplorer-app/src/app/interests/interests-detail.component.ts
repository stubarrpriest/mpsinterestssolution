import { Component, OnInit } from '@angular/core';
import { InterestCollectionService } from './interest.collection.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IInterestCollection } from './interest.collection';

@Component({
  selector: 'app-interests-detail',
  templateUrl: './interests-detail.component.html',
  styleUrls: ['./interests-detail.component.css']
})
export class InterestsDetailComponent implements OnInit {

  interestCollection: IInterestCollection;

  errorMessage = '';

  constructor(private route: ActivatedRoute,
    private router: Router,
    private interestCollectionService: InterestCollectionService) { }

  ngOnInit() {
    const param = this.route.snapshot.paramMap.get("identifier");
    if (param) {
      this.getInterestCollection(param);
    }
  }

  getInterestCollection(identifier: string) {
    this.interestCollectionService.getInterest(identifier).subscribe({
      next: interestCollection => this.interestCollection = interestCollection,
      error: err => this.errorMessage = err
    });
  }
}
