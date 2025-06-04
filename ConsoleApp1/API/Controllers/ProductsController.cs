using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ConsoleApp1.Application.Services;
using ConsoleApp1.Domain.Entities;
using ConsoleApp1.DTOs.Request;
using ConsoleApp1.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleApp1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;
    private readonly IMapper _mapper;

    public ProductsController(ProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        var productDtos = _mapper.Map<List<ProductResponse>>(products);
        return Ok(productDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();

        var productDto = _mapper.Map<ProductResponse>(product);
        return Ok(productDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateRequest productCreate)
    {
        var productEntity = _mapper.Map<Product>(productCreate);
        productEntity.SetStock(productCreate.WarehouseStock);

        var created = await _productService.CreateAsync(productEntity);
        var productResponse = _mapper.Map<ProductResponse>(created);

        return CreatedAtAction(nameof(Get), new { id = productResponse.Id }, productResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateRequest productUpdate)
    {
        var productEntity = _mapper.Map<Product>(productUpdate);
        var success = await _productService.UpdateAsync(id, productEntity);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _productService.DeleteAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}
