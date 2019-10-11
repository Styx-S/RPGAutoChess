using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessManager : MonoBehaviour
{

    private Dictionary<ChessBase,Vector2Int> mChessList = new Dictionary<ChessBase, Vector2Int>();
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
        mChessList.Add(new ChessBase(),new Vector2Int(0,0));
        mChessList.Add(new ChessBase(),new Vector2Int(1,1));
        mChessList.Add(new ChessBase(),new Vector2Int(1,2));
        mChessList.Add(new ChessBase(),new Vector2Int(5,6));
        mChessList.Add(new ChessBase(),new Vector2Int(4,5));
        mChessList.Add(new ChessBase(),new Vector2Int(5,4));
    }

    /* 为chess选择一个攻击target，target可以不在chess攻击范围内 */
    public void selectTarget(ChessBase chess) {
        float minLen = float.MaxValue;
        ChessBase target = null;
        Vector2Int chessPosition = mChessList[chess];
        foreach (ChessBase e in mChessList.Keys) {
            if (e == chess) {
                continue;
            }
            if (/* e是chess的友军 */false) {
                continue;
            }
            Vector2Int ePosition = mChessList[e];
            int len = System.Math.Abs(chessPosition.x - ePosition.x) + System.Math.Abs(chessPosition.y - ePosition.y);
        }
    }
}
