﻿using System.Collections.Generic;
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
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public UsersController(UserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        var userList = _mapper.Map<List<UserResponse>>(users);
        return Ok(userList);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();

        var userDto = _mapper.Map<UserResponse>(user);
        return Ok(userDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateRequest userCreate)
    {
        var userEntity = _mapper.Map<User>(userCreate);
        var createdUser = await _userService.CreateUserAsync(userEntity);
        var userResponse = _mapper.Map<UserResponse>(createdUser);

        return CreatedAtAction(nameof(Get), new { id = userResponse.UserId }, userResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateRequest userUpdate)
    {
        var userEntity = _mapper.Map<User>(userUpdate);
        var result = await _userService.UpdateUserAsync(id, userEntity);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        return result ? NoContent() : NotFound();
    }
}