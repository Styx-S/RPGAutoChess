using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ChessCompletionMessage : MessageBase {
    public ChessBase[] data;
    public int chessNum;
    
    public ChessCompletionMessage(int chessNum, ChessBase[] data) : base(MessageInfoType.Chess, MessageInfoKindType.Completion, 0) {
        this.chessNum = chessNum;
        this.data = data;
    }
}
