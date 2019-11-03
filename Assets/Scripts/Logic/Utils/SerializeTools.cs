using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;

public class SerializeTools {
    public static void serializeObjectToSocket(Socket to, Object obj) {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        bf.Serialize(memoryStream, obj);
        to.Send(memoryStream.GetBuffer(), (int)memoryStream.Length, SocketFlags.None);
    }
}
