import {Component, inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogContent, MatDialogRef} from '@angular/material/dialog';
import {HomeMadeProductContentModel} from '../../models/product-ingredient';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-complete-task',
  imports: [
    MatDialogContent,
    RouterLink
  ],
  templateUrl: './complete-task.component.html',
  styleUrl: './complete-task.component.scss'
})
export class CompleteTaskComponent {
  readonly dialogRef = inject(MatDialogRef<CompleteTaskComponent>);
  data = inject<string>(MAT_DIALOG_DATA);
}
