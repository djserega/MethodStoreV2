using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore.Service
{
    internal class TcpService
    {
        private readonly IPAddress _currentIP = Dns.GetHostAddresses(Environment.MachineName).First();
        private TcpClient tcpClient;

        internal TcpService()
        {
            try
            {
                tcpClient = new TcpClient(_currentIP.ToString(), 1397);

                //Messages.Show(tcpClient.Connected ? "Приложение подключено к сервису" : "Ошибка подключения к сервису");
            }
            catch (SocketException ex)
            {
                Messages.Show($"Не удалось подключиться к сервису.\n{ex.Message}");
            }
        }

        ~TcpService()
        {
            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient = null; 
            }
        }

        internal bool Connected { get { return tcpClient?.Connected ?? false; } }

        internal void GetListMethods()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("GetListMethods###");

            SendMessage(stringBuilder.ToString());
        }

        private void SendMessage(string message)
        {
            if (!Connected)
                return;

            byte[] byteMessage = Encoding.UTF8.GetBytes(message);

            tcpClient.GetStream().Write(byteMessage, 0, byteMessage.Length);

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
                Messages.Show($"error:{ex.Message}");
            }

            Messages.Show(data.Replace("###", string.Empty));
        }

    }
}
