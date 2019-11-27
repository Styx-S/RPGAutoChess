using System;

class TimerNode {
    public double time;
    internal TimerAction action;
    internal TimerNode nextNode;
    internal TimerReceipt receipt;

    public TimerNode(double time, TimerAction action) {
        this.time = time;
        this.action = action;
        this.nextNode = null;
    }

    public void runAction() {
        if (action != null) {
            action();
        }
        if (receipt != null) {
            receipt.finished = true;
        }
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
                node.runAction();
                node = node.nextNode;
            }
        }
        head.nextNode = node;
    }

    /*  到期提醒
        interval: 距调用方法需等待的时间(seconds)
        action: 到时间调用的方法
     */
    public TimerReceipt setTimer(float interval, TimerAction action) {
        double currentTime = time();
        double timerTime = currentTime + interval;
        TimerNode newNode = new TimerNode(timerTime, action);
        insertTimerNode(newNode);
        TimerReceipt receipt = new TimerReceipt(this, newNode, currentTime, timerTime);
        return receipt;
    }

    internal void insertTimerNode(TimerNode newNode) {
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

    /*  删除一个节点
        由于外界不应当知道TimerNode这一实现，所以应通过TimerReceipt.cancel来调用
     */
    internal bool deleteTimerNode(TimerNode delNode) {
        TimerNode node = head;
        while(head.nextNode != null) {
            if (head.nextNode == delNode) {
                head.nextNode = delNode.nextNode;
                head.nextNode.nextNode = null;
                return true;
            }
        }
        return false;
    }

    public static TimerTools getTimerTools() {
        TimerTools timerTools = (TimerTools)ManagerCollection.getCollection().GetManager(name);
        return timerTools;
    }

    internal double time() {
        long ms = DateTime.Now.ToUniversalTime().Ticks / 10000;
        return ((double)ms) / 1000;
    }
}

// TimerReceipt: 回执 用于取消Timer
public class TimerReceipt {
    private TimerTools mTool;
    private TimerNode mNode;
    private double mInsertTime;     // timer节点插入时间    
    private double mTimerTime;      // timer节点计划执行时间    
    public bool finished = false;   // 该任务是否已执行
    internal TimerReceipt(TimerTools timerTools, TimerNode timerNode, double insertTime, double timerTime) {
        mTool = timerTools;
        mNode = timerNode;
        mInsertTime = insertTime;
        mTimerTime = timerTime;
    }

    /* 取消Timer */
    public bool cancel() {
        return mTool.deleteTimerNode(mNode);
    }

    /*  变更Timer时间
        delta为正表示延后，delta为负表示提前
        返回值表示变更时间后该Timer是否已到期立即执行
    */
    public bool updateTimer(float delta) {
        if (finished) {
            return true;
        } else {
            mTool.deleteTimerNode(mNode);
            mTimerTime += delta;
            if (mTimerTime < mTool.time()) {
                // 立即执行
                mNode.runAction();
                return true;
            } else {
                // 重新插入
                mNode.time = mTimerTime;
                mTool.insertTimerNode(mNode);
                return false;
            }
        }   
    }
}
