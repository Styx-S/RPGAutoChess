using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform thisTransform = null;

    private const string kControllerName = "GameController";

    void Awake() {
        thisTransform = GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start() {
        initCamera();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void initCamera() {
        MapManager map = (MapManager)ManagerCollection.getCollection().GetManager(CommonDefine.kManagerMapName);
        if (map == null) {
            Debug.Log("null MapManager");
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
        Vector3 position = new Vector3((m - 1) * CommonDefine.kChessBoardDistanceUnit / 2 + CommonDefine.kDatumPointX,
            (n - 1) * CommonDefine.kChessBoardDistanceUnit / 2 + CommonDefine.kDatumPointY,
            CommonDefine.kCameraZAxisOffset);
        thisTransform.position = position;
    }
}
