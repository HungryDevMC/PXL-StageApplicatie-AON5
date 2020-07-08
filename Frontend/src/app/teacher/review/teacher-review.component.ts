import {Component, Input, OnInit} from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'teacher-review-app',
  templateUrl: './teacher-review.component.html',
  styleUrls: ['./teacher-review.component.css']
})

export class TeacherReviewComponent {

  @Input() userId = localStorage.getItem('userId');

}

