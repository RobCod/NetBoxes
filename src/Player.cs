using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetBoxes.Network.Packets;

namespace NetBoxes
{
    public class Player
    {
        public int PeerId { get; set; }
        public Vector2 Position { get; set; }

        public Player(int peerId, Vector2 position)
        {
            PeerId = peerId;
            Position = position;
        }

        public void Update()
        {
            if (this == Context.Game.LocalPlayer)
            {
                Vector2 velocity = Vector2.Zero;

                if (Context.Keyboard.IsKeyDown(Keys.D))
                    velocity = new Vector2(1f, velocity.Y);

                if (Context.Keyboard.IsKeyDown(Keys.A))
                    velocity = new Vector2(-1f, velocity.Y);

                if (Context.Keyboard.IsKeyDown(Keys.S))
                    velocity = new Vector2(velocity.X, 1f);

                if (Context.Keyboard.IsKeyDown(Keys.W))
                    velocity = new Vector2(velocity.X, -1f);

                if (Context.Game.Peer != null && velocity != Vector2.Zero)
                {
                    if (Context.Game.IsHost)
                    {
                        Position = new Vector2(Position.X + velocity.X, Position.Y + velocity.Y);

                        PlayerStatePacket statePacket = new PlayerStatePacket(PeerId, Position);
                        Context.Game.Peer.SendPacket(statePacket);
                    }
                    else
                    {
                        Context.Game.Peer.SendPacket(new PlayerInputPacket(velocity));
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Context.Game.BoxTexture, Position, null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
        }
    }
}
