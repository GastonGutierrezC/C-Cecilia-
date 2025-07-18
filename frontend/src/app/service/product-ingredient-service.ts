import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment.development';
import {ProductData, ProductModel} from '../models/products';
import {
  HomeMadeProductContentModel,
  HomeMadeProductData,
  HomeMadeProductModel,
  ProductIngredientData,
  ProductIngredientModel
} from '../models/product-ingredient';

@Injectable({
  providedIn: 'root'
})
export class ProductIngredientService {

  private http = inject(HttpClient);
  api = environment.baseApiUrl;

  getProducts() {
    return this.http.get<HomeMadeProductContentModel[]>(this.api + '/productingredient/homemade');
  }

  createProduct(productData: HomeMadeProductData) {
    return this.http.post<boolean>(this.api + '/productingredient/homemade/', productData);
  }
  editProduct(productData: HomeMadeProductModel) {
    return this.http.put<boolean>(this.api + '/productingredient/homemade/' + productData.product.id, productData);
  }

  deleteProduct(id: number) {
    return this.http.delete<boolean>(this.api + '/productingredient/homemade/' + id);
  }
}
