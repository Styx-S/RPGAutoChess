using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 托管所有的Manager实例 */
public class ManagerCollection : MonoBehaviour
{
    List<ManagerInterface> managers = new List<ManagerInterface>();

    public ManagerCollection() {
        this.addManager(new MapManager());
        this.addManager(new ChessManager());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void addManager(ManagerInterface manager) {
        managers.Add(manager);
        manager.init();
    }

    public ManagerInterface GetManager(string name) {
        foreach(ManagerInterface manager in this.managers) {
            if(manager.getName().Equals(name)) {
                return manager;
            }
        }
        return null;
    }

    public static ManagerCollection getCollection() {
        ManagerCollection managers = GameObject.FindGameObjectWithTag(CommonDefine.kControllerName)
            .GetComponent<ManagerCollection>();
        if (managers == null) {
            GameObject.FindGameObjectWithTag(CommonDefine.kControllerName)
                .AddComponent<ManagerCollection>();
            return  GameObject.FindGameObjectWithTag(CommonDefine.kControllerName)
                .GetComponent<ManagerCollection>();
        }
        return managers;
    }
}
