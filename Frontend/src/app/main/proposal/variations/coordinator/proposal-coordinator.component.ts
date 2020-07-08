import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'proposal-coordinator-app',
  templateUrl: './proposal-coordinator.component.html',
  styleUrls: ['./proposal-coordinator.component.css']
})

export class ProposalCoordinatorComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router) {
  }

  ngOnInit() {

    const internshipId = this.route.snapshot.paramMap.get('id');
    const url = 'https://localhost:3000/api/Internship/' + internshipId;

    fetch(url, {
      headers: {
        "Authorization": "Bearer " + localStorage.getItem('token')
      }
    })
      .then((response) => {
        if(response.status === 401 || response.status === 403) {
          window.location.assign('/#/unauthorized-page');
        }
        return response.json()
      })
      .then((data) => {
        switch(data.internshipState) {

          case 0:
            this.router.navigate(['/proposal/coordinator/new/' + internshipId]);
            break;
          case 1:
          case 2:
          case 3:
            this.router.navigate(['/proposal/coordinator/basic/' + internshipId]);
            break;
          case 4:
            this.router.navigate(['/proposal/coordinator/teachers_reviewed/' + internshipId]);
            break;
          case 5:
          case 6:
            this.router.navigate(['/proposal/coordinator/basic/' + internshipId]);
            break;
          }
      });
  }
}
