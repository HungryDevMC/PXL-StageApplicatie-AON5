import { Component } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'register-pxl-component',
  templateUrl: './register-pxl.component.html',
  styleUrls: ['./register-pxl.component.css']
})

export class RegisterPxlComponent {
  registerAccount() {
    const url = 'https://localhost:3000/api/authentication/register';

    const password1 = (document.getElementById('password') as HTMLInputElement).value;
    const password2 = (document.getElementById('password2') as HTMLInputElement).value;
    const surname = (document.getElementById('surname') as HTMLInputElement).value;
    const name = (document.getElementById('name') as HTMLInputElement).value;

    if (password1 === password2) {

    const registerJson = {
      email: (document.getElementById('email') as HTMLInputElement).value,
      password: password1,
      firstName: surname,
      lastName: name
    };

    fetch(url, {
      method: 'POST',
      body: JSON.stringify(registerJson),
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + localStorage.getItem('token')
      }
    })
      .then((response) => {
        if (response.status === 200) {
          console.log(response);
          window.location.assign('/#/login');
        } else {
          response.json().then(errorJson => {
            document.getElementById('output').innerText = 'Error ' + response.status + ' - ' + errorJson.Status;
            throw new Error(`error with status ${response.status}`);
          });
        }
      });
    } else {
      document.getElementById('output').innerText = 'Error passwords do not match!';
    }
  }
}

