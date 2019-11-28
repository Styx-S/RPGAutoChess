public enum DeathReason {
    Other,
    ChessAttack,    // 由于来自棋子的伤害导致死亡
}

public class DeathInfo {
    private ChessBase mAttacker;
    private float mAttackDamage;

    public ChessBase attacker {
        get {
            return mAttacker;
        }
    }

    public float attackDamage {
        get {
            return mAttackDamage;
        }
    }

    public DeathInfo(ChessBase attacker = null, float attackDamage = 0) {
        mAttacker = attacker;
        mAttackDamage = 0;
    }

    public static DeathInfo makeInfoByAttack(ChessBase attacker, float attackDamage) {
        return new DeathInfo(attacker, attackDamage);
    }

}

public class ChessDeathEvent : Event {
    private ChessBase mChess;
    private DeathReason mReason;
    private DeathInfo mInfo;

    public ChessBase chess {
        get {
            return mChess;
        }
    }

    public DeathReason reason {
        get {
            return mReason;
        }
    }

    public DeathInfo info {
        get {
            return mInfo;
        }
    }

    public ChessDeathEvent(ChessBase chess, DeathReason reason, DeathInfo info) : base(EventName.ChessDeathEventName) {
        mChess = chess;
        mReason = reason;
        mInfo = info;
    }
}
