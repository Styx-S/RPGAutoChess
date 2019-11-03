using System.Collections;
using System.Collections.Generic;

/* 托管所有的Manager实例 */
public class ManagerCollection {
    List<ManagerInterface> managers = new List<ManagerInterface>();
    private static ManagerCollection instance = new ManagerCollection();

    private ManagerCollection() {
        managers.Add(new TimerTools());
        managers.Add(new RandomManager());
        managers.Add(new MapManager());
        managers.Add(new ChessManager());  
        managers.Add(new IOManager());
    } // 私有构造函数
    
    public static ManagerCollection getCollection() {
        return instance;
    }

    public void init() {
        foreach(ManagerInterface manager in managers) {
            manager.init();
        }
    }

    public void update() {
        foreach(ManagerInterface manager in managers) {
            manager.update();
        }
    }


    public ManagerInterface GetManager(string name) {
        foreach(ManagerInterface manager in managers) {
            if(manager.getName().Equals(name)) {
                return manager;
            }
        }
        return null;
    }
    
}
