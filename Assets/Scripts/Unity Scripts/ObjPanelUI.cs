using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjPanelUI : MonoBehaviour
{
    [SerializeField]
    public ChessBase chess {
        get; set;
    }

    private GameObject ObjAttrPref;
    private GameObject ObjImagPref;
    private Dictionary<string,GameObject> dicChessState = new Dictionary<string, GameObject>();

    void Awake() {
        ObjAttrPref = Resources.Load(CommonDefine.kObjAttrPrefabPath) as GameObject;
        ObjImagPref = Resources.Load(CommonDefine.kObjImagPrefabPath) as GameObject;
        chess = null;
    }
    
    void Start() {
        initObjImag();
        initObjAttr(CommonDefine.kPanelChessHP);
        initObjAttr(CommonDefine.kPanelChessAtk);
        initObjAttr(CommonDefine.kPanelChessAtkRad);
        initObjAttr(CommonDefine.kPanelChessAtkDel);
        initObjAttr(CommonDefine.kPanelChessMob);
        initObjAttr(CommonDefine.kPanelChessMovDel);
    }

    void Update() {
        if (chess != null) {
            setPanelActive(true);
            dicChessState[CommonDefine.kPanelChessHP].GetComponent<ObjAttr>().Value
                .GetComponent<Text>().text = chess.status.HP.ToString();
            dicChessState[CommonDefine.kPanelChessAtk].GetComponent<ObjAttr>().Value
                .GetComponent<Text>().text = chess.status.strength.ToString();
            dicChessState[CommonDefine.kPanelChessAtkRad].GetComponent<ObjAttr>().Value
                .GetComponent<Text>().text = chess.status.attachRadius.ToString();
            dicChessState[CommonDefine.kPanelChessAtkDel].GetComponent<ObjAttr>().Value
                .GetComponent<Text>().text = chess.status.attachCoolingDelay.ToString();
            dicChessState[CommonDefine.kPanelChessMob].GetComponent<ObjAttr>().Value
                .GetComponent<Text>().text = chess.status.mobility.ToString();
            dicChessState[CommonDefine.kPanelChessMovDel].GetComponent<ObjAttr>().Value
                .GetComponent<Text>().text = chess.status.moveCoolingDelay.ToString();
        } else {
            setPanelActive(false);
        }
    }

    private void initObjImag() { //TODO:没写完 要改
        GameObject obj = Instantiate(ObjImagPref);
        obj.SetActive(false);
        dicChessState.Add(CommonDefine.kPanelChessName,obj);
    }

    private void initObjAttr(string name) {
        GameObject obj = Instantiate(ObjAttrPref);
        obj.GetComponent<ObjAttr>().Key.GetComponent<Text>().text = name;
        obj.SetActive(false);
        dicChessState.Add(name,obj);
    }

    private void setPanelActive(bool isActive) {
        foreach (GameObject e in dicChessState.Values) {
            if (isActive != e.activeSelf) {
                e.SetActive(isActive);
            }
        }
    }
}
