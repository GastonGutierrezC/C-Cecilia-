import {Component, inject, OnInit} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {MatButton} from "@angular/material/button";
import {MAT_DIALOG_DATA, MatDialog, MatDialogContent, MatDialogRef, MatDialogTitle} from "@angular/material/dialog";
import {MatFormField, MatInput, MatLabel, MatSuffix} from "@angular/material/input";
import {ProductModel} from '../../models/products';
import {ProductService} from '../../service/product-service';
import {MatIcon} from '@angular/material/icon';
import {IngredientService} from '../../service/ingredient-service';
import {IngredientModel} from '../../models/ingredient';

@Component({
  selector: 'app-edit-ingredient',
  imports: [
    FormsModule,
    MatButton,
    MatDialogContent,
    MatDialogTitle,
    MatFormField,
    MatIcon,
    MatInput,
    MatLabel,
    MatSuffix,
    ReactiveFormsModule
  ],
  templateUrl: './edit-ingredient.component.html',
  styleUrl: './edit-ingredient.component.scss'
})
export class EditIngredientComponent{
  data = inject<IngredientModel>(MAT_DIALOG_DATA);
  readonly dialog = inject(MatDialog);
  ingredientService = inject(IngredientService)
  ingredientForm = new FormGroup({
    name: new FormControl(this.data.name, [Validators.required]),
    ingredientUnit: new FormControl(this.data.ingredientUnit, [Validators.required]),
    unitPrice: new FormControl(this.data.unitPrice, [Validators.required]),
    sellPrice: new FormControl(this.data.unitPrice, [Validators.required]),
  })
  editProduct() {
    if (this.ingredientForm.valid
      && this.ingredientForm.value.name !== null
      && this.ingredientForm.value.ingredientUnit !== null
      && this.ingredientForm.value.unitPrice !== null
      && this.ingredientForm.value.unitPrice !== undefined
      && this.ingredientForm.value.sellPrice !== null
      && this.ingredientForm.value.sellPrice !== undefined
      && this.ingredientForm.value.name !== undefined
      && this.ingredientForm.value.ingredientUnit !== undefined
      ) {
      this.ingredientService.editIngredient({
        id: this.data.id,
        name: this.ingredientForm.value.name,
        unitPrice: this.ingredientForm.value.unitPrice,
        ingredientUnit: this.ingredientForm.value.ingredientUnit,
        quantity: this.data.quantity,
        sellPrice: this.ingredientForm.value.sellPrice
      }).subscribe(res => {
        this.dialogRef.close(res)
      })
    }
  }

  readonly dialogRef = inject(MatDialogRef<EditIngredientComponent>);
}
