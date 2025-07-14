import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment.development';
import {ProductData, ProductModel} from '../models/products';
import {IngredientData, IngredientModel} from '../models/ingredient';

@Injectable({
  providedIn: 'root'
})
export class IngredientService {

  private http = inject(HttpClient);
  api = environment.baseApiUrl;

  getIngredients() {
    return this.http.get<IngredientModel[]>(this.api + '/ingredient');
  }

  createIngredient(productData: IngredientData) {
    return this.http.post<boolean>(this.api + '/ingredient', productData);
  }
  editIngredient(productData: IngredientModel) {
    return this.http.put<boolean>(this.api + '/ingredient/' + productData.id, productData);
  }

  deleteIngredient(id: number) {
    return this.http.delete<boolean>(this.api + '/ingredient/' + id);
  }
}
