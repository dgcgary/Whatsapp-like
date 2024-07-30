using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server {

class Program {

// Metodo Main
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
 
            byte[] messageSent = Encoding.ASCII.GetBytes(msg + ".");
            int byteSent = sender.Send(messageSent);
 
            byte[] messageReceived = new byte[1024];
            int byteRecv = sender.Receive(messageReceived);
            Console.WriteLine("> {0}", Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
 

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }         
        catch (ArgumentNullException ane) {
             
            Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
        }
         
        catch (SocketException se) {
             
            Console.WriteLine("SocketException : {0}", se.ToString());
        }
         
        catch (Exception e) {
            Console.WriteLine("Unexpected exception : {0}", e.ToString());
        }
    }
     
    catch (Exception e) {
         
        Console.WriteLine(e.ToString());
    }
}
//Metodo Servidor: Escucha conexiones entrantes en un puerto específico, recibe mensajes de los clientes y envía una respuesta.
public static void Servidor(int port)
{
	IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
	IPAddress ipAddr = ipHost.AddressList[0];
	IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);

	Socket listener = new Socket(ipAddr.AddressFamily,
				SocketType.Stream, ProtocolType.Tcp);

	try {

		listener.Bind(localEndPoint);

		listener.Listen(10);

    Console.WriteLine("Servidor abierto");
		while (true) {
			

			Socket clientSocket = listener.Accept();

			byte[] bytes = new Byte[1024];
			string data = null;

			while (true) {

				int numByte = clientSocket.Receive(bytes);
				
				data += Encoding.ASCII.GetString(bytes,
										0, numByte);
											
				if (data.IndexOf(".") > -1)
					break;
			}

			Console.WriteLine("-> {0} ", data);
			byte[] message = Encoding.ASCII.GetBytes("Recibido!");

			clientSocket.Send(message);

			clientSocket.Shutdown(SocketShutdown.Both);
			clientSocket.Close();
		}
	}
	
	catch (Exception e) {
		Console.WriteLine(e.ToString());
	}
}
}
}