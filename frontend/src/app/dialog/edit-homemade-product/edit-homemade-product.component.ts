import {Component, inject, OnInit, signal} from '@angular/core';
import {ProductModel} from '../../models/products';
import {MAT_DIALOG_DATA, MatDialog, MatDialogContent, MatDialogRef} from '@angular/material/dialog';
import {ProductService} from '../../service/product-service';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {ProductIngredientService} from '../../service/product-ingredient-service';
import {
  HomeMadeProductContentModel,
  HomeMadeProductIngredientModel,
  HomeMadeProductModel
} from '../../models/product-ingredient';
import {IngredientModel} from '../../models/ingredient';
import {IngredientService} from '../../service/ingredient-service';
import {MatError, MatFormField, MatHint, MatInput, MatLabel} from '@angular/material/input';
import {
  MatCard,
  MatCardContent,
  MatCardFooter,
  MatCardHeader,
  MatCardSubtitle,
  MatCardTitle
} from '@angular/material/card';
import {MatButton, MatIconButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';

@Component({
  selector: 'app-edit-homemade-product',
  imports: [
    MatError,
    MatHint,
    MatLabel,
    MatFormField,
    MatDialogContent,
    ReactiveFormsModule,
    MatInput,
    MatCardContent,
    MatIcon,
    MatIconButton,
    MatCardSubtitle,
    MatCardHeader,
    MatCard,
    MatCardFooter,
    MatButton,
    MatCardTitle,
    FormsModule
  ],
  templateUrl: './edit-homemade-product.component.html',
  styleUrl: './edit-homemade-product.component.scss'
})
export class EditHomemadeProductComponent implements OnInit {
  data = inject<HomeMadeProductContentModel>(MAT_DIALOG_DATA);
  image64?: string;
  readonly dialog = inject(MatDialog);
  fileName?: string;
  homemadeProductService = inject(ProductIngredientService)
  productForm = new FormGroup({
    name: new FormControl(this.data.product.name, [Validators.required]),
    inPrice: new FormControl(this.data.product.inPrice, [Validators.required, Validators.min(0)]),
    sellPrice: new FormControl(this.data.product.sellPrice, [Validators.required, Validators.min(0)]),
  })
  filteredIngredients:  IngredientModel[] = [];
  ingredientService = inject(IngredientService)
  ingredients:  IngredientModel[] = [];
  newProductIngredientsChips = signal<string[]>([])
  newProductIngredients: HomeMadeProductIngredientModel[] = [];

  ngOnInit() {
    this.image64 = this.data.product.image;

    this.ingredientService.getIngredients().subscribe(ingredients => {
      this.ingredients = ingredients
      this.filteredIngredients = ingredients
      const names = this.data.ingredients.map(i => {
        const ingredient = ingredients.find(ing => ing.id === i.id);
        return ingredient?.name ?? '';
      }).filter(name => name !== '');

      this.newProductIngredientsChips.set(names);
    })
    this.newProductIngredients = this.newProductIngredients.concat(this.data.ingredients.map(value =>  ({
      ingredientId: value.id,
      price: value.unitPrice,
      quantity: value.quantity
    })))
  }

  convertToBase64(event: any) {
    const file = event.target.files[0]
    if (file) {
      const validExtensions = ['image/png', 'image/jpeg'];
      if (!validExtensions.includes(file.type)) {
        alert(`Formato no válido. Solo se permiten archivos .png y .jpg ${file.type}`);
        return;
      }
      this.fileName = file.name;
      const reader = new FileReader();
      reader.onload = () => {
        this.image64 = (reader.result as string).split(',')[1];
      };
      reader.readAsDataURL(file);
    }
    return;
  }
  editProduct() {
    if (this.productForm.valid
      && this.productForm.value.name !== null
      && this.productForm.value.inPrice !== null
      && this.productForm.value.sellPrice !== null
      && this.productForm.value.sellPrice !== undefined
      && this.productForm.value.name !== undefined
      && this.productForm.value.inPrice !== undefined
      && this.image64) {
      this.homemadeProductService.editProduct({
        product: {
          id: this.data.product.id,
          name: this.productForm.value.name,
          inPrice: this.productForm.value.inPrice,
          sellPrice: this.productForm.value.sellPrice,
          image: this.image64,
          quantity: this.data.product.quantity
        },
        ingredients: this.newProductIngredients
      }).subscribe(res => {
        this.dialogRef.close(res)
      })
    }
  }


  updatePreparationPrice() {
    let price = this.newProductIngredients.reduce((prev, curr) =>
      prev += (curr.price ?? 0) * curr.quantity, 0)
    this.productForm.controls.inPrice.setValue(price)
  }
  selectIngredient(ingredient: IngredientModel) {
    if (this.newProductIngredients.filter(value => value.ingredientId === ingredient.id).length > 0) return
    this.newProductIngredients.push({ingredientId: ingredient.id, quantity: 1, price: ingredient.unitPrice})
    this.newProductIngredientsChips.update(value => value.concat([ingredient.name]))
    this.updatePreparationPrice()
  }
  deselectIngredient(id: number) {
    this.newProductIngredients = this.newProductIngredients.filter(i => i.ingredientId !== id)
    this.newProductIngredientsChips.update(value => value.filter(value => value !== this.ingredients.find(value1 => value1.id === id)!.name))
    this.updatePreparationPrice()
  }

  readonly dialogRef = inject(MatDialogRef<EditHomemadeProductComponent>);
}
