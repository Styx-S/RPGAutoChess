using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;

public class SerializeTools {
    public static void serializeObjectToSocket(Socket to, Object obj) {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        bf.Serialize(memoryStream, obj);
        byte[] byteNums = intToBytes((int)memoryStream.Length);
        writeToSocket(to, byteNums, byteNums.Length);
        writeToSocket(to, memoryStream.GetBuffer(), (int) memoryStream.Length);
    }

    public static object deserializeObjectFromSocket(Socket from) {
        byte[] numBytes = new byte[4];
        readFromSocket(from, numBytes, numBytes.Length);
        int num = bytesToInt(numBytes);
        byte[] buffer = new byte[num];
        readFromSocket(from, buffer, num);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        return binaryFormatter.Deserialize(new MemoryStream(buffer));
    }

    public static void serializeObjectToJson (Stream stream, Object obj) {
        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(obj.GetType());
        jsonSerializer.WriteObject(stream, obj);
    }

    public static T deserializeObjectFromJson<T>(Stream stream) {
        DataContractSerializer jsonSerializer = new DataContractSerializer(typeof(T));
        return (T)jsonSerializer.ReadObject(stream);
    }

    public static void writeToSocket(Socket socket, byte[] buffer, int length) {
        int leftCount = length;
        while (leftCount > 0) {
            leftCount -= socket.Send(buffer, length - leftCount, leftCount, SocketFlags.None);
        }
    }

    public static void readFromSocket(Socket socket, byte[] buffer, int length) {
        int leftCount = length;
        while (leftCount > 0) {
            leftCount -= socket.Receive(buffer, length - leftCount, leftCount, SocketFlags.None);
        }
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
