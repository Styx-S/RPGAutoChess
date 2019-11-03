using System.Collections;
using System.Collections.Generic;
using System;

public enum RequestTarget {
    MapManager,
    ChessManager,
}

[Serializable]
public class RequestUser {
    int id;
    public RequestUser(int id) {
        this.id = id;
    }

    public override bool Equals(object obj) {

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return id == ((RequestUser)obj).id;
    }
    public override int GetHashCode() {
        return id;
    }
}

[Serializable]
public class RequestBase {
    public RequestTarget target;
    public int requestId;
    public object requestArgs;
    public RequestUser user;

    public RequestBase() {}
    public RequestBase(RequestTarget target, int requestId) {
        this.target = target;
        this.requestId = requestId;
    }
}
