using Core.DTOs.ResponseDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Services;

public interface IProductIngredientService
{
    Task<List<ProductIngredientSimpleResponse>> GetAllProductsAndIngredientsAsync();
}

