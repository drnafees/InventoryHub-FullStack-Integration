using InventoryHub.Shared.Models;

namespace InventoryHub.Api.Services;

public class InventoryService : IInventoryService
{
    private readonly List<InventoryItem> _items = new()
    {
        new InventoryItem { Id = 1, Name = "Laptop Stand", SKU = "ACC-001", Category = "Accessories", Quantity = 45, UnitPrice = 29.99m },
        new InventoryItem { Id = 2, Name = "Mechanical Keyboard", SKU = "PER-002", Category = "Peripherals", Quantity = 120, UnitPrice = 89.50m },
        new InventoryItem { Id = 3, Name = "Wireless Mouse", SKU = "PER-003", Category = "Peripherals", Quantity = 85, UnitPrice = 34.99m },
        new InventoryItem { Id = 4, Name = "USB-C Hub", SKU = "ACC-004", Category = "Accessories", Quantity = 60, UnitPrice = 49.99m },
        new InventoryItem { Id = 5, Name = "27-inch Monitor", SKU = "DIS-005", Category = "Displays", Quantity = 20, UnitPrice = 299.99m }
    };

    public Task<PagedResult<InventoryItem>> GetItemsAsync(string? searchTerm, string? category, int pageNumber, int pageSize)
    {
        var query = _items.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(i => i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     i.SKU.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(i => i.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = query.Count();
        var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return Task.FromResult(new PagedResult<InventoryItem>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        });
    }

    public Task<InventoryItem?> GetByIdAsync(int id)
    {
        return Task.FromResult(_items.FirstOrDefault(i => i.Id == id));
    }

    public Task<InventoryItem> CreateAsync(InventoryItem item)
    {
        item.Id = _items.Any() ? _items.Max(i => i.Id) + 1 : 1;
        item.LastUpdated = DateTime.UtcNow;
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<bool> UpdateAsync(int id, InventoryItem item)
    {
        var existing = _items.FirstOrDefault(i => i.Id == id);
        if (existing == null) return Task.FromResult(false);

        existing.Name = item.Name;
        existing.SKU = item.SKU;
        existing.Category = item.Category;
        existing.Quantity = item.Quantity;
        existing.UnitPrice = item.UnitPrice;
        existing.LastUpdated = DateTime.UtcNow;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var existing = _items.FirstOrDefault(i => i.Id == id);
        if (existing == null) return Task.FromResult(false);

        _items.Remove(existing);
        return Task.FromResult(true);
    }
}