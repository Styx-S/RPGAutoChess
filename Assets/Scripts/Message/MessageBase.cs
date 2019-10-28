using System.Collections;
using System.Collections.Generic;

public enum MessageInfoType {
    BattleMapGrid,      // 战斗棋盘
    Chess,              // 棋子信息
}

public enum MessageInfoKindType {
    Completion,     // 全量（初始化）信息
    Increment,      // 增量（变化通告）信息
}

public class MessageBase {
    public MessageInfoType type;
    public MessageInfoKindType kindType;
    public int messageId;   // 用于在某种类型的消息中继续表示区别

    public MessageBase(MessageInfoType type, MessageInfoKindType kindType, int messageId) {
        this.type = type;
        this.kindType = kindType;
        this.messageId = messageId;
    }

    // 简化发送消息给ControllerCenter的语法
    public void sendToControllerConter() {
        UnityControllerCenter center = UnityControllerCenter.getCenter();
        center.sendMessage(this);
    }
}
