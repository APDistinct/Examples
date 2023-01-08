namespace FLChat.WebService.MediaType
{
    public interface IPictureCutter
    {
        //bool IsFoursquare(byte[] data);
        void Cut(ref byte[] data);
    }
}
