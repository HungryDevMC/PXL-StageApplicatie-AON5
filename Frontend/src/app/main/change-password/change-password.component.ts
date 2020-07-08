import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {

  changePassword() {
    const oldPassword = (document.getElementById('oldPass') as HTMLInputElement).value;
    const newPassword1 = (document.getElementById('newPass1') as HTMLInputElement).value;
    const newPassword2 = (document.getElementById('newPass2') as HTMLInputElement).value;
    let output = document.getElementById('output');

    if (newPassword1 != newPassword2) {
      output.innerText = 'Wachtwoorden komen niet overeen'
    } else {
      const url = 'https://localhost:3000/api/authentication/password/change';

      const passwordChange = {
        oldPassword: oldPassword,
        newPassword: newPassword1
      };

      fetch(url, {
        method: 'POST',
        body: JSON.stringify(passwordChange),
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
          "Authorization": "Bearer " + localStorage.getItem('token')
        }
      })
        .then((response) => {
          if (response.status === 401 || response.status === 403) {
          window.location.assign('/#/unauthorized-page');
          }
          if (response.status === 200) {
            let role = localStorage.getItem('role');
            console.log(response);
            window.location.assign('/#/' + role);
          } else {
            response.json().then(errorJson => {
              document.getElementById('output').innerText = 'Error ' + response.status + ' - ' + errorJson.Status;
              throw new Error(`error with status ${response.status}`);
            });
          }

        });
    }
  }
}
