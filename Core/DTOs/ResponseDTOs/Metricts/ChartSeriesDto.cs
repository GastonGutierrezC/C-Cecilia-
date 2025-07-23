namespace Core.DTOs.ResponseDTOs
{
  
    public class ChartDataPointDto
    {
        public string Name { get; set; }  
        public double Value { get; set; }
    }

 
    public class ChartSeriesDto
    {
        public string Name { get; set; } 
        public List<ChartDataPointDto> Series { get; set; } = new(); 
    }

   
    public class SalesMetricsDto
    {
        public List<ChartSeriesDto> Series { get; set; } = new(); 
        public string Name { get; set; } = "CombinedSales";
        public double Value { get; set; } 
    }
}
