import {Component, inject} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {MatDialogActions, MatDialogContent, MatDialogRef, MatDialogTitle} from "@angular/material/dialog";

@Component({
  selector: 'app-resize-image-dialog',
    imports: [
        MatButton,
        MatDialogActions,
        MatDialogContent,
        MatDialogTitle
    ],
  templateUrl: './resize-image-dialog.component.html',
  styleUrl: './resize-image-dialog.component.scss'
})
export class ResizeImageDialogComponent {
  readonly dialogRef = inject(MatDialogRef<ResizeImageDialogComponent>);
}
