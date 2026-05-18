using FlouBackend.Business.DTOs.Requests.FolderRequests;
using FlouBackend.Business.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlouBackend.Controllers;

[ApiController]
[Route("api/folders")]
[Authorize]
public class FoldersController : ControllerBase
{
    private readonly IFolderService _folderService;

    public FoldersController(IFolderService folderService) => _folderService = folderService;

    private string GetUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // POST /api/folders
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFolderRequest request)
    {
        var result = await _folderService.CreateAsync(request, GetUserId());

        if (result is null)
            return NotFound(); // parent folder not found or depth limit exceeded

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // GET /api/folders/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _folderService.GetByIdAsync(id, GetUserId());

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    // GET /api/folders/root
    [HttpGet("root")]
    public async Task<IActionResult> GetRootFolders()
    {
        var folders = await _folderService.GetRootFoldersAsync(GetUserId());
        return Ok(folders);
    }

    [HttpPut("{id:guid}")]

    public async Task<IActionResult> UpdateFolderById([FromBody] UpdateFolderRequest folder, Guid id)
    {
        var result = await _folderService.UpdateAsync(folder,id,GetUserId());
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);

    }
}
