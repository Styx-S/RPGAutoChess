using System.Collections;
using System.Collections.Generic;

public enum ChessAnimationType {
    Move,       // 棋子移动
    Attach,     // 棋子攻击
    Hurt,       // 棋子受伤害
}

public class ChessAnimationArgs {
    public ChessAnimationType animationType {
        get {
            return animationType;
        }
        private set {
            this.animationType = value;
        }
    }

    public ChessAnimationArgs(ChessAnimationType type) {
        this.animationType = type;
    }

}

/* 棋子移动动效所需参数 */
public class ChessAnimationMoveArgs : ChessAnimationArgs {
    
    public ChessAnimationMoveArgs() : base(ChessAnimationType.Move) {

    }
}

/* 棋子攻击动效所需参数 */
public class ChessAnimationAttachArgs : ChessAnimationArgs {

    public ChessAnimationAttachArgs() : base(ChessAnimationType.Attach) {

    }
}

/* 棋子受伤害动效所需参数 */
public class ChessAnimationHurtArgs : ChessAnimationArgs {

    public ChessAnimationHurtArgs() : base(ChessAnimationType.Hurt) {

    }
}
