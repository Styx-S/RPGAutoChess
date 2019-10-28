using System.Collections;
using System.Collections.Generic;

public enum ControllerMessageType {
    chessCreate,        // 棋子创建
    chessRemove,        // 棋子移除
    chessMove,          // 棋子移动
    chessAttach,        // 棋子攻击
}

public class ControllerMessage {
    public ControllerMessageType type {
        get; set;
    }

    public ControllerMessage(ControllerMessageType type) {
        this.type = type;
    }

    public static void send(ControllerMessage message) {
        UnityControllerCenter center = UnityControllerCenter.getCenter();
        center.sendMessage(message);
    }
}

public class ControllerMessage_chessCreate : ControllerMessage {
    public ChessBase chess {
        get; set;
    }

    public ControllerMessage_chessCreate(ChessBase chess) : base(ControllerMessageType.chessCreate) {
        this.chess = chess;
    }
}

public class ControllerMessage_chessRemove : ControllerMessage {
    public ChessBase chess {
        get; set;
    }

    public ControllerMessage_chessRemove(ChessBase chess) : base(ControllerMessageType.chessRemove) {
        this.chess = chess;
    }
}

public class ControllerMessage_chessMove : ControllerMessage {
    public ChessBase chess {
        get; set;
    }
    public ChessLocation from {
        get; set;
    }
    public ChessLocation to {
        get; set;
    }

    public ControllerMessage_chessMove(ChessBase chess, ChessLocation from, ChessLocation to)
        : base(ControllerMessageType.chessMove) {

        this.chess = chess;
        this.from = from;
        this.to = to;
    }
}

public class ControllerMessage_chessAttach : ControllerMessage {
    public ChessBase attacher {
        get; set;
    }
    public ChessBase victim {
        get; set;
    }
    public float causeDamage {
        get; set;
    }

    public ControllerMessage_chessAttach(ChessBase attacher, ChessBase victim, float causeDamage)
        : base(ControllerMessageType.chessAttach) {

        this.attacher = attacher;
        this.victim = victim;
        this.causeDamage = causeDamage;
    }
}
