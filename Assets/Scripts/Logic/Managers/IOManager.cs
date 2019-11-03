using System.Collections.Concurrent;

public delegate void serverMessageConsumeDelegate(int count);
public class IOManager : ManagerInterface {
    ConcurrentQueue<NetMessage> notificationMessages = new ConcurrentQueue<NetMessage>(); // 通告信息
    ConcurrentQueue<RequestBase> requestMessages = new ConcurrentQueue<RequestBase>(); // 请求消息
    private serverMessageConsumeDelegate consumeDelegate;

    string ManagerInterface.getName() {
        return CommonDefine.kManagerIOName;
    }

    void ManagerInterface.init() {

    }

    void ManagerInterface.update() {
        int count = requestMessages.Count;
        for (int i = 0; i < count; i++) {
            RequestBase request;
            requestMessages.TryDequeue(out request);
            handleRequest(request);
        }
        /* 每次update后产生了新的输出，通知进行处理 */
        if (consumeDelegate != null && notificationMessages.Count != 0) {
            consumeDelegate(notificationMessages.Count);
        }
    }

    public void sendMessage(MessageBase message) {
        if (message != null) {
            notificationMessages.Enqueue(new NetMessage(null, message));
        }
    }

    public void sendServerMessage(NetMessage message) {
        if (message != null) {
            notificationMessages.Enqueue(message);
        }
    }

    public void registerConsumeDelegate(serverMessageConsumeDelegate consumeDelegate) {
        if (consumeDelegate != null) {
            this.consumeDelegate = consumeDelegate;
        }
    }

    public NetMessage consumeOneMessage() {
        if (notificationMessages.Count != 0) {
            NetMessage message;
            if (notificationMessages.TryDequeue(out message)) {
                return message;
            }
        }
        return null;
    }

    public void sendRequest(RequestBase request) {
        requestMessages.Enqueue(request);
    }

    public void handleRequest(RequestBase request) {
        switch(request.target) {
        case RequestTarget.ChessManager:
            ChessManager chessManager = 
                (ChessManager) ManagerCollection.getCollection().GetManager(CommonDefine.kManagerChessName);
            if (chessManager == null) {
                return;
            }
            ChessBase[] chessList = chessManager.getChessList();
            ChessCompletionMessage message = new ChessCompletionMessage(chessList.Length, chessManager.getChessList());
            sendServerMessage(new NetMessage(request.user, message));
            break;
        default:
            break;
        }
    }
}
