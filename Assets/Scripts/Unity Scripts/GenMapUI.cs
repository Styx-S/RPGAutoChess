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

    }

    // Update is called once per frame
    void Update() {
        
    }

    /* 界面生成地图 */
    public void genMap(Map map) {
        int m = map.width;
        int n;
        if (m > 0) {
            n = map.height;
        } else {
            return;
        }
        GameObject prefGrid = (GameObject)Resources.Load(CommonDefine.kMapGridPrefabPath);
		for (int i = 0;i < m;i++) {
            for (int j = 0;j < n;j++) {
                Vector3 position = new Vector3(CommonDefine.kDatumPointX + i * CommonDefine.kChessBoardDistanceUnit,
                    CommonDefine.kDatumPointY + j * CommonDefine.kChessBoardDistanceUnit, 
                    CommonDefine.kBoardZAxisOffset);
                GameObject grid = Instantiate(prefGrid);
                grid.transform.position = position;
                grid.transform.parent = thisTransform;
            }
        }
	}
}
