﻿using AlmaDeMalta.api.Services.Impl;
using AlmaDeMalta.Api.Services;
using AlmaDeMalta.Common.Contracts.Attributes;
using AlmaDeMalta.Common.Contracts.Contracts;

namespace AlmaDeMalta.api.Services;
[ServiceClass(typeof(PurchaseOrderNumberPrefixService), StrategyEnum.Scoped)]
public interface IPurchaseOrderNumberPrefixService: IBaseService<PurchaseOrderNumberPrefix>
{
}
