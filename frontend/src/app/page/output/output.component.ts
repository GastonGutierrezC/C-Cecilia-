import {AfterViewInit, Component, computed, inject, OnInit, signal, viewChild} from '@angular/core';
import {MatTab, MatTabGroup} from '@angular/material/tabs';
import {ProductService} from '../../service/product-service';
import {IngredientService} from '../../service/ingredient-service';
import {ProductModel} from '../../models/products';
import {IngredientModel} from '../../models/ingredient';
import {MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {MatAutocomplete, MatAutocompleteTrigger, MatOption} from '@angular/material/autocomplete';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {OutputData, OutputInfo} from '../../models/output';
import {MatButton} from '@angular/material/button';
import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell,
  MatHeaderCellDef, MatHeaderRow, MatHeaderRowDef, MatNoDataRow, MatRow, MatRowDef,
  MatTable,
  MatTableDataSource
} from '@angular/material/table';
import {HomeMadeProductContentModel} from '../../models/product-ingredient';
import {MatPaginator} from '@angular/material/paginator';
import {MatSortHeader} from '@angular/material/sort';
import {MatIcon} from '@angular/material/icon';
import {AsyncPipe} from '@angular/common';
import {Observable, startWith} from 'rxjs';
import {map} from 'rxjs/operators';
import {OutputService} from '../../service/output-service';
import {MatDialog} from '@angular/material/dialog';
import {CompleteTaskComponent} from '../../dialog/complete-task/complete-task.component';

@Component({
  selector: 'app-output',
  imports: [
    MatTabGroup,
    MatTab,
    MatFormField,
    MatLabel,
    MatAutocompleteTrigger,
    ReactiveFormsModule,
    MatOption,
    MatAutocomplete,
    MatInput,
    MatButton,
    MatTable,
    MatCell,
    MatCellDef,
    MatColumnDef,
    MatHeaderCell,
    MatHeaderCellDef,
    MatHeaderRow,
    MatRow,
    MatRowDef,
    MatHeaderRowDef,
    MatPaginator,
    MatIcon,
    MatNoDataRow,
  ],
  templateUrl: './output.component.html',
  styleUrl: './output.component.scss'
})
export class OutputComponent implements OnInit, AfterViewInit{
  productService = inject(ProductService)
  ingredientService = inject(IngredientService)
  outputService = inject(OutputService)
  products: ProductModel[] = []
  ingredients: IngredientModel[] = []
  readonly dialog = inject(MatDialog);
  ingredientOutput = new FormGroup({
    ingredient: new FormControl<IngredientModel | null>(null, [Validators.required]),
    quantity: new FormControl<number>(0, [Validators.required])
  })
  productOutput = new FormGroup({
    product: new FormControl<ProductModel | null>(null, [Validators.required]),
    quantity: new FormControl<number>(0, [Validators.required])
  })
  outputData = signal<OutputInfo[]>([])
  columns: string[] = ['name', 'partialPrice', 'quantity', 'isProduct', 'delete']
  productsDataSource: MatTableDataSource<OutputInfo> = new MatTableDataSource<OutputInfo>();
  paginator = viewChild.required(MatPaginator);

  ngOnInit() {
    this.productService.getProducts().subscribe(products => {
      this.products = products;
    })
    this.ingredientService.getIngredients().subscribe(ingredients => {
      this.ingredients = ingredients;
    })
  }
  ngAfterViewInit() {
    this.productsDataSource = new MatTableDataSource(this.outputData())
    this.productsDataSource.paginator = this.paginator()
  }

  addProduct() {
    if (this.productOutput.value.product
      && this.productOutput.value.quantity !== null
      && this.productOutput.value.quantity !== undefined
    ) {
      this.outputData.set(
        this.outputData().concat({
        id: this.productOutput.value.product.id,
        name: this.productOutput.value.product.name,
        isProduct: true,
        quantity: this.productOutput.value.quantity,
        partialPrice: this.productOutput.value.quantity * this.productOutput.value.product.sellPrice
      }))
      this.productsDataSource = new MatTableDataSource(this.outputData())
    }
  }

  addIngredient() {
    if (this.ingredientOutput.value.ingredient
      && this.ingredientOutput.value.quantity !== null
      && this.ingredientOutput.value.quantity !== undefined
    ) {
      this.outputData.set(
        this.outputData().concat({
        id: this.ingredientOutput.value.ingredient.id,
        name: this.ingredientOutput.value.ingredient.name,
        isProduct: false,
        quantity: this.ingredientOutput.value.quantity,
        partialPrice: this.ingredientOutput.value.quantity * this.ingredientOutput.value.ingredient.sellPrice
      }))
      this.productsDataSource = new MatTableDataSource(this.outputData())
    }
  }

  totalPrice = computed<number>(() => {
    return this.outputData().reduce((init, value)=> init += value.partialPrice, 0)
  })
  removeSale(product: OutputInfo) {
    this.outputData.set(this.outputData().filter(item => item.id !== product.id))
    this.productsDataSource = new MatTableDataSource(this.outputData())
  }

  sale() {

    let data = this.outputData().map(product => {
      let p = new OutputData()
      p.id = product.id
      p.quantity = product.quantity
      p.isProduct = product.isProduct
      return p
    })
    this.outputService.createOutputProductAndIngredients(data).subscribe({
      next: (res) => {
        if (res) this.dialog.open(CompleteTaskComponent, {
          data: "Venta registrada con exito"
        })
      }
    })
  }
}

