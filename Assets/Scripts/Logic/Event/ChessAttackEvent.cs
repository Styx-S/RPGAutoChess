
public class ChessAttackEvent : Event {
    private ChessBase mAttacker;
    private ChessBase mVictim;

    /* 攻击者 */
    public ChessBase attacker {
        get {
            return mAttacker;
        }
    }
    /* 被攻击者 */
    public ChessBase victim {
        get {
            return mVictim;
        }
    }

    // 事件涉及的数值处理需要将结果写在事件当中传递，便于多个事件处理的叠加与覆写
    // 最后由事件的抛出方统一处理，同时交UI显示
    // 例如：如果有技能释放事件，同时handler将改变技能的冷却时间，那么事件中应当传递原冷却值、线性叠加冷却值、以及非线性叠加冷却链表
    // 在添加事件携带的数值不要忘记在事件抛出处对新增加的值作出响应

    /* 攻击伤害 */
    public float damage;
    /* 攻击吸血 */
    public float steal = 0;


    public ChessAttackEvent(ChessBase attacker, ChessBase victim, float damage) : base(EventName.ChessAttackEventName) {
        mAttacker = attacker;
        mVictim = victim;
        this.damage = damage;
    }
}
