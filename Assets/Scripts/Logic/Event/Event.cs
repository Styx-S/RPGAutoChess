/*
    此文件存放事件系统相关定义
    事件系统工作流程:
    1. 希望处理事件的模块向事件派发器注册对应事件处理器
    2. 希望与提供给其他模块交互能力的地方初始化事件并抛出
    3. 事件派发器按照优先级(优先级高的处理器后调用，覆写低优先级处理器结果)依次调用事件处理器，事件处理器将处理结果写在事件中
    4. 调用结果返回抛出点，进行相应处理
*/



// 事件
// 关于事件的设计准则参看ChessAttackEvent事件的说明
public class Event {
    /*  该事件是否被取消, 当事件被取消时事件抛出者应该取消对应的行动
        正常情况下，如果低优先级处理器取消了事件，高优先级处理器应当无
        视这个事件 */
    public bool isCanceled = false;

    private string mName;
    /* 事件名 */
    public string name {
        get {
            return mName;
        }
    }

    public Event(string name) {
        mName = name;
    }
}

// 事件处理器处理事件的优先级
// 低优先级处理器先处理事件，高优先级的处理器可以覆盖低优先级处理事件的结果
// 同优先级处理器处理顺序不确定
public enum EventPriority {
    low,    // 低优先级，最先处理事件
    normal, // 正常优先级
    high,   // 高优先级
    final,  // 最终优先级；一个事件只能拥有一个final处理器
}


// 事件处理器
// 多个BUFF可以对应一个处理器，引用计数
public interface EventHandler {
    void handleEvent(Event e);
}

// 事件派发者
public interface EventDispatcher {
    bool registerEventHandler(string eventName, EventPriority priority, EventHandler handler);
    bool unregisterEventHandler(string eventName, EventHandler handler);

    Event throwEvent(Event e);
}

// 同时具有线性与非线性叠加事件
// 如技能释放事件(技能冷却时间)、移动事件(移动冷却时间)、攻击事件(攻击冷却时间)
public class OverlayEvent : Event {
    // 增益值
    protected float mBuffValue;
    // 线性时间减少 
    protected float mLinearUpgrade = 1;
    // 非线性时间减少
    protected float mNoLinearUpgrade = 1;

    public float buffValue {
        get {
            // 先结算线性增益，再结算非线性增益
            return mBuffValue * mLinearUpgrade * mNoLinearUpgrade;
        }
    }

    public float addLinearUpgrade(float value) {
        mLinearUpgrade += value;
        return mLinearUpgrade;
    }

    public float addNoLinearUpgrade(float value) {
        mLinearUpgrade *= value;
        return mNoLinearUpgrade;
    }
    
    public OverlayEvent(string eventName) : base(eventName) {

    }
}
