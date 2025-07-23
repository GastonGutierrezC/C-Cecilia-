public class ProviderNameDto
{
    public string Name { get; set; }
}

public class TargetAmountDto
{
    public string Name { get; set; } 
    public double Value { get; set; } 
}

public class AccumulatedToDateDto
{
    public string Name { get; set; } 
    public double Value { get; set; } 
}

public class PercentageAchievedDto
{
    public string Name { get; set; } 
    public double Value { get; set; }
}

public class ClosingTrendDto
{
    public string Name { get; set; }  
    public double Value { get; set; } 
}

public class TrendPercentageDto
{
    public string Name { get; set; } 
    public double Value { get; set; } 
}

public class RequiredDailySalesDto
{
    public string Name { get; set; }
    public double Value { get; set; } 
}

public class DailySalesEntry
{
    public string Name { get; set; } 
    public double Value { get; set; } 
}


public class ProviderSeriesDto
{
    public ProviderNameDto ProviderName { get; set; } = new();
    public TargetAmountDto TargetAmount { get; set; } = new();
    public AccumulatedToDateDto AccumulatedToDate { get; set; }
    public PercentageAchievedDto PercentageAchieved { get; set; } = new();
    public ClosingTrendDto ClosingTrend { get; set; } = new();
    public TrendPercentageDto TrendPercentage { get; set; } = new();
    public RequiredDailySalesDto RequiredDailySales { get; set; } = new();  

    public List<DailySalesEntry> Series { get; set; }

}
