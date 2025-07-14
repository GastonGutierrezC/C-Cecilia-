import {AfterViewInit, Component, inject, OnInit, viewChild} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatButton, MatIconButton} from '@angular/material/button';
import {MatCard, MatCardContent, MatCardFooter, MatCardHeader, MatCardTitle} from '@angular/material/card';
import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell, MatHeaderCellDef,
  MatHeaderRow,
  MatHeaderRowDef,
  MatRow, MatRowDef, MatTable, MatTableDataSource
} from '@angular/material/table';
import {MatError, MatFormField, MatHint, MatInput, MatLabel, MatSuffix} from '@angular/material/input';
import {MatSort, MatSortHeader} from '@angular/material/sort';
import {MatPaginator} from '@angular/material/paginator';
import {MatIcon} from '@angular/material/icon';
import {ProductModel} from '../../models/products';
import {ProductService} from '../../service/product-service';
import {MatDialog} from '@angular/material/dialog';
import {ConfirmationDialogComponent} from '../../dialog/confirmation-dialog/confirmation-dialog.component';
import {EditThirdProductComponent} from '../../dialog/edit-third-product/edit-third-product.component';
import {IngredientModel} from '../../models/ingredient';
import {IngredientService} from '../../service/ingredient-service';
import {EditIngredientComponent} from '../../dialog/edit-ingredient/edit-ingredient.component';

@Component({
  selector: 'app-ingredient',
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
    MatFormField,
    MatHeaderCell,
    MatHeaderRow,
    MatHeaderRowDef,
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
    MatHeaderCellDef,
    MatError,
    MatHint
  ],
  templateUrl: './ingredient.component.html',
  styleUrl: './ingredient.component.scss'
})
export class IngredientComponent implements OnInit, AfterViewInit{
  onCreationMode = false
  tableView = true
  columns: string[] = ['name', 'ingredientUnit', 'unitPrice', 'sellPrice', 'quantity', 'delete', 'edit']
  dataSource: MatTableDataSource<IngredientModel> = new MatTableDataSource<IngredientModel>();
  ingredientService = inject(IngredientService)
  ingredients:  IngredientModel[] = [];
  filterName = new FormControl('');
  filteredIngredient: IngredientModel[] = [];
  readonly dialog = inject(MatDialog);
  ingredientForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    ingredientUnit: new FormControl('', [Validators.required]),
    unitPrice: new FormControl(0, [Validators.required, Validators.min(0)]),
    sellPrice: new FormControl(0, [Validators.required, Validators.min(0)]),
  })


  paginator = viewChild.required(MatPaginator);
  sort = viewChild.required(MatSort);

  ngOnInit() {
    this.ingredientService.getIngredients().subscribe(ingredients => {
      this.dataSource = new MatTableDataSource(ingredients);
      this.ingredients = ingredients;
      this.filteredIngredient = ingredients;
      this.dataSource.paginator = this.paginator()
      this.dataSource.sort = this.sort()
    })
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
    this.dataSource.filterPredicate = (data, filter1) => data.name.toLowerCase().trim().includes(filter1);
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  ngAfterViewInit() {
    this.ingredientService.getIngredients().subscribe(ingredients => {
      this.dataSource = new MatTableDataSource<IngredientModel>(ingredients);
      this.ingredients = ingredients;
      this.filteredIngredient = ingredients;
      this.dataSource.paginator = this.paginator()
      this.dataSource.sort = this.sort()
    })
  }

  filter(){
    this.filteredIngredient = this.ingredients.filter(product => product.name.toLowerCase().trim().includes(this.filterName.value?.toLowerCase().trim() ?? ''))
  }

  createIngredient() {
    if (this.ingredientForm.valid
      && this.ingredientForm.value.name !== null
      && this.ingredientForm.value.ingredientUnit !== null
      && this.ingredientForm.value.unitPrice !== null
      && this.ingredientForm.value.unitPrice !== undefined
      && this.ingredientForm.value.sellPrice !== null
      && this.ingredientForm.value.sellPrice !== undefined
      && this.ingredientForm.value.name !== undefined
      && this.ingredientForm.value.ingredientUnit !== undefined) {
      this.ingredientService.createIngredient({
        name: this.ingredientForm.value.name,
        ingredientUnit: this.ingredientForm.value.ingredientUnit,
        unitPrice: this.ingredientForm.value.unitPrice,
        sellPrice: this.ingredientForm.value.sellPrice,
        quantity: 0
      }).subscribe(res => {
        this.ingredientService.getIngredients().subscribe(ingredients => {
          this.ingredients = ingredients;
          this.filteredIngredient = ingredients;
          this.dataSource = new MatTableDataSource(ingredients);
          this.dataSource.paginator = this.paginator()
          this.dataSource.sort = this.sort()
        })
      })
    }
  }

  deleteIngredient(id: number) {
    this.dialog.open(ConfirmationDialogComponent, {
    }).afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.ingredientService.deleteIngredient(id).subscribe(res => {
          this.ingredientService.getIngredients().subscribe(ingredients => {
            this.dataSource = new MatTableDataSource(ingredients);
            this.ingredients = ingredients;
            this.filteredIngredient = ingredients;
            this.dataSource.paginator = this.paginator()
            this.dataSource.sort = this.sort()
          })
        })
      }
    })
  }

  editIngredient(ingredientModel: IngredientModel) {
    this.dialog.open(EditIngredientComponent, {
      data: ingredientModel
    }).afterClosed().subscribe({
      next: (res: boolean) => {
        if (res) {
          this.ingredientService.getIngredients().subscribe(ingredients => {
            this.ingredients = ingredients;
            this.filteredIngredient = ingredients;
            this.dataSource = new MatTableDataSource(ingredients);
          })
        }
      }
    })
  }
}
