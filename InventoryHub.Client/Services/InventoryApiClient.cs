using System.Net.Http.Json;
using InventoryHub.Shared.Models;

namespace InventoryHub.Client.Services;

public class InventoryApiClient : IInventoryApiClient
{
    private readonly HttpClient _httpClient;

    public InventoryApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResult<InventoryItem>?> GetItemsAsync(string? search, string? category, int pageNumber, int pageSize)
    {
        var url = $"api/inventory?search={search}&category={category}&pageNumber={pageNumber}&pageSize={pageSize}";
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResult<InventoryItem>>>(url);
        return response?.Data;
    }

    public async Task<InventoryItem?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<InventoryItem>>($"api/inventory/{id}");
        return response?.Data;
    }

    public async Task<bool> CreateItemAsync(InventoryItem item)
    {
        var res = await _httpClient.PostAsJsonAsync("api/inventory", item);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateItemAsync(int id, InventoryItem item)
    {
        var res = await _httpClient.PutAsJsonAsync($"api/inventory/{id}", item);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var res = await _httpClient.DeleteAsync($"api/inventory/{id}");
        return res.IsSuccessStatusCode;
    }
}