using System;
using System.Collections.Generic;
using Demo.WebAPI.Models;

namespace Demo.WebAPI.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDictionary<Guid, Product> _productStore;

    public ProductRepository()
        => _productStore = new Dictionary<Guid, Product>();

    public Guid Add(ProductBase product)
    {
        var id = Guid.NewGuid();

        _productStore.Add(id, new Product
        {
            Id = id,
            Name = product.Name,
            Quantity = product.Quantity,
        });

        return id;
    }


    public IEnumerable<Product> List()
        => _productStore.Values;


    public Product Get(Guid id)
    {
        if(!_productStore.ContainsKey(id))
        {
            throw new Exception("Product not found");
        }

        return _productStore[id];
    }

    public void Update(Guid id, ProductBase product)
    {
        if(!_productStore.ContainsKey(id))
        {
            throw new Exception("Product not found");
        }

        _productStore[id] = new Product
        {
            Id = id,
            Name = product.Name,
            Quantity = product.Quantity,
        };
    }

    public void Delete(Guid id)
    {
        if(!_productStore.ContainsKey(id))
        {
            throw new Exception("Product not found");
        }

        _productStore.Remove(id);
    }
}
