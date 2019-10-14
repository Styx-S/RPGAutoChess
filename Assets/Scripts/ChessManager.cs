using System.Collections;
using System.Collections.Generic;

public class ChessManager : ManagerInterface
{

    private List<ChessBase> mChessList = new List<ChessBase>();
    private Dictionary<ChessBase,Dictionary<ChessBase,int>> mDistanceMap = new Dictionary<ChessBase, Dictionary<ChessBase, int>>();
    private ChessBase[][] mChessMap;

    string ManagerInterface.getName() {
        return CommonDefine.kManagerChessName;
    }
    void ManagerInterface.init() {
        loadChess();
    }
    void ManagerInterface.update() {
        foreach(ChessBase chess in mChessList) {
            chess.act();
        }
    }

    /* 从某个地方载入棋子到list中 */
    public void loadChess() {
        mChessMap = new ChessBase[6][];
        for (int i = 0;i < mChessMap.Length;i++) {
            mChessMap[i] = new ChessBase[7];
            for (int j = 0;j < mChessMap[i].Length;j++) {
                mChessMap[i][j] = null;
            }
        }
        Player playerA = new Player("1","A");   //暂时hardcode，之后用不到
        Player playerB = new Player("2","B");
        addChess(new ChessBase(playerA),new ChessLocation(0,0));
        addChess(new ChessBase(playerA),new ChessLocation(1,1));
        addChess(new ChessBase(playerA),new ChessLocation(1,2));
        addChess(new ChessBase(playerB),new ChessLocation(5,6));
        addChess(new ChessBase(playerB),new ChessLocation(4,5));
        addChess(new ChessBase(playerB),new ChessLocation(5,4));
    }

    /* delegate： 用于通过ChessManager迭代查找时的过滤条件 */
    public delegate bool chessFilterMethod(ChessBase chess);
    public List<ChessBase> pFindChesses(chessFilterMethod filter) {
        List<ChessBase> results = new List<ChessBase>();
        foreach(ChessBase chess in mChessList) {
            if (filter(chess)) {
                results.Add(chess);
            }
        }
        return results;
    }

    /* 为chess选择一个攻击target，target可以不在chess攻击范围内 */
    public ChessBase selectTarget(ChessBase chess) {
        int minLen = int.MaxValue;
        ChessBase target = null;
        foreach (ChessBase e in mChessList) {
            if (e == chess) {
                continue;
            }
            if (e.owner == chess.owner) {
                continue;
            }
            int len = mDistanceMap[chess][e];
            if (len < minLen) {
                minLen = len;
                target = e;
            }
        }
        return target;
    }

    /* 获取chess的位置 */
    public ChessLocation getChessPosition(ChessBase chess) {
        return chess.location;
    }

    /* 移动棋子 */
    public void moveChess(ChessBase chess, ChessLocation location) {
        // 通知控制器移动棋子
        UnityControllerCenter.getCenter().sendMessage(new ControllerMessage_chessMove(chess, chess.location, location));

        mChessMap[chess.location.x][chess.location.y] = null;
        chess.location = location;
        mChessMap[chess.location.x][chess.location.y] = chess;
        foreach (ChessBase e in mDistanceMap[chess].Keys) {
            int distance = getDistance(chess,e);
            mDistanceMap[chess][e] = distance;
            mDistanceMap[e][chess] = distance;
        }
    }

    /* 棋子与目标棋子交互（攻击，技能等） */
    public void actChess(ChessBase chess, ChessBase target ,int opID) {
        // TODO: 棋子与目标棋子交互内容
    }

    /* 移除棋子 */
    public void removeChess(ChessBase chess) {
        // 通知控制器销毁GameObject
        UnityControllerCenter.getCenter().sendMessage(new ControllerMessage_chessRemove(chess));

        mChessList.Remove(chess);
        mChessMap[chess.location.x][chess.location.y] = null;
        mDistanceMap.Remove(chess);
        foreach (ChessBase e in mDistanceMap.Keys) {
            mDistanceMap[e].Remove(chess);
        }
    }

    /* 增加棋子 */
    public void addChess(ChessBase chess, ChessLocation location) {
        // 由ChessManager来通知Chess的新位置
        chess.notifyLocation(this, location);

        // 通知控制器生成棋子
        UnityControllerCenter.getCenter().sendMessage(new ControllerMessage_chessCreate(chess));
        
        mChessList.Add(chess);
        mChessMap[chess.location.x][chess.location.y] = chess;
        Dictionary<ChessBase,int> dic = new Dictionary<ChessBase, int>();
        foreach (ChessBase e in mDistanceMap.Keys) {
            int distance = getDistance(chess,e);
            mDistanceMap[e].Add(chess,distance);
            dic.Add(e,distance);
        }
        mDistanceMap.Add(chess,dic);
    }

    /* 获取chess某个距离内所有其他棋子 */
    public List<ChessBase> getChessInRange (ChessBase chess, int range) {
        List<ChessBase> list = new List<ChessBase>();
        foreach (ChessBase e in mChessList) {
            int len = mDistanceMap[chess][e];
            if (len <= range) {
                list.Add(e);
            }
        }
        return list;
    }

    /* 获取两个chess的距离 */
    public static int getDistance(ChessBase chess ,ChessBase target) {
        ChessLocation chessPosition = chess.location;
        ChessLocation targetPosition = target.location;
        return ChessLocation.getDistance(chess.location,target.location);
    }

    /* 棋子逼近目标位置 */
    public ChessLocation findActualTarget(ChessBase chess, int mobility, ChessLocation target) { // 写麻了，之后再优化
        if (mobility <= 0) {
            return chess.location;
        }
        List<ChessLocation> targets = new List<ChessLocation>();
        if (mobility >= ChessLocation.getDistance(chess.location,target)) {
            if (mChessMap[target.x][target.y] == null) {
                return target;
            } else {
                List<ChessLocation> neighbor = ChessLocation.getNeighbor(target);
                foreach (ChessLocation e in neighbor) {
                    int distance = ChessLocation.getDistance(chess.location,e);
                    if (mobility >= distance) {
                        targets.Add(findActualTarget(chess,distance,e));
                    }
                }
            }
        } else {
            List<ChessLocation> limit = ChessLocation.getLimit(chess.location,target,mobility);
            foreach (ChessLocation e in limit) {
                targets.Add(findActualTarget(chess,mobility,e));
            }
        }
        if (targets.Count > 0) {
            return targets[0];
        } else {
            return chess.location;
        }
    }
}
