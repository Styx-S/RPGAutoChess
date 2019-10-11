using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenPlayMap : MonoBehaviour
{
	private GameObject[][] points;
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
	
	public void genMap(int m, int n) {
		for (int i = 0;i < m;i++) {
            for (int j = 0;j < n;j++) {
                GameObject grid = Resources.Load("Textures/MapGrid") as GameObject;
                Vector3 position = new Vector3(i + 0.5f,j + 0.5f,0);
                grid.transform.position = position;
                grid.transform.parent = thisTransform;
            }
        }
	}
}
