using AdvertAPI.Models;

namespace AdvertAPI.Services
{
    public interface IAdvertStorageService
    {
        Task<string> AddAsync(AdvertModel advert);
        Task<bool> ConfirmAsync(ConfirmAdvertModel confirmation);
        Task<AdvertModel?> GetByIdAsync(string id);
        Task<List<AdvertModel>> GetAllAsync();
    }
}
