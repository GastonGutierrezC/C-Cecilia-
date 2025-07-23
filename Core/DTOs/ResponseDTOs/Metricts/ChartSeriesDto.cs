namespace Core.DTOs.ResponseDTOs
{
    // Punto individual en una serie
    public class ChartDataPointDto
    {
        public string Name { get; set; }  // Ejemplo: "2025-07-01"
        public double Value { get; set; } // Ejemplo: 100.0
    }

    // Serie de datos (ej: ProductSales, IngredientSales)
    public class ChartSeriesDto
    {
        public string Name { get; set; } // Ejemplo: "ProductSales"
        public List<ChartDataPointDto> Series { get; set; } = new(); // Siempre se inicializa
    }

    // DTO final que contiene todas las series y el total combinado
    public class SalesMetricsDto
    {
        public List<ChartSeriesDto> Series { get; set; } = new(); // ProductSales, IngredientSales
        public string Name { get; set; } = "CombinedSales";
        public double Value { get; set; } // Total combinado
    }
}
