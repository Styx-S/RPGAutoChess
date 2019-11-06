using System;

[Serializable]
public class MapCompletionMessage : MessageBase {
    public Map data;
    public int mapNum;
    
    public MapCompletionMessage(int mapNum, Map data) : base(MessageInfoType.BattleMapGrid, MessageInfoKindType.Completion, 0) {
        this.mapNum = mapNum;
        this.data = data;
    }
}