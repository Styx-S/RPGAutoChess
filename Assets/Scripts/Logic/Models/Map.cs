using System;

[Serializable]
public class Map {
    // 列主序： mGrids[x][y]表示X方向上第x格、Y方向上第y格的位置
    public MapGrid[][] mGrids;

    public int width {
        get {
            return mGrids.Length;
        }
    }

    public int height {
        get {
            if (mGrids.Length > 0) {
                return mGrids[0].Length;
            } else {
                return 0;
            }
        }
    }

    public Map(int x, int y) {
        mGrids = new MapGrid[x][];
        for (int i = 0; i < x; i++) {
            mGrids[i] = new MapGrid[y];
            for (int j = 0; j < y; j++) {
                mGrids[i][j] = new MapGrid(new ChessLocation(i,j));
            }
        }
    }
}
