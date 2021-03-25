using Microsoft.Xna.Framework;

namespace NetBoxes.Network.Packets
{
    public class PlayerInputPacket
    {
        public float X { get; set; }
        public float Y { get; set; }

        public PlayerInputPacket()
        {
        }

        public PlayerInputPacket(float x, float y)
        {
            X = x;
            Y = y;
        }

        public PlayerInputPacket(Vector2 vector)
            : this(vector.X, vector.Y)
        {
        }
    }
}
