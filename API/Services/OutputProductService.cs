using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;

namespace API.Services;

public class OutputProductService : IOutputProductService
{
    private readonly IGenericRepository<Output> _outputRepo;
    private readonly IGenericRepository<OutputProducts> _outputProductRepo;
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IMapper _mapper;

    public OutputProductService(
        IGenericRepository<Output> outputRepo,
        IGenericRepository<OutputProducts> outputProductRepo,
        IGenericRepository<Product> productRepo,
        IMapper mapper)
    {
        _outputRepo = outputRepo;
        _outputProductRepo = outputProductRepo;
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<bool> RegisterOutputProductsAsync(List<CreateProductOutputRequest> outputRequests)
    {
        if(outputRequests == null || !outputRequests.Any()) 
            throw new ArgumentException("Output requests cannot be null or empty.");

        var output = new Output
        {
            OutputDate = DateTime.UtcNow
        };

        await _outputRepo.AddAsync(output);
        await _outputRepo.SaveChangesAsync();

        foreach (var request in outputRequests)
        {
            var product = await _productRepo.GetByIdAsync(request.Id);
            if (product == null)
                throw new Exception($"Product with ID {request.Id} does not exist.");

            if (product.Quantity < request.Quantity)
                throw new Exception($"Insufficient quantity for product {product.Name}. Available: {product.Quantity}, Requested: {request.Quantity}");

            var outputProduct = new OutputProducts
            {
                ProductId = product.Id,
                Quantity = request.Quantity,
                OutputId = output.Id
            };
            await _outputProductRepo.AddAsync(outputProduct);
            product.Quantity -= request.Quantity;
            await _productRepo.UpdateAsync(product);
        }
        await _outputProductRepo.SaveChangesAsync();
        await _productRepo.SaveChangesAsync();

        return true;

    }

}
