import {Component, OnInit} from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'main-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class MainHeaderComponent implements OnInit {

  ngOnInit() {
    let account = document.getElementById('logout');
    if (localStorage.getItem('name') == null) {
      account.style.display = "none";
    } else {

      let name = document.getElementById('name');
      name.innerHTML = localStorage.getItem('name');

      let account = document.getElementById('logout');
      account.style.display = "block";
    }
  }

  logout() {
    window.location.assign('/#/');
    window.localStorage.clear();
  }

  homePage() {
    if (localStorage.getItem('role') != null) {
      window.location.assign('/#/' + localStorage.getItem('role'));
    }
  }
}
