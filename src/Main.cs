using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using NetBoxes.Network;

namespace NetBoxes
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Texture2D BoxTexture { get; set; }

        public Player LocalPlayer { get; set; }
        public List<Player> Players { get; set; }

        public IGamePeer Peer { get; set; }

        public bool IsHost
        {
            get { return Peer != null && Peer is GameServer; }
        }

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;

            _graphics.ApplyChanges();

            Players = new List<Player>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            BoxTexture = new Texture2D(GraphicsDevice, 16, 16);
            Color[] boxTextureData = new Color[BoxTexture.Width * BoxTexture.Height];

            for (int index = 0; index < boxTextureData.Length; ++index)
                boxTextureData[index] = Color.White;

            BoxTexture.SetData(boxTextureData);
        }

        protected override void Update(GameTime gameTime)
        {
            Context.Update();

            if (Context.Keyboard.IsKeyPressed(Keys.Escape))
                Exit();

            if (Context.Keyboard.IsKeyPressed(Keys.Q))
            {
                if (Peer == null)
                {
                    Peer = new GameServer(9005);
                    Peer.Start();
                }
            }

            if (Context.Keyboard.IsKeyPressed(Keys.E))
            {
                if (Peer == null)
                {
                    Peer = new GameClient("127.0.0.1", 9005);
                    Peer.Start();
                }
            }

            if (Peer != null)
                Peer.Update();

            for (int index = 0; index < Players.Count; ++index)
                Players[index].Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);

            for (int index = 0; index < Players.Count; ++index)
                Players[index].Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
