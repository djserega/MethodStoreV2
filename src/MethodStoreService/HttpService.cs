using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MethodStoreService
{
    internal sealed class HttpService
    {
        private readonly IPAddress _currentIP = Dns.GetHostAddresses(Environment.MachineName).First();
        private readonly int _port = 1397;
        private TcpListener _tcpListener;
        private bool _isActive;

        internal HttpService()
        {
            _tcpListener = new TcpListener(_currentIP, _port);
        }

        ~HttpService()
        {
            _isActive = false;
            _tcpListener.Stop();
        }

        internal string GetIPPortService() => $"{_currentIP}:{_port}";

        internal async void StartService()
        {
            _isActive = true;
            _tcpListener.Start();

            while (_isActive)
            {
                TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();
                await Task.Run(() => HandleClient(tcpClient));
            }
        }

        private void HandleClient(TcpClient tcpClient)
        {
            string data = ReadMessage(tcpClient);
            if (string.IsNullOrWhiteSpace(data))
                return;

            if (data.StartsWith("error:"))
                SendMessage(tcpClient, data);
            else
                HandleMessageFromTcpClient(tcpClient, data);
        }

        private void HandleMessageFromTcpClient(TcpClient tcpClient, string request)
        {
            switch (request)
            {
                case "GetListMethods###":
                    SendMessage(tcpClient, GetListMethods());
                    break;
                default:
                    SendMessage(tcpClient, "Запрос не поддерживается или находится в разработке.");
                    break;
            }
        }

        private string GetListMethods()
        {
            return "Hello World!";
        }


        private string ReadMessage(TcpClient tcpClient)
        {
            string data = string.Empty;
            byte[] buffer = new byte[128];

            int countData = 0;
            try
            {
                while ((countData = tcpClient.GetStream().Read(buffer, 0, buffer.Length)) > 0)
                {
                    data += Encoding.UTF8.GetString(buffer);

                    if (data.IndexOf("###") > 0)
                        break;
                }
            }
            catch (Exception ex)
            {
                return $"error:{ex.Message}";
            }

            return data.Replace("\0", string.Empty).Trim();
        }

        private void SendMessage(TcpClient tcpClient, string message)
        {
            byte[] byteMessage = Encoding.UTF8.GetBytes($"{message}###");

            tcpClient.GetStream().Write(byteMessage, 0, byteMessage.Length);
        }

    }
}
