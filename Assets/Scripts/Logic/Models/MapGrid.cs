using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class MapGrid {
    private ChessLocation mPosition;
    public ChessLocation position {
        get {
            return mPosition;
        }
    }

    public MapGrid(ChessLocation position) {
        mPosition = position;
    }

}
