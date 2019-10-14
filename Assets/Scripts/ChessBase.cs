using System.Collections;
using System.Collections.Generic;

public class ChessBase
{
    private ChessStatus status;         // 棋子状态
    public Player owner {               // 棋子所属玩家
        get; set;
    }
    private ChessManager chessManager;  // 当前托管的Manager   
    public ChessLocation location {     // 棋子当前位置
        get; set;
    }
    public bool isDead;             // 棋子是否死亡
    public ChessBase(Player owner, float HP = 100, float strength = 5, int attachRadius = 1, float attachCoolingDelay = 1.0f,
        int mobility = 1, float moveCoolingDelay = 1.0f) {
        
        this.owner = owner;
        this.status = new ChessStatus(HP, strength, attachRadius, attachCoolingDelay, mobility, moveCoolingDelay);
        // 注册status回调
        this.status.onDeadDelegate = new onDead(this.die);
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
            ChessLocation moveTo = chessManager.findActualTarget(this, status.getMobility(), target.location);
            if (moveTo == this.location) {
                // 无法移动
                return;
            } else {
                chessManager.moveChess(this, moveTo);
                status.setMoveCooling();
            }
        } else {
            // 当前是否处于可攻击状态
            if (this.status.canAttach()) {
                target.underAttach(this, status.getAttachDamage());
                status.setAttachCooling();
            }
        }
    }

    public void notifyLocation(ChessManager chessManager, ChessLocation location) {
        this.chessManager = chessManager;
        this.location = location;
    }

    /* 棋子被攻击 
       @return 攻击造成的实际伤害
         */
    public float underAttach(ChessBase attacher, float damage) {
        float casueDamage = this.status.damage(damage);
        // 通知控制器
        UnityControllerCenter.getCenter().sendMessage(new ControllerMessage_chessAttach(attacher, this, casueDamage));
        return casueDamage;
    }

    /* 调用后棋子死亡 */
    public void die() {
        this.isDead = true;
        chessManager.removeChess(this);
    }


}
