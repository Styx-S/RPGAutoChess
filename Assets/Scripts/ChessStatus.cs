using System.Collections;
using System.Collections.Generic;

/*
    @description: 管理角色状态以及各种Buff效果
 */
public class ChessStatus
{
    private float HP;           // 血量
    private float strength;     // 攻击力
    private float attachDelay;  // 攻击间隔
    private int mobility;       // 移动力

    public ChessStatus(float HP, float strength, float attachDelay, int mobility) {
        this.HP = HP;
        this.strength = strength;
        this.attachDelay = attachDelay;
        this.mobility = mobility;
    }

    public bool isDead() {
        return HP <= 0;
    }

    private float lastAttachTime = 0;
    /* 目前是否可以攻击 */
    public bool canAttach() {
        // 获取当前时钟
        // float deltaTime 
        return true;
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
}
