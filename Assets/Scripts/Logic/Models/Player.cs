using System;

[Serializable]
public class Player
{
    private string uid;     // 用户唯一标识符
    private string name;    // 用户名

    public Player(string uid, string name) {
        this.uid = uid;
        this.name = name;
    }

    public string getName() {
        return name;
    }

    // TODO: PlayerManager: player store & load

    // override object.Equals
    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return this.uid.Equals(((Player)obj).uid);
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        return this.uid.GetHashCode();
    }
}
