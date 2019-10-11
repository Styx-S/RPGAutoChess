using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenMapUI : MonoBehaviour {
    private Transform thisTransform = null;

    private const string kControllerName = "GameController";
    private const string kGridName = "MapGrid";
    private const string kGridSprite = "Textures/MapGrid";
    
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
        MapGrid[][] grids = GameObject.FindGameObjectWithTag(kControllerName).GetComponent<MapManager>().mapGrids;
        int m = grids.Length;
        int n;
        if (m > 0) {
            n = grids[0].Length;
        } else {
            return;
        }
		for (int i = 0;i < m;i++) {
            for (int j = 0;j < n;j++) {
                GameObject grid = new GameObject(kGridName);
                Sprite spr = Resources.Load<Sprite>(kGridSprite);
                grid.AddComponent<SpriteRenderer>().sprite = spr;
                Vector3 position = new Vector3(i + 0.5f,j + 0.5f,0);
                grid.transform.position = position;
                grid.transform.parent = thisTransform;
            }
        }
	}
}
