using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    private MapGrid [][] mMapGrids;
    public MapGrid [][] mapGrids {
        get {
            return mMapGrids;
        }
    }
    void Awake() {
        genMapGrid(6,7);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* 生成逻辑棋盘 */
    public void genMapGrid(int m, int n) {
        mMapGrids = new MapGrid[m][];
        for (int i = 0;i < m;i++) {
            mMapGrids[i] = new MapGrid[n];
            for (int j = 0;j < n;j++) {
                MapGrid grid = new MapGrid(new Vector2Int(i,j));
                mMapGrids[i][j] = grid;
            }
        }
    }
}
