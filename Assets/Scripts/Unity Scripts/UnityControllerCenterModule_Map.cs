using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityControllerCenterModule_Map : IUnityControllerCenterModule {
    bool IUnityControllerCenterModule.tryHandleMessage(MessageBase message) {
        if (message.type != MessageInfoType.BattleMapGrid) {
            return false;
        }
        if (message.kindType == MessageInfoKindType.Completion) {
            MapCompletionMessage mapCompletionMessage = (MapCompletionMessage)message;
            if (mapCompletionMessage.mapNum > 0) {
                notifyCamera(mapCompletionMessage.data);
                notifyMapUI(mapCompletionMessage.data);
            }
        } else {

        }
        return true;
    }

    private void notifyCamera(Map map) {
        GameObject camera = GameObject.FindGameObjectWithTag(CommonDefine.kMainCameraNameTag);
        CameraController script;
        if (camera != null && (script = camera.GetComponent<CameraController>())) {
            script.calCameraPosition(map);
        }
        
    }

    private void notifyMapUI(Map map) {
        GameObject mapUI = GameObject.FindGameObjectWithTag(CommonDefine.kMapUINameTag);
        GenMapUI script;
        if (mapUI != null && (script = mapUI.GetComponent<GenMapUI>())) {
            script.genMap(map);
        }
    }
}
