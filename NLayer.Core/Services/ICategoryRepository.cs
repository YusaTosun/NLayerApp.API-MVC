using NLayer.Core.Models;
using NLayer.Core.Repository;

namespace NLayer.Core.Services
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetSingleCategoryByIdWithProductsAsync(int categoryId);
    }
}
