using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server {

class Program {

    // Método Principal
    static void Main(string[] args)
    {
        if (args[0] == "-port"){
            Servidor(Int32.Parse(args[1]));
        }
        else if (args[0] == "-msg"){
            Cliente(Int32.Parse(args[1]), args[2]);
        }
        else {
            Console.WriteLine("Uso\n Abrir server: -port <puerto>\nEnviar mensaje: -msg <puerto> <mensaje>");
        }
    }

    // Método Cliente: Establece una conexion TCP con el servidor, envia un mensaje y luego recibe una respuesta
    static void Cliente(int port, string msg)
    {
        try {

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try {
                sender.Connect(localEndPoint);
                Console.WriteLine("| {0} |", sender.RemoteEndPoint.ToString());

                byte[] messageSent = Encoding.ASCII.GetBytes(msg);
                int byteSent = sender.Send(messageSent);

                byte[] messageReceived = new byte[1024];
                int byteRecv = sender.Receive(messageReceived);
                Console.WriteLine("> {0}", Encoding.ASCII.GetString(messageReceived, 0, byteRecv));

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (Exception e) {
                Console.WriteLine("Excepción: {0}", e.ToString());
            }
        }
        catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

}
}
