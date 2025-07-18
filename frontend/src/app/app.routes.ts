import { Routes } from '@angular/router';
import { HomeComponent } from './page/home/home.component';
import {ThirdProductComponent} from './page/third-product/third-product.component';
import {IngredientComponent} from './page/ingredient/ingredient.component';
import {HomemadeProductsComponent} from './page/homemade-products/homemade-products.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'third-products', component: ThirdProductComponent },
  { path: 'ingredients', component: IngredientComponent },
  { path: 'homemade-products', component: HomemadeProductsComponent },
];
