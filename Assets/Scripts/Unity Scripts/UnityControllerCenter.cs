using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

// 用于处理不同消息类型的模块
public interface IUnityControllerCenterModule {
    bool tryHandleMessage(MessageBase message);
}

public class UnityControllerCenter : MonoBehaviour
{
    private ConcurrentQueue<MessageBase> messageQueue = new ConcurrentQueue<MessageBase>();
    private List<IUnityControllerCenterModule> modules = new List<IUnityControllerCenterModule>();

    void Awake() {
        modules.Add(new CenterModule_Chess());
        modules.Add(new UnityControllerCenterModule_Map());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dispose();
    }

    public static UnityControllerCenter getCenter() {
        UnityControllerCenter center = GameObject.FindGameObjectWithTag(CommonDefine.kControllerNameTag)
            .GetComponent<UnityControllerCenter>();
        if (center == null) {
            GameObject.FindGameObjectWithTag(CommonDefine.kControllerNameTag)
                .AddComponent<UnityControllerCenter>();
            return  GameObject.FindGameObjectWithTag(CommonDefine.kControllerNameTag)
                .GetComponent<UnityControllerCenter>();
        }
        return center;
    }

    public void sendMessage(MessageBase message) {
        messageQueue.Enqueue(message);
    }

    private void dispose() {
        while(messageQueue.Count > 0) {
            MessageBase message;
            if(messageQueue.TryDequeue(out message)) {
                bool canHandleFlag = false;
                foreach(IUnityControllerCenterModule module in modules) {
                    if (module.tryHandleMessage(message)) {
                        // 成功处理，退出循环
                        canHandleFlag = true;
                        break;
                    }
                    // 寻找下一个能处理的模块
                    continue;
                }
                if (!canHandleFlag) {
                    Debug.Log("一个不支持的消息被丢弃");
                }
            }            

        }
    }
}
