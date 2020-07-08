import {Component, Input, OnInit, HostListener} from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'proposal-list',
  templateUrl: './proposal-list.component.html',
  styleUrls: ['./proposal-list.component.css']
})

export class ProposalListComponent implements OnInit {
  @Input() apiLink: string;

  constructor() {
  }

  @HostListener('window:focus', ['$event'])
    onFocus(event: any): void {
      let output = document.getElementById('list') as HTMLInputElement;
      output.innerHTML = '';
      this.ngOnInit();
    }

  ngOnInit() {
    const url = 'https://localhost:3000/api/' + this.apiLink;

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
        const output = document.createElement('ul');
        data.forEach(exercise => {
          const exerciseForm = exercise;
          const li = document.createElement('li');
          const item = document.createElement('proposal-item');
          item.setAttribute('id', exercise.id);
          item.setAttribute('title', exerciseForm.description);
          item.setAttribute('fieldOfStudy', exerciseForm.requiredFieldsOfStudy);
          item.setAttribute('toolsUsed', exerciseForm.environment);
          item.setAttribute('description', exercise.description);

          let descriptionSplit = exercise.description.split(' ');
          let summary = '';

          if (descriptionSplit.length < 10) {
            for (let i = 0; i < descriptionSplit.length; i++) {
              summary += ' ' + descriptionSplit[i]
            }
          } else {
            for (let i = 0; i < 10; i++) {
              summary += ' ' + descriptionSplit[i]
            }
            summary += '...';
          }

          item.innerText = 'Titel: ' + exerciseForm.title + ' --- Studierichting: ' + exerciseForm.requiredFieldsOfStudy + ' --- ' + exerciseForm.environment + '\n Description: ' + summary;
          const role = localStorage.getItem('role');
          if (role != "student" && role != "teacher") {

            let statusText = 'Ingediend';
            let color = '#004AB9';
            const statusDiv = document.createElement('div');
            switch (exerciseForm.internshipState) {
              case 1:
                statusText = 'Leerkrachten beoordelen';
                color = '#CEBE30';
                break;
              case 2:
                statusText = 'Leerkrachten goedgekeurd';
                color = '#0CB669';
                break;
              case 3:
                statusText = 'Leerkrachten afgekeurd';
                color = '#C72C12';
                break;
              case 4:
                statusText = 'Coördinator beoordeelt';
                color = '#ce9230';
                break;
              case 5:
                statusText = 'Goedgekeurd';
                color = '#0cb602';
                break;
              case 6:
                statusText = 'Afgekeurd';
                color = '#C72C12';
                break;
            }
            statusDiv.style.backgroundColor = color;
            statusDiv.classList.add('statusDiv');
            statusDiv.innerText = statusText;
            item.appendChild(statusDiv);
          }
          li.addEventListener('onmouseover', () => {
            li.style.backgroundColor = '#57a518';
          });
          li.addEventListener('click', () => {
            window.open(window.location.origin + '#/proposal/' + localStorage.getItem('role') + "/" + exercise.id);
          });
          li.appendChild(item);
          output.appendChild(li);
        });
        document.getElementById('list').appendChild(output);
      });
  }

}
