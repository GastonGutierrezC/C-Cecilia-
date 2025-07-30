import { Routes } from '@angular/router';
import { HomeComponent } from './page/home/home.component';
import {ThirdProductComponent} from './page/third-product/third-product.component';
import {IngredientComponent} from './page/ingredient/ingredient.component';
import {HomemadeProductsComponent} from './page/homemade-products/homemade-products.component';
import {OutputComponent} from './page/output/output.component';
import {InputComponent} from './page/input/input.component';
import {EconomicMetricComponent} from './page/economic-metric/economic-metric.component';
import {authGuard} from './guard/auth.guard';
import {ProductMetricComponent} from './page/product-metric/product-metric.component';
import {ProviderMetricsComponent} from './page/provider-metrics/provider-metrics.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'cecilia', children: [
      { path: 'third-products', component: ThirdProductComponent },
      { path: 'ingredients', component: IngredientComponent },
      { path: 'homemade-products', component: HomemadeProductsComponent },
      { path: 'outputs', component: OutputComponent },
      { path: 'inputs', component: InputComponent },
      { path: 'eco-metrics', component: EconomicMetricComponent },
      { path: 'stock-metrics', component: ProductMetricComponent },
      { path: 'provider-metrics', component: ProviderMetricsComponent },
    ], canActivateChild: [authGuard]},
];
