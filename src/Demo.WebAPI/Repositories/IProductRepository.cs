using System;
using System.Collections.Generic;
using Demo.WebAPI.Models;

namespace Demo.WebAPI.Repositories;

public interface IProductRepository
{
    Guid Add(ProductBase product);
    IEnumerable<Product> List();
    Product Get(Guid id);
    void Update(Guid id, ProductBase product);
    void Delete(Guid id);
}
