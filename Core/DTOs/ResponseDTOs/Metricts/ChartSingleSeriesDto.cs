public class DailySalesDataPointDto
{
    public string Name { get; set; } 
    public double Value { get; set; }  
}

public class ProductSalesSeriesDto
{
    public string Name { get; set; }    
    public double Value { get; set; } 
    public List<DailySalesDataPointDto> Series { get; set; }
    
}
