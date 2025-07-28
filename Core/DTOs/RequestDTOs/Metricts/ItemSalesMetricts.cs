namespace Core.DTOs.RequestDTOs
{
    public class ItemSalesMetrictsRequestDto
    {
        public int ItemId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
