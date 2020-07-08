import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'proposal-coordinator-app',
  templateUrl: './proposal-teachers-reviewed-coordinator.component.html',
  styleUrls: ['./proposal-teachers-reviewed-coordinator.component.css']
})

export class ProposalTeachersReviewedCoordinatorComponent implements OnInit {
  constructor(private route: ActivatedRoute) {
  }

  ngOnInit() {
    const internshipId = this.route.snapshot.paramMap.get('id');
    const url = 'https://localhost:3000/api/Internship/' + internshipId;

    const approvedSide = document.getElementById('approvedFeedback');
    const deniedSide = document.getElementById('deniedFeedback');

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
        console.log(data);
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
      });
  }

  postFeedback(isApproved) {
    const internshipId = this.route.snapshot.paramMap.get('id');
    const feedback = document.getElementById('feedback') as HTMLInputElement;
    let state = isApproved ? 5 : 6;
    let url = "https://localhost:3000/api/Internship/feedback/coordinator";
    let data = {internshipID: internshipId, feedback: feedback.value, reviewedState: state};

    fetch(url, {
      method: 'POST',
      body: JSON.stringify(data),
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('token')
      }
    })
      .then((response) => {
        if (response.status === 200) {
          alert("Feedback ingediend voor opdracht!");
          window.close();
        } else if (response.status === 401 || response.status === 403) {
          window.location.assign('/#/unauthorized-page');
        } else {
          response.json().then(errorJson => {
            throw new Error(`error with status ${response.status}`);
          });
        }
      });
  }

}
