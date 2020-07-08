import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import { Router } from '@angular/router';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'proposal-coordinator-app',
  templateUrl: './proposal-company.component.html',
  styleUrls: ['./proposal-company.component.css']
})

export class ProposalCompanyComponent implements OnInit {

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
          case 1:
          case 2:
          case 3:
          case 4:
          case 5:
            this.router.navigate(['/proposal/company/basic/' + internshipId]);
            break;
          case 6:
            this.router.navigate(['/proposal/company/update/' + internshipId]);
            break;
        }
      });
  }
}
