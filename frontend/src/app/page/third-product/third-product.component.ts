import {AfterViewInit, Component, inject, OnInit, signal, viewChild} from '@angular/core';
import {MatButton, MatFabButton, MatIconButton} from '@angular/material/button';
import {ProductService} from '../../service/product-service';
import {ProductModel} from '../../models/products';
import {MatError, MatFormField, MatHint, MatInput, MatLabel, MatPrefix, MatSuffix} from '@angular/material/input';
import {MatOption} from '@angular/material/core';
import {MatSelect} from '@angular/material/select';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatIcon} from '@angular/material/icon';
import {MatCard, MatCardContent, MatCardFooter, MatCardHeader, MatCardTitle} from '@angular/material/card';
import {MatDialog} from '@angular/material/dialog';
import {ConfirmationDialogComponent} from '../../dialog/confirmation-dialog/confirmation-dialog.component';
import {ResizeImageDialogComponent} from '../../dialog/resize-image-dialog/resize-image-dialog.component';
import {
  MatCell, MatCellDef,
  MatColumnDef,
  MatHeaderCell, MatHeaderCellDef,
  MatHeaderRow, MatHeaderRowDef,
  MatNoDataRow,
  MatRow, MatRowDef,
  MatTable, MatTableDataSource
} from '@angular/material/table';
import {MatSort, MatSortHeader} from '@angular/material/sort';
import {MatPaginator} from '@angular/material/paginator';
import {MatMenu, MatMenuItem, MatMenuTrigger} from '@angular/material/menu';
import {EditThirdProductComponent} from '../../dialog/edit-third-product/edit-third-product.component';

@Component({
  selector: 'app-third-product',
  imports: [
    MatButton,
    MatInput,
    MatOption,
    MatLabel,
    MatFormField,
    MatSelect,
    MatIcon,
    MatFabButton,
    ReactiveFormsModule,
    MatCard,
    MatCardHeader,
    MatCardTitle,
    MatCardFooter,
    MatCardContent,
    MatIconButton,
    MatSuffix,
    MatPaginator,
    MatNoDataRow,
    MatHeaderRow,
    MatRow,
    MatCell,
    MatHeaderCell,
    MatColumnDef,
    MatTable,
    MatSort,
    MatHeaderCellDef,
    MatSortHeader,
    MatHeaderRowDef,
    MatCellDef,
    MatRowDef,
    MatMenu,
    MatMenuItem,
    MatMenuTrigger,
    MatError,
    MatHint
  ],
  templateUrl: './third-product.component.html',
  styleUrl: './third-product.component.scss'
})
export class ThirdProductComponent implements OnInit, AfterViewInit{
  onCreationMode = false
  tableView = true
  columns: string[] = ['name', 'inPrice', 'sellPrice', 'quantity', 'delete', 'edit']
  dataSource: MatTableDataSource<ProductModel> = new MatTableDataSource<ProductModel>();
  productService = inject(ProductService)
  products:  ProductModel[] = [];
  image64?: string;
  fileName?: string;
  filterName = new FormControl('');
  filteredProducts: ProductModel[] = [];
  readonly dialog = inject(MatDialog);
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
    this.productService.getProducts().subscribe(products => {
      this.dataSource = new MatTableDataSource(products);
      this.products = products;
      this.filteredProducts = products;

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
    this.productService.getProducts().subscribe(products => {
      this.dataSource = new MatTableDataSource<ProductModel>(products);
      this.products = products;
      this.filteredProducts = products;
      this.dataSource.paginator = this.paginator()
      this.dataSource.sort = this.sort()
    })
  }

  filter(){
    this.filteredProducts = this.products.filter(product => product.name.toLowerCase().trim().includes(this.filterName.value?.toLowerCase().trim() ?? ''))
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
      this.productService.createProduct({
        name: this.productForm.value.name,
        inPrice: this.productForm.value.inPrice,
        sellPrice: this.productForm.value.sellPrice,
        image: this.image64,
        quantity: 0
      }).subscribe(res => {
        this.productService.getProducts().subscribe(products => {
          this.products = products;
          this.fileName = undefined;
          this.image64 = undefined;
          this.filteredProducts = products;
          this.dataSource = new MatTableDataSource(products)

          this.dataSource.paginator = this.paginator()
          this.dataSource.sort = this.sort()
        })
      })
    }
  }

  deleteProduct(id: number) {
    this.dialog.open(ConfirmationDialogComponent, {
    }).afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.productService.deleteProduct(id).subscribe(res => {
          this.productService.getProducts().subscribe(products => {
            this.dataSource = new MatTableDataSource(products);
            this.products = products;
            this.filteredProducts = products;
            this.dataSource.paginator = this.paginator()
            this.dataSource.sort = this.sort()
          })
        })
      }
    })
  }

  editProduct(product: ProductModel) {
    this.dialog.open(EditThirdProductComponent, {
      data: product
    }).afterClosed().subscribe({
      next: (res: boolean) => {
        if (res) {
          this.productService.getProducts().subscribe(products => {
            this.products = products;
            this.filteredProducts = products;
            this.dataSource = new MatTableDataSource(products);
            this.dataSource.paginator = this.paginator()
            this.dataSource.sort = this.sort()
          })
        }
      }
    })
  }
}
