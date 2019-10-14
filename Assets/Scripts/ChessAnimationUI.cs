using System.Collections;
using System.Collections.Generic;

public enum ChessAnimationType {
    Move,       // 棋子移动
    Attach,     // 棋子攻击
    Hurt,       // 棋子受伤害
}

public class ChessAnimationArgs {
    public ChessAnimationType animationType {
        get; set;
    }

    public ChessAnimationArgs(ChessAnimationType type) {
        this.animationType = type;
    }

}

/* 棋子移动动效所需参数 */
public class ChessAnimationMoveArgs : ChessAnimationArgs {
    public ChessLocation from {
        get; set;
    }
    public ChessLocation to {
        get; set;
    }

    public ChessAnimationMoveArgs(ChessLocation from, ChessLocation to) : base(ChessAnimationType.Move) {
        this.from = from;
        this.to = to;
    }

    public static ChessAnimationMoveArgs transfrom(ControllerMessage_chessMove message) {
        return new ChessAnimationMoveArgs(message.from, message.to);
    }
}

/* 棋子攻击动效所需参数 */
public class ChessAnimationAttachArgs : ChessAnimationArgs {

    public ChessAnimationAttachArgs() : base(ChessAnimationType.Attach) {

    }
}

/* 棋子受伤害动效所需参数 */
public class ChessAnimationHurtArgs : ChessAnimationArgs {
    public float causeDamage {
        get; set;
    }

    public ChessAnimationHurtArgs(float causeDamage) : base(ChessAnimationType.Hurt) {
        this.causeDamage = causeDamage;
    }
}
