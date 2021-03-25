using Microsoft.Xna.Framework;
using NetBoxes.Network.Server;

namespace NetBoxes.Network.Packets
{
    public class PlayerStatePacket
    {
        public static PlayerStatePacket FromPlayerState(int peerId, PlayerState state)
        {
            return new PlayerStatePacket()
            {
                PeerId = peerId,
                X = state.X,
                Y = state.Y,
            };
        }

        public int PeerId { get; set; }

        public float X { get; set; }
        public float Y { get; set; }

        public PlayerStatePacket()
        {
        }

        public PlayerStatePacket(int peerId, float x, float y)
        {
            PeerId = peerId;
            X = x;
            Y = y;
        }

        public PlayerStatePacket(int peerId, Vector2 velocity)
            : this(peerId, velocity.X, velocity.Y)
        {
        }
    }
}
