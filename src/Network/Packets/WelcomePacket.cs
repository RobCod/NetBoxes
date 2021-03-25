
namespace NetBoxes.Network.Packets
{
    public class WelcomePacket
    {
        public int PeerId { get; set; }

        public WelcomePacket()
        {
        }

        public WelcomePacket(int peerId)
        {
            PeerId = peerId;
        }
    }
}
