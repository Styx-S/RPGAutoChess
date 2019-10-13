using System.Collections;
using System.Collections.Generic;

public class ChessBase
{
    private ChessStatus status;         // 棋子状态
    private Player owner;               // 棋子所属玩家
    private ChessManager chessManager;  // 当前托管的Manager   
    public ChessLocation location {     // 棋子当前位置
        get; set;
    }

    public Player Owner {           // 棋子所属玩家
        get; set;
    }
    public ChessBase(Player owner, float HP = 100, float strength = 5, int attachRadius = 1, float attachCoolingDelay = 1.0f,
        int mobility = 1, float moveCoolingDelay = 1.0f) {
        
        this.owner = owner;
        this.status = new ChessStatus(HP, strength, attachRadius, attachCoolingDelay, mobility, moveCoolingDelay);
    }

    // 进行一次行动
    public void act() {
        stupidAI();
    }
    private void stupidAI() {
        // 当前是否拥有可攻击对象
        ChessBase target = chessManager.selectTarget(this);
        if (target == null) {
            return;
        }
        if (ChessManager.getDistance(this, target) > status.getAttachRadius()) {
            // 处于攻击范围之外, 向它移动
            // 移动
            status.setMoveCooling();
        } else {
            // 当前是否处于可攻击状态
            if (this.status.canAttach()) {
                // 攻击
                status.setAttachCooling();
            }
        }
    }

    public void notifyLocation(ChessManager chessManager, ChessLocation location) {
        this.chessManager = chessManager;
        this.location = location;
    }


}
