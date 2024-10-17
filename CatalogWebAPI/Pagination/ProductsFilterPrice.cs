﻿namespace CatalogWebAPI.Pagination;

public class ProductsFilterPrice : QueryStringParameters
{
    public decimal? Price { get; set; }
    public string? PriceCriterion { get; set; }
}
