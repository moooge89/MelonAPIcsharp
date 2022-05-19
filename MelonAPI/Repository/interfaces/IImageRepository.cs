namespace MelonAPI.Repository
{
    public interface IImageRepository
    {
        Byte[] DownloadImage(int imageId);

        void UploadImage(byte[] image);
    }
}
