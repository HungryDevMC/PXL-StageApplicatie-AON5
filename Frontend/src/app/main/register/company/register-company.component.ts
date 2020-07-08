import {Component, Input, OnInit} from '@angular/core';
// @ts-ignore
import {} from 'googlemaps';

let map;
let map2;
let marker = null;
let marker2 = null;

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'register-company-component',
  templateUrl: './register-company.component.html',
  styleUrls: ['./register-company.component.css']
})

export class RegisterCompanyComponent implements OnInit {
  @Input() companyPersonTitle: string;
  @Input() companyPersonName: string;
  @Input() companyPersonSurname: string;
  @Input() companyNumber: string;
  @Input() companyEmail: string;

  @Input() contactPersonTitle: string;
  @Input() contactPersonName: string;
  @Input() contactPersonSurname: string;
  @Input() contactNumber: string;
  @Input() contactEmail: string;

  ngOnInit() {
    this.initMap();
  }

  initMap() {
    const centerOfMap = new google.maps.LatLng(50.84820885219421, 4.3443020041482105);

    const options = {
      center: centerOfMap,
      zoom: 7
    };

    map = new google.maps.Map(document.getElementById('mapCompany'), options);
    map2 = new google.maps.Map(document.getElementById('mapStage'), options);

    // Listen for any clicks on the map.
    google.maps.event.addListener(map, 'click', event => {
      // Get the location that the user clicked.
      const clickedLocation = event.latLng;
      // If the marker hasn't been added.
      if (marker === null) {
        // Create the marker.
        marker = new google.maps.Marker({
          position: clickedLocation,
          map,
          draggable: true
        });
        // tslint:disable-next-line:no-shadowed-variable
        google.maps.event.addListener(marker, 'dragend', event => {
          this.markerLocation();
        });
      } else {
        marker.setPosition(clickedLocation);
      }
      this.markerLocation();
    });

    google.maps.event.addListener(map2, 'click', event => {
      // Get the location that the user clicked.
      const clickedLocation = event.latLng;
      // If the marker hasn't been added.
      if (marker2 === null) {
        // Create the marker.
        marker2 = new google.maps.Marker({
          position: clickedLocation,
          map: map2,
          draggable: true
        });
        // tslint:disable-next-line:no-shadowed-variable
        google.maps.event.addListener(marker2, 'dragend', event => {
          this.markerLocation2();
        });
      } else {
        marker2.setPosition(clickedLocation);
      }
      this.markerLocation2();
    });
  }

  markerLocation() {
    const currentLocation = marker.getPosition();
    (document.getElementById('lat1') as HTMLInputElement).value = currentLocation.lat();
    (document.getElementById('lng1') as HTMLInputElement).value = currentLocation.lng();
  }

  markerLocation2() {
    const currentLocation = marker2.getPosition();
    (document.getElementById('lat2') as HTMLInputElement).value = currentLocation.lat();
    (document.getElementById('lng2') as HTMLInputElement).value = currentLocation.lng();
  }

  registerCompany() {
    const url = 'https://localhost:3000/api/authentication/register/company';

    const output = (document.getElementById("output") as HTMLOutputElement);

    const password = (document.getElementById("password") as HTMLInputElement).value;
    const password2 = (document.getElementById("passwordAgain") as HTMLInputElement).value;

    if (password === password2) {

      const company = {
        companyName: (document.getElementById("companyName") as HTMLInputElement).value,
        password: password,
        employeeCount: (document.getElementById("employeeCount") as HTMLInputElement).value,
        itEmployeeCount: (document.getElementById("itEmployeeCount") as HTMLInputElement).value,
        supportingITEmployees: (document.getElementById("supportingItEmployees") as HTMLInputElement).value,
        lat1: (document.getElementById('lat1') as HTMLInputElement).value,
        lng1: (document.getElementById('lng1') as HTMLInputElement).value,
        lat2: (document.getElementById('lat2') as HTMLInputElement).value,
        lng2: (document.getElementById('lng2') as HTMLInputElement).value,
        contact_Title: this.contactPersonTitle,
        contact_Name: this.contactPersonName,
        contact_Surname: this.contactPersonSurname,
        contact_Email: this.contactEmail,
        contact_Number: this.contactNumber,
        company_Title: this.companyPersonTitle,
        company_Name: this.companyPersonName,
        company_Surname: this.companyPersonSurname,
        company_Email: this.companyEmail,
        company_Number: this.companyNumber
      };

      console.log(company);

      fetch(url, {
        method: 'POST',
        body: JSON.stringify(company),
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + localStorage.getItem('token')
        }
      })
        .then((response) => {
          if (response.status == 200) {
            alert('Confirm by clicking on the links in the emails that have been sent to the accounts!');
            window.location.assign(window.location.href + "/#/login")
          }
        })

    } else {
      output.style.color = "red";
      output.innerText = "Passwords don't match!";
    }
  }
}

