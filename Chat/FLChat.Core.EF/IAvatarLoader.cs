using FLChat.DAL.Model;

namespace FLChat.Core
{
    public interface IAvatarLoader
    {
        void TryLoadAvatar(ChatEntities entities, IOuterMessage message, Transport transport);
    }

    public interface IAvatarProvider
    {
        byte[] GetAvatarPicture(string messageAvatarUrl, string outerSystemUserId);
    }
}