import {ChangeDetectionStrategy, Component, inject, OnInit} from '@angular/core';
import {BarChartModule, LegendPosition} from "@swimlane/ngx-charts";
import {MatAutocomplete, MatAutocompleteTrigger, MatOption} from "@angular/material/autocomplete";
import {MatButton} from "@angular/material/button";
import {
  MatDatepicker, MatDatepickerInput,
  MatDatepickerToggle,
} from "@angular/material/datepicker";
import {MatError, MatFormField, MatHint, MatInput, MatLabel, MatSuffix} from "@angular/material/input";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {MetricsService} from '../../service/metrics-service';
import {ProviderService} from '../../service/provider-service';
import {ProviderModel} from '../../models/provider';
import {provideNativeDateAdapter} from '@angular/material/core';

@Component({
  selector: 'app-provider-metrics',
  imports: [
    BarChartModule,
    MatAutocomplete,
    MatAutocompleteTrigger,
    MatButton,
    MatError,
    MatSuffix,
    MatFormField,
    MatHint,
    MatInput,
    MatLabel,
    MatOption,
    ReactiveFormsModule,
    MatDatepicker,
    MatDatepickerInput,
    MatDatepickerToggle
  ],
  providers: [provideNativeDateAdapter()],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './provider-metrics.component.html',
  styleUrl: './provider-metrics.component.scss'
})
export class ProviderMetricsComponent implements OnInit{

  providerForm = new FormGroup({
    provider: new FormControl<ProviderModel | null>(null, [Validators.required]),
  })
  providers: ProviderModel[] = []
  metrics: any
  providerService = inject(ProviderService)
  metricsService = inject(MetricsService)
  dateForm = new FormGroup({
    date: new FormControl<Date | null>(new Date()),
  })
  ngOnInit() {
    this.providerService.getProviders().subscribe({
      next: (res) => {
        this.providers = res;
        this.providerForm.controls.provider.setValue(res[0])
        this.metricsService.getProviderMetrics({
          providerId: this.providers[0].id,
          year: new Date().getFullYear(),
          month:  new Date().getMonth() + 1,
        }).subscribe(res => {
          this.metrics = res;
        })
      }
    })

  }

  applyFilter() {
    if (this.dateForm.valid
      && this.dateForm.value.date !== undefined
      && this.dateForm.value.date !== null
      && this.providerForm.valid
      && this.providerForm.value.provider !== undefined
      && this.providerForm.value.provider !== null) {
      this.metricsService.getProviderMetrics({
        providerId: this.providers[0].id,
        year: this.dateForm.value.date.getFullYear(),
        month: this.dateForm.value.date.getMonth() + 1,
      }).subscribe(res => {
        this.metrics = res;
      })
    }

  }

  protected readonly LegendPosition = LegendPosition;
}
