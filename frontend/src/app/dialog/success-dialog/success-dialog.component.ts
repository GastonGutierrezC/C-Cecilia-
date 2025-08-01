import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

export interface SuccessDialogData {
  title?: string;
  message: string;
  icon?: string;
  buttonText?: string;
  width?: string;
}

@Component({
  selector: 'app-success-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule],
  templateUrl: './success-dialog.component.html',
  styleUrls: ['./success-dialog.component.scss']
})
export class SuccessDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<SuccessDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SuccessDialogData
  ) {
    this.data = {
      title: data.title || '¡Éxito!',
      message: data.message,
      icon: data.icon || 'check_circle',
      buttonText: data.buttonText || 'Aceptar',
      width: data.width || '400px'
    };
  }

  closeDialog(): void {
    this.dialogRef.close(true);
  }
}
