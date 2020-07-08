import {Component, OnInit} from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'coordinator-userview',
  templateUrl: './coordinator-userview.component.html',
  styleUrls: ['./coordinator-userview.component.css']
})
export class CoordinatorUserViewComponent implements OnInit {
  public static selectedAccounts = [];

  ngOnInit() {
    const companyOutput = document.getElementById('companyOutput') as HTMLOutputElement;
    const teacherOutput = document.getElementById('teacherOutput') as HTMLOutputElement;
    const studentOutput = document.getElementById('studentOutput') as HTMLOutputElement;

    function addOrRemoveToArray(id) {
      const user = (document.getElementById(id) as HTMLInputElement);
      if (user.checked == true) {
        CoordinatorUserViewComponent.selectedAccounts.push(user.id);
      } else {
        CoordinatorUserViewComponent.selectedAccounts.splice(CoordinatorUserViewComponent.selectedAccounts.indexOf(id), 1);
      }
    }

    const url = 'https://localhost:3000/api/authentication/users/';

    fetch(url + "student", {
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
        const output = document.createElement('ul');
        output.style.padding = "0";
        data.forEach(function (user) {
          const li = document.createElement('li');
          li.style.color = 'white';
          const input = document.createElement('input');

          input.type = "checkbox";
          input.id = user.id;
          input.addEventListener('click', () => {
            addOrRemoveToArray(user.id);
          });
          li.appendChild(input);
          li.append(user.firstName + " " + user.lastName);
          output.appendChild(li);
        });
        studentOutput.appendChild(output);
      });

    fetch(url + "company", {
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
        const output = document.createElement('ul');
        output.style.padding = "0";
        data.forEach(function (user) {
          const li = document.createElement('li');
          li.style.color = 'white';
          const input = document.createElement('input');

          input.type = "checkbox";
          input.id = user.id;
          input.addEventListener('click', () => {
            addOrRemoveToArray(user.id);
          });
          li.appendChild(input);
          li.append(user.firstName + " " + user.lastName);
          output.appendChild(li);
        });
        companyOutput.appendChild(output);
      });

    fetch(url + "teacher", {
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
        const output = document.createElement('ul');
        output.style.padding = "0";
        data.forEach(function (user) {
          const li = document.createElement('li');
          li.style.color = 'white';
          const input = document.createElement('input');

          input.type = "checkbox";
          input.id = user.id;
          input.addEventListener('click', () => {
            addOrRemoveToArray(user.id);
          });
          li.appendChild(input);
          li.append(user.firstName + " " + user.lastName);
          output.appendChild(li);
        });
        teacherOutput.appendChild(output);
      });
  }

  deactivateAccounts() {
    let url = 'https://localhost:3000/api/authentication/validate';
    CoordinatorUserViewComponent.selectedAccounts.forEach(function (user) {
      let validateJson = {validated: false, id: user};
      fetch(url, {
        method: 'POST',
        body: JSON.stringify(validateJson),
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + localStorage.getItem('token')
        }
      }).then((response) => {
        if(response.status === 401 || response.status === 403) {
          window.location.assign('/#/unauthorized-page');
        }
      })
    });
    alert("Accounts deactivated!");
    console.log(CoordinatorUserViewComponent.selectedAccounts)
  }

}
