using System.Collections;
using System.Collections.Generic;

public class ChessBase
{
    private float HP;           // 血量
    private float strength;     // 攻击力
    private float attachDelay;  // 攻击间隔
    private int mobility;       // 移动力

    public ChessBase(float HP = 100, float strength = 5, float attachDelay = 1.0f, int mobility = 1) {
        this.HP = HP;
        this.strength = strength;
        this.attachDelay = attachDelay;
        this.mobility = mobility;
    }

    // 当前位于的棋盘
    // private ChessBoard currentBoard;

    // 进行一次行动
    public void act() {
        stupidAI();
    }
    private void stupidAI() {
        // 当前是否拥有可攻击对象

        // 向最近的敌人移动
    }

    public void setInBoard(/* ChessBoard chessboard */) {
        // 设置当前棋盘

        // 通知当前棋盘
    }
}
