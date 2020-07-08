import {Component, Input, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'proposal-coordinator-app',
  templateUrl: './proposal-new-coordinator.component.html',
  styleUrls: ['./proposal-new-coordinator.component.css']
})

export class ProposalNewCoordinatorComponent implements OnInit {
  public static selectedTeachers = [];

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit() {

    const internshipId = this.route.snapshot.paramMap.get('id');
    const baseUrl = 'https://localhost:3000/api/Internship/teachers/';
    const linkedTeachersUrl = baseUrl + internshipId;
    let allTeachers = [];

    function addOrRemoveToArray(id) {
      const teacher = (document.getElementById(id) as HTMLInputElement);
      if (teacher.checked == true) {
        ProposalNewCoordinatorComponent.selectedTeachers.push(teacher.id);
      } else {
        ProposalNewCoordinatorComponent.selectedTeachers.splice(ProposalNewCoordinatorComponent.selectedTeachers.indexOf(id), 1);
      }
    }

    fetch(baseUrl, {
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
        const output = document.createElement('ul');
        output.style.padding = "0";
        data.forEach(function (teacher) {
          allTeachers.push(teacher.id);
          const li = document.createElement('li');
          li.style.color = 'white';
          const input = document.createElement('input');

          input.type = "checkbox";
          input.id = teacher.id;
          input.addEventListener('click', () => {
            addOrRemoveToArray(teacher.id);
          });
          li.appendChild(input);
          li.append(teacher.firstName + " " + teacher.lastName);
          output.appendChild(li);
        });
        document.getElementById('teachersOutput').appendChild(output);
      });

    fetch(linkedTeachersUrl, {
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
        const output = document.createElement('ul');
        output.style.padding = "0";
        data.forEach(function (teacher) {
          allTeachers.push(teacher.id);
          const li = document.createElement('li');
          li.style.color = 'white';
          const input = document.createElement('input');

          input.type = "checkbox";
          input.id = teacher.id;
          input.addEventListener('click', () => {
            addOrRemoveToArray(teacher.id);
          });
          li.appendChild(input);
          li.append(teacher.firstName + " " + teacher.lastName);
          output.appendChild(li);
        });
        document.getElementById('teachersOutput').appendChild(output);
      })
  }


  postSelectedTeachers() {
    const url = 'https://localhost:3000/api/Internship/teachers/assign';
    const internshipId = this.route.snapshot.paramMap.get('id');

    console.log(ProposalNewCoordinatorComponent.selectedTeachers);

    const data = {teachers: ProposalNewCoordinatorComponent.selectedTeachers, internshipID: internshipId};

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
          alert("Leerkrachten toegewezen aan opdracht!");
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

  deleteProposal() {
    const url = 'https://localhost:3000/api/Internship/remove/';
    const internshipId = this.route.snapshot.paramMap.get('id');

    fetch(url + internshipId, {
      method: 'DELETE',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('token')
      }
    })
      .then((response) => {
        if (response.status === 200) {
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
