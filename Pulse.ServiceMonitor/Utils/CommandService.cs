using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Pulse.ServiceMonitor.Utils
{
    public class CommandService
    {
        private readonly ManualResetEvent connectDone = new ManualResetEvent(false);
        private readonly ManualResetEvent sendDone = new ManualResetEvent(false);
        private readonly ManualResetEvent recDone = new ManualResetEvent(false);
        private string response = string.Empty;

        public string ServiceCommands(string command)
        {
            try
            {
                Logging.LogMessage(LoggingLevel.Info, $"Process Command: {command}");
                IPAddress localIp = IPAddress.Parse("127.0.0.1");
                IPEndPoint localEp = new IPEndPoint(localIp, 9998);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(localEp);
                //client.BeginConnect(localEp, new AsyncCallback(ConnectCallback), client);
                //connectDone.WaitOne();

                Send(client, command);
                sendDone.WaitOne();

                Receive(client);
                recDone.WaitOne();

                client.Shutdown(SocketShutdown.Both);
                client.Close();

                return response;
            }
            catch (Exception ex)
            {
                Logging.LogErrorMessage("Error while processing the service command", ex);
                return "FAILED";
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);

                connectDone.Set();
            }
            catch (Exception ex)
            {
                Logging.LogErrorMessage("There was an error in the connection process", ex);
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.WorkSocket = client;

                client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception ex)
            {
                Logging.LogErrorMessage("There was an error in the process receiving messages", ex);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.WorkSocket;
                int bytesRead = client.EndReceive(ar);
                if (bytesRead > 0)
                {
                    state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
                    client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    if (state.Sb.Length > 1)
                    {
                        response = state.Sb.ToString();
                    }
                    recDone.Set();
                    Logging.LogMessage(LoggingLevel.Info, $"Message received from JobMonitor service: {response}");
                }
            }
            catch (Exception ex)
            {
                Logging.LogErrorMessage("There was an error in the process of receiving a message", ex);
            }
        }

        private void Send(Socket client, string command)
        {
            Logging.LogMessage(LoggingLevel.Info, $"Message Sent: {command}");
            byte[] byteCommand = Encoding.ASCII.GetBytes(command);
            client.BeginSend(byteCommand, 0, byteCommand.Length, 0, new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                int bytesSent = client.EndSend(ar);
                sendDone.Set();
                Logging.LogMessage(LoggingLevel.Info, $"{bytesSent} message sent");
            }
            catch (Exception ex)
            {
                Logging.LogErrorMessage("There was an error in the process sending a message", ex);
            }
        }
    }

    public class StateObject
    {
        public const int BufferSize = 4096;
        private Socket _workSocket;
        public Socket WorkSocket
        {
            get => _workSocket;
            set
            {
                if (value != _workSocket)
                    _workSocket = value;
            }
        }
        public byte[] Buffer { get; set; } = new byte[BufferSize];
        public StringBuilder Sb { get; set; } = new StringBuilder();
    }
}