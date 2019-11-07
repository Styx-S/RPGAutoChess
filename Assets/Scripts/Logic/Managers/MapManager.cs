using System.Collections;
using System.Collections.Generic;

public class MapManager : ManagerInterface, CanConfigureSceneInterface {
    private Map mMap;
    public Map map {
        get {
            return mMap;
        }
    }

    private string mSceneName;
    public string sceneName {
        get {
            return mSceneName;
        }
    }

    public MapManager() {

    }

    public MapManager(string sceneName){
        mSceneName = sceneName;
    }

    string ManagerInterface.getName() {
        return CommonDefine.kManagerMapName;
    }
    void ManagerInterface.init() {
        if (sceneName != null) {
            SceneUtil.getUtil().configureFromScene(this, sceneName);
        }
    }

    void ManagerInterface.update() {
        // do nothing
    }

    void CanConfigureSceneInterface.configure(Scene scene) {
        this.mMap = scene.map;
    }
}
