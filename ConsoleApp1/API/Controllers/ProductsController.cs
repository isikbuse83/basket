using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ConsoleApp1.Services;
using AutoMapper;
using ConsoleApp1.DTOs.Request;
using ConsoleApp1.Reponse;
using ConsoleApp1.Domain.Entities;


namespace ConsoleApp1.Controllers;

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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();

        var productDto = _mapper.Map<ProductResponse>(product);
        return Ok(productDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateRequest productCreate)
    {
        try
        {
            var productEntity = _mapper.Map<Domain.Product>(productCreate);
            var created = await _productService.CreateAsync(productEntity);
            var productResponse = _mapper.Map<ProductResponse>(created);
            return CreatedAtAction(nameof(Get), new { id = productResponse.Id }, productResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductUpdateRequest productUpdate)
    {
        var productEntity = _mapper.Map<Domain.Product>(productUpdate);
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
