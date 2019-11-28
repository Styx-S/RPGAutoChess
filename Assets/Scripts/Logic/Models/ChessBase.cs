using System;

[Serializable]
public class ChessBase
{
    public ChessStatus status {
        get;
    }         // 棋子状态
    public Player owner {               // 棋子所属玩家
        get; set;
    }
    [NonSerialized]
    private ChessManager chessManager;  // 当前托管的Manager   
    public ChessLocation location {     // 棋子当前位置
        get; set;
    }

    public ChessBase target {
        get; set;
    }
    public bool isDead;             // 棋子是否死亡

    private static int nextChessID = 0;
    public int chessID = nextChessID++;

    public ChessBase(Player owner, ChessLocation location,float HP = 100, float strength = 5, int attachRadius = 1, float attachCoolingDelay = 1.0f,
        int mobility = 1, float moveCoolingDelay = 1.0f) {
        
        this.owner = owner;
        this.location = location;
        this.status = new ChessStatus(HP, strength, attachRadius, attachCoolingDelay, mobility, moveCoolingDelay);
        // 注册status回调
        this.status.onDeadDelegate = new onDead(this.die);

        this.target = null;
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
        } else {
            ChessSelectTargetEvent selectTargetEvent = (ChessSelectTargetEvent)EventHelper.getDispatcher()
                .throwEvent(new ChessSelectTargetEvent(this, target));
            if (selectTargetEvent.isCanceled == true || selectTargetEvent.target == null) {
                return;
            } else {
                target = selectTargetEvent.target;
            }
        }
        if (ChessManager.getDistance(this, target) > status.getAttackRadius()) {
            // 处于攻击范围之外, 向它移动
            ChessLocation moveTo = chessManager.findActualTarget(this, status.getMobility(), target.location);
            if (moveTo == this.location || !status.canMove()) {
                // 无法移动
                return;
            } else {
                ChessMoveEvent moveEvent = (ChessMoveEvent)EventHelper.getDispatcher()
                    .throwEvent(new ChessMoveEvent(this, moveTo, status.moveCoolingDelay));
                if (moveEvent.isCanceled == false) {
                    chessManager.moveChess(moveEvent.chess, moveEvent.moveTo);
                    status.setMoveCooling(moveEvent.moveCoolingValue);
                }
            }
        } else {
            // 当前是否处于可攻击状态
            if (this.status.canAttack()) {
                // 交由其他部分响应攻击事件
                ChessAttackEvent attackEvent = (ChessAttackEvent)EventHelper.getDispatcher()
                    .throwEvent(new ChessAttackEvent(this, target, status.getAttackDamage(), status.attackCoolingDelay));
                if (attackEvent.isCanceled == false) {
                    // 攻击事件结果处理
                    attackEvent.victim.underAttach(attackEvent.attacker, attackEvent.damage);
                    status.setAttackCooling(attackEvent.attackCoolingValue);
                    // TODO: 这里暂时只对事件中造成对伤害和冷却进行了处理，更多效果后续再说
                }
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
    public float underAttach(ChessBase attacker, float damage) {
        float casueDamage = this.status.damage(attacker, damage);
        // TODO: 暂时在这里发出攻击事件，后续应该会写一个事件处理系统
        ((IOManager)ManagerCollection.getCollection().GetManager(CommonDefine.kManagerIOName))
            .sendMessage(new ChessIncrementMessage_chessAttach(attacker, this, casueDamage));
        return casueDamage;
    }

    /* 调用后棋子死亡 */
    public void die(DeathReason reason, DeathInfo info) {
        ChessDeathEvent deathEvent = (ChessDeathEvent)EventHelper.getDispatcher()
            .throwEvent(new ChessDeathEvent(this, reason, info));
        if (deathEvent.isCanceled) {
            return;
        }
        this.isDead = true;
        chessManager.removeChess(this);
    }

    public override int GetHashCode() {
        return chessID;
    }

    public override bool Equals(object obj) {
        
        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }
        
        return chessID == ((ChessBase)obj).chessID;
    }
}
