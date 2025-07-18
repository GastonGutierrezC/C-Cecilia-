import {ProductData, ProductModel} from './products';
import {IngredientModel} from './ingredient';

export interface ProductIngredientModel {
  id: number;
  ingredientId: number;
  productId: number;
  quantity: number;
}

export interface ProductIngredientData {
  id: number;
  ingredientId: number;
  productId: number;
  quantity: number;
}

export interface HomeMadeProductIngredientModel {
  ingredientId: number;
  quantity: number;
  price?: number;
}

export interface HomeMadeProductData {
  product: ProductData;
  ingredients: HomeMadeProductIngredientModel[];
}


export interface HomeMadeProductModel {
  product: ProductModel;
  ingredients: HomeMadeProductIngredientModel[];
}

export interface HomeMadeProductContentModel {
  product: ProductModel;
  ingredients: IngredientModel[];
}
