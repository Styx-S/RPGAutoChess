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
        MapGrid[][] grids = GameObject.FindGameObjectWithTag(kControllerName).GetComponent<MapManager>().mapGrids;
        int m = grids.Length;
        int n;
        if (m > 0) {
            n = grids[0].Length;
        } else {
            return;
        }
        Vector3 position = new Vector3(m / 2,n / 2,-10);
        thisTransform.position = position;
    }
}
