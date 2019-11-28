
public class ChessSpawnEvent : Event {
    private ChessBase mChess;
    private ChessLocation mSpawnLocation;

    public ChessBase chess {
        get {
            return mChess;
        } set {
            mChess = value;
        }
    }

    public ChessLocation spawnLocation {
        get {
            return mSpawnLocation;
        } set {
            mSpawnLocation = value;
        }
    }

    public ChessSpawnEvent(ChessBase chess, ChessLocation spawnLocation) : base(EventName.ChessSpawnEventName) {
        mChess = chess;
        mSpawnLocation = spawnLocation;
    }
}
