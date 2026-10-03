using Microsoft.AspNetCore.Mvc;
using InventoryHub.Api.Services;
using InventoryHub.Shared.Models;

namespace InventoryHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<InventoryItem>>>> GetItems([FromQuery] string? search, [FromQuery] string? category, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _inventoryService.GetItemsAsync(search, category, pageNumber, pageSize);
        return Ok(new ApiResponse<PagedResult<InventoryItem>> { Success = true, Message = "Items retrieved successfully", Data = result });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<InventoryItem>>> GetById(int id)
    {
        var item = await _inventoryService.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(new ApiResponse<InventoryItem> { Success = false, Message = $"Item with ID {id} not found" });
        }
        return Ok(new ApiResponse<InventoryItem> { Success = true, Data = item });
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<InventoryItem>>> Create([FromBody] InventoryItem item)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<InventoryItem> { Success = false, Message = "Invalid payload" });
        }
        var created = await _inventoryService.CreateAsync(item);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ApiResponse<InventoryItem> { Success = true, Message = "Item created successfully", Data = created });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] InventoryItem item)
    {
        var updated = await _inventoryService.UpdateAsync(id, item);
        if (!updated) return NotFound(new ApiResponse<bool> { Success = false, Message = "Item not found" });
        return Ok(new ApiResponse<bool> { Success = true, Message = "Item updated successfully", Data = true });
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var deleted = await _inventoryService.DeleteAsync(id);
        if (!deleted) return NotFound(new ApiResponse<bool> { Success = false, Message = "Item not found" });
        return Ok(new ApiResponse<bool> { Success = true, Message = "Item deleted successfully", Data = true });
    }
}