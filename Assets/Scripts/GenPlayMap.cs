using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenPlayMap : MonoBehaviour
{
	private GameObject[][] points;
    private TransForm thisTransForm = null;

    void Awake() {
        thisTransForm = GetComponent<TransForm>();
    }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
	
	public genMap(int m, int n) {
		for (int i = 0;i < m;i++) {
            for (int j = 0;j < n;j++) {
                GameObject grid = Resources.Load("Textures/MapGrid") as GameObject;
                grid.transform.position.x = i + 0.5;
                grid.transform.position.y = n + 0.5;
                grid.transform.parent = thisTransForm;
            }
        }
	}
}
