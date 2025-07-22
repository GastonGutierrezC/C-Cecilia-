import {Component, inject} from '@angular/core';
import {MatDialogContent, MatDialogRef} from '@angular/material/dialog';
import {MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatButton} from '@angular/material/button';
import {UserService} from "../../service/user-service";

@Component({
  selector: 'app-login',
  imports: [
    MatDialogContent,
    MatFormField,
    MatLabel,
    MatInput,
    MatFormField,
    MatLabel,
    MatButton,
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  readonly dialogRef = inject(MatDialogRef<LoginComponent>);

  userService = inject(UserService);
  userForm = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    username: new FormControl<string>('', [Validators.required]),
  })

    login() {
      if (this.userForm.valid
        && this.userForm.value.email !== undefined
        && this.userForm.value.email !== null
        && this.userForm.value.username !== undefined
        && this.userForm.value.username !== null)
      {
        this.userService.login({
          email: this.userForm.value.email,
          username: this.userForm.value.username,
        }).subscribe({
          next: (res) => {
            localStorage.setItem('token', res.token);
            this.userService.getTokenData()
            window.location.reload();
          }
        })

      }

    }
}
