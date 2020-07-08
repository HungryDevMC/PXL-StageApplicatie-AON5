import { Component } from '@angular/core';
import { Router } from "@angular/router";

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'login-component',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent {
  constructor(private router: Router) {}

  verifyLogin() {
    const url = 'https://localhost:3000/api/authentication/login';

    const loginJson = {
      email: (document.getElementById('email') as HTMLInputElement).value,
      password: (document.getElementById('password') as HTMLInputElement).value};

    fetch(url, {
      method: 'POST',
      body: JSON.stringify(loginJson),
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json'
      }
    })
      .then((response) => {
        if (response.status === 200) {
          response.json().then(accountJson => {
            localStorage.setItem('token', accountJson.token);
            localStorage.setItem('role', accountJson.role.toString().toLowerCase());
            localStorage.setItem('userId', accountJson.user.id);
            localStorage.setItem('name', accountJson.user.firstName + " " + accountJson.user.lastName);
            this.router.navigate(['/' + accountJson.role.toString().toLowerCase() + "/"]);
          });
        } else {
          response.json().then(errorJson => {
            document.getElementById('output').innerText = 'Error ' + response.status + ' - Invalid email or password';
            throw new Error(`error with status ${response.status}`);
          });
        }

      });
  }
}

