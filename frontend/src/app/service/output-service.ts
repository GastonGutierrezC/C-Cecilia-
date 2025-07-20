import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment.development';
import {OutputData} from '../models/output';

@Injectable({
  providedIn: 'root'
})
export class OutputService {

  private http = inject(HttpClient);
  api = environment.baseApiUrl;

  createOutputProduct(data: OutputData[]){
    return this.http.post<boolean>(this.api + '/OutputProduct/createOutputProducts', data);
  }

  createOutputIngredient(data: OutputData[]){
    return this.http.post<boolean>(this.api + '/OutputProduct/createOutputIngredients', data);
  }
}
