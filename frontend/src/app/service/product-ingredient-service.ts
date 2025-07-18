import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment.development';
import {ProductData, ProductModel} from '../models/products';
import {ProductIngredientData, ProductIngredientModel} from '../models/product-ingredient';

@Injectable({
  providedIn: 'root'
})
export class ProductIngredientService {

  private http = inject(HttpClient);
  api = environment.baseApiUrl;

  getProducts() {
    return this.http.get<ProductIngredientModel[]>(this.api + '/productingredient');
  }

  createProduct(productData: ProductIngredientData) {
    return this.http.post<boolean>(this.api + '/productingredient', productData);
  }
  editProduct(productData: ProductIngredientModel) {
    return this.http.put<boolean>(this.api + '/productingredient/' + productData.id, productData);
  }
}
