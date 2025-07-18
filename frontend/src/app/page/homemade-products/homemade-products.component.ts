import {AfterViewInit, Component, inject, OnInit, signal, viewChild} from '@angular/core';
import {MatButton, MatIconButton} from "@angular/material/button";
import {
  MatCard,
  MatCardContent,
  MatCardFooter,
  MatCardHeader,
  MatCardSubtitle,
  MatCardTitle
} from "@angular/material/card";
import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell, MatHeaderCellDef,
  MatHeaderRow,
  MatHeaderRowDef, MatNoDataRow,
  MatRow, MatRowDef, MatTable, MatTableDataSource
} from "@angular/material/table";
import {MatError, MatFormField, MatHint, MatInput, MatLabel, MatSuffix} from "@angular/material/input";
import {MatSort, MatSortHeader} from "@angular/material/sort";
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {ProductModel} from '../../models/products';
import {ProductService} from '../../service/product-service';
import {MatDialog} from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';
import {ConfirmationDialogComponent} from '../../dialog/confirmation-dialog/confirmation-dialog.component';
import {EditThirdProductComponent} from '../../dialog/edit-third-product/edit-third-product.component';
import {MatIcon} from '@angular/material/icon';
import {IngredientData, IngredientModel} from '../../models/ingredient';
import {IngredientService} from '../../service/ingredient-service';
import {
  HomeMadeProductContentModel,
  HomeMadeProductIngredientModel,
  HomeMadeProductModel
} from '../../models/product-ingredient';
import {ProductIngredientService} from '../../service/product-ingredient-service';
import {EditHomemadeProductComponent} from '../../dialog/edit-homemade-product/edit-homemade-product.component';
@Component({
  selector: 'app-homemade-products',
  imports: [
    FormsModule,
    MatButton,
    MatCard,
    MatCardContent,
    MatCardFooter,
    MatCardHeader,
    MatCardTitle,
    MatCell,
    MatCellDef,
    MatColumnDef,
    MatError,
    MatFormField,
    MatHeaderCell,
    MatHeaderRow,
    MatHeaderRowDef,
    MatHint,
    MatIcon,
    MatIconButton,
    MatInput,
    MatLabel,
    MatPaginator,
    MatRow,
    MatRowDef,
    MatSort,
    MatSortHeader,
    MatSuffix,
    MatTable,
    ReactiveFormsModule,
    MatNoDataRow,
    MatHeaderCellDef,
    MatCardSubtitle
  ],
  templateUrl: './homemade-products.component.html',
  styleUrl: './homemade-products.component.scss'
})
export class HomemadeProductsComponent implements OnInit, AfterViewInit{
  onCreationMode = true
  tableView = true
  columns: string[] = ['name', 'inPrice', 'sellPrice', 'quantity', 'delete', 'edit']
  productsDataSource: MatTableDataSource<HomeMadeProductContentModel> = new MatTableDataSource<HomeMadeProductContentModel>();
  homemadeProductService = inject(ProductIngredientService)
  ingredientService = inject(IngredientService)
  products:  HomeMadeProductContentModel[] = [];
  ingredients:  IngredientModel[] = [];
  image64?: string;
  fileName?: string;
  filterProductName = new FormControl('');
  filteredIngredients:  IngredientModel[] = [];
  filteredProducts: HomeMadeProductContentModel[] = [];
  readonly dialog = inject(MatDialog);
  newProductIngredientsChips = signal<string[]>([])
  newProductIngredients: HomeMadeProductIngredientModel[] = [];
  productForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    inPrice: new FormControl(0, [Validators.required, Validators.min(0)]),
    sellPrice: new FormControl(0, [Validators.required, Validators.min(0)]),
  })


  paginator = viewChild.required(MatPaginator);
  sort = viewChild.required(MatSort);

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
  ngOnInit() {
    this.homemadeProductService.getProducts().subscribe(products => {
      this.productsDataSource = new MatTableDataSource(products);
      this.products = products;
      this.filteredProducts = products;
      this.productsDataSource.paginator = this.paginator()
      this.productsDataSource.sort = this.sort()
    })
    this.ingredientService.getIngredients().subscribe(ingredients => {
      this.ingredients = ingredients
      this.filteredIngredients = ingredients
    })
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.productsDataSource.filter = filterValue.trim().toLowerCase();
    this.productsDataSource.filterPredicate = (data, filter1) => data.product.name.toLowerCase().trim().includes(filter1);
    if (this.productsDataSource.paginator) {
      this.productsDataSource.paginator.firstPage();
    }
  }
  ngAfterViewInit() {
    this.homemadeProductService.getProducts().subscribe(products => {
      this.productsDataSource = new MatTableDataSource<HomeMadeProductContentModel>(products);
      this.products = products;
      this.filteredProducts = products;
      this.productsDataSource.paginator = this.paginator()
      this.productsDataSource.sort = this.sort()
    })
  }

  filter(){
    this.filteredProducts = this.products.filter(product => product.product.name.toLowerCase().trim().includes(this.filterProductName.value?.toLowerCase().trim() ?? ''))
  }

  createProduct() {
    if (this.productForm.valid
      && this.productForm.value.name !== null
      && this.productForm.value.inPrice !== null
      && this.productForm.value.sellPrice !== null
      && this.productForm.value.sellPrice !== undefined
      && this.productForm.value.name !== undefined
      && this.productForm.value.inPrice !== undefined
      && this.image64) {
      this.homemadeProductService.createProduct({
        product: {
          name: this.productForm.value.name,
          inPrice: this.productForm.value.inPrice,
          sellPrice: this.productForm.value.sellPrice,
          image: this.image64,
          quantity: 0
        },
        ingredients: this.newProductIngredients
      }).subscribe(res => {
        this.homemadeProductService.getProducts().subscribe(products => {
          this.products = products;
          this.fileName = undefined;
          this.image64 = undefined;
          this.filteredProducts = products;
          this.productsDataSource = new MatTableDataSource(products)

          this.productsDataSource.paginator = this.paginator()
          this.productsDataSource.sort = this.sort()
        })
      })
    }
  }

  deleteProduct(id: number) {
    this.dialog.open(ConfirmationDialogComponent, {
    }).afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.homemadeProductService.deleteProduct(id).subscribe(res => {
          this.homemadeProductService.getProducts().subscribe(products => {
            this.productsDataSource = new MatTableDataSource(products);
            this.products = products;
            this.filteredProducts = products;
            this.productsDataSource.paginator = this.paginator()
            this.productsDataSource.sort = this.sort()
          })
        })
      }
    })
  }

  editProduct(product: HomeMadeProductContentModel) {
    this.dialog.open(EditHomemadeProductComponent, {
      data: product
    }).afterClosed().subscribe({
      next: (res: boolean) => {
        if (res) {
          this.homemadeProductService.getProducts().subscribe(products => {
            this.products = products;
            this.filteredProducts = products;
            this.productsDataSource = new MatTableDataSource(products);
            this.productsDataSource.paginator = this.paginator()
            this.productsDataSource.sort = this.sort()
          })
        }
      }
    })
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

}
