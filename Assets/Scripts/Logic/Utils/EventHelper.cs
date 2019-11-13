using System.Collections.Generic;

// 所有对EventDispatcher的访问走这里
public class EventHelper {
    private static EventDispatcher instance = new SimpleEventDispatcher();
    public static EventDispatcher getDispatcher() {
        return instance;
    }
}

// 实现EventDispatcher接口
public class SimpleEventDispatcher : EventDispatcher {
    bool EventDispatcher.registerEventHandler(string eventName, EventPriority priority, EventHandler handler) {
        HandlerNode head;
        if (!reigsteredEventHandlerHeadDic.ContainsKey(eventName)) {
            // 如果该事件还未注册，首先初始化head指针
            head = new HandlerNode(null, 0, null);
            reigsteredEventHandlerHeadDic[eventName] = head;
        } else {
            head = reigsteredEventHandlerHeadDic[eventName];
        }
        HandlerNode insertPoint = head;
        while(insertPoint.nextNode != null) {
            if (insertPoint.nextNode.priority > priority) {
                break;
            }
            insertPoint = insertPoint.nextNode;
        }
        HandlerNode newNode = new HandlerNode(handler, priority, insertPoint.nextNode);
        insertPoint.nextNode = newNode;
        return true;
    }

    bool EventDispatcher.unregisterEventHandler(string eventName, EventHandler handler) {
        if (reigsteredEventHandlerHeadDic.ContainsKey(eventName)) {
            HandlerNode head;
            reigsteredEventHandlerHeadDic.TryGetValue(eventName, out head);
            while(head.nextNode != null) {
                if (head.nextNode.handler.Equals(handler)) {
                    head.nextNode = head.nextNode.nextNode;
                    head.nextNode.nextNode = null;
                    return true;
                }
                head = head.nextNode;
            }
        }
        return false;
    }

    Event EventDispatcher.throwEvent(Event e) {
        if (reigsteredEventHandlerHeadDic.ContainsKey(e.name)) {
            HandlerNode head;
            reigsteredEventHandlerHeadDic.TryGetValue(e.name, out head);
            while (head.nextNode != null) {
                head = head.nextNode;
                if (head.handler != null) {
                    head.handler.handleEvent(e);
                }
            }
        }
        return e;
    }

    private Dictionary<string, HandlerNode> reigsteredEventHandlerHeadDic = new Dictionary<string, HandlerNode>();

}

// Handler链表节点
public class HandlerNode {
    public EventHandler handler;
    public EventPriority priority;
    public HandlerNode nextNode;

    public HandlerNode(EventHandler h, EventPriority p, HandlerNode n) {
        handler = h;
        priority = p;
        nextNode = n;
    }
}
