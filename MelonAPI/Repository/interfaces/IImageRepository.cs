namespace MelonAPI.Repository
{
    public interface IImageRepository
    {
        Byte[] DownloadImage(int imageId);

        int UploadImage(byte[] image);
    }
}
