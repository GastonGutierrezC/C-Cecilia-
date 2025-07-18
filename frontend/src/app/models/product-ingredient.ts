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

export interface HomeMadeProductModel {
  ingredientId: number;
  quantity: number;
}
