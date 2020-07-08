import {Component} from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'proposal-teacher-app',
  templateUrl: './proposal-teacher.component.html',
  styleUrls: ['./proposal-teacher.component.css']
})

export class ProposalTeacherComponent {
  constructor(private route: ActivatedRoute) {
  }

  postFeedback(isApproved) {
    const internshipId = this.route.snapshot.paramMap.get('id');
    const feedback = document.getElementById('feedback') as HTMLInputElement;

    let state = isApproved ? 2 : 3;

    let url = "https://localhost:3000/api/Internship/feedback/teacher";
    let data = {internshipID: internshipId, teacherId: localStorage.getItem('userId'), feedback: feedback.value, reviewedState: state};

    console.log(data);
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
        } else if(response.status === 401 || response.status === 403) {
          window.location.assign('/#/unauthorized-page');
        } else {
          response.json().then(errorJson => {
            throw new Error(`error with status ${response.status}`);
          });
        }
      });
  }

}

