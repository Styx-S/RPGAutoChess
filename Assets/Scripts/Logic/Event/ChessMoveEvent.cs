
public class ChessMoveEvent : OverlayEvent {

    private ChessBase mChess;
    private ChessLocation mMoveTo;

    /* 将要移动的棋子 */
    public ChessBase chess {
        get {
            return mChess;
        }
    }
    /* 移动的目标点 */
    public ChessLocation moveTo {
        get {
            return mMoveTo;
        }
    }
    /* 移动冷却时间 */
    public float moveCoolingValue {
        get {
            return buffValue;
        }
    }


    public ChessMoveEvent(ChessBase chess, ChessLocation moveTo, float CoolingValue) : base(EventName.ChessMoveEventName) {
        mChess = chess;
        mMoveTo = moveTo;
        mBuffValue = CoolingValue;
    }
}
