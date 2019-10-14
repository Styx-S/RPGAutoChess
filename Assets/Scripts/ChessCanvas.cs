using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject chessText = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayText(string text) {
        if (chessText != null) {
            GameObject hud = Instantiate(chessText, transform)as GameObject;
            hud.GetComponent<Text>().text = text;
        }
    }
}
