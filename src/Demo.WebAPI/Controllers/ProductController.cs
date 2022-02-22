using System;
using Demo.WebAPI.Models;
using Demo.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace Demo.WebAPI.Controllers;

[ApiController]
[Route("product")]
public class ProductController : ControllerBase
{
    private static readonly Gauge _productGaugeMetrics = Metrics.CreateGauge("demoapi_product_gauge", "Number of products");

    private static readonly Counter _productPostRequestCounterMetrics = Metrics.CreateCounter("demoapi_product_post_request_counter", "Number of POST requests");
    private static readonly Counter _productListRequestCounterMetrics = Metrics.CreateCounter("demoapi_product_list_request_counter", "Number of LIST requests");
    private static readonly Counter _productGetRequestCounterMetrics = Metrics.CreateCounter("demoapi_product_get_request_counter", "Number of GET requests");
    private static readonly Counter _productUpdateRequestCounterMetrics = Metrics.CreateCounter("demoapi_product_update_request_counter", "Number of UPDATE requests");
    private static readonly Counter _productDeleteRequestCounterMetrics = Metrics.CreateCounter("demoapi_product_delete_request_counter", "Number of DELETE requests");

    private readonly IProductRepository _service;
    public ProductController(IProductRepository service)
        => _service = service;

    [HttpPost]
    public IActionResult Post(ProductBase product)
    {
        _productPostRequestCounterMetrics.Inc();

        var id = _service.Add(product);

        _productGaugeMetrics.Inc();

        return new CreatedAtRouteResult(
           "GetProduct",
           new { id },
           id
        );
    }

    [HttpGet]
    public IActionResult List()
    {
        _productListRequestCounterMetrics.Inc();

        return Ok(_service.List());
    }

    [HttpGet("{id:guid}", Name = "GetProduct")]
    public IActionResult Get([FromRoute] Guid id)
    {
        _productGetRequestCounterMetrics.Inc();

        return Ok(_service.Get(id));
    }

    [HttpPut("{id:guid}")]
    public IActionResult Put([FromRoute] Guid id, ProductBase product)
    {
        _productUpdateRequestCounterMetrics.Inc();

        _service.Update(id, product);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        _productDeleteRequestCounterMetrics.Inc();

        _service.Delete(id);

        _productGaugeMetrics.Dec();

        return NoContent();
    }
}
