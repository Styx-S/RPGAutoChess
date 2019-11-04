using System;

[Serializable]
public class MapCompletionMessage : MessageBase {
    public MapGrid[][] data;
    public int mapNum;
    
    public MapCompletionMessage(int mapNum, MapGrid[][] data) : base(MessageInfoType.BattleMapGrid, MessageInfoKindType.Completion, 0) {
        this.mapNum = mapNum;
        this.data = data;
    }
}