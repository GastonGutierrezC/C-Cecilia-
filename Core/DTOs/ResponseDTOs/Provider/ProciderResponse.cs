namespace Core.DTOs.ResponseDTOs;

public class ProviderResponse
{
    public required int ProviderId { get; set; }
    public required string Name { get; set; }
    public required double ObjetiveMount { get; set; }

}