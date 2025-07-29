import {ChangeDetectionStrategy, Component, inject, OnInit} from '@angular/core';
import {LegendPosition, LineChartModule, NgxChartsModule} from '@swimlane/ngx-charts';
import {
  MatDatepickerToggle,
  MatDateRangeInput,
  MatDateRangePicker,
  MatEndDate,
  MatStartDate
} from '@angular/material/datepicker';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatError, MatFormField, MatHint, MatInput, MatLabel, MatSuffix} from '@angular/material/input';
import {MetricsService} from '../../service/metrics-service';
import {MatOption, provideNativeDateAdapter} from '@angular/material/core';
import {MatAutocomplete, MatAutocompleteTrigger} from '@angular/material/autocomplete';
import {ProductIngredientService} from '../../service/product-ingredient-service';
import {ItemModel} from '../../models/metrics';
import {MatButton} from '@angular/material/button';

@Component({
  selector: 'app-product-metric',
  imports: [
    LineChartModule,
    MatDateRangeInput,
    MatDateRangePicker,
    MatDatepickerToggle,
    MatEndDate,
    MatError,
    MatFormField,
    MatHint,
    MatLabel,
    MatStartDate,
    MatSuffix,
    ReactiveFormsModule,
    MatLabel,
    MatHint,
    MatSuffix,
    MatError,
    NgxChartsModule,
    MatAutocomplete,
    MatAutocompleteTrigger,
    MatInput,
    MatOption,
    MatButton
  ],
  providers: [provideNativeDateAdapter()],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './product-metric.component.html',
  styleUrl: './product-metric.component.scss'
})
export class ProductMetricComponent implements OnInit{
  protected readonly LegendPosition = LegendPosition;
  productForm = new FormGroup({
    product: new FormControl<ItemModel | null>(null, [Validators.required]),
  })
  products: ItemModel[] = []
  metrics: any
  productIngredientService = inject(ProductIngredientService)
  metricsService = inject(MetricsService)
  dateRangeForm = new FormGroup({
    start: new FormControl<Date | null>(new Date(new Date().setFullYear(2024))),
    end: new FormControl<Date | null>(new Date()),
  })
  ngOnInit() {
    this.productIngredientService.getAllItems().subscribe({
      next: (res) => {
        this.products = res;
        this.productForm.controls.product.setValue(res[0])
        this.metricsService.getItemMetrics({
          itemId: this.products[0].id,
          isProduct: this.products[0].isProduct,
          startDate: new Date(new Date().setFullYear(2024)).toISOString().split('T')[0],
          endDate: new Date().toISOString().split('T')[0]
        }).subscribe(res => {
          this.metrics = [res];
        })
      }
    })

  }

  applyFilter() {
    if (this.dateRangeForm.valid
      && this.dateRangeForm.value.start !== undefined
      && this.dateRangeForm.value.end !== undefined
      && this.dateRangeForm.value.start !== null
      && this.dateRangeForm.value.end !== null
    && this.productForm.valid
    && this.productForm.value.product !== undefined
      && this.productForm.value.product !== null) {
      this.metricsService.getItemMetrics({
        itemId: this.productForm.value.product.id,
        isProduct: this.productForm.value.product.isProduct,
        startDate: this.dateRangeForm.value.start.toISOString().split('T')[0],
        endDate: this.dateRangeForm.value.end.toISOString().split('T')[0]
      }).subscribe(res => {

        this.metrics = [res];
        console.log(this.metrics)
      })
    }

  }
}
