using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(BreadContext context)
    {
        Console.WriteLine(">>>> Ejecutando SeedData");
        if (!await context.Provider.AnyAsync())
        {
            var provider = new Provider
            {
                Name = "C-CECILIA",
                ObjetiveMount = 0
            };
            context.Provider.Add(provider);
            await context.SaveChangesAsync();
        }

        if (!await context.Users.AnyAsync())
        {
            var users = new List<User>
            {
                new User { Username = "Mario", Email = "mario@gmail.com" },
                new User { Username = "Carlos", Email = "carlos@gmail.com" },
                new User { Username = "Melissa", Email = "melissa@gmail.com" }
            };
            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }

        if (!await context.Ingredients.AnyAsync())
        {
            var ingredients = new List<Ingredient>
            {
                new Ingredient { Name = "Harina", Quantity = 100, IngredientUnit = "kg", UnitPrice = 10, SellPrice = 15 },
                new Ingredient { Name = "Azúcar", Quantity = 50, IngredientUnit = "kg", UnitPrice = 8, SellPrice = 12 },
                new Ingredient { Name = "Levadura", Quantity = 20, IngredientUnit = "kg", UnitPrice = 5, SellPrice = 8 },
                new Ingredient { Name = "Chocolate", Quantity = 30, IngredientUnit = "kg", UnitPrice = 20, SellPrice = 30 },
            };
            context.Ingredients.AddRange(ingredients);
            await context.SaveChangesAsync();
        }

        if (!await context.Products.AnyAsync())
        {
            var provider = await context.Provider.FirstAsync();

            var products = new List<Product>
            {
                new Product { Name = "Pan", Quantity = 30, InPrice = 5, SellPrice = 10, ProviderId = provider.Id },
                new Product { Name = "Torta", Quantity = 10, InPrice = 15, SellPrice = 30, ProviderId = provider.Id },
                new Product { Name = "Bizcocho", Quantity = 20, InPrice = 7, SellPrice = 14, ProviderId = provider.Id }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var ingredients = await context.Ingredients.ToListAsync();
            var productPan = products[0];
            var productTorta = products[1];

            var productIngredients = new List<ProductIngredients>
            {
                new ProductIngredients { ProductId = productPan.Id, IngredientId = ingredients.First(i => i.Name == "Harina").Id, Quantity = 1 },
                new ProductIngredients { ProductId = productPan.Id, IngredientId = ingredients.First(i => i.Name == "Levadura").Id, Quantity = 0.1 },

                new ProductIngredients { ProductId = productTorta.Id, IngredientId = ingredients.First(i => i.Name == "Harina").Id, Quantity = 1.5 },
                new ProductIngredients { ProductId = productTorta.Id, IngredientId = ingredients.First(i => i.Name == "Azúcar").Id, Quantity = 0.5 },
                new ProductIngredients { ProductId = productTorta.Id, IngredientId = ingredients.First(i => i.Name == "Chocolate").Id, Quantity = 0.3 }
            };

            context.ProductIngredients.AddRange(productIngredients);
            await context.SaveChangesAsync();
        }

        if (!await context.Inputs.AnyAsync())
        {
            var users = await context.Users.ToListAsync();
            var ingredients = await context.Ingredients.ToListAsync();
            var products = await context.Products.ToListAsync();

            for (int i = 0; i < 5; i++)
            {
                var input = new Input
                {
                    InputDate = DateTime.UtcNow.AddDays(-i)
                };
                context.Inputs.Add(input);
                await context.SaveChangesAsync();

                context.InputUsers.Add(new InputUser
                {
                    InputId = input.Id,
                    UserId = users[i % users.Count].Id
                });

                context.InputIngredients.AddRange(new[]
                {
                    new InputIngredients { InputId = input.Id, IngredientId = ingredients[0].Id, Quantity = 2 + i },
                    new InputIngredients { InputId = input.Id, IngredientId = ingredients[1].Id, Quantity = 1 + i }
                });

                context.InputProducts.Add(new InputProducts
                {
                    InputId = input.Id,
                    ProductId = products[0].Id,
                    Quantity = 5 + i
                });

                await context.SaveChangesAsync();
            }
        }

        if (!await context.Outputs.AnyAsync())
        {
            var users = await context.Users.ToListAsync();
            var ingredients = await context.Ingredients.ToListAsync();
            var products = await context.Products.ToListAsync();

            for (int i = 0; i < 5; i++)
            {
                var output = new Output
                {
                    OutputDate = DateTime.UtcNow.AddDays(-i)
                };
                context.Outputs.Add(output);
                await context.SaveChangesAsync();

                context.OutputUsers.Add(new OutputUser
                {
                    OutputId = output.Id,
                    UserId = users[i % users.Count].Id
                });

                context.OutputIngredients.Add(new OutputIngredients
                {
                    OutputId = output.Id,
                    IngredientId = ingredients[0].Id,
                    Quantity = 0.5 + i
                });

                context.OutputProducts.Add(new OutputProducts
                {
                    OutputId = output.Id,
                    ProductId = products[1].Id,
                    Quantity = 1 + i
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
