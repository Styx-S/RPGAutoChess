using System;

class TimerNode {
    public double time;
    public TimerAction action;
    public TimerNode nextNode;

    public TimerNode(double time, TimerAction action) {
        this.time = time;
        this.action = action;
        this.nextNode = null;
    }
}

public delegate void TimerAction(); //时间到时执行动作

public class TimerTools : ManagerInterface
{
    private TimerNode head = new TimerNode(0, null);    // 队列头
    private static string name = "TimerTools";

    string ManagerInterface.getName() {
        return name;
    }

    void ManagerInterface.init() {

    }

    void ManagerInterface.update() {
        double currentTime = time();
        TimerNode node = head.nextNode;
        while(node != null) {
            if (node.time > currentTime) {
                break;
            } else {
                node.action();
                node = node.nextNode;
            }
        }
        head.nextNode = node;
    }

    /*  到期提醒
        interval: 距调用方法需等待的时间(seconds)
        action: 到时间调用的方法
     */
    public void setTimer(float interval, TimerAction action) {
        double currentTime = time();
        TimerNode newNode = new TimerNode(currentTime + interval, action);
        TimerNode insertPoint = head;
        while(insertPoint.nextNode != null) {
            if (insertPoint.nextNode.time >= newNode.time) {
                break;
            }
            insertPoint = insertPoint.nextNode;
        }
        if(insertPoint.nextNode != null) {
            newNode.nextNode = insertPoint.nextNode;
        }
        insertPoint.nextNode = newNode;
    }

    public static TimerTools getTimerTools() {
        TimerTools timerTools = (TimerTools)ManagerCollection.getCollection().GetManager(name);
        return timerTools;
    }

    private double time() {
        long ms = DateTime.Now.ToUniversalTime().Ticks / 10000;
        return ((double)ms) / 1000;
    }
}
