import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'email-verification',
  templateUrl: './email-verification.component.html',
  styleUrls: ['./email-verification.component.css']
})

export class EmailVerificationComponent implements OnInit {

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit() {
    let output = document.getElementById('output') as HTMLOutputElement;

    let url = 'https://localhost:3000/api/authentication/verify';

    const splitUrl = window.location.href.split('&');

    let verifyToken = splitUrl[0].split("t=")[1];
    let user = splitUrl[1].replace('u=', '');
    //verifyToken = decodeURIComponent(verifyToken);
    console.log(verifyToken);
    console.log(user);

    fetch(url, {
      method: 'POST',
      body: JSON.stringify({id: user, verificationToken: verifyToken}),
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json'
      }
    })
      .then((response) => {
        if (response.status === 401 || response.status === 403) {
          window.location.assign('/#/unauthorized-page');
        }
        if (response.status === 200) {
          let h1 = document.createElement('h1');
          h1.textContent = 'Email verificatie is gelukt!';
          let p = document.createElement('p');
          p.textContent = 'Je account moet nu goedgekeurd worden door de coordinator. Eenmaal dit goedgekeurd is, verkrijg je toegang tot het stageplatform.';
          output.append(h1);
          output.append(p);
        } else {
          response.json().then(errorJson => {
            let h1 = document.createElement('h1');
            h1.textContent = 'Error ' + response.status + ' - ' + errorJson.content;
            output.append(h1);
            throw new Error(`error with status ${response.status}`);
          });
        }

      });
  }

}
