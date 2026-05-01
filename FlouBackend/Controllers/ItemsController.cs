using System.Security.Claims;
using FlouBackend.Business.DTOs.Requests;
using FlouBackend.Business.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlouBackend.Controllers;

[ApiController]
[Route("api/items")]
[Authorize]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemsController(IItemService itemService) => _itemService = itemService;

    // Extracts the userId from the JWT token that was sent with the request
    private string GetUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemRequest request)
    {
        var result = await _itemService.CreateAsync(request, GetUserId());
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _itemService.GetAllForUserAsync(GetUserId());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _itemService.GetByIdAsync(id, GetUserId());

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
