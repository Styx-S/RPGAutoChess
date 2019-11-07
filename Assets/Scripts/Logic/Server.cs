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

    public Server() {
        // 逻辑的初始化写这里
        ((IOManager)ManagerCollection.getCollection().GetManager(CommonDefine.kManagerIOName)).registerConsumeDelegate(dispatchMessageWorker);
        ManagerCollection.getCollection().init();
    }

    public void run() { 
        Thread serverThread = new Thread(handleServerWorker);
        Thread networkThread = new Thread(handleNetworkWorker);
        serverThread.Start();
        networkThread.Start();
    }

    private void handleServerWorker() {
        bool stopServer = false;
        // 标准时间间隔
        const float standardT = 1000.0f / CommonDefine.kLogicUpdateFPS;
        // 预测下一帧执行时间
        float predictT = standardT;
        // 实际执行时间
        float currentT;
        Stopwatch watcher = new Stopwatch();
        while(!stopServer) {
            watcher.Reset();
            watcher.Start();
            ManagerCollection.getCollection().update();
            watcher.Stop();

            currentT = watcher.ElapsedMilliseconds;
            // 预测时间更新公式： 预测结果 与 实际结果的加权和 （削抖）
            predictT = currentT * 0.5f + predictT * 0.5f;
            if(currentT < standardT) {
                if (currentT + predictT < 2 * standardT) {
                    if (currentT < predictT) {
                        // 每帧执行时间增长趋势
                        Thread.Sleep((int) (2 * standardT - currentT - predictT)/2);
                    } else {
                        // 每帧执行时间下降趋势
                        Thread.Sleep((int) (standardT - currentT));
                    }
                }
            } else {
                // 当前帧执行时间超出标准，不睡眠
            }
        }
    }

    private void dispatchMessageWorker(int count) {
        // Thread t = new Thread(dispatchMessage);
        // t.Start(count);
        dispatchMessage(count);
    }

    /* 将消息发送给对应客户端 */
    private void dispatchMessage(object obj) {
        int count = (int) obj;
        ICollection<Socket> sockets = socketDic.Values;
        IOManager io = (IOManager)ManagerCollection.getCollection().GetManager(CommonDefine.kManagerIOName);
        for (int i = 0; i < count; i++) {
            NetMessage serverMessage = io.consumeOneMessage();
            //DebugLogger.log("dequeue: "+serverMessage);
            if (serverMessage == null) {
                continue;
            }
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
            NetRequest request = SerializeTools.deserializeObjectFromSocket(socket) as NetRequest;
            request.requestBody.user = identifier;
            IO.sendRequest(request.requestBody);
        }
    }
}
