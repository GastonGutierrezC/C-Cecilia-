import {Component, inject, OnInit} from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AsyncPipe } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import {MatMenu, MatMenuItem, MatMenuTrigger} from '@angular/material/menu';
import {RouterLink} from '@angular/router';
import {MatDialog} from '@angular/material/dialog';
import {LoginComponent} from '../dialog/login/login.component';

import {UserService} from '../service/user-service';
import {TokenData} from '../models/user';


@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatMenu,
    MatMenuTrigger,
    MatMenuItem,
    RouterLink,
  ]
})
export class LayoutComponent implements OnInit{

  protected readonly window = window;

  userService = inject(UserService);
  data: TokenData | undefined = undefined
  dialog = inject(MatDialog);
  ngOnInit() {
    this.data = this.userService.getTokenData()
  }


  login() {
    this.dialog.open(LoginComponent, {}).afterClosed().subscribe({
      next: () => {
      }
    });

  }

  logout() {
    this.userService.logout()
  }
}
