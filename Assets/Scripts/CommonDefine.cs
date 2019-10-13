﻿using System.Collections;
using System.Collections.Generic;

public class CommonDefine
{
    public const string kControllerName = "GameController";         // Controller GameObject
    public const string kMapGridObjectName = "MapGridObject";       // 生成的Map块名字
    public const string kMapGridSpritePath = "Textures/MapGrid";    // Map块素材载入路径

    public const string kManagerMapName = "MapManager";         // MapManager键
    public const string kManagerChessName = "ChessManager";     // ChessManager键
}

/* 棋盘中的逻辑位置 */
public class ChessLocation {
    public int x {get; set;}
    public int y {get; set;}

    public ChessLocation(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static int getDistance(ChessLocation a, ChessLocation b) {
        return System.Math.Abs(a.x - b.x) + System.Math.Abs(a.y - b.y);
    }
}
