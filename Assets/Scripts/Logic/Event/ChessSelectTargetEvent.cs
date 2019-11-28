
public class ChessSelectTargetEvent : Event {
    private ChessBase mChess;
    private ChessBase mTarget;

    public ChessBase chess {
        get {
            return mChess;
        }
    }

    public ChessBase target {
        get {
            return mTarget;
        } set {
            mTarget = value;
        }
    }


    public ChessSelectTargetEvent(ChessBase chess, ChessBase target) : base(EventName.ChessSelectTargetEventName) {
        mChess = chess;
        mTarget = target;
    }
}
