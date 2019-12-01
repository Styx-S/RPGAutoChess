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
        panel.GetComponent<ObjPanelUI>().obj = gameObject;
    }
}
