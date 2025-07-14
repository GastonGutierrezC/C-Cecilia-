import {Component, inject, OnInit} from '@angular/core';
import {MatButton} from "@angular/material/button";
import {
  MAT_DIALOG_DATA, MatDialog,
  MatDialogActions,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle
} from "@angular/material/dialog";
import {ProductService} from '../../service/product-service';
import {ProductModel} from '../../models/products';
import {MatFormField, MatInput, MatLabel, MatSuffix} from '@angular/material/input';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {ResizeImageDialogComponent} from '../resize-image-dialog/resize-image-dialog.component';
import {MatIcon} from '@angular/material/icon';

@Component({
  selector: 'app-edit-third-product',
  imports: [
    MatButton,
    MatDialogActions,
    MatDialogContent,
    MatDialogTitle,
    MatFormField,
    MatLabel,
    MatInput,
    MatFormField,
    ReactiveFormsModule,
    MatSuffix,
    MatIcon
  ],
  templateUrl: './edit-third-product.component.html',
  styleUrl: './edit-third-product.component.scss'
})
export class EditThirdProductComponent implements OnInit{
  data = inject<ProductModel>(MAT_DIALOG_DATA);
  image64?: string;
  readonly dialog = inject(MatDialog);
  fileName?: string;
  productService = inject(ProductService)
  productForm = new FormGroup({
    name: new FormControl(this.data.name, [Validators.required]),
    inPrice: new FormControl(this.data.inPrice, [Validators.required]),
    sellPrice: new FormControl(this.data.sellPrice, [Validators.required]),
  })

  ngOnInit() {
    this.image64 = this.data.image;
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
      this.productService.editProduct({
        id: this.data.id,
        name: this.productForm.value.name,
        inPrice: this.productForm.value.inPrice,
        sellPrice: this.productForm.value.sellPrice,
        image: this.image64,
        quantity: 0
      }).subscribe(res => {
        this.dialogRef.close(res)
      })
    }
  }

  readonly dialogRef = inject(MatDialogRef<EditThirdProductComponent>);
}
