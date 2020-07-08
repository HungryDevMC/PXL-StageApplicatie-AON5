import {Component, OnInit} from '@angular/core';
import Chart from 'chart.js';
import {getSyntheticPropertyName} from "@angular/compiler/src/render3/util";

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'coordinator-home',
  templateUrl: './coordinator-home.component.html',
  styleUrls: ['./coordinator-home.component.css']
})
export class CoordinatorHomeComponent implements OnInit {
  onSubmit() {
    let uploadFile = document.getElementById('upload') as HTMLInputElement;

    if (uploadFile.files.length != 1) {
      return;
    }

    const formData = new FormData();
    formData.append('file', uploadFile.files[0]);

    fetch("https://localhost:3000/api/authentication/register/pxl/csv", {
      method: 'POST',
      body: formData
    })
      .then((response) => {
        if (response.status == 200) {
          window.alert("Accounts succesvol toegevoegd!");
        } else if (response.status === 401 || response.status === 403) {
          window.location.assign('/#/unauthorized-page');
        } else {
          window.alert('Fout error: ' + response.status);
        }
      })
  }

  ngOnInit() {
    function postValidate(isValidated, userAccount) {
      let url = 'https://localhost:3000/api/authentication/validate';
      let validateJson = {validated: isValidated, id: userAccount};
      fetch(url, {
        method: 'POST',
        body: JSON.stringify(validateJson),
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + localStorage.getItem('token')
        }
      })
        .then((response) => {
          if (response.status === 401 || response.status === 403) {
            window.location.assign('/#/unauthorized-page');
          }
          if (response.status == 200) {
            alert('Account has been ' + (isValidated ? "activated!" : "deactivated!"));
          }
        })

    }

    let url = 'https://localhost:3000/api/Internship/all';

    let ingediendCount = 0;
    let lBeoordelenCount = 0;
    let lGoedgekeurdCount = 0;
    let lAfgekeurdCount = 0;
    let cBeordeeltCount = 0;
    let goedgekeurdCount = 0;
    let afgekeurdCount = 0;

    fetch(url, {
      headers: {
        "Authorization": "Bearer " + localStorage.getItem('token')
      }
    })
      .then((response) => {
        if(response.status === 401 || response.status === 403) {
          window.location.assign('/#/unauthorized-page');
        }
        return response.json();
      })
      .then((data) => {
        data.forEach(exercise => {

          switch (exercise.internshipState) {
            case 0:
              ingediendCount++;
              break;
            case 1:
              lBeoordelenCount++;
              break;
            case 4:
              cBeordeeltCount++;
              break;
            case 5:
              goedgekeurdCount++;
              break;
            case 6:
              afgekeurdCount++;
              break;
          }
        });

        var ctx = document.getElementById('myChart');
        var myChart = new Chart(ctx, {
          type: 'doughnut',
          data: {
            labels: ['Ingediend', 'Leerkrachten beoordelen', 'Coördinator beoordeelt', 'Goedgekeurd', 'Afgekeurd'],
            datasets: [{
              label: 'Alle opdrachten',
              data: [ingediendCount, lBeoordelenCount, cBeordeeltCount, goedgekeurdCount, afgekeurdCount],
              backgroundColor: [
                'rgba(0, 74, 185, 0.8)',
                'rgba(206, 146, 48, 0.8)',
                'rgba(206, 146, 48, 0.8)',
                'rgba(12, 182, 2, 0.8)',
                'rgba(199, 44, 18, 0.8)'
              ],
              borderColor: [
                'rgba(0, 74, 185, 0.6)',
                'rgba(206, 146, 48, 0.6)',
                'rgba(206, 146, 48, 0.6)',
                'rgba(12, 182, 2, 0.6)',
                'rgba(199, 44, 18, 0.6)'
              ],
              borderWidth: 1
            }]
          },
          options: {}
        });
      });

    url = 'https://localhost:3000/api/authentication/unvalidated';

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
        data.forEach(account => {
          let output = document.createElement('tr');
          let firstName = document.createElement('td');
          firstName.innerHTML = account.firstName;
          output.appendChild(firstName);

          let lastName = document.createElement('td');
          lastName.innerHTML = account.lastName;
          output.appendChild(lastName);

          let acceptButton = document.createElement('td');
          acceptButton.innerHTML = '<button>Goedkeuren</button>';
          acceptButton.addEventListener('click', function (event) {
            postValidate(true, account.id);
          });
          output.appendChild(acceptButton);

          let declineButton = document.createElement('td');
          declineButton.innerHTML = '<button>Afkeuren</button>';
          declineButton.addEventListener('click', function (event) {
            postValidate(false, account.id);
          });
          output.appendChild(declineButton);

          document.getElementById('accountList').appendChild(output);
        });
      });


  }


}
