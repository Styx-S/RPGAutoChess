using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Client {
    private Socket socket;
    private UnityControllerCenter centerRef;


    public void run(IPAddress ip, int port, UnityControllerCenter center) {
        centerRef = center;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try{
            socket.Connect(ip, port);
            Thread t = new Thread(handleReceive);
            t.Start();

            // TODO: 下面为测试代码
            SerializeTools.serializeObjectToSocket(socket, new NetRequest(new RequestBase(RequestTarget.ChessManager, 0)));
            SerializeTools.serializeObjectToSocket(socket, new NetRequest(new RequestBase(RequestTarget.MapManager, 0)));
        }
        catch(Exception e) {
            Console.WriteLine("Fail to connect: " + e);
        }
        
    }

    public void stop() {

    }

    private void handleReceive() {
        try {
            while(true) {
                NetMessage message = SerializeTools.deserializeObjectFromSocket(socket) as NetMessage;
                centerRef.sendMessage(message.messageBody);
            }
        }
        catch (Exception e) {
            DebugLogger.log("Fail to handle receive: "+e);
        }
    }

    private void handleSend() {

    }

}
