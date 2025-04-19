using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;

namespace AlmaDeMalta.api.Mappers;
    public class ProductMapper: Mapper<ProductRequest, Response, Product>
    {
        public override Product ToEntity(ProductRequest source)
        {
            return new Product
            {
                Name = source.Name,
                Description = source.Description,
                Price = source.Price,
                Brand = source.Brand,
                Category = source.Category,
                ImageUrl = source.ImageUrl,
                Type = source.Type,
                Unit = source.Unit
            };
        }
    public override Response FromEntity(Product source)
    {
        return Response.CreateBuilder()
            .WithBody(source.Id)
            .WithStatus(201)
            .WithSuccessMessage("Product created successfully")
            .Build();
    }
}
