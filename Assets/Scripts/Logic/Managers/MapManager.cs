using System.Collections;
using System.Collections.Generic;

public class MapManager : ManagerInterface {
    private Map mMap;
    public Map map {
        get {
            return mMap;
        }
    }
    string ManagerInterface.getName() {
        return CommonDefine.kManagerMapName;
    }
    void ManagerInterface.init() {
        this.mMap = new Map(10,10);
    }

    void ManagerInterface.update() {
        // do nothing
    }

    public void configureMap(Map map) {
        this.mMap = map;
    }
}
