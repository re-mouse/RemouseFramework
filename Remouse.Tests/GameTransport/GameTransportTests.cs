using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Cysharp.Threading.Tasks;
using Remouse.DI;
using Remouse.Network.Client;
using Remouse.Network.Server;
using NUnit.Framework;
using Shared.Test;
using Remouse.Utils;

namespace Remouse.Network.Sockets.Tests
{
    public class GameTransportTests
    {
        private const int TimeOutSeconds = 5;
        private static int currentPort = 14344;
        private ContainerBuilder _containerBuilder;

        [SetUp]
        public void Setup()
        {
            _containerBuilder = new ContainerBuilder(); 
            _containerBuilder.AddModule<NetworkClientModule<TestAuthorizeProvider>>();
            _containerBuilder.AddModule<NetworkServerModule<TestAuthorizer>>();
            LLogger.SetLogger(new TestLLogger());
        }
        
        [Test]
        public void GameClientAndGameServer_ShouldConnectWithinTimeout()
        {
            var container = _containerBuilder.Build();

            var gameServer = container.Resolve<IServerTransport>();
            var gameClientEvents = container.Resolve<IClientTransportEvents>();
            
            var gameClient = container.Resolve<IClientTransport>();
            var gameServerEvents = container.Resolve<IServerTransportEvents>();
            
            bool isClientConnected = false;
            bool isServerConnected = false;
            gameClientEvents.Connected += () => isClientConnected = true;
            gameServerEvents.Connected += p => isServerConnected = true;

            gameServer.Start((ushort)Interlocked.Increment(ref currentPort), TimeOutSeconds);
            var connectTask = gameClient.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort), new CancellationToken()).AsTask();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while ((!isClientConnected || !isServerConnected) && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }

            stopwatch.Stop();

            bool isConnected = isClientConnected || isServerConnected;
            Assert.IsTrue(isConnected, $"Client and server did not connect to server within the timeout period. Client connect result: {connectTask.Result}");
            Assert.IsTrue(isClientConnected, "Client did not raised connection event");
            Assert.IsTrue(isServerConnected, "Server did not raised connection event");
        }
        
        [Test]
        public void GameClient_ShouldConnectAndDisconnect()
        {
            var container = _containerBuilder.Build();

            var gameServer = container.Resolve<IServerTransport>();
            var gameClientEvents = container.Resolve<IClientTransportEvents>();
            
            var gameClient = container.Resolve<IClientTransport>();
            var gameServerEvents = container.Resolve<IServerTransportEvents>();
            
            bool isClientConnected = false;
            bool isServerConnected = false;
            bool isServerDisconnected = false;
            bool isClientDisconnected = false;
            gameClientEvents.Connected += () => isClientConnected = true;
            gameServerEvents.Connected += p => isServerConnected = true;
            gameServerEvents.Disconnected += p => isServerDisconnected = true;
            gameClientEvents.Disconnected += () => isClientDisconnected = true;

            gameServer.Start((ushort)Interlocked.Increment(ref currentPort), TimeOutSeconds);
            var connectTask = gameClient.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort), new CancellationToken()).AsTask();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while ((!isClientConnected || !isServerConnected) && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            gameClient.Disconnect();
            
            while ((!isClientDisconnected || !isServerDisconnected) && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }

            stopwatch.Stop();

            bool isConnected = isClientConnected || isServerConnected;
            Assert.IsTrue(isConnected, $"Client and server did not connect to server within the timeout period. Client connect result: {connectTask.Result}");
            Assert.IsTrue(isClientConnected, "Client did not raised connection event");
            Assert.IsTrue(isServerConnected, "Server did not raised connection event");
            
            bool isDisconnected = isClientDisconnected || isServerDisconnected;
            Assert.IsTrue(isDisconnected, $"Client and server did not discconnected within the timeout period.");
            Assert.IsTrue(isClientDisconnected, "Client did not raised disconnection event");
            Assert.IsTrue(isServerDisconnected, "Server did not raised disconnection event");
        }
        
        [Test]
        public void GameServer_ShouldDisconnectPlayer()
        {
            var container = _containerBuilder.Build();

            var gameServer = container.Resolve<IServerTransport>();
            var gameClientEvents = container.Resolve<IClientTransportEvents>();
            
            var gameClient = container.Resolve<IClientTransport>();
            var gameServerEvents = container.Resolve<IServerTransportEvents>();
            
            bool isClientConnected = false;
            bool isServerConnected = false;
            bool isServerDisconnected = false;
            bool isClientDisconnected = false;
            IPlayer player = null;
            gameClientEvents.Connected += () => isClientConnected = true;
            gameServerEvents.Connected += p =>
            {
                isServerConnected = true;
                player = p;
            };
            gameServerEvents.Disconnected += p => isServerDisconnected = true;
            gameClientEvents.Disconnected += () => isClientDisconnected = true;

            gameServer.Start((ushort)Interlocked.Increment(ref currentPort), TimeOutSeconds);
            var connectTask = gameClient.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort), new CancellationToken()).AsTask();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while ((!isClientConnected || !isServerConnected) && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            bool isConnected = isClientConnected || isServerConnected;
            Assert.IsTrue(isConnected, $"Client and server did not connect to server within the timeout period. Client connect result: {connectTask.Result}");
            Assert.IsTrue(isClientConnected, "Client did not raised connection event");
            Assert.IsTrue(isServerConnected, "Server did not raised connection event");
            
            player?.Disconnect();
            
            while ((!isClientDisconnected || !isServerDisconnected) && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }

            stopwatch.Stop();
            
            bool isDisconnected = isClientDisconnected || isServerDisconnected;
            Assert.IsTrue(isDisconnected, $"Client and server did not discconnected within the timeout period.");
            Assert.IsTrue(isClientDisconnected, "Client did not raised disconnection event");
            Assert.IsTrue(isServerDisconnected, "Server did not raised disconnection event");
        }
        
        [Test]
        public void GameClient_ShouldSendNetworkMessage()
        {
            var container = _containerBuilder.Build();

            var gameServer = container.Resolve<IServerTransport>();
            var gameClientEvents = container.Resolve<IClientTransportEvents>();
            
            var gameClient = container.Resolve<IClientTransport>();
            var gameServerEvents = container.Resolve<IServerTransportEvents>();
            
            bool isClientConnected = false;
            bool isServerConnected = false;
            bool correctMessageReceived = false;
            gameClientEvents.Connected += () => isClientConnected = true;
            gameServerEvents.Connected += p => isServerConnected = true;
            gameServerEvents.MessageReceived += (p, m) =>
            {
                var message = m as TestTextMessage;
                var text = message?.text;
                correctMessageReceived = text == "Test";
                correctMessageReceived = true;
            };  

            gameServer.Start((ushort)Interlocked.Increment(ref currentPort), TimeOutSeconds);
            var connectTask = gameClient.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort), new CancellationToken()).AsTask();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while ((!isClientConnected || !isServerConnected) && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            Assert.IsTrue(isClientConnected || isServerConnected, $"Client and server did not connect to server within the timeout period. Client connect result: {connectTask.Result}");
            Assert.IsTrue(isClientConnected, "Client did not raised connection event");
            Assert.IsTrue(isServerConnected, "Server did not raised connection event");

            gameClient.Send(new TestTextMessage() {text = "Test"});
            
            while (!correctMessageReceived && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            stopwatch.Stop();
            
            Assert.IsTrue(correctMessageReceived);
        }
        
        [Test]
        public void GameClient_ShouldReceiveNetworkMessage()
        {
            var container = _containerBuilder.Build();

            var gameServer = container.Resolve<IServerTransport>();
            var gameClientEvents = container.Resolve<IClientTransportEvents>();
            
            var gameClient = container.Resolve<IClientTransport>();
            var gameServerEvents = container.Resolve<IServerTransportEvents>();
            
            bool isClientConnected = false;
            bool isServerConnected = false;
            bool correctMessageReceived = false;
            IPlayer player = null;
            gameClientEvents.Connected += () => isClientConnected = true;
            gameServerEvents.Connected += p =>
            {
                player = p;
                isServerConnected = true;
            };
            gameClientEvents.MessageReceived += m =>
            {
                Assert.IsTrue(m is TestTextMessage, "Received wrong type of message");
                var message = m as TestTextMessage;
                var text = message?.text;
                Assert.IsTrue(text == "Test", "Received wrong message");
                correctMessageReceived = true;
            };  

            gameServer.Start((ushort)Interlocked.Increment(ref currentPort), TimeOutSeconds);
            var connectTask = gameClient.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort), new CancellationToken()).AsTask();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while ((!isClientConnected || !isServerConnected) && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            bool isConnected = isClientConnected || isServerConnected;
            Assert.IsTrue(isConnected, $"Client and server did not connect to server within the timeout period. Client connect result: {connectTask.Result}");
            Assert.IsTrue(isClientConnected, "Client did not raised connection event");
            Assert.IsTrue(isServerConnected, "Server did not raised connection event");

            player.Send(new TestTextMessage() {text = "Test"});
            
            while (!correctMessageReceived && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            stopwatch.Stop();
            
            Assert.IsTrue(correctMessageReceived);
        }
        
        [Test]
        public void GameClient_ShouldHandle10Messages()
        {
            var container = _containerBuilder.Build();

            var gameServer = container.Resolve<IServerTransport>();
            var gameClientEvents = container.Resolve<IClientTransportEvents>();
            
            var gameClient = container.Resolve<IClientTransport>();
            var gameServerEvents = container.Resolve<IServerTransportEvents>();
            
            bool isClientConnected = false;
            bool isServerConnected = false;
            IPlayer player = null;
            int correctMessagesCount = 0;
            gameClientEvents.Connected += () => isClientConnected = true;
            gameServerEvents.Connected += p =>
            {
                player = p;
                isServerConnected = true;
            };
            gameClientEvents.MessageReceived += (m) =>
            {
                Assert.IsTrue(m is TestTextMessage, "Received wrong type of message");
                var message = m as TestTextMessage;
                var text = message?.text;
                Assert.IsTrue(text == "Tesst", "Received wrong message");
                correctMessagesCount++;
            };  

            gameServer.Start((ushort)Interlocked.Increment(ref currentPort), TimeOutSeconds);
            var connectTask = gameClient.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort), new CancellationToken()).AsTask();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while ((!isClientConnected || !isServerConnected) && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            bool isConnected = isClientConnected || isServerConnected;
            Assert.IsTrue(isConnected, $"Client and server did not connect to server within the timeout period. Client connect result: {connectTask.Result}");
            Assert.IsTrue(isClientConnected, "Client did not raised connection event");
            Assert.IsTrue(isServerConnected, "Server did not raised connection event");

            for (int i = 0; i < 10; i++)
                player.Send(new TestTextMessage() {text = "Tesst"});
            
            while (correctMessagesCount != 10 && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            stopwatch.Stop();
            
            Assert.AreEqual(10, correctMessagesCount, "Not all messages was received");
        }
        
        [Test]
        public void GameServer_ShouldHandle10Messages()
        {
            var container = _containerBuilder.Build();

            var gameServer = container.Resolve<IServerTransport>();
            var gameClientEvents = container.Resolve<IClientTransportEvents>();
            
            var gameClient = container.Resolve<IClientTransport>();
            var gameServerEvents = container.Resolve<IServerTransportEvents>();
            
            bool isClientConnected = false;
            bool isServerConnected = false;
            IPlayer player = null;
            int correctMessagesCount = 0;
            gameClientEvents.Connected += () => isClientConnected = true;
            gameServerEvents.Connected += p =>
            {
                player = p;
                isServerConnected = true;
            };
            gameServerEvents.MessageReceived += (p, m) =>
            {
                Assert.IsTrue(m is TestTextMessage, "Received wrong type of message");
                var message = m as TestTextMessage;
                var text = message?.text;
                Assert.IsTrue(text == "Tesst", "Received wrong message");
                correctMessagesCount++;
            };  

            gameServer.Start((ushort)Interlocked.Increment(ref currentPort), TimeOutSeconds);
            var connectTask = gameClient.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort), new CancellationToken()).AsTask();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while ((!isClientConnected || !isServerConnected) && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            bool isConnected = isClientConnected || isServerConnected;
            Assert.IsTrue(isConnected, $"Client and server did not connect to server within the timeout period. Client connect result: {connectTask.Result}");
            Assert.IsTrue(isClientConnected, "Client did not raised connection event");
            Assert.IsTrue(isServerConnected, "Server did not raised connection event");

            for (int i = 0; i < 10; i++)
                gameClient.Send(new TestTextMessage() {text = "Tesst"});
            
            while (correctMessagesCount != 10 && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            stopwatch.Stop();
            
            Assert.AreEqual(10, correctMessagesCount, "Not all messages was received");
        }
        
        [Test]
        public void GameClient_CorrectErrorOnWrongCredentials()
        {
            var container = _containerBuilder.Build();

            var gameServer = container.Resolve<IServerTransport>();
            var gameClientEvents = container.Resolve<IClientTransportEvents>();
            
            var gameClient = container.Resolve<IClientTransport>();
            var gameServerEvents = container.Resolve<IServerTransportEvents>();

            container.Resolve<TestAuthorizeProvider>().SetSecret("Wrong");
            
            bool isClientConnected = false;
            bool isServerConnected = false;
            gameClientEvents.Connected += () => isClientConnected = true;
            gameServerEvents.Connected += p => isServerConnected = true;

            gameServer.Start((ushort)Interlocked.Increment(ref currentPort), TimeOutSeconds);
            var connectTask = gameClient.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort), new CancellationToken()).AsTask();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (!connectTask.IsCompleted && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }

            stopwatch.Stop();

            bool isConnected = isClientConnected || isServerConnected;
            Assert.IsTrue(!isConnected, $"Client and server connected but with wrong credentials. Client connect result: {connectTask.Result}");
            Assert.AreEqual(ClientConnectResult.InvalidCredentials, connectTask.Result);
        }
        
        [Test]
        public void GameServer_ShouldIgnoreBrokenMessageAndReceiveCorrect()
        {
            var container = _containerBuilder.Build();

            var gameServer = container.Resolve<IServerTransport>();
            var gameClientEvents = container.Resolve<IClientTransportEvents>();
            
            var gameClient = container.Resolve<IClientTransport>();
            var gameServerEvents = container.Resolve<IServerTransportEvents>();
            
            bool isClientConnected = false;
            bool isServerConnected = false;
            IPlayer player = null;
            int correctMessagesCount = 0;
            gameClientEvents.Connected += () => isClientConnected = true;
            gameServerEvents.Connected += p =>
            {
                player = p;
                isServerConnected = true;
            };
            gameServerEvents.MessageReceived += (p, m) =>
            {
                Assert.IsTrue(m is TestTextMessage, "Received wrong type of message");
                var message = m as TestTextMessage;
                var text = message?.text;
                Assert.IsTrue(text == "Tesst", "Received wrong message");
                correctMessagesCount++;
            };  

            gameServer.Start((ushort)Interlocked.Increment(ref currentPort), TimeOutSeconds);
            var connectTask = gameClient.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort), new CancellationToken()).AsTask();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while ((!isClientConnected || !isServerConnected) && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            bool isConnected = isClientConnected || isServerConnected;
            Assert.IsTrue(isConnected, $"Client and server did not connect to server within the timeout period. Client connect result: {connectTask.Result}");
            Assert.IsTrue(isClientConnected, "Client did not raised connection event");
            Assert.IsTrue(isServerConnected, "Server did not raised connection event");

            for (int i = 0; i < 10; i++)
            {
                gameClient.Send(new TestTextMessage() {text = "Tesst"});
                gameClient.Send(new TestWrongDeserializedMessage() {text = "Tesst"});
            }
            
            while (correctMessagesCount != 10 && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            stopwatch.Stop();
            
            Assert.AreEqual(10, correctMessagesCount, "Not all messages was received");
        }
        
        [Test]
        public void GameClient_ShouldIgnoreBrokenMessage()
        {
            var container = _containerBuilder.Build();

            var gameServer = container.Resolve<IServerTransport>();
            var gameClientEvents = container.Resolve<IClientTransportEvents>();
            
            var gameClient = container.Resolve<IClientTransport>();
            var gameServerEvents = container.Resolve<IServerTransportEvents>();
            
            bool isClientConnected = false;
            bool isServerConnected = false;
            IPlayer player = null;
            int correctMessagesCount = 0;
            gameClientEvents.Connected += () => isClientConnected = true;
            gameServerEvents.Connected += p =>
            {
                player = p;
                isServerConnected = true;
            };
            gameClientEvents.MessageReceived += (m) =>
            {
                Assert.IsTrue(m is TestTextMessage, "Received wrong type of message");
                var message = m as TestTextMessage;
                var text = message?.text;
                Assert.IsTrue(text == "Tesst", "Received wrong message");
                correctMessagesCount++;
            };  

            gameServer.Start((ushort)Interlocked.Increment(ref currentPort), TimeOutSeconds);
            var connectTask = gameClient.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort), new CancellationToken()).AsTask();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while ((!isClientConnected || !isServerConnected) && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            bool isConnected = isClientConnected || isServerConnected;
            Assert.IsTrue(isConnected, $"Client and server did not connect to server within the timeout period. Client connect result: {connectTask.Result}");
            Assert.IsTrue(isClientConnected, "Client did not raised connection event");
            Assert.IsTrue(isServerConnected, "Server did not raised connection event");

            for (int i = 0; i < 10; i++)
            {
                player.Send(new TestTextMessage() {text = "Tesst"});
                player.Send(new TestWrongDeserializedMessage() {text = "Tesst"});
            }
            
            while (correctMessagesCount != 10 && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeOutSeconds))
            {
                gameServer.Update();
                gameClient.Update();
                Thread.Sleep(10);
            }
            
            stopwatch.Stop();
            
            Assert.AreEqual(10, correctMessagesCount, "Not all messages was received");
        }
    }
}