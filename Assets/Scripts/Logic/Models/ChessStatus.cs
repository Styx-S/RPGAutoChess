using System;

public delegate void onDead();          // 棋子死亡时回调

/*
    @description: 管理角色状态以及各种Buff效果
 */
 [Serializable]
public class ChessStatus
{
    private float mHP;                  // 血量
    private float mStrength;            // 攻击力
    private int mAttackRadius;        // 攻击范围
    private float mAttackCoolingDelay;  // 攻击间隔
    private int mMobility;            // 移动力
    private float mMoveCoolingDelay;    // 移动间隔

    

    public float HP {
        get {
            return mHP;
        } set {
            mHP = HP;
        }
    }
    public float strength {
        get {
            return mStrength;
        } set {
            mStrength = strength;
        }
    }
    public int attackRadius {
        get {
            return mAttackRadius;
        } set {
            mAttackRadius = attackRadius;
        }
    }
    public float attackCoolingDelay {
        get {
            return mAttackCoolingDelay;
        } set {
            // 更改时要重新计算是否在冷却当中
            if (attackCooling) {
                attackCoolingTimerReceipt.updateTimer(attackCoolingDelay - mAttackCoolingDelay);
            }
            mAttackCoolingDelay = attackCoolingDelay;
        }
    }
    public int mobility {
        get {
            return mMobility;
        } set {
            mMobility = mobility;
        }
    }
    public float moveCoolingDelay {
        get {
            return mMoveCoolingDelay;
        } set {
            // 重新计算冷却时间
            if (moveCooling) {
                moveCoolingTimerReceipt.updateTimer(moveCoolingDelay - mMoveCoolingDelay);
            }
            mMoveCoolingDelay = moveCoolingDelay;
        }
    }

    [NonSerialized]
    private bool attackCooling; // 攻击冷却状态
    [NonSerialized]
    private TimerReceipt attackCoolingTimerReceipt;
    [NonSerialized]
    private bool moveCooling;   // 移动冷却状态
    [NonSerialized]
    private TimerReceipt moveCoolingTimerReceipt;

    public onDead onDeadDelegate;

    public ChessStatus(float HP, float strength, int attackRadius, float attackCoolingDelay,
        int mobility, float moveCoolingDelay) {
        
        // TODO: 这里有一处奇怪的问题，如果把所有的mValue改为this.value就会出现问题，后面再研究
        mHP = HP;
        mStrength = strength;
        mAttackRadius = attackRadius;
        mAttackCoolingDelay = attackCoolingDelay;
        mMobility = mobility;
        mMoveCoolingDelay = moveCoolingDelay;
    }

    public bool isDead() {
        return HP <= 0;
    }

    private float lastAttachTime = 0;
    /* 目前是否可以攻击 */
    public bool canAttack() {
        return !attackCooling;
    }

    /* 进入攻击冷却 */
    public void setAttackCooling() {
        attackCooling = true;
        attackCoolingTimerReceipt = TimerTools.getTimerTools().setTimer(attackCoolingDelay, new TimerAction(resetAttackCooling));
    }

    /* 获取目前攻击力 */
    public float getAttackDamage() {
        return strength;
    }

    /* 获取目前攻击范围 */
    public int getAttackRadius() {
        return attackRadius;
    }

    /* 受到伤害 */
    public float damage(float value) {
        float causeDamage = value < this.HP ? value : this.HP;
        this.HP = this.HP - causeDamage;
        if (this.HP <= 0 && onDeadDelegate != null) {
            onDeadDelegate();
        }
        return causeDamage;
    }

    /* 目前是否可以移动 */
    public bool canMove() {
        return !moveCooling;
    }

    /* 进入移动冷却 */
    public void setMoveCooling() {
        moveCooling = true;
        moveCoolingTimerReceipt = TimerTools.getTimerTools().setTimer(moveCoolingDelay, new TimerAction(resetMoveCooling));
    }

    /* 获取移动力 */
    public int getMobility() {
        return mobility;
    }

    private void resetAttackCooling() {
        this.attackCooling = false;
    }
    private void resetMoveCooling() {
        this.moveCooling = false;
    }
}
