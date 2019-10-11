using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid {
    private Vector2Int mPosition;
    public Vector2Int position {
        get {
            return mPosition;
        }
    }

    public MapGrid(Vector2Int position) {
        mPosition = position;
    }

}
