using InventoryHub.Shared.Models;

namespace InventoryHub.Client.Services;

public interface IInventoryApiClient
{
    Task<PagedResult<InventoryItem>?> GetItemsAsync(string? search, string? category, int pageNumber, int pageSize);
    Task<InventoryItem?> GetByIdAsync(int id);
    Task<bool> CreateItemAsync(InventoryItem item);
    Task<bool> UpdateItemAsync(int id, InventoryItem item);
    Task<bool> DeleteItemAsync(int id);
}