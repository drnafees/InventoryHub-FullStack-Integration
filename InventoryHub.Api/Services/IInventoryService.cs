using InventoryHub.Shared.Models;

namespace InventoryHub.Api.Services;

public interface IInventoryService
{
    Task<PagedResult<InventoryItem>> GetItemsAsync(string? searchTerm, string? category, int pageNumber, int pageSize);
    Task<InventoryItem?> GetByIdAsync(int id);
    Task<InventoryItem> CreateAsync(InventoryItem item);
    Task<bool> UpdateAsync(int id, InventoryItem item);
    Task<bool> DeleteAsync(int id);
}