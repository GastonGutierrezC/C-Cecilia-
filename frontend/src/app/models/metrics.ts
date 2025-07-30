export interface EcoMetricsData {
  startDate: string;
  endDate: string;
}

export interface ItemMetricsData {
  itemId: number;
  isProduct: boolean;
  startDate: string;
  endDate: string;
}

export interface ItemModel {
  id: number;
  name: string;
  isProduct: boolean;
}
