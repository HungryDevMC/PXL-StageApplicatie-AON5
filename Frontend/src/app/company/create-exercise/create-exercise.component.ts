import { Component } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'company-update-exercise',
  templateUrl: './create-exercise.component.html',
  styleUrls: ['./create-exercise.component.css']
})

export class CompanyCreateExerciseComponent {

  createExercise() {
    const url = 'https://localhost:3000/api/Internship';

    const title = (document.getElementById('title') as HTMLInputElement);

    const study = [];

    if ((document.getElementById('AON') as HTMLInputElement).checked === true) {
      study.push('AON');
    }
    if ((document.getElementById('SNB') as HTMLInputElement).checked === true) {
      study.push('SNB');
    }
    if ((document.getElementById('SWM') as HTMLInputElement).checked === true) {
      study.push('SWM');
    }
    if ((document.getElementById('AI') as HTMLInputElement).checked === true) {
      study.push('AI');
    }

    const description = (document.getElementById('description') as HTMLInputElement).value;
    const environment = [];

    if ((document.getElementById('java') as HTMLInputElement).checked === true) {
      environment.push('Java');
    }
    if ((document.getElementById('net') as HTMLInputElement).checked === true) {
      environment.push('.Net');
    }
    if ((document.getElementById('web') as HTMLInputElement).checked === true) {
      environment.push('Web');
    }
    if ((document.getElementById('mobile') as HTMLInputElement).checked === true) {
      environment.push('Mobile');
    }
    if ((document.getElementById('systems') as HTMLInputElement).checked === true) {
      environment.push('Systemen en netwerken');
    }
    if ((document.getElementById('software') as HTMLInputElement).checked === true) {
      environment.push('Software testing');
    }
    if ((document.getElementById('other') as HTMLInputElement).checked === true) {
      environment.push((document.getElementById('otherText') as HTMLInputElement).value);
    }

    const technicalDescription =  (document.getElementById('technicalDescription') as HTMLInputElement).value;
    const extraRequirements =  (document.getElementById('extraRequirements') as HTMLInputElement).value;
    const theme =  (document.getElementById('theme') as HTMLInputElement).value;
    const activities = [];

    if ((document.getElementById('interview') as HTMLInputElement).checked === true) {
      activities.push('Sollicitatiegesprek');
    }
    if ((document.getElementById('CV') as HTMLInputElement).checked === true) {
      activities.push('CV');
    }
    if ((document.getElementById('commute') as HTMLInputElement).checked === true) {
      activities.push('Vergoeding /  tegemoetkoming in verplaatsingskosten');
    }

    let amountOfStudents;

    if ((document.getElementById('1student') as HTMLInputElement).checked === true) {
      amountOfStudents = 1;
    } else {
      amountOfStudents = 2;
    }

    const names = (document.getElementById('names') as HTMLInputElement).value.split(',');
    for (let i = 0; i < names.length; i++) {
      names[i] = names[i].trim();
    }

    const remarks =  (document.getElementById('remarks') as HTMLInputElement).value;

    const period = [];

    if ((document.getElementById('sem1') as HTMLInputElement).checked === true) {
      period.push('Semester 1 (oktober - januari)');
    }
    if ((document.getElementById('sem2') as HTMLInputElement).checked === true) {
      period.push('Semester 2 (februari - juni)');
    }

    const exercise = {
      title: title.value,
      requiredFieldsOfStudy: study,
      assignedStudents: names,
      environment,
      technicalDescription: technicalDescription,
      extraRequirements: extraRequirements,
      researchTheme: theme,
      activities: activities,
      requiredStudentsAmount: amountOfStudents,
      additionalRemarks: remarks,
      periodOfInternship: period,
      description
    };

    console.log(JSON.stringify(exercise));

    /*
    Status legend

    BeingProcessed = 0,
    InReviewByTeacher = 1,
    ApprovedByTeacher = 2,
    Rejected = 3,
    InReviewByCoordinator = 4,
    ApprovedByAll = 5
     */

    fetch(url, {
      method: 'POST',
      body: JSON.stringify(exercise),
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        "Authorization": "Bearer " + localStorage.getItem('token')
      }
    })
      .then((response) => {
        if (response.status === 200) {
          console.log(response);
          window.location.assign('/#/company/home');
        } else if (response.status === 401 || response.status === 403) {
          window.location.assign('/#/unauthorized-page');
        } else {
          response.json().then(errorJson => {
            document.getElementById('output').innerText = 'Error ' + response.status + ' - ' + errorJson.Status;
            throw new Error(`error with status ${response.status}`);
          });
        }

      });

  }

}
