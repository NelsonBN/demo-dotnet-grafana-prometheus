using System;

namespace Demo.WebAPI.Models;

public class ProductBase
{
    public string Name { get; set; }
    public int Quantity { get; set; }
}
public class Product : ProductBase
{
    public Guid Id { get; set; }
}
