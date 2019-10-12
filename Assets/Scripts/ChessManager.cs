using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessManager : MonoBehaviour
{

    private List<ChessBase> mChessList = new List<ChessBase>();
    private Dictionary<ChessBase,Dictionary<ChessBase,int>> mDistanceMap = new Dictionary<ChessBase, Dictionary<ChessBase, int>>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* 从某个地方载入棋子到list中 */
    public void loadChess() {
        Player playerA = new Player("1","A");   //暂时hardcode，之后用不到
        Player playerB = new Player("2","B");
        addChess(new ChessBase(playerA,new Vector2Int(0,0)));
        addChess(new ChessBase(playerA,new Vector2Int(1,1)));
        addChess(new ChessBase(playerA,new Vector2Int(1,2)));
        addChess(new ChessBase(playerB,new Vector2Int(5,6)));
        addChess(new ChessBase(playerB,new Vector2Int(4,5)));
        addChess(new ChessBase(playerB,new Vector2Int(5,4)));
    }

    /* 为chess选择一个攻击target，target可以不在chess攻击范围内 */
    public ChessBase selectTarget(ChessBase chess) {
        int minLen = int.MaxValue;
        ChessBase target = null;
        foreach (ChessBase e in mChessList) {
            if (e == chess) {
                continue;
            }
            if (e.Owner == chess.Owner) {
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
    public Vector2Int getChessPosition(ChessBase chess) {
        return chess.position;
    }

    /* 移动棋子 */
    public void moveChess(ChessBase chess, Vector2Int position) {
        chess.position = position;
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
        mChessList.Remove(chess);
        mDistanceMap.Remove(chess);
        foreach (ChessBase e in mDistanceMap.Keys) {
            mDistanceMap[e].Remove(chess);
        }
    }

    /* 增加棋子 */
    public void addChess(ChessBase chess) {
        mChessList.Add(chess);
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
    public int getDistance(ChessBase chess ,ChessBase target) {
        Vector2Int chessPosition = chess.position;
        Vector2Int targetPosition = target.position;
        return System.Math.Abs(chessPosition.x - targetPosition.x) + System.Math.Abs(chessPosition.y - targetPosition.y);
    }
}
