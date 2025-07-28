import { Component, computed } from '@angular/core';
import {MatButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {RouterLink} from '@angular/router';
import { UserService } from '../../service/user-service';

@Component({
  selector: 'app-home',
  imports: [
    MatButton,
    MatIcon,
    RouterLink,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  constructor(private userService: UserService) {
    if(!this.userService.currentUserId()) {
      this.userService.getTokenData();
    }
  }

  userName = computed(() => this.userService.currentUserId()?.Username || null)
}
