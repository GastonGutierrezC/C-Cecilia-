import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment.development';
import {OutputData} from '../models/output';
import {EcoMetricsData, ItemMetricsData} from '../models/metrics';

@Injectable({
  providedIn: 'root'
})
export class MetricsService {

  private http = inject(HttpClient);
  api = environment.baseApiUrl;

  getEcometrics(data: EcoMetricsData){
    return this.http.post<any>(this.api + '/api/SalesMetrics', data);
  }

  getItemMetrics(data: ItemMetricsData){
    return this.http.post<any>(this.api + '/api/SingleItemSalesMetrics', data);
  }

}
