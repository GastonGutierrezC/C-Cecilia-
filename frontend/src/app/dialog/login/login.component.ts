import {Component, inject} from '@angular/core';
import {MatDialogContent, MatDialogRef} from '@angular/material/dialog';
import {MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {MatButton} from '@angular/material/button';

@Component({
  selector: 'app-login',
  imports: [
    MatDialogContent,
    MatFormField,
    MatLabel,
    MatInput,
    MatFormField,
    MatLabel,
    MatButton
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  readonly dialogRef = inject(MatDialogRef<LoginComponent>);

  userForm = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    username: new FormControl<string>('', [Validators.required]),
  })
}
