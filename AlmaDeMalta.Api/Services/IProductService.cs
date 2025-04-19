using AlmaDeMalta.api.Requests;
using AlmaDeMalta.Common.Contracts.Attributes;

namespace AlmaDeMalta.Api.Services;
    [ServiceClass(targetClass: typeof(ProductService), strategy: StrategyEnum.Scoped)]
    public interface IProductService: IBaseService<ProductRequest>
    {

    }
