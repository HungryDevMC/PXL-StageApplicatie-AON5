import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'proposal-basic-app',
  templateUrl: './proposal-basic.component.html',
  styleUrls: ['./proposal-basic.component.css']
})

export class ProposalBasicComponent implements OnInit {
  @Input() description: string;
  @Input() fieldsOfStudy: string[];
  @Input() toolsUsed: string[];
  @Input() toolsInformation: string;
  @Input() necessities: string[];
  @Input() theme: string;
  @Input() activities: string[];
  @Input() requiredStudentAmount: number;
  @Input() optionalComment: string;
  @Input() periods: string[];
  @Input() title: string[];

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');

    const url = 'https://localhost:3000/api/Internship/' + id;

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
        this.description = data.description;
        const descriptionElement = document.getElementById('description') as HTMLInputElement;
        descriptionElement.value = data.description;
        this.fieldsOfStudy = data.requiredFieldsOfStudy;
        this.toolsUsed = data.environment;
        this.toolsInformation = data.technicalDescription;
        this.necessities = data.extraRequirements;
        this.theme = data.researchTheme;
        this.activities = data.activities;
        this.requiredStudentAmount = data.requiredStudentsAmount;
        this.optionalComment = data.additionalRemarks;
        this.periods = data.periods;
        this.title = data.title;
      });
  }

}

