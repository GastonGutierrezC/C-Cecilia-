import {ChangeDetectionStrategy, Component, inject, OnInit} from '@angular/core';
import {LegendPosition, LineChartModule} from '@swimlane/ngx-charts';
import {FormControl, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {
  MatDatepickerToggle,
  MatDateRangeInput,
  MatDateRangePicker,
  MatEndDate,
  MatStartDate
} from '@angular/material/datepicker';
import {MatError, MatFormField, MatHint, MatLabel, MatSuffix} from '@angular/material/input';
import {MetricsService} from '../../service/metrics-service';
import {provideNativeDateAdapter} from '@angular/material/core';

@Component({
  selector: 'app-economic-metric',
  imports: [
    LineChartModule,
    MatFormField,
    MatLabel,
    MatDateRangeInput,
    MatEndDate,
    MatStartDate,
    ReactiveFormsModule,
    MatSuffix,
    MatDatepickerToggle,
    MatHint,
    MatDateRangePicker,
    MatError
  ],
  providers: [provideNativeDateAdapter()],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './economic-metric.component.html',
  styleUrl: './economic-metric.component.scss'
})
export class EconomicMetricComponent implements OnInit{

  protected readonly LegendPosition = LegendPosition;
  economicData: any;
  metrics: any = []
  metricsService = inject(MetricsService)
  dateRangeForm = new FormGroup({
    start: new FormControl<Date | null>(new Date(new Date().setFullYear(2024))),
    end: new FormControl<Date | null>(new Date()),
  })
  ngOnInit() {
    this.metricsService.getEcometrics({
      startDate: new Date(new Date().setFullYear(2024)).toISOString().split('T')[0],
      endDate: new Date().toISOString().split('T')[0]
    }).subscribe(res => {
      this.metrics = res;
    })
  }

  applyFilter() {
    if (this.dateRangeForm.valid
      && this.dateRangeForm.value.start !== undefined
      && this.dateRangeForm.value.end !== undefined
      && this.dateRangeForm.value.start !== null
      && this.dateRangeForm.value.end !== null) {
      this.metricsService.getEcometrics({
        startDate: this.dateRangeForm.value.start.toISOString().split('T')[0],
        endDate: this.dateRangeForm.value.end.toISOString().split('T')[0]
      }).subscribe(res => {
        this.metrics = res;
      })
    }

  }


}
