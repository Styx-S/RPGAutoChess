using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBase
{
    private ChessStatus status;     // 棋子状态
    private Player owner;           // 棋子所属玩家
    public Vector2Int position {    // 棋子当前位置
        get {
            return position;
        } set {
            this.position = value;
        }
    }

    public Player Owner {           // 棋子所属玩家
        get {
            return owner;
        }
    }
    public ChessBase(Player owner, Vector2Int position, float HP = 100, float strength = 5,
        float attachCoolingDelay = 1.0f, int mobility = 1, float moveCoolingDelay = 1.0f) {
        this.owner = owner;
        this.status = new ChessStatus(HP, strength, attachCoolingDelay, mobility, moveCoolingDelay);
        this.position = position;
    }

    // 当前位于的棋盘
    // private ChessBoard currentBoard;

    // 进行一次行动
    public void act() {
        stupidAI();
    }
    private void stupidAI() {
        // 当前是否拥有可攻击对象

            // 当前是否处于可攻击状态
            if (this.status.canAttach()) {
                
            }

        // 向最近的敌人移动
    }

    public void setInBoard(/* ChessBoard chessboard */) {
        // 设置棋子的当前棋盘

        // 通知当前棋盘
    }
}
