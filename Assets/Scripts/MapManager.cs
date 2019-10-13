using System.Collections;
using System.Collections.Generic;

public class MapManager : ManagerInterface {
    private MapGrid [][] mMapGrids;
    public MapGrid [][] mapGrids {
        get {
            return mMapGrids;
        }
    }
    string ManagerInterface.getName() {
        return CommonDefine.kManagerMapName;
    }
    void ManagerInterface.init() {
        genMapGrid(6,7);
    }

    void ManagerInterface.update() {
        // do nothing
    }

    /* 生成逻辑棋盘 */
    public void genMapGrid(int m, int n) {
        mMapGrids = new MapGrid[m][];
        for (int i = 0;i < m;i++) {
            mMapGrids[i] = new MapGrid[n];
            for (int j = 0;j < n;j++) {
                MapGrid grid = new MapGrid(new ChessLocation(i,j));
                mMapGrids[i][j] = grid;
            }
        }
    }
}
