using System.Collections;
using System.Collections.Generic;
using System;

public class CommonDefine
{
    public const string kControllerNameTag = "GameController";         // Controller GameObject
    public const string kMainCameraNameTag = "MainCamera";              // Camera tag
    public const string kMapUINameTag = "MapUI";                        // MapUI tag
    public const string kObjPanelTag = "ObjPanel";
    public const string kMapGridObjectName = "MapGridObject";       // 生成的Map块名字
    public const string kMapGridSpritePath = "Textures/MapGrid";    // Map块素材载入路径
    public const string kChessPrefabPath = "Prefab/DemoChess";   // chess素材载入路径
    public const string kSceneSaveRootPath = "Scene/";          // Scene根路径

    public const string kMapGridPrefabPath = "Prefab/MapGrid";
    public const string kObjAttrPrefabPath = "Prefab/ObjAttr";
    public const string kObjImagPrefabPath = "Prefab/ObjImag";

    public const string kManagerMapName = "MapManager";         // MapManager键
    public const string kManagerChessName = "ChessManager";     // ChessManager键
    public const string kManagerRandomName = "RandomManager";     // RandomManager键
    public const string kManagerIOName = "IOManager";       // IOManager键

    public const float  kDatumPointX = 0.5f,            // 逻辑0位置X轴基准点
                        kDatumPointY = 0.5f;            // 逻辑0位置Y轴我基准点
    public const float  kChessBoardDistanceUnit = 1f;   // 棋盘单位长度
    public const float  kChessZAxisOffset = 0f;         // 棋子Z轴坐标
    public const float  kBoardZAxisOffset = 100f;       // 棋盘Z轴坐标
    public const float  kCameraZAxisOffset = -100f;     // 摄像机Z轴坐标
    // public static readonly RGBA kPlayerColor = new RGBA(0.8f,0.8f,1,1);
    public const int kDefaultPort = 53216;      // 默认端口

    public const int kLogicUpdateFPS = 30;

    public const string kPanelChessName = "名字";
    public const string kPanelChessHP = "血量";
    public const string kPanelChessAtk = "攻击力";
    public const string kPanelChessAtkRad = "攻击范围";
    public const string kPanelChessAtkDel = "攻击间隔";
    public const string kPanelChessMob = "移动力";
    public const string kPanelChessMovDel = "移动间隔";

    public class kPlayerColor {
        public const float r = 0.8f;
        public const float g = 0.8f;
        public const float b = 1;
        public const float a = 1;
    }

    public class kEnemyColor {
        public const float r = 1;
        public const float g = 0.8f;
        public const float b = 0.8f;
        public const float a = 1;
    }
    

    public enum fontSize {
        small = 1,
        mid = 2,
        big = 3
    }
}

/* 棋盘中的逻辑位置 */
[Serializable]
public class ChessLocation {
    public int x {get; set;}
    public int y {get; set;}

    public ChessLocation(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public override string ToString() {
        return "("+x + ", " +y+ ")";
    }

    public static int getDistance(ChessLocation a, ChessLocation b) {
        return System.Math.Abs(a.x - b.x) + System.Math.Abs(a.y - b.y);
    }

    public static List<ChessLocation> getNeighbor(ChessLocation a) {
        List<ChessLocation> list = new List<ChessLocation>();
        list.Add(new ChessLocation(a.x - 1,a.y));
        list.Add(new ChessLocation(a.x + 1,a.y));
        list.Add(new ChessLocation(a.x,a.y - 1));
        list.Add(new ChessLocation(a.x,a.y + 1));
        return list;
    }

    /* 获取a点到目标点可以移动的极限点 */
    public static List<ChessLocation> getLimit(ChessLocation a, ChessLocation target, int mobility) {
        List<ChessLocation> list = new List<ChessLocation>();
        int d = getDistance(a,target) - mobility;
        int n = d + 1;
        if (target.x > a.x) {
            if (target.y > a.y) {
                if (d > mobility) {
                    int m = mobility + 1;
                    for (int i = 0;i < m;i++) {
                        list.Add(new ChessLocation(a.x + mobility - i,a.y + i));
                    }
                } else {
                    for (int i = 0;i < n;i++) {
                        list.Add(new ChessLocation(target.x - d + i,target.y - i));
                    }
                }
            } else if (target.y < a.y) {
                if (d > mobility) {
                    int m = mobility + 1;
                    for (int i = 0;i < m;i++) {
                        list.Add(new ChessLocation(a.x + mobility - i,a.y - i));
                    }
                } else {
                    for (int i = 0;i < n;i++) {
                        list.Add(new ChessLocation(target.x - d + i,target.y + i));
                    }
                }
            } else {
                list.Add(new ChessLocation(a.x + mobility,a.y));
            }
        }else if (target.x < a.x) {
            if (target.y > a.y) {
                if (d > mobility) {
                    int m = mobility + 1;
                    for (int i = 0;i < m;i++) {
                        list.Add(new ChessLocation(a.x - mobility + i,a.y + i));
                    }
                } else {
                    for (int i = 0;i < n;i++) {
                        list.Add(new ChessLocation(target.x + d - i,target.y - i));
                    }
                }

            } else if (target.y < a.y) {
                if (d > mobility) {
                    int m = mobility + 1;
                    for (int i = 0;i < m;i++) {
                        list.Add(new ChessLocation(a.x - mobility + i,a.y - i));
                    }
                } else {
                    for (int i = 0;i < n;i++) {
                        list.Add(new ChessLocation(target.x + d - i,target.y + i));
                    }
                }
            } else {
                list.Add(new ChessLocation(a.x - mobility,a.y));
            }
        } else {
            if (target.y > a.y) {
                list.Add(new ChessLocation(a.x,a.y + mobility));
            } else {
                list.Add(new ChessLocation(a.x,a.y - mobility));
            }
        }
        return list;
    }

}
