using System.Collections;
using System.Collections.Generic;
using System;

public enum ChessIncrementMessageType {
    chessCreate,        // 棋子创建
    chessRemove,        // 棋子移除
    chessMove,          // 棋子移动
    chessAttach,        // 棋子攻击
}

[Serializable]
public class ChessIncrementMessage : MessageBase {
    public ChessIncrementMessage(ChessIncrementMessageType type)
        : base(MessageInfoType.Chess, MessageInfoKindType.Increment, (int)type) {
        
    }
}

[Serializable]
public class ChessIncrementMessage_chessCreate : ChessIncrementMessage {
    public ChessBase chess {
        get; set;
    }

    public ChessIncrementMessage_chessCreate(ChessBase chess) : base(ChessIncrementMessageType.chessCreate) {
        this.chess = chess;
    }
}

[Serializable]
public class ChessIncrementMessage_chessRemove : ChessIncrementMessage {
    public ChessBase chess {
        get; set;
    }

    public ChessIncrementMessage_chessRemove(ChessBase chess) : base(ChessIncrementMessageType.chessRemove) {
        this.chess = chess;
    }
}

[Serializable]
public class ChessIncrementMessage_chessMove : ChessIncrementMessage {
    public ChessBase chess {
        get; set;
    }
    public ChessLocation from {
        get; set;
    }
    public ChessLocation to {
        get; set;
    }

    public ChessIncrementMessage_chessMove(ChessBase chess, ChessLocation from, ChessLocation to)
        : base(ChessIncrementMessageType.chessMove) {

        this.chess = chess;
        this.from = from;
        this.to = to;
    }
}

[Serializable]
public class ChessIncrementMessage_chessAttach : ChessIncrementMessage {
    public ChessBase attacher {
        get; set;
    }
    public ChessBase victim {
        get; set;
    }
    public float causeDamage {
        get; set;
    }

    public ChessIncrementMessage_chessAttach(ChessBase attacher, ChessBase victim, float causeDamage)
        : base(ChessIncrementMessageType.chessAttach) {

        this.attacher = attacher;
        this.victim = victim;
        this.causeDamage = causeDamage;
    }
}
