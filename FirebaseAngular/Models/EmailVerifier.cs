using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace FirebaseAngular.Models
{
    public class EmailVerifier : IDisposable
    {
        private bool _useSMTPMethod;
        MXServer responsecode = new MXServer();
        private CommandExecuter _commander = new CommandExecuter();

        public EmailVerifier()
        {
            // _useSMTPMethod = useSMTPMethod;
        }

        private int _count;

        public MXServer IsEmailVerified(string email)
        {
            string domain = email.Substring(email.IndexOf("@") + 1);
            var servers = _commander.GetMXServers(domain);
            MXServer ServerData = new MXServer();
            Socket socket = null;

            foreach (MXServer mxserver in servers)
            {
                IPHostEntry ipHost = Dns.Resolve(mxserver.Email);
                //IPHostEntry ipHost = Dns.Resolve("www.google.com");
                IPEndPoint endPoint = new IPEndPoint(ipHost.AddressList[0], 25);
                socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(endPoint);

                if (!CheckResponse(socket, ResponseEnum.ConnectSuccess))
                {
                    socket.Close();

                }
                else
                {
                    // If connected, send SMTP commands
                    {
                        SendData(socket, string.Format("HELO {0}\r\n", "hi"));
                        if (!CheckResponse(socket, ResponseEnum.GenericSuccess))
                        {
                            socket.Close();
                            continue;
                        }

                        //SendData(socket, string.Format("MAIL FROM:  <{0}>\r\n", "from@domain.com"));
                        SendData(socket, string.Format("MAIL FROM:  <{0}>\r\n", email));
                        CheckResponse(socket, ResponseEnum.GenericSuccess);

                        SendData(socket, string.Format("RCPT TO:  <{0}>\r\n", email));
                        bool result = CheckResponse(socket, ResponseEnum.GenericSuccess);
                        if (!result)
                        {
                            socket.Close();
                            continue;
                        }
                        else
                        {
                            ServerData.status = true;
                            ServerData.statusCode = responsecode.statusCode;
                            ServerData.Email = email;
                            ServerData.Prefrence = servers.Select(x=>x.Prefrence).FirstOrDefault();
                            return ServerData;
                        }
                        // return true; 

                        // return result;
                    }
                }
            }
            ServerData.status = false;
            ServerData.statusCode = responsecode.statusCode;
            ServerData.Email = email;
            ServerData.Prefrence = servers.Select(x => x.Prefrence).FirstOrDefault();
            return ServerData;
            //return false;
        }

        private void SendData(Socket socket, string message)
        {
            byte[] _msg = Encoding.ASCII.GetBytes(message);
            socket.Send(_msg, 0, _msg.Length, SocketFlags.None);
        }

        private string _lastResponse;

        private bool CheckResponse(Socket socket, ResponseEnum responseEnum)
        {
            _lastResponse = string.Empty;

            // int response;
            byte[] bytes = new byte[1024];

            int count = 1;
            while (socket.Available == 0)
            {
                System.Threading.Thread.Sleep(1000);
                count++;

                if (count > 10)
                    return false;
            }

            socket.Receive(bytes, 0, socket.Available, SocketFlags.None);
            _lastResponse = Encoding.ASCII.GetString(bytes);
            responsecode.statusCode = Convert.ToInt32(_lastResponse.Substring(0, 3));

            if (responsecode.statusCode == (int)responseEnum)
                return true;

            return false;
        }

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion
    }

    public enum ResponseEnum : int
    {
        ConnectSuccess = 220,
        GenericSuccess = 250,
        DataSuccess = 354,
        QuitSuccess = 221
    }
}
