using System.Collections;
using System.Collections.Generic;

/*
    @description: 管理角色状态以及各种Buff效果
 */
public class ChessStatus
{
    private float HP;                   // 血量
    private float strength;             // 攻击力
    private float attachCoolingDelay;   // 攻击间隔
    private int mobility;               // 移动力
    private float moveCoolingDelay;     // 移动间隔

    private bool attachCooling; // 攻击冷却状态
    private bool moveCooling;   // 移动冷却状态

    public ChessStatus(float HP, float strength, float attachCoolingDelay, int mobility, float moveCoolingDelay) {
        this.HP = HP;
        this.strength = strength;
        this.attachCoolingDelay = attachCoolingDelay;
        this.mobility = mobility;
        this.moveCoolingDelay = moveCoolingDelay;
    }

    public bool isDead() {
        return HP <= 0;
    }

    private float lastAttachTime = 0;
    /* 目前是否可以攻击 */
    public bool canAttach() {
        return !attachCooling;
    }

    /* 进入攻击冷却 */
    public void setAttachCooling() {
        TimerTools.getTimerTools().setTimer(attachCoolingDelay, new TimerAction(resetAttachCooling));
    }

    /* 获取目前攻击力 */
    public float attachDamage() {
        return strength;
    }

    /* 受到伤害 */
    public float damage(float value) {
        float causeDamage = value < this.HP ? value : this.HP;
        this.HP = this.HP - causeDamage;
        return causeDamage;
    }

    /* 目前是否可以移动 */
    public bool canMove() {
        return !moveCooling;
    }

    /* 进入移动冷却 */
    public void setMoveCooling() {
        TimerTools.getTimerTools().setTimer(moveCoolingDelay, new TimerAction(resetMoveCooling));
    }

    /* 获取移动力 */
    public int getMobility() {
        return mobility;
    }

    private void resetAttachCooling() {
        this.attachCooling = false;
    }
    private void resetMoveCooling() {
        this.moveCooling = false;
    }
}
