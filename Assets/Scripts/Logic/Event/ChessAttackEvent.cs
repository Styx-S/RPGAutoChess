
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
    /* 攻击伤害 */
    public float damage;


    public ChessAttackEvent(ChessBase attacker, ChessBase victim, float damage) : base(EventName.ChessAttackEventName) {
        mAttacker = attacker;
        mVictim = victim;
        this.damage = damage;
    }
}
