import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment.development';
import {ProductData, ProductModel} from '../models/products';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private http = inject(HttpClient);
  api = environment.baseApiUrl;

  getProducts() {
    return this.http.get<ProductModel[]>(this.api + '/product');
  }

  createProduct(productData: ProductData) {
    return this.http.post<boolean>(this.api + '/product', productData);
  }
  editProduct(productData: ProductModel) {
    return this.http.put<boolean>(this.api + '/product/' + productData.id, productData);
  }

  deleteProduct(id: number) {
    return this.http.delete<boolean>(this.api + '/product/' + id);
  }
}
