import {HttpClient, HttpParams} from '@angular/common/http';
import {inject, Injectable, signal} from '@angular/core';

import {Router} from '@angular/router';
import {loginResponse, TokenData, UserData} from '../models/user';
import {environment} from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  api = environment.baseApiUrl;
  private router = inject(Router)
  currentUserId = signal<TokenData | null>(null)

  logout() {
    localStorage.removeItem('token');
    this.router.navigateByUrl('/')
    window.location.reload();
  }
  getTokenData() {
    let value = localStorage.getItem('token')
    if (value) {
      let jwtString = atob(value.split('.')[1]);
      let jwt: TokenData = JSON.parse(jwtString)
      this.currentUserId.set(jwt)
      return jwt
    }
    return undefined
  }

  login(values: UserData) {
    return this.http.post<loginResponse>(`${environment.baseApiUrl}/User/login`, values)
  }
}
