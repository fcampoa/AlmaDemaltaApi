using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
using System.Net;

namespace AlmaDeMalta.api.Mappers;
    public class ProductMapper: Mapper<ProductRequest, Response, Product>
    {
        public override Product ToEntity(ProductRequest source)
        {
            return new Product()
            {
                Id = source.Id,
                isActive = source.IsActive,
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
}
