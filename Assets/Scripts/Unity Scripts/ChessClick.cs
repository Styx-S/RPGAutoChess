using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessClick : MonoBehaviour
{
    [SerializeField]
    private GameObject panel = null;
    
    void Awake() {
        panel = GameObject.FindGameObjectWithTag(CommonDefine.kObjPanelTag);
    }

    void OnMouseUpAsButton() {
        ChessBase chess = GetComponent<ChessUI>().chess;
        panel.GetComponent<ObjPanelUI>().chess = chess;
    }
}
