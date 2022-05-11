namespace MelonAPI.Repository { 

    public interface IContextRepository
    {
        int LoadCurrentUserId(string token);
    }
}
