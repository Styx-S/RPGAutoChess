using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform thisTransform = null;

    void Awake() {
        thisTransform = GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void calCameraPosition(Map map) {
        int m = map.width;
        int n;
        if (m > 0) {
            n = map.height;
        } else {
            return;
        }
        Vector3 position = new Vector3((m - 1) * CommonDefine.kChessBoardDistanceUnit / 2 + CommonDefine.kDatumPointX,
            (n - 1) * CommonDefine.kChessBoardDistanceUnit / 2 + CommonDefine.kDatumPointY,
            CommonDefine.kCameraZAxisOffset);
        thisTransform.position = position;
    }
}
