using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenMapUI : MonoBehaviour {
    private Transform thisTransform = null;
    
    void Awake() {
        thisTransform = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start() {
        genMap();
    }

    // Update is called once per frame
    void Update() {
        
    }

    /* 界面生成地图 */
    public void genMap() {
        MapManager map =(MapManager)ManagerCollection.getCollection().GetManager(CommonDefine.kManagerMapName);
        if (map == null) {
            return;
        }
        MapGrid[][] grids = map.mapGrids;
        int m = grids.Length;
        int n;
        if (m > 0) {
            n = grids[0].Length;
        } else {
            return;
        }
        GameObject prefGrid = (GameObject)Resources.Load(CommonDefine.kMapGridPrefabPath);
		for (int i = 0;i < m;i++) {
            for (int j = 0;j < n;j++) {
                Vector3 position = new Vector3(i + 0.5f,j + 0.5f,100);
                GameObject grid = Instantiate(prefGrid);
                grid.transform.position = position;
                grid.transform.parent = thisTransform;
            }
        }
	}
}
