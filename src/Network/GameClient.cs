using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using NetBoxes.Network.Packets;

namespace NetBoxes.Network
{
    public class GameClient : IGamePeer
    {
        private NetPacketProcessor _packetProcessor;

        private EventBasedNetListener _listener;
        private NetManager _client;

        private string _host;
        private int _port;

        public GameClient(string host, int port)
        {
            _packetProcessor = new NetPacketProcessor();

            _listener = new EventBasedNetListener();
            _client = new NetManager(_listener);

            _host = host;
            _port = port;
        }

        public void Start()
        {
            _packetProcessor.SubscribeReusable<WelcomePacket, NetPeer>(OnWelcomePacketReceive);
            _packetProcessor.SubscribeReusable<PlayerStatePacket, NetPeer>(OnPlayerStatePacketReceive);

            _listener.NetworkReceiveEvent += OnNetworkReceive;

            _client.Start();
            _client.Connect(_host, _port, "NetBoxes");
        }

        public void SendPacket<T>(T packet) where T : class, new()
        {
            _client.FirstPeer.Send(_packetProcessor.Write<T>(packet), DeliveryMethod.ReliableOrdered);
        }

        public void Update()
        {
            _client.PollEvents();
        }

        private void OnWelcomePacketReceive(WelcomePacket packet, NetPeer peer)
        {
            Context.Game.LocalPlayer = new Player(packet.PeerId, Vector2.Zero);
            Context.Game.Players.Add(Context.Game.LocalPlayer);
        }

        private void OnPlayerStatePacketReceive(PlayerStatePacket packet, NetPeer peer)
        {
            Player player = null;

            for (int index = 0; index < Context.Game.Players.Count; ++index)
            {
                Player current = Context.Game.Players[index];

                if (current.PeerId == packet.PeerId)
                {
                    player = current;
                    break;
                }
            }

            if (player == null)
            {
                player = new Player(packet.PeerId, new Vector2(packet.X, packet.Y));
                Context.Game.Players.Add(player);
            }
            else
            {
                player.Position = new Microsoft.Xna.Framework.Vector2(packet.X, packet.Y);
            }
        }

        private void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            _packetProcessor.ReadAllPackets(reader, peer);
        }
    }
}
