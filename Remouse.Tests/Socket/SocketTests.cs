using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ReDI;
using Remouse.Serialization;
using Remouse.Utils;
using Shared.Test;

namespace Remouse.Network.Sockets.Tests
{
    public class SocketTests
    {
        private const int TimeoutSeconds = 5;
        
        private static int currentPort = 12344;

        private Container _container;
        private ContainerBuilder _containerBuilder;

        [SetUp]
        public void SetUp()
        {
            _containerBuilder = new ContainerBuilder(); 
            _containerBuilder.AddModule<NetworkSocketsModule>();
            _container = _containerBuilder.Build();
            
            LLogger.SetLogger(new TestLLogger());
        }

        [Test]
        public void ClientAndServer_ShouldConnectWithinTimeout()
        {
            var (localClientSocket, localServerSocket) = GetClientServerSockets();

            bool isConnected = false;
            localClientSocket.Connected += () => isConnected = true;
            localServerSocket.GotConnectionRequest += request => request.Accept();

            localServerSocket.Start((ushort)Interlocked.Increment(ref currentPort), 5);
            localClientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort));

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (!isConnected && stopwatch.Elapsed < TimeSpan.FromSeconds(5))
            {
                localClientSocket.PollEvents();
                localServerSocket.PollEvents();
                Thread.Sleep(10);
            }

            stopwatch.Stop();

            localClientSocket.Dispose();
            localServerSocket.Dispose();

            Assert.IsTrue(isConnected, "Client did not connect to server within the timeout period.");
        }
        
        [Test]
        public void Server_ShouldNotAcceptConnectionsBeyondLimit()
        {
            var maxConnections = 105;
            var serverSocket = _container.Resolve<IServerSocket>();
            ushort port = (ushort)Interlocked.Increment(ref currentPort);
            
            serverSocket.Start(port, maxConnections);
            serverSocket.GotConnectionRequest += request => request.Accept();
            
            var clients = new List<IClientSocket>();
            int successfulConnections = 0;

            for (int i = 0; i < maxConnections + 10; i++) // Trying to connect more than the max limit
            {
                var clientSocket = _container.Resolve<IClientSocket>();
                clientSocket.Connected += () =>
                {
                    successfulConnections++;
                };
                clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, port));
                clients.Add(clientSocket);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            while (stopwatch.Elapsed < TimeSpan.FromSeconds(TimeoutSeconds))
            {
                serverSocket.PollEvents();
                foreach (var client in clients)
                    client.PollEvents();
                Thread.Sleep(10);
            }
            stopwatch.Stop();

            Assert.AreEqual(maxConnections, successfulConnections, "Invalid connections than expected were successfully made.");

            foreach (var client in clients)
                client.Dispose();

            serverSocket.Dispose();
        }
        
        [Test]
        public void Server_ShouldHandle500ConnectionsWithDataFlood()
        {
            var clientCount = 500; // Большое количество клиентов для "флуда"
            var serverSocket = _container.Resolve<IServerSocket>();
            var clients = new List<IClientSocket>();
            byte[] testData = new byte[512]; // 10 MB
            new Random().NextBytes(testData);

            int successfulConnections = 0;
            int dataReceivedCount = 0;

            serverSocket.GotConnectionRequest += request => request.Accept();
            serverSocket.DataReceived += (connection, data) =>
            {
                if (data.RawAvailable.Length == testData.Length)
                {
                    Interlocked.Increment(ref dataReceivedCount);
                }
            };

            serverSocket.Start((ushort)Interlocked.Increment(ref currentPort), clientCount + 10); // +10 на всякий случай

            for (int i = 0; i < clientCount; i++)
            {
                var clientSocket = _container.Resolve<IClientSocket>();
                clientSocket.Connected += () => 
                {
                    Interlocked.Increment(ref successfulConnections);
                    clientSocket.Send(new BytesWriter(testData), DeliveryMethod.ReliableOrdered);
                };
                clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort));
                clients.Add(clientSocket);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.Elapsed < TimeSpan.FromSeconds(TimeoutSeconds))
            {
                serverSocket.PollEvents();
                foreach (var client in clients)
                    client.PollEvents();
                Thread.Sleep(10);
            }
            stopwatch.Stop();
            
            Assert.AreEqual(clientCount, successfulConnections, "Not all clients connected successfully during the flood test.");
            Assert.AreEqual(clientCount, dataReceivedCount, "Server did not receive all the data during the flood test.");

            foreach (var client in clients)
                client.Dispose();

            serverSocket.Dispose();
        }



        [Test]
        public void Server_ShouldHandleRapidConnectDisconnect()
        {
            int disconnectCount = 0;
            int connectCount = 0;
            int requestsCount = 0;
            var clientCount = 500; // Larger number to put stress on the socket
            var serverSocket = _container.Resolve<IServerSocket>();
            var connections = new List<Connection>(); 
            serverSocket.Start((ushort)Interlocked.Increment(ref currentPort), clientCount + 10);
            serverSocket.GotConnectionRequest += request =>
            {
                requestsCount++;
                request.Accept();
            };
            
            serverSocket.Connected += con =>
            {
                connections.Add(con);
                connectCount++;
            };
            serverSocket.Disconnected += con =>
            {
                connections.Remove(con);
                disconnectCount++;
            };
            var clients = new List<IClientSocket>();

            for (int i = 0; i < clientCount; i++)
            {
                var clientSocket = _container.Resolve<IClientSocket>();
                clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort));
                clients.Add(clientSocket);
                if (i % 10 == 0) // Randomly disconnect some clients
                {
                    clientSocket.Disconnect();
                }
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (connectCount != clientCount && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeoutSeconds))
            {
                serverSocket.PollEvents();
                foreach (var client in clients)
                    client.PollEvents();
                Thread.Sleep(5); // Shorter sleep to create more stress
            }
            stopwatch.Stop();

            foreach (var connection in connections)
            {
                connection.Disconnect();
            }
            
            stopwatch.Start();
            while (disconnectCount != clientCount && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeoutSeconds))
            {
                serverSocket.PollEvents();
                foreach (var client in clients)
                    client.PollEvents();
                Thread.Sleep(5); // Shorter sleep to create more stress
            }
            stopwatch.Stop();

            foreach (var client in clients)
                client.Dispose();

            serverSocket.Dispose();
    
            Assert.AreEqual(clientCount, requestsCount);
            Assert.AreEqual(clientCount, connectCount);
            Assert.AreEqual(clientCount, disconnectCount);
        }
        
        [Test]
        public async void Client_ShouldConnectAndSendDataAsync()
        {
            var (clientSocket, serverSocket) = GetClientServerSockets();

            serverSocket.GotConnectionRequest += request =>
            {
                request.Accept();
            };

            bool receivedData = false;
            serverSocket.DataReceived += (connection, reader) =>
            {
                string str = reader.ReadString();
                if (str == "Hello")
                {
                    receivedData = true;
                }

                return;
            };

            clientSocket.Connected += () =>
            {
                var writer = new BytesWriter();
                writer.WriteString("Hello");
                clientSocket.Send(writer, DeliveryMethod.Sequenced);
            };

            serverSocket.Start((ushort)Interlocked.Increment(ref currentPort), 5);

            var resultTask = clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort));

            var stopwatch = Stopwatch.StartNew();
            while (!receivedData && stopwatch.Elapsed.Seconds < TimeoutSeconds)
            {
                clientSocket.PollEvents();
                serverSocket.PollEvents();
                Thread.Sleep(15);
            }
            
            var result = resultTask.Result;
            
            Assert.AreEqual(true, clientSocket.IsConnected);
            Assert.AreEqual(SocketConnectResult.Successful, result);

            clientSocket.Dispose();
            serverSocket.Dispose();
        }

        [Test]
        public void Client_ShouldConnect()
        {
            var (clientSocket, serverSocket) = GetClientServerSockets();

            serverSocket.GotConnectionRequest += request =>
            {
                request.Accept();
            };

            serverSocket.Start((ushort)Interlocked.Increment(ref currentPort), 5);
            
            var resultTask = clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort));

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (!resultTask.IsCompleted && stopwatch.Elapsed.Seconds < TimeoutSeconds)
            {
                clientSocket.PollEvents();
                serverSocket.PollEvents();
                Thread.Sleep(15);
            }
            
            var result = resultTask.Result;
            
            Assert.AreEqual(true, clientSocket.IsConnected);
            Assert.AreEqual(SocketConnectResult.Successful, result);

            clientSocket.Dispose();
            serverSocket.Dispose();
        }

        [Test]
        public void Client_ShouldConnectAndReceiveDataAsync()
        {
            var task = Client_ShouldConnectAndReceiveDataAsyncLogic();
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed.Seconds < TimeoutSeconds && !task.IsCompleted)
            {
                Thread.Sleep(15);
            }
            
            Assert.AreEqual(true, task.IsCompleted);
            Assert.AreEqual(true, task.Result);
        }
        
        [Test]
        public void Client_ShouldConnectAsync()
        {
            var task = Task.Run(Client_ShouldConnectAsyncLogic);
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed.Seconds < TimeoutSeconds && !task.IsCompleted)
            {
                Thread.Sleep(15);
            }
            
            Assert.AreEqual(true, task.IsCompleted);
            Assert.AreEqual(true, task.Result);
        }
        
        public async Task<bool> Client_ShouldConnectAsyncLogic()
        {
            var (clientSocket, serverSocket) = GetClientServerSockets();

            serverSocket.GotConnectionRequest += request =>
            {
                request.Accept();
            };

            serverSocket.Start((ushort)Interlocked.Increment(ref currentPort), 5);
            
            var connectingTask = clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort));
            
            while (!connectingTask.IsCompleted)
            {
                clientSocket.PollEvents();
                serverSocket.PollEvents();
                Thread.Sleep(50);
            }
            if (connectingTask.Result != SocketConnectResult.Successful)
            {
                TestContext.Out.WriteLine("Not connected");
                return false;
            }

            if (clientSocket.IsConnected != true)
            {
                TestContext.Out.WriteLine("Not connected");
                return false;
            }
            
            clientSocket.Dispose();
            serverSocket.Dispose();

            return true;
        }
        
        public async Task<bool> Client_ShouldConnectAndReceiveDataAsyncLogic()
        {
            var (clientSocket, serverSocket) = GetClientServerSockets();

            serverSocket.GotConnectionRequest += request =>
            {
                var peer = request.Accept();
                var networkBytesWriter = new BytesWriter();
                networkBytesWriter.WriteString("Hello");
                peer.Send(networkBytesWriter, DeliveryMethod.ReliableOrdered);
            };

            bool dataReceived = false;

            clientSocket.DataReceived += reader =>
            {
                if (reader.ReadString() == "Hello")
                    dataReceived = true;
            };

            serverSocket.Start((ushort)Interlocked.Increment(ref currentPort), 5);
            
            var connectTask = clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort));
            
            while (!connectTask.IsCompleted)
            {
                clientSocket.PollEvents();
                serverSocket.PollEvents();
                Thread.Sleep(15);
            }

            if (connectTask.Result != SocketConnectResult.Successful || !clientSocket.IsConnected)
            {
                TestContext.Out.WriteLine($"Not connected. Connect result = {connectTask}, isConnected = {clientSocket.IsConnected}");
                return false;
            }

            var stopwatch = Stopwatch.StartNew();
            while (!dataReceived && clientSocket.IsConnected && stopwatch.Elapsed.Seconds < TimeoutSeconds)
            {
                clientSocket.PollEvents();
                serverSocket.PollEvents();
            }

            if (dataReceived != true)
            {
                TestContext.Out.WriteLine("Data not received");
                return false;
            }
            
            clientSocket.Dispose();
            serverSocket.Dispose();

            return true;
        }
        
        [Test]
        public async void Client_CorrectStatusCodeOnInvalidIPAsync()
        {
            var (clientSocket, serverSocket) = GetClientServerSockets();
            
            IPAddress invalidAddress = IPAddress.Parse("127.0.0.3");
            var result = await clientSocket.ConnectAsync(new IPEndPoint(invalidAddress, currentPort));
            
            Assert.AreEqual(false, clientSocket.IsConnected);
            Assert.AreEqual(SocketConnectResult.Timeout, result);
            
            clientSocket.Dispose();
            serverSocket.Dispose();
        }

        [Test]
        public void Client_ShouldHandleDataFlood()
        {
            var (clientSocket, serverSocket) = GetClientServerSockets();
            bool isDataReceived = false;
            byte[] testData = new byte[1024 * 1024]; // 1 MB
            new Random().NextBytes(testData);

            clientSocket.DataReceived += (data) =>
            {
                TestContext.Out.WriteLine($"Client received data. Size - {data.RawAvailable.Length}");
                if (data.RawAvailable.Length == testData.Length)
                {
                    isDataReceived = true;
                }
                else
                {
                    Assert.Fail($"Client received unexpected data size: {data.RawAvailable.Length}");
                }
            };

            serverSocket.GotConnectionRequest += request =>
            {
                TestContext.Out.WriteLine($"Server send data. Size - {testData.Length}");
                request.Accept().Send(new BytesWriter(testData), DeliveryMethod.ReliableOrdered);
            };

            serverSocket.Start((ushort)Interlocked.Increment(ref currentPort), 5);
            clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort));

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (!isDataReceived && stopwatch.Elapsed < TimeSpan.FromSeconds(5))
            {
                serverSocket.PollEvents();
                clientSocket.PollEvents();
                Thread.Sleep(10);
            }

            stopwatch.Stop();

            clientSocket.Dispose();
            serverSocket.Dispose();

            Assert.IsTrue(isDataReceived, "Client did not handle data flood.");
        }

        [Test]
        public void Client_ShouldHandleRepeatedConnectDisconnect()
        {
            var serverSocket = _container.Resolve<IServerSocket>();
            serverSocket.GotConnectionRequest += request => request.Accept();

            serverSocket.Start((ushort)Interlocked.Increment(ref currentPort), 5);

            int count = 100;

            for (int i = 0; i < count; i++) // Connect and disconnect 100 times
            {
                bool isConnected = false;
                bool isDisconnected = false;
                var clientSocket = _container.Resolve<IClientSocket>();
                clientSocket.Connected += () => { isConnected = true; };
                clientSocket.Disconnected += () => { isDisconnected = true; };

                clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort));

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (!isConnected && stopwatch.Elapsed < TimeSpan.FromSeconds(5))
                {
                    serverSocket.PollEvents();
                    clientSocket.PollEvents();
                    Thread.Sleep(10);
                }

                stopwatch.Stop();

                Assert.IsTrue(isConnected, $"Client did not connect. Processed {i} of {100}");

                clientSocket.Disconnect();
                stopwatch.Reset();
                stopwatch.Start();
                while (!isDisconnected && stopwatch.Elapsed < TimeSpan.FromSeconds(5))
                {
                    serverSocket.PollEvents();
                    clientSocket.PollEvents();
                    Thread.Sleep(10);
                }

                stopwatch.Stop();

                clientSocket.Dispose();
                Assert.IsTrue(isDisconnected, $"Client did not disconnect. Processed {i} of {100}");
            }

            serverSocket.Dispose();
        }

        [Test]
        public void Server_ShouldHandle100SimultaneousConnections()
        {
            var serverSocket = _container.Resolve<IServerSocket>();
            var clients = new List<IClientSocket>();
            int connectedClientsCount = 0;

            serverSocket.GotConnectionRequest += request =>
            {
                request.Accept();
                Interlocked.Increment(ref connectedClientsCount);
            };

            serverSocket.Start((ushort)Interlocked.Increment(ref currentPort), 105);

            for (int i = 0; i < 100; i++)
            {
                var clientSocket = _container.Resolve<IClientSocket>();
                clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort));
                clients.Add(clientSocket);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (connectedClientsCount < 100 && stopwatch.Elapsed < TimeSpan.FromSeconds(TimeoutSeconds))
            {
                serverSocket.PollEvents();
                foreach (var client in clients)
                    client.PollEvents();
                Thread.Sleep(10);
            }

            stopwatch.Stop();

            foreach (var client in clients)
                client.Dispose();

            serverSocket.Dispose();

            Assert.AreEqual(100, connectedClientsCount, "Not all clients connected successfully.");
        }

        [Test]
        public void Server_ShouldExchangeMessagesWith100Clients()
        {
            int clientCount = 100;
            int cycleCount = 3;
            var messageBaseSize = 10;

            var serverSocket = _container.Resolve<IServerSocket>();
            var clients = new List<IClientSocket>();

            Dictionary<Connection, byte[]> connectionToData = new Dictionary<Connection, byte[]>();
            Dictionary<Connection, int> connectionToCycle = new Dictionary<Connection, int>();

            serverSocket.GotConnectionRequest += request =>
            {
                var connection = request.Accept();
                var initialMessage = new byte[messageBaseSize];
                new Random().NextBytes(initialMessage);
                connectionToData[connection] = initialMessage;
                connectionToCycle[connection] = 1; // Initialize the cycle count for this connection
                connection.Send(new BytesWriter(initialMessage), DeliveryMethod.ReliableOrdered);
            };

            serverSocket.DataReceived += (connection, data) =>
            {
                byte[] originalMessage = connectionToData[connection];
                if (data.RawAvailable.Length == originalMessage.Length + 2)
                {
                    if (connectionToCycle[connection] < cycleCount)
                    {
                        var nextMessage = new byte[originalMessage.Length + 4];
                        Array.Copy(originalMessage, nextMessage, originalMessage.Length);
                        connection.Send(new BytesWriter(nextMessage), DeliveryMethod.ReliableOrdered);
                        connectionToData[connection] = nextMessage;
                        connectionToCycle[connection]++;
                    }
                    else
                    {
                        connection.Disconnect();
                    }
                }
                else
                {
                    Assert.Fail(
                        $"Unexpected message length from client. Expected {originalMessage.Length + 2}, but got {data.RawAvailable.Length}");
                }
            };

            serverSocket.Start((ushort)Interlocked.Increment(ref currentPort), clientCount + 5);

            for (int i = 0; i < clientCount; i++)
            {
                var clientSocket = _container.Resolve<IClientSocket>();
                clientSocket.DataReceived += (data) =>
                {
                    var responseMessage = new byte[data.RawAvailable.Length + 2];
                    Array.Copy(data.RawAvailable.ToArray(), responseMessage, data.RawAvailable.Length);
                    clientSocket.Send(new BytesWriter(responseMessage), DeliveryMethod.ReliableOrdered);
                };
                clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, currentPort));
                clients.Add(clientSocket);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.Elapsed < TimeSpan.FromSeconds(TimeoutSeconds))
            {
                serverSocket.PollEvents();
                foreach (var client in clients)
                    client.PollEvents();
                Thread.Sleep(10);
            }

            stopwatch.Stop();

            foreach (var client in clients)
                client.Dispose();

            serverSocket.Dispose();

            // Ensure that all clients have been processed through the cycles
            Assert.AreEqual(clientCount, connectionToCycle.Count,
                $"Not all clients went through the message exchange cycles. Expected {clientCount}, Got {connectionToCycle.Count}");
        }

        private (IClientSocket clientSocket, IServerSocket serverSocket) GetClientServerSockets()
        {
            _container = _containerBuilder.Build();
            var clientSocket = _container.Resolve<IClientSocket>();
            var serverSocket = _container.Resolve<IServerSocket>();
            return (clientSocket, serverSocket);
        }
    }
}