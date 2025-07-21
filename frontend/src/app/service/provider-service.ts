import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment.development';
import {ProductData, ProductModel} from '../models/products';
import {ProviderData, ProviderModel} from '../models/provider';

@Injectable({
  providedIn: 'root'
})
export class ProviderService {

  private http = inject(HttpClient);
  api = environment.baseApiUrl;

  getProviders() {
    return this.http.get<ProviderModel[]>(this.api + '/provider');
  }

  createProvider(providerData: ProviderData) {
    return this.http.post<boolean>(this.api + '/provider', providerData);
  }
  editProvider(providerData: ProviderModel) {
    return this.http.put<boolean>(this.api + '/provider/' + providerData.id, providerData);
  }

  deleteProvider(id: number) {
    return this.http.delete<boolean>(this.api + '/provider/' + id);
  }
}
