namespace Core.DTOs.ResponseDTOs;

public class ProductIngredientSimpleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsProduct { get; set; }
    }