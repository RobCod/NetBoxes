
namespace NetBoxes.Network
{
    public interface IGamePeer
    {
        void Start();
        void SendPacket<T>(T packet) where T : class, new();
        void Update();
    }
}
