using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services.Impl;
using AlmaDeMalta.Common.Contracts.Attributes;
using AlmaDeMalta.Common.Contracts.Contracts;
using System.Linq.Expressions;

namespace AlmaDeMalta.Api.Services;
    [ServiceClass(targetClass: typeof(ProductService), strategy: StrategyEnum.Scoped)]
    public interface IProductService: IBaseService<Product>
    {
    }
