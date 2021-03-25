using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using NetBoxes.Network.Packets;
using NetBoxes.Network.Server;

namespace NetBoxes.Network
{
    public class GameServer : IGamePeer
    {
        private NetPacketProcessor _packetProcessor;

        private EventBasedNetListener _listener;
        private NetManager _server;

        private Dictionary<int, PlayerState> _players;

        private int _port;

        public GameServer(int port)
        {
            _packetProcessor = new NetPacketProcessor();

            _listener = new EventBasedNetListener();
            _server = new NetManager(_listener);

            _players = new Dictionary<int, PlayerState>();
            _players.Add(-1, new PlayerState());

            Context.Game.LocalPlayer = new Player(-1, Vector2.Zero);
            Context.Game.Players.Add(Context.Game.LocalPlayer);

            _port = port;
        }

        public void Start()
        {
            _packetProcessor.SubscribeReusable<PlayerInputPacket, NetPeer>(OnPlayerInputPacketReceive);

            _listener.ConnectionRequestEvent += OnConnectionRequest;
            _listener.PeerConnectedEvent += OnPeerConnected;

            _listener.NetworkReceiveEvent += OnNetworkReceive;

            _server.Start(_port);
            System.Console.WriteLine("Server started at *:{0}", _server.LocalPort);
        }

        public void SendPacket<T>(T packet) where T : class, new()
        {
            _server.SendToAll(_packetProcessor.Write<T>(packet), DeliveryMethod.ReliableOrdered);
        }

        public void Update()
        {
            _server.PollEvents();
        }

        private void OnConnectionRequest(ConnectionRequest request)
        {
            if (_server.ConnectedPeersCount < 10)
                request.AcceptIfKey("NetBoxes");
            else
                request.Reject();
        }

        private void OnPeerConnected(NetPeer peer)
        {
            System.Console.WriteLine("Connection from {0}!", peer.EndPoint);
            _players.Add(peer.Id, new PlayerState());

            Context.Game.Players.Add(new Player(peer.Id, Vector2.Zero));

            WelcomePacket packet = new WelcomePacket(peer.Id);
            peer.Send(_packetProcessor.Write(packet), DeliveryMethod.ReliableOrdered);

            foreach (KeyValuePair<int, PlayerState> pair in _players)
            {
                PlayerStatePacket statePacket = PlayerStatePacket.FromPlayerState(pair.Key, pair.Value);
                peer.Send(_packetProcessor.Write(statePacket), DeliveryMethod.ReliableOrdered);
            }
        }

        private void OnPlayerInputPacketReceive(PlayerInputPacket inputPacket, NetPeer peer)
        {
            if (_players.ContainsKey(peer.Id))
            {
                _players[peer.Id].X += inputPacket.X;
                _players[peer.Id].Y += inputPacket.Y;

                for (int index = 0; index < Context.Game.Players.Count; ++index)
                {
                    Player player = Context.Game.Players[index];

                    if (player.PeerId == peer.Id)
                    {
                        player.Position = new Vector2(player.Position.X + inputPacket.X,
                            player.Position.Y + inputPacket.Y);

                        break;
                    }
                }

                SendPacket(PlayerStatePacket.FromPlayerState(peer.Id, _players[peer.Id]));
            }
        }

        private void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            _packetProcessor.ReadAllPackets(reader, peer);
        }
    }
}
