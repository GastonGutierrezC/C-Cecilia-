import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment.development';
import {OutputData} from '../models/output';

@Injectable({
  providedIn: 'root'
})
export class InputService {

  private http = inject(HttpClient);
  api = environment.baseApiUrl;

  createInputProductAndIngredients(data: OutputData[]){
    return this.http.post<boolean>(this.api + '/input/register', data);
  }

}
