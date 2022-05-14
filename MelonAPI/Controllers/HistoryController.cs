using MelonAPI.Model;
using MelonAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MelonAPI.Controllers
{
    [Route("/history")]
    [ApiController]
    public class HistoryController : ControllerBase
    {

        private readonly IHistoryRepository historyRepository;

        private readonly IContextRepository contextRepository;

        public HistoryController(IHistoryRepository historyRepository,
                                 IContextRepository contextRepository)
        {
            this.historyRepository = historyRepository;
            this.contextRepository = contextRepository;
        }

        [HttpGet]
        public Dictionary<int, List<ProductLight>> Get()
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            return historyRepository.LoadHistories(userId);
        }

    }
}
