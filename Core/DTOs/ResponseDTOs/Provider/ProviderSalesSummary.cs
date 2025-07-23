namespace Core.DTOs.ResponseDTOs;

public class ProviderSalesSummaryDto
{
    public string ProviderName { get; set; } = string.Empty;

    public double TargetAmount { get; set; }

    public double AccumulatedToDate { get; set; }

    public double PercentageAchieved { get; set; }

    public double ClosingTrend { get; set; }

    public double TrendPercentage { get; set; }

    public double RequiredDailySales { get; set; }

    public List<DailySalesEntry> DailySales { get; set; } = new();
}

public class DailySalesEntry
{
    public DateTime Date { get; set; }

    public double Amount { get; set; }
}
