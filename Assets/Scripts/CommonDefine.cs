using System.Collections;
using System.Collections.Generic;

public class CommonDefine
{
    public const string kControllerName = "GameController";         // Controller GameObject
    public const string kMapGridObjectName = "MapGridObject";       // 生成的Map块名字
    public const string kMapGridSpritePath = "Textures/MapGrid";    // Map块素材载入路径
}

/* 棋盘中的逻辑位置 */
public class ChessLocation {
    public int x {
        get {
            return x;
        } set {
            x = value;
        }
    }
    public int y {
        get {
            return y;
        } set {
            y = value;
        }
    }

    public ChessLocation(int x, int y) {
        this.x = x;
        this.y = y;
    }
}
