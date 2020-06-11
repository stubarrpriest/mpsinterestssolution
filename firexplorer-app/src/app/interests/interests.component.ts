import { Component, OnInit } from '@angular/core';
import { InterestSummaryService } from './interests.summary.service';
import { IInterestSummary } from './interest.summary';

@Component({
  selector: 'app-interests',
  templateUrl: './interests.component.html',
  styleUrls: ['./interests.component.css']
})
export class InterestsComponent implements OnInit {

  errorMessage: string = "";

  interestSummaries: IInterestSummary[];

  constructor(private interestsSummaryService: InterestSummaryService) { }

  ngOnInit(): void {
        this.interestsSummaryService.getInterests().subscribe({
            next: interestSummaries => {
                this.interestSummaries = interestSummaries;
                console.log(`Interest summaries initialised ${interestSummaries.length}`);
            },
            error: err => {this.errorMessage = err}
        });
      }
}
