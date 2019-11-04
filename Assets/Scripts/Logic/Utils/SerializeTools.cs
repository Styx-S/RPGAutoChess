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
        to.Send(intToBytes((int)memoryStream.Length));
        to.Send(memoryStream.GetBuffer(), (int)memoryStream.Length, SocketFlags.None);
    }

    public static object deserializeObjectFromSocket(Socket from) {
        byte[] numBytes = new byte[4];
        from.Receive(numBytes);
        int num = bytesToInt(numBytes);
        byte[] buffer = new byte[num];
        from.Receive(buffer);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        return binaryFormatter.Deserialize(new MemoryStream(buffer));
    }

    public static int bytesToInt(byte[] bytes) {
        int integer = bytes[3] & 0xFF;
        integer |= bytes[2] << 8;
        integer |= bytes[1] << 16;
        integer |= bytes[0] << 24;
        return integer;
    }

    public static byte[] intToBytes(int integer) {
        byte[] bytes = new byte[4];
        bytes[3] = (byte) (integer & 0xFF);
        bytes[2] = (byte) (integer >> 8 & 0xFF);
        bytes[1] = (byte) (integer >> 16 & 0xFF);
        bytes[0] = (byte) (integer >> 24 & 0xFF);
        return bytes;
    }
}
