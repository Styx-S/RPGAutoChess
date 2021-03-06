﻿using System.Collections;
using System.Collections.Generic;

public class ChessManager : ManagerInterface, CanConfigureSceneInterface
{

    private List<ChessBase> mChessList = new List<ChessBase>();
    private Dictionary<ChessBase,Dictionary<ChessBase,int>> mDistanceMap = new Dictionary<ChessBase, Dictionary<ChessBase, int>>();
    private ChessBase[][] mChessMap;
    private string mSceneName;
    public string sceneName {
        get {
            return mSceneName;
        }
    }

    public ChessManager() {

    }
    public ChessManager(string sceneName) {
        mSceneName = sceneName;
    }

    string ManagerInterface.getName() {
        return CommonDefine.kManagerChessName;
    }
    void ManagerInterface.init() {
        if (mSceneName != null) {
            SceneUtil.getUtil().configureFromScene(this, mSceneName);
        }
    }
    void ManagerInterface.update() {
        ChessBase[] chesses = new ChessBase[mChessList.Count];
        mChessList.CopyTo(chesses);
        foreach(ChessBase chess in chesses) {
            if (!chess.isDead) {
                chess.act();
            }
        }
    }

    void CanConfigureSceneInterface.configure(Scene scene) {
        mChessMap = new ChessBase[scene.map.width][];
        for (int i = 0; i < mChessMap.Length; i++) {
            mChessMap[i] = new ChessBase[scene.map.height];
            for (int j = 0; j < mChessMap[i].Length; j++) {
                mChessMap[i][j] = null;
            }
        }
        foreach(ChessBase chess in scene.chesses) {
            addChess(chess, chess.location);
        }
    }

    /* delegate： 用于通过ChessManager迭代查找时的过滤条件 */
    public delegate bool chessFilteRandomUtilethod(ChessBase chess);
    public List<ChessBase> pFindChesses(chessFilteRandomUtilethod filter) {
        List<ChessBase> results = new List<ChessBase>();
        foreach(ChessBase chess in mChessList) {
            if (filter(chess)) {
                results.Add(chess);
            }
        }
        return results;
    }

    /* 为chess选择一个攻击target，target可以不在chess攻击范围内 */
    public ChessBase selectTarget(ChessBase chess) { //改改
        int minLen = int.MaxValue;
        List<ChessBase> targets = new List<ChessBase>();
        foreach (ChessBase e in mChessList) {
            if (e == chess) {
                continue;
            }
            if (e.owner == chess.owner) {
                continue;
            }
            int len = mDistanceMap[chess][e];
            if (len < minLen) {
                targets.Clear();
                minLen = len;
                targets.Add(e);
            } else if (len == minLen) {
                targets.Add(e);
            } 
        }
        if (chess.target != null) {
            if (targets.Contains(chess.target)) {
                return chess.target;
            }
        }
        if (targets.Count != 0) {
            System.Random r = new System.Random(RandomUtil.next());
            ChessBase target = targets[r.Next(0,targets.Count)];
            chess.target = target;
            return target;
        } else {
            chess.target = null;
            return null;
        }
        
    }

    /* 获取chess的位置 */
    public ChessLocation getChessPosition(ChessBase chess) {
        return chess.location;
    }

    /* 移动棋子 */
    public void moveChess(ChessBase chess, ChessLocation location) {
        // 通知IOManager棋子移动消息
        ((IOManager)ManagerCollection.getCollection().GetManager(CommonDefine.kManagerIOName))
            .sendMessage(new ChessIncrementMessage_chessMove(chess, chess.location, location));

        mChessMap[chess.location.x][chess.location.y] = null;
        chess.location = location;
        mChessMap[chess.location.x][chess.location.y] = chess;
        ChessBase[] keys = new ChessBase[mDistanceMap[chess].Keys.Count];
        mDistanceMap[chess].Keys.CopyTo(keys,0);
        foreach (ChessBase e in keys) {
            int distance = getDistance(chess,e);
            mDistanceMap[chess][e] = distance;
            mDistanceMap[e][chess] = distance;
        }
    }

    public ChessBase[] getChessList() {
        // TODO：这里对于ChessBase没有拷贝,考虑对于ChessBase到时候会进行序列化/反序列化，后续无需解决这个问题
        return mChessList.ToArray();
    }

    /* 棋子与目标棋子交互（攻击，技能等） */
    public void actChess(ChessBase chess, ChessBase target ,int opID) {
        // TODO: 棋子与目标棋子交互内容
    }

    /* 移除棋子 */
    public void removeChess(ChessBase chess) {
        // 通知IOManager棋子移除消息
        ((IOManager)ManagerCollection.getCollection().GetManager(CommonDefine.kManagerIOName))
            .sendMessage(new ChessIncrementMessage_chessRemove(chess));

        mChessList.Remove(chess);
        mChessMap[chess.location.x][chess.location.y] = null;
        mDistanceMap.Remove(chess);
        foreach (ChessBase e in mDistanceMap.Keys) {
            mDistanceMap[e].Remove(chess);
        }
    }

    /* 增加棋子 */
    public bool addChess(ChessBase chess, ChessLocation location) {
        ChessSpawnEvent spawnEvent = (ChessSpawnEvent)EventHelper.getDispatcher()
            .throwEvent(new ChessSpawnEvent(chess, location));
        if (spawnEvent.isCanceled) {
            return false;
        }
        // 由ChessManager来通知Chess的新位置
        chess.notifyLocation(this, spawnEvent.spawnLocation);

        // 通知IOManager棋子生成消息
        ((IOManager)ManagerCollection.getCollection().GetManager(CommonDefine.kManagerIOName))
            .sendMessage(new ChessIncrementMessage_chessCreate(chess));
        
        mChessList.Add(chess);
        mChessMap[chess.location.x][chess.location.y] = chess;
        Dictionary<ChessBase,int> dic = new Dictionary<ChessBase, int>();
        foreach (ChessBase e in mDistanceMap.Keys) {
            int distance = getDistance(chess,e);
            mDistanceMap[e].Add(chess,distance);
            dic.Add(e,distance);
        }
        mDistanceMap.Add(chess,dic);
        return true;
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
        // if (mobility <= 0) {
        //     return chess.location;
        // }
        // List<ChessLocation> targets = new List<ChessLocation>();
        // if (mobility >= ChessLocation.getDistance(chess.location,target)) {
        //     if (mChessMap[target.x][target.y] == null) {
        //         return target;
        //     } else {
        //         List<ChessLocation> neighbor = ChessLocation.getNeighbor(target);
        //         foreach (ChessLocation e in neighbor) {
        //             int distance = ChessLocation.getDistance(chess.location,e);
        //             if (mobility >= distance) {
        //                 targets.Add(findActualTarget(chess,distance,e));
        //             }
        //         }
        //     }
        // } else {
        //     List<ChessLocation> limit = ChessLocation.getLimit(chess.location,target,mobility);
        //     foreach (ChessLocation e in limit) {
        //         targets.Add(findActualTarget(chess,mobility,e));
        //     }
        // }
        // if (targets.Count > 0) {
        //     return targets[0];
        // } else {
        //     return chess.location;
        // }

        HashSet<ChessLocation> targets = new HashSet<ChessLocation>();
        HashSet<ChessLocation> barrier = new HashSet<ChessLocation>();
        HashSet<ChessLocation> travers = new HashSet<ChessLocation>();
        HashSet<ChessLocation> temp = new HashSet<ChessLocation>();
        int xMax = mChessMap.Length;
        int yMax = mChessMap[0].Length;
        barrier.Add(target);
        while (targets.Count == 0) {
            foreach (ChessLocation b in barrier) {
                List<ChessLocation> neighbor = ChessLocation.getNeighbor(b);
                foreach (ChessLocation n in neighbor) {
                    if ((n.x >= xMax) || (n.y >= yMax) || (n.x < 0) || (n.y < 0)) {
                        continue;
                    }
                    if (mChessMap[n.x][n.y] == null) {
                        targets.Add(n);
                    } else if (!travers.Contains(n)) {
                        travers.Add(n);
                        temp.Add(n);
                    }
                }
            }
            barrier.Clear();
            foreach (ChessLocation t in temp) {
                barrier.Add(t);
            }
            temp.Clear();
        }
        System.Random r;
        foreach (ChessLocation t in targets) {
            if (mobility >= ChessLocation.getDistance(chess.location,t)) {
                temp.Add(t);
            }
        }
        if (temp.Count != 0) {
            ChessLocation[] tempTargets = new ChessLocation[temp.Count];
            temp.CopyTo(tempTargets);
            r = new System.Random(RandomUtil.next());
            return tempTargets[r.Next(0,tempTargets.Length)];
        }
        ChessLocation[] locTargets = new ChessLocation[targets.Count];
        targets.CopyTo(locTargets);
        r = new System.Random(RandomUtil.next());
        ChessLocation locTarget = locTargets[r.Next(0,locTargets.Length)];
        List<ChessLocation> limit = ChessLocation.getLimit(chess.location,locTarget,mobility);
        List<ChessLocation> trueLimit = new List<ChessLocation>();
        foreach (ChessLocation e in limit) {
            if (mChessMap[e.x][e.y] == null) {
                trueLimit.Add(e);
            }
        }
        int newMob = mobility - 1;//
        while ((trueLimit.Count == 0) && (newMob > 0)) {
            limit = ChessLocation.getLimit(chess.location,locTarget,newMob);
            foreach (ChessLocation e in limit) {
                if (mChessMap[e.x][e.y] == null) {
                    trueLimit.Add(e);
                }
            }
            newMob--;
        }//
        if (trueLimit.Count == 0) {
            return chess.location; 
        } else {
            r = new System.Random(RandomUtil.next());
            ChessLocation t = trueLimit[r.Next(0,trueLimit.Count)];
            return t;
        }
    }
}
