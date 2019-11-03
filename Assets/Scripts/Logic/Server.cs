using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class Server {
    private ConcurrentDictionary<RequestUser, Socket> socketDic = new ConcurrentDictionary<RequestUser, Socket>();
    private ConcurrentDictionary<Socket, RequestUser> userSocketDic = new ConcurrentDictionary<Socket, RequestUser>(); // 先简单维护一下双向map
    private int idSeed = 0; // 用于区分不同的客户端

    public void run() {
        ((IOManager)ManagerCollection.getCollection().GetManager(CommonDefine.kManagerIOName)).registerConsumeDelegate(dispatchMessage);
        Thread serverThread = new Thread(handleServerWorker);
        Thread networkThread = new Thread(handleNetworkWorker);
        serverThread.Start();
        networkThread.Start();
    }

    private void handleServerWorker() {
        ManagerCollection.getCollection().init();
        bool stopServer = false;
        // 标准时间间隔
        const float standardT = 1000.0f / CommonDefine.kLogicUpdateFPS;
        // 预测下一帧执行时间
        float predictT = standardT;
        // 实际执行时间
        float currentT;
        // 累计误差时间
        float deltaT = 0f;
        int flagCount = 0;
        Stopwatch watcher = new Stopwatch();
        while(!stopServer) {
            watcher.Start();
            ManagerCollection.getCollection().update();
            watcher.Stop();

            currentT = watcher.ElapsedMilliseconds;
            deltaT += standardT - currentT;
            // 预测时间更新公式： 预测结果 与 实际结果的加权和 （削抖）
            predictT = currentT * 0.5f + predictT * 0.5f;
            if (deltaT > 0) {
                float sleepT;
                if (predictT < standardT) {
                    sleepT = 0.7f * deltaT;
                } else {
                    // 当前存在正误差，但预测接下来执行时间将会上升
                    if (deltaT * 2 < predictT - standardT) {
                        // 误差时间过小，期待通过超长执行时间来补全
                        sleepT = 0.0f;
                    } else if (deltaT < predictT - standardT) {
                        // 尽管下帧可能运行时间较长，但是需要减小一定误差
                        sleepT = (predictT - standardT - deltaT) / 2;
                    } else {
                        // 误差时间过大
                        sleepT = deltaT / 2;
                    }
                }
                if (flagCount > 0) {
                    flagCount--;
                }
                deltaT -= (int)sleepT;
                Thread.Sleep((int)sleepT);
            } else {
                // 误差为负，说明当前fps低于设定值
                flagCount++;
                if (flagCount > 10) {
                    // 清空当前记录，防止因为某段时间的抖动影响后续计算
                    deltaT = 0f;
                    predictT = standardT;
                }
            }
        }
    }

    /* 将消息发送给对应客户端 */
    private void dispatchMessage(int count) {
        ICollection<Socket> sockets = socketDic.Values;
        IOManager io = (IOManager)ManagerCollection.getCollection().GetManager(CommonDefine.kManagerIOName);
        for (int i = 0; i < count; i++) {
            NetMessage serverMessage = io.consumeOneMessage();
            if (serverMessage.sendTo == null) {
                foreach (Socket socket in sockets) {
                    SerializeTools.serializeObjectToSocket(socket, serverMessage);
                }
            } else {
                Socket target;
                if (socketDic.TryGetValue(serverMessage.sendTo, out target)) {
                    SerializeTools.serializeObjectToSocket(target, serverMessage);
                }
            }
        }
    }

    /* 监听端口建立新连接 */
    private void handleNetworkWorker() {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), CommonDefine.kDefaultPort));
        serverSocket.Listen(int.MaxValue); // 最大连接数？
        while(true) {
            Socket socket = serverSocket.Accept();
            Thread singleConnectWorker = new Thread(new ParameterizedThreadStart(handleSingleConnect));
            RequestUser newUser = new RequestUser(idSeed++);
            socketDic.TryAdd(newUser, socket);
            userSocketDic.TryAdd(socket, newUser);
            singleConnectWorker.Start(socket);
        }
    }

    /* 处理单个连接 */
    private void handleSingleConnect(object socketObj) {
        IOManager IO = (IOManager)ManagerCollection.getCollection().GetManager(CommonDefine.kManagerIOName);
        RequestUser identifier;
        Socket socket = (Socket) socketObj;
        while(!userSocketDic.TryGetValue(socket, out identifier));
        while(true) {
            Byte[] buffer = new byte[1024 * 1024];
            int length = socket.Receive(buffer);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            NetRequest request = binaryFormatter.Deserialize(new MemoryStream(buffer)) as NetRequest;
            request.requestBody.user = identifier;
            IO.sendRequest(request.requestBody);
        }
    }
}
