using MelonAPI.Model;

namespace MelonAPI.Repository
{
    public interface IHistoryRepository
    {
        Dictionary<int, List<ProductLight>> LoadHistories(int userId);
    }
}
