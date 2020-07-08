import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'proposal-coordinator-app',
  templateUrl: './proposal-company-basic.component.html',
  styleUrls: ['./proposal-company-basic.component.css']
})

export class ProposalCompanyBasicComponent implements OnInit {
  constructor(private route: ActivatedRoute) {
  }

  ngOnInit() {
    const internshipId = this.route.snapshot.paramMap.get('id');
    const url = 'https://localhost:3000/api/Internship/' + internshipId;

    const approvedSide = document.getElementById('approvedFeedback');
    const deniedSide = document.getElementById('deniedFeedback');
    const coordinatorSide = document.getElementById('coordinatorFeedback');

    fetch(url, {
      headers: {
        "Authorization": "Bearer " + localStorage.getItem('token')
      }
    })
      .then((response) => {
        if (response.status === 401 || response.status === 403) {
          window.location.assign('/#/unauthorized-page');
        }
        return response.json()
      })
      .then((data) => {
        if (data.internshipState > 1) {
          const reviews = data.reviewers;
          reviews.forEach(function (review) {
            let sideToAddTo;
            let backgroundColor;
            switch (review.stateOfTeacher) {
              case 2:
                sideToAddTo = approvedSide;
                backgroundColor = '#1ec913';
                break;
              case 3:
                sideToAddTo = deniedSide;
                backgroundColor = '#f00f12';
                break;
            }
            let textArea = document.createElement('textarea');
            textArea.readOnly = true;
            textArea.style.backgroundColor = backgroundColor;
            textArea.value = review.feedback;
            sideToAddTo.appendChild(textArea);
          });
          if (data.internshipState >= 5) {
            let textArea = document.createElement('textarea');
            textArea.readOnly = true;
            let backgroundColor = "#1ec913";
            if (data.internshipState === 6) {
              backgroundColor = '#f00f12';
            }
            textArea.style.backgroundColor = backgroundColor;
            textArea.value = data.feedback;
            coordinatorSide.appendChild(textArea);
          }
        } else {
          const feedbackSection = (document.getElementById('feedbackSection') as HTMLInputElement);
          feedbackSection.hidden = true;
        }
      });

  }

}
