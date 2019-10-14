using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChessText : MonoBehaviour
{
    
    private float speed = 1.5f;
    private float timer = 0f;
    private float time = 0.8f;
    private Transform thisTransform = null;
    
    void Awake() {
        thisTransform = GetComponent<Transform>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void scroll() {
        thisTransform.Translate(Vector3.up * speed * Time.deltaTime);
        timer += Time.deltaTime;
        GetComponent<Text>().fontSize--;
        GetComponent<Text>().color = new Color(1,0,0,1 - timer);
        Destroy(gameObject,time);
    }
}
