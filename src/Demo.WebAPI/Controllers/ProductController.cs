using System;
using Demo.WebAPI.Models;
using Demo.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebAPI.Controllers;

[ApiController]
[Route("product")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _service;
    public ProductController(IProductRepository service)
        => _service = service;

    [HttpPost]
    public IActionResult Post(ProductBase product)
    {
        var id = _service.Add(product);

        return new CreatedAtRouteResult(
           "GetProduct",
           new { id },
           id
        );
    }

    [HttpGet]
    public IActionResult Get()
        => Ok(_service.List());

    [HttpGet("{id:guid}", Name = "GetProduct")]
    public IActionResult Get([FromRoute] Guid id)
        => Ok(_service.Get(id));

    [HttpPut("{id:guid}")]
    public IActionResult Put([FromRoute] Guid id, ProductBase product)
    {
        _service.Update(id, product);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        _service.Delete(id);

        return NoContent();
    }
}
