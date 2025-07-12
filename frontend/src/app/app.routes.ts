import { Routes } from '@angular/router';
import { HomeComponent } from './page/home/home.component';
import {ThirdProductComponent} from './page/third-product/third-product.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'third-products', component: ThirdProductComponent },
];
