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

 