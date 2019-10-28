using System.Collections;
using System.Collections.Generic;


public delegate void onDead();          // 棋子死亡时回调

/*
    @description: 管理角色状态以及各种Buff效果
 */
public class ChessStatus
{
    private float HP;                   // 血量
    private float strength;             // 攻击力
    private int attachRadius;         // 攻击范围
    private float attachCoolingDelay;   // 攻击间隔
    private int mobility;               // 移动力
    private float moveCoolingDelay;     // 移动间隔

    private bool attachCooling; // 攻击冷却状态
    private bool moveCooling;   // 移动冷却状态

    public onDead onDeadDelegate;

    public ChessStatus(float HP, float strength, int attachRadius, float attachCoolingDelay,
        int mobility, float moveCoolingDelay) {
        
        this.HP = HP;
        this.strength = strength;
        this.attachRadius = attachRadius;
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
        attachCooling = true;
        TimerTools.getTimerTools().setTimer(attachCoolingDelay, new TimerAction(resetAttachCooling));
    }

    /* 获取目前攻击力 */
    public float getAttachDamage() {
        return strength;
    }

    /* 获取目前攻击范围 */
    public int getAttachRadius() {
        return attachRadius;
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
